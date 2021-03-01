using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

using Xunit.Abstractions;

namespace CakeContrib.Guidelines.Tasks.IntegrationTests.Fixtures
{
    public class E2eTestFixture : IDisposable
    {
        private readonly string tempFolder;
        private readonly ITestOutputHelper logger;

        private string packageIcon = "$(CakeContribGuidelinesIconDestinationLocation)";
        private string packageIconUrl = "https://some/icon/somewhere.png";
        private bool hasStylecopJson = true;
        private bool hasStylecopReference = true;
        private bool hasEditorConfig = true;
        private readonly List<string> customContent = new List<string>();
        private string targetFrameworks = "netstandard2.0;net461";
        private readonly List<string> references = new List<string>();

        public E2eTestFixture(string tempFolder, ITestOutputHelper logger)
        {
            this.tempFolder = tempFolder;
            this.logger = logger;
        }

        public BuildRunResult Run()
        {
            var projectFile = WriteProject();
            return BuildProject(projectFile);
        }

        private string WriteProject()
        {
            var targets = GetTargetsToImport();
            var csproj = Path.Combine(tempFolder, "default.csproj");
            var template = @"
<Project Sdk=""Microsoft.NET.Sdk"">
    <Import Project=""{0}"" />

    <PropertyGroup>
        <TargetFrameworks>{5}</TargetFrameworks>
        {2}
    </PropertyGroup>

    <ItemGroup>
      {3}
    </ItemGroup>

    {4}

    <Import Project=""{1}"" />
</Project>
";

            var properties = new List<string>();
            var items = new List<string>();

            if (!string.IsNullOrEmpty(packageIcon))
            {
                properties.Add($"<PackageIcon>{packageIcon}</PackageIcon>");
            }
            if (!string.IsNullOrEmpty(packageIconUrl))
            {
                properties.Add($"<PackageIconUrl>{packageIconUrl}</PackageIconUrl>");
            }
            if (hasStylecopJson)
            {
                var stylecopJson = Path.Combine(tempFolder, "stylecop.json");
                items.Add($@"<AdditionalFiles Include=""{stylecopJson}"" Link=""stylecop.json"" />");
                File.WriteAllText(stylecopJson, @"{
  ""$schema"": ""https://raw.githubusercontent.com/DotNetAnalyzers/StyleCopAnalyzers/master/StyleCop.Analyzers/StyleCop.Analyzers/Settings/stylecop.schema.json"",
  ""settings"": { }
}");
            }
            if (hasEditorConfig)
            {
                var editorconfig = Path.Combine(tempFolder, ".editorconfig");
                items.Add($@"<None Include=""{editorconfig}"" Link="".editorconfig"" />");
                File.WriteAllText(editorconfig, "root = true");
            }
            if (hasStylecopReference)
            {
                items.Add(@"
<PackageReference Include=""StyleCop.Analyzers"" Version=""1.1.118"">
    <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    <PrivateAssets>all</PrivateAssets>
</PackageReference>");
            }
            items.AddRange(references);

            File.WriteAllText(csproj, string.Format(template,
                targets.Item1,
                targets.Item2,
                string.Join(Environment.NewLine, properties),
                string.Join(Environment.NewLine, items),
                string.Join(Environment.NewLine, customContent),
                targetFrameworks));

            return csproj;
        }

        internal void WithoutPackageIcon()
        {
            packageIcon = null;
        }

        internal void WithPackageIcon(string packageIconPath)
        {
            packageIcon = packageIconPath;
        }

        internal void WithoutPackageIconUrl()
        {
            packageIconUrl = null;
        }

        internal void WithCustomContent(string content)
        {
            customContent.Add(content);
        }

        internal void WithPackageReference(
            string packageName,
            string version,
            string privateAssets = null,
            params Tuple<string, string>[] additionalAttributes)
        {
            var reference = new StringBuilder();
            reference.Append($@"<PackageReference Include=""{packageName}"" Version=""{version}""");
            if (privateAssets != null)
            {
                reference.Append($@" PrivateAssets=""{privateAssets}""");
            }

            foreach ((string key, string val) in additionalAttributes)
            {
                reference.Append($@" {key}=""{val}""");
            }

            reference.Append(" />");
            references.Add(reference.ToString());
        }

        internal void WithoutStylecopReference()
        {
            hasStylecopReference = false;
        }

        internal void WithoutFileStylecopJson()
        {
            hasStylecopJson = false;
        }

        internal void WithoutFileEditorconfig()
        {
            hasEditorConfig = false;
        }

        internal void WithTargetFrameworks(string targetFrameworks)
        {
            this.targetFrameworks = targetFrameworks;
        }

        private Tuple<string, string> GetTargetsToImport()
        {
            var codeBase = typeof(E2eTestFixture).Assembly.CodeBase;
            var assemblyPath = Uri.UnescapeDataString(new UriBuilder(codeBase).Path);
            var folder = Path.GetDirectoryName(assemblyPath);

            // now search up until we find the solution-file
            while (!Directory.GetFiles(folder, "*.sln", SearchOption.TopDirectoryOnly).Any())
            {
                folder = Path.GetDirectoryName(folder);
            }

            var buildFolder = Path.Combine(folder, "Guidelines", "build");
            return new Tuple<string, string>(
                Path.Combine(buildFolder, "CakeContrib.Guidelines.props"),
                Path.Combine(buildFolder, "CakeContrib.Guidelines.targets")
            );
        }

        private BuildRunResult BuildProject(string projectFile)
        {
            // restore
            var psi = new ProcessStartInfo
            {
                UseShellExecute = false,
                RedirectStandardError = false,
                RedirectStandardOutput = false,
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                ErrorDialog = false,
                WorkingDirectory = Path.GetDirectoryName(projectFile),
                FileName = "dotnet",
                Arguments = "restore " + projectFile
            };

            using (Process process = Process.Start(psi))
            {
                process.WaitForExit();
            }

#if NETCORE
            logger.WriteLine("running: dotnet build " + projectFile);
#else
            logger.WriteLine("running: dotnet msbuild " + projectFile);
#endif

            psi = new ProcessStartInfo
            {
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                ErrorDialog = false,
                WorkingDirectory = Path.GetDirectoryName(projectFile),
                FileName = "dotnet",
#if NETCORE
                Arguments = "build -nologo " + projectFile
#else
                Arguments = "msbuild -nologo " + projectFile
#endif
            };

            var exitcode = -1;
            var allOutput = new List<string>();
            using (Process process = Process.Start(psi))
            {
                process.OutputDataReceived += (o, e) =>
                {
                    if (e.Data == null)
                    {
                        return;
                    }

                    logger.WriteLine(e.Data);
                    allOutput.Add(e.Data);
                };

                process.BeginOutputReadLine();
                process.ErrorDataReceived += (o, e) =>
                {
                    if (e.Data == null)
                    {
                        return;
                    }
                    logger.WriteLine("ERROR: " + e.Data);
                    allOutput.Add(e.Data);
                };
                process.BeginErrorReadLine();

                process.WaitForExit();
                logger.WriteLine("Exit code:" + process.ExitCode);
                exitcode = process.ExitCode;
            }

            var errorLines = allOutput.Where(x => x.IndexOf(" error ", StringComparison.OrdinalIgnoreCase) > -1).ToList();
            var warnLines = allOutput.Where(x => x.IndexOf(" warning ", StringComparison.OrdinalIgnoreCase) > -1).ToList();
            return new BuildRunResult
            {
                Output = allOutput,
                ErrorLines = errorLines,
                WarningLines = warnLines,
                ExitCode = exitcode
            };
        }

        public void Dispose()
        {
            Directory.Delete(tempFolder, true);
        }

        public class BuildRunResult
        {
            public List<string> WarningLines { get; internal set; }
            public int ExitCode { get; internal set; }
            public List<string> ErrorLines { get; internal set; }
            public List<string> Output { get; internal set; }

            public bool IsErrorExitCode
            {
                get { return ExitCode != 0; }
            }
        }
    }
}
