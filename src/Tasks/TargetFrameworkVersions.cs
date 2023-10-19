using System;
using System.Collections.Generic;
using System.Linq;

using CakeContrib.Guidelines.Tasks.Extensions;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace CakeContrib.Guidelines.Tasks
{
    /// <summary>
    /// The Task to check for References for the guideline
    /// <see href="https://cake-contrib.github.io/CakeContrib.Guidelines/guidelines/TargetFramework"/>.
    /// </summary>
    public class TargetFrameworkVersions : Task
    {
#if DEBUG
        private const MessageImportance LogLevel = MessageImportance.High;
#else
        private const MessageImportance LogLevel = MessageImportance.Low;
#endif

        private const string NetStandard20 = "netstandard2.0";
        private const string Net46 = "net46";
        private const string Net461 = "net461";
        private const string NetCore31 = "netcoreapp3.1";
        private const string Net50 = "net5.0";
        private const string Net60 = "net6.0";
        private const string Net70 = "net7.0";

        private static readonly TargetsDefinitions DefaultTarget = new TargetsDefinitions
        {
            Name = "Default",
            RequiredTargets = new[]
            {
                TargetsDefinition.From(Net60),
                TargetsDefinition.From(Net70),
            },
            SuggestedTargets = Array.Empty<TargetsDefinition>(),
        };

        private static readonly Dictionary<Predicate<Differentiator>, TargetsDefinitions> SpecificTargets =
            new Dictionary<Predicate<Differentiator>, TargetsDefinitions>
            {
                {
                    d => d.IsModuleProject && d.Version.LessThan(CakeVersions.V2),
                    new TargetsDefinitions
                    {
                        Name = "Module",
                        RequiredTargets = new[] { TargetsDefinition.From(NetStandard20) },
                    }
                },
                {
                    d => d.IsModuleProject && d.Version.GreaterEqual(CakeVersions.V2) && d.Version.LessThan(CakeVersions.V3),
                    new TargetsDefinitions
                    {
                        Name = "Module",
                        RequiredTargets = new[] { TargetsDefinition.From(NetCore31) },
                    }
                },
                {
                    d => d.IsModuleProject && d.Version.GreaterEqual(CakeVersions.V3),
                    new TargetsDefinitions
                    {
                        Name = "Module",
                        RequiredTargets = new[] { TargetsDefinition.From(Net60) },
                    }
                },
                {
                    d => !d.IsModuleProject && d.Version.GreaterEqual(CakeVersions.Vo26) && d.Version.LessThan(CakeVersions.V1),
                    new TargetsDefinitions
                    {
                        Name = "0.26.0 <= x < 1.0.0",
                        RequiredTargets = new[] { TargetsDefinition.From(NetStandard20) },
                        SuggestedTargets = new[] { TargetsDefinition.From(Net461, Net46) },
                    }
                },
                {
                    d => !d.IsModuleProject && d.Version.GreaterEqual(CakeVersions.V1) && d.Version.LessThan(CakeVersions.V2),
                    new TargetsDefinitions
                    {
                        Name = "1.0.0 <= x < 2.0.0",
                        RequiredTargets = new[] { TargetsDefinition.From(NetStandard20) },
                        SuggestedTargets = new[]
                        {
                            TargetsDefinition.From(Net461, Net46),
                            TargetsDefinition.From(Net50),
                        },
                    }
                },
                {
                    d => !d.IsModuleProject && d.Version.GreaterEqual(CakeVersions.V2) && d.Version.LessThan(CakeVersions.V3),
                    new TargetsDefinitions
                    {
                        Name = "2.0.0 <= x < 3.0.0",
                        RequiredTargets = new[]
                        {
                            TargetsDefinition.From(NetCore31),
                            TargetsDefinition.From(Net50),
                            TargetsDefinition.From(Net60),
                        },
                        SuggestedTargets = Array.Empty<TargetsDefinition>(),
                    }
                },
                {
                    d => !d.IsModuleProject && d.Version.GreaterEqual(CakeVersions.V3),
                    new TargetsDefinitions
                    {
                        Name = "x >= 3.0.0",
                        RequiredTargets = new[]
                        {
                            TargetsDefinition.From(Net60),
                            TargetsDefinition.From(Net70),
                        },
                        SuggestedTargets = Array.Empty<TargetsDefinition>(),
                    }
                },
            };

        /// <summary>
        /// Gets or sets the References.
        /// </summary>
        [Required]
        public ITaskItem[] References { get; set; }

        /// <summary>
        /// Gets or sets the TargetFrameworks.
        /// </summary>
        [Required]
        public ITaskItem[] TargetFrameworks { get; set; }

        /// <summary>
        /// Gets or sets the TargetFramework.
        /// </summary>
        [Required]
        public ITaskItem TargetFramework { get; set; }

        /// <summary>
        /// Gets or sets the ProjectType.
        /// </summary>
        [Required]
        public string ProjectType { get; set; }

        /// <summary>
        /// Gets or sets Targets to omit. I.e. if those are missing, they will not be reported.
        /// </summary>
        public ITaskItem[] Omitted { get; set; }

        /// <summary>
        /// Gets or sets the project file.
        /// </summary>
        [Required]
        public string ProjectFile { get; set; }

        /// <summary>
        /// Gets or sets an explicit Cake version instead of doing Cake.Core detection.
        /// </summary>
        public string CakeVersion { get; set; }

        /// <summary>
        /// Gets or sets the warnings that are suppressed.
        /// </summary>
        public string[] NoWarn { get; set; }

        /// <summary>
        /// Gets or sets the warnings that should be raised as errors.
        /// </summary>
        public string[] WarningsAsErrors { get; set; }

        /// <inheritdoc />
        public override bool Execute()
        {
            if (!CakeProjectType.IsOneOf(ProjectType, CakeProjectType.Addin, CakeProjectType.Module))
            {
                Log.LogMessage(
                    LogLevel,
                    $"No TFM suggested for {ProjectType} projects.");
                return true;
            }

            if (string.IsNullOrEmpty(CakeVersion))
            {
                // find cake.core version
                var cakeCore =
                    References?.FirstOrDefault(x => x.ToString().Equals("Cake.Core", StringComparison.OrdinalIgnoreCase));
                if (cakeCore == null)
                {
                    Log.LogMessage(
                        LogLevel,
                        "Could not find Cake.Core reference. Using default TargetVersions.");
                    return Execute(DefaultTarget);
                }

                CakeVersion = cakeCore.GetMetadata("version");
                var prereleaseIndex = CakeVersion.IndexOf("-", StringComparison.Ordinal);
                if (prereleaseIndex > -1)
                {
                    var prerelease = CakeVersion;
                    CakeVersion = CakeVersion.Substring(0, prereleaseIndex);
                    Log.CcgTrace($"Cake.Core has a version of {prerelease}. Assuming a prerelease and correcting version to {CakeVersion}.");
                }
            }
            else
            {
                Log.CcgTrace($"Cake version explicitly set to {CakeVersion}.");
            }

            if (!Version.TryParse(CakeVersion, out Version version))
            {
                Log.CcgWarning(
                    7,
                    ProjectFile,
                    $"Cake version was {CakeVersion} which is not a valid version. Using default TargetVersions.",
                    NoWarn,
                    WarningsAsErrors);
                return Execute(DefaultTarget);
            }

            Log.LogMessage(
                LogLevel,
                $"Cake reference version is {version}");

            foreach (var targetsDefinition in SpecificTargets)
            {
                var differentiator = new Differentiator
                {
                    IsModuleProject = CakeProjectType.Module.Is(ProjectType),
                    Version = version,
                };
                var match = targetsDefinition.Key(differentiator);
                if (!match)
                {
                    continue;
                }

                return Execute(targetsDefinition.Value);
            }

            Log.LogMessage(
                LogLevel,
                $"Could not find a specific TargetVersions-setting for Cake version {version}. Using default TargetVersions.");
            return Execute(DefaultTarget);
        }

        private bool Execute(TargetsDefinitions targets)
        {
            Log.LogMessage(
                LogLevel,
                $"Verifying TargetVersions-setting against rule: {targets.Name}");
            var allTargets = new List<string>();
            if (TargetFramework != null)
            {
                allTargets.Add(TargetFramework.ToString());
            }

            if (TargetFrameworks != null)
            {
                allTargets.AddRange(TargetFrameworks.Select(x => x.ToString()));
            }

            allTargets = allTargets.Distinct().ToList();

            // first, check required targets
            Log.LogMessage(
                LogLevel,
                $"Comparing TargetFramework[s] ({string.Join(";", allTargets)}) to required: {string.Join(",", targets.RequiredTargets.Select(x => x.Name))}.");

            foreach (var requiredTarget in targets.RequiredTargets)
            {
                if (Omitted != null && Omitted.Any(x => x.ToString().Equals(requiredTarget.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    Log.LogMessage(
                        LogLevel,
                        $"Required TargetFramework '{requiredTarget.Name}' is set to omit.");
                    continue;
                }

                if (allTargets.Any(x => x.Equals(requiredTarget.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    continue;
                }

                var found = requiredTarget.Alternatives?.Any(alternative => allTargets.Contains(alternative));
                if (found.GetValueOrDefault(false))
                {
                    continue;
                }

                Log.CcgError(
                    7,
                    ProjectFile,
                    "Missing required target: " + requiredTarget.Name);
                return false;
            }

            // now, check suggested targets
            Log.LogMessage(
                LogLevel,
                $"Comparing TargetFramework[s] ({string.Join(";", allTargets)}) to suggested: {string.Join(",", targets.SuggestedTargets.Select(x => x.Name))}.");

            foreach (var suggestedTarget in targets.SuggestedTargets)
            {
                if (Omitted != null && Omitted.Any(x => x.ToString().Equals(suggestedTarget.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    Log.LogMessage(
                        LogLevel,
                        $"Suggested TargetFramework '{suggestedTarget.Name}' is set to omit.");
                    continue;
                }

                if (allTargets.Any(x => x.Equals(suggestedTarget.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    continue;
                }

                var found = suggestedTarget.Alternatives?.Any(alternative => allTargets.Contains(alternative));
                if (found.GetValueOrDefault(false))
                {
                    continue;
                }

                Log.CcgWarning(
                    7,
                    ProjectFile,
                    "Missing suggested target: " + suggestedTarget.Name,
                    NoWarn,
                    WarningsAsErrors);
            }

            return true;
        }

        private class TargetsDefinitions
        {
            public TargetsDefinitions()
            {
                RequiredTargets = Array.Empty<TargetsDefinition>();
                SuggestedTargets = Array.Empty<TargetsDefinition>();
            }

            public string Name { get; set; }

            public TargetsDefinition[] RequiredTargets { get; set; }

            public TargetsDefinition[] SuggestedTargets { get; set; }
        }

        private class TargetsDefinition
        {
            public string Name { get; private set; }

            public string[] Alternatives { get; private set; }

            public static TargetsDefinition From(string name, params string[] alternatives)
            {
                return new TargetsDefinition { Name = name, Alternatives = alternatives ?? Array.Empty<string>(), };
            }
        }

        private class Differentiator
        {
            internal Version Version { get; set; }

            internal bool IsModuleProject { get; set; }
        }
    }
}
