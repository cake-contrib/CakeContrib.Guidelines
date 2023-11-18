using System.ComponentModel;

using Microsoft.Build.Construction;

using Spectre.Console;
using Spectre.Console.Cli;

namespace Generators.Commands;

public class PackageReferencesCommand : Command<PackageReferencesCommand.Settings>
{
    private readonly IAnsiConsole console;

    public PackageReferencesCommand(
        IAnsiConsole console)
    {
        this.console = console;
    }

    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<CakeSourcePath>")]
        public string CakeSourcePath { get; init; }

        [CommandOption("-f|--flatList")]
        [Description("Print only a flat list of the references, not a code fragment")]
        public bool FlatList { get; init; }

        public Predicate<string>[] ProjectsToSkip => new Predicate<string>[]
        {
            x => x.Equals("Build.csproj", StringComparison.OrdinalIgnoreCase),
            x => x.Equals("Cake.Cli.csproj", StringComparison.OrdinalIgnoreCase),
            x => x.Contains(".Tests.", StringComparison.OrdinalIgnoreCase),
            x => x.Equals("Cake.Tool.csproj", StringComparison.OrdinalIgnoreCase),
        };

        public Predicate<PkgReference>[] ReferencesToSkip => new Predicate<PkgReference>[]
        {
            x => x.Name.StartsWith("Basic.Reference.Assemblies", StringComparison.OrdinalIgnoreCase),
        };
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        var projects = Directory.GetFiles(
            settings.CakeSourcePath,
            "*.csproj",
            new EnumerationOptions { RecurseSubdirectories = true, MatchCasing = MatchCasing.CaseInsensitive });

        console.WriteLine($"Found {projects.Length} projects");

        var references = new Dictionary<string, PkgReference[]>();

        foreach (string proj in projects)
        {
            var file = Path.GetFileName(proj);
            if (settings.ProjectsToSkip.Any(p => p(file)))
            {
                console.MarkupLineInterpolated($"[silver]Skipping project: {file}[/]");
                continue;
            }

            console.MarkupLineInterpolated($"Processing {file}");
            var refs = ProcessProject(proj, settings.ReferencesToSkip);
            if (refs.Length > 0)
            {
                references.Add(file, refs);
            }
        }

        console.MarkupLine("Flattening references");
        var allRefs = references
            .SelectMany(x => x.Value)
            .DistinctBy(x => $"{x.Name}|{x.Version ?? "0"}")
            .ToLookup(x => x.Name)
            .OrderBy(x => x.Key)
            .ToArray();

        foreach (var multiRef in allRefs.Where(x => x.Count() > 1))
        {
            console.MarkupLineInterpolated(
                $"[silver]WARN: {multiRef.Key} has multiple versions: {string.Join(", ", multiRef.Select(x => x.Version))}");
        }

        var gitInfo = GetGitInformation(settings.CakeSourcePath);
        var flatRefs = allRefs.ToDictionary(x => x.Key, x => x.First().Version);
        if (settings.FlatList)
        {
            PrintFlatList(flatRefs, gitInfo);
        }
        else
        {
            PrintCodeFragment(flatRefs, gitInfo);
        }

        return 0;
    }

    private string? GetGitInformation(string path)
    {
        console.MarkupLine("checking git commit/tag");
        var gitCloneDir = path;
        string gitDir;
        do
        {
            gitDir = Path.Combine(gitCloneDir, ".git");
            if (Directory.Exists(gitDir))
            {
                break;
            }

            gitCloneDir = Directory.GetParent(gitCloneDir)?.FullName;
        } while (gitCloneDir != null && Directory.Exists(gitCloneDir));

        if (!Directory.Exists(gitDir))
        {
            console.MarkupLineInterpolated($"[silver]No .git dir found.[/]");
            return null;
        }

        var head = Path.Combine(gitDir, "HEAD");
        if (!File.Exists(head))
        {
            console.MarkupLineInterpolated($"[silver]{head} is missing.[/]");
            return null;
        }

        var commit = File.ReadAllText(head);

        console.MarkupLineInterpolated($"[silver]HEAD is at {commit}[/]");

        var tagsDir = Path.Combine(gitDir, "refs", "tags");
        if (!Directory.Exists(tagsDir))
        {
            console.MarkupLineInterpolated($"[silver]no tags dir available.[/]");
            return commit;
        }

        var tags = Directory.GetFiles(tagsDir)
            .Select(f =>
            {
                var content = File.ReadAllText(f);

                return new { Tag = Path.GetFileNameWithoutExtension(f), Commit = content };
            })
            .Where(x => x.Commit.Equals(commit, StringComparison.OrdinalIgnoreCase))
            .ToArray();

        if (tags.Length == 0)
        {
            console.MarkupLineInterpolated($"[silver]no tag matches the current commit.[/]");
            return commit;
        }

        if (tags.Length > 1)
        {
            console.MarkupLineInterpolated($"[silver]multiple tags match the current commit: {string.Join(", ", tags.Select(t => t.Tag))}[/]");
        }

        return tags.Last().Tag;
    }

    private void PrintCodeFragment(IReadOnlyDictionary<string, string?> refs, string? gitTag)
    {
        var varName = "references";
        console.WriteLine();
        if (gitTag != null)
        {
            var pascalCaseGitTag = gitTag[..1].ToUpper() + gitTag[1..];
            console.WriteLine($"// parsed from Cake: {gitTag}");
            if (gitTag.Contains('.'))
            {
                varName = "Cake" + pascalCaseGitTag.Replace(".", "");
            }
            else
            {
                varName = "CakeCommit_" + gitTag[..8];
            }
        }

        console.WriteLine($"private static readonly Dictionary<string, string> {varName} = new Dictionary<string, string>");
        console.WriteLine("{");
        foreach ((string name, string? version) in refs)
        {
            console.WriteLine($"  {{ \"{name}\", \"{version ?? "0"}\" }},");
        }
        console.WriteLine("};");
    }

    private void PrintFlatList(IReadOnlyDictionary<string, string?> refs, string? gitTag)
    {
        if (gitTag != null)
        {
            console.WriteLine($"#### Cake {gitTag}");
        }
        var t = new Table()
            .AddColumn("Reference")
            .AddColumn("Version")
            .Border(TableBorder.Markdown);
        foreach ((string name, string? version) in refs)
        {
            t.AddRow(name, version ?? string.Empty);
        }

        console.Write(t);
    }

    private PkgReference[] ProcessProject(
        string fullFile,
        Predicate<PkgReference>[] referencesToSkip)
    {
        var project = ProjectRootElement.Open(fullFile);
        if (project == null)
        {
            console.MarkupLineInterpolated($"[red]Failed to parse {Path.GetFileName(fullFile)}[/]");
            return Array.Empty<PkgReference>();
        }

        var foo = project.ItemGroups
            .SelectMany(g => g.Children)
            .Where(c => c.ElementName.Equals("PackageReference", StringComparison.OrdinalIgnoreCase))
            .Cast<ProjectItemElement>()
            .ToArray();
        var references = foo
            .Select(x => new PkgReference
            {
                Name = x.Include,
                Version = x.Metadata
                    .FirstOrDefault(y => y.Name.Equals("Version", StringComparison.OrdinalIgnoreCase))?.Value,
            })
            .Where(x => !referencesToSkip.Any(p => p(x)))
            .ToArray();

        return references;
    }

    public class PkgReference
    {
        public string Name { get; init; } = string.Empty;
        public string? Version { get; init; }
    }
}
