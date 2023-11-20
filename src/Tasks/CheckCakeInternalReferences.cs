using System;
using System.Collections.Generic;
using System.Linq;

using CakeContrib.Guidelines.Tasks.Extensions;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace CakeContrib.Guidelines.Tasks
{
    /// <summary>
    /// The Task to check for cake internal package references.
    /// For the guideline <see href="https://cake-contrib.github.io/CakeContrib.Guidelines/guidelines/CakeInternalReferences"/>.
    /// </summary>
    public class CheckCakeInternalReferences : Task
    {
        private const int CcgRule = 10;

#if DEBUG
        private const MessageImportance LogLevel = MessageImportance.High;
#else
        private const MessageImportance LogLevel = MessageImportance.Low;
#endif

        // parsed from Cake: v0.38
        private static readonly Dictionary<string, string> CakeV038 = new Dictionary<string, string>
        {
            { "Autofac", "4.9.4" },
            { "Microsoft.CodeAnalysis.CSharp.Scripting", "3.6.0" },
            { "Microsoft.CSharp", "4.5.0" },
            { "Microsoft.DotNet.PlatformAbstractions", "3.1.0" },
            { "Microsoft.NETCore.Platforms", "3.1.0" },
            { "Microsoft.Win32.Registry", "4.4.0" },
            { "Newtonsoft.Json", "12.0.2" },
            { "NuGet.Common", "5.4.0" },
            { "NuGet.Frameworks", "5.4.0" },
            { "NuGet.Packaging", "5.4.0" },
            { "NuGet.Protocol", "5.4.0" },
            { "NuGet.Resolver", "5.4.0" },
            { "NuGet.Versioning", "5.4.0" },
            { "xunit", "2.4.1" },
        };

        // parsed from Cake: v1.0.0
        private static readonly Dictionary<string, string> CakeV10 = new Dictionary<string, string>
        {
            { "Autofac", "6.1.0" },
            { "Microsoft.CodeAnalysis.CSharp.Scripting", "3.9.0-1.final" },
            { "Microsoft.CSharp", "4.7.0" },
            { "Microsoft.DotNet.PlatformAbstractions", "3.1.6" },
            { "Microsoft.Extensions.DependencyInjection", "5.0.1" },
            { "Microsoft.NETCore.Platforms", "5.0.0" },
            { "Microsoft.Win32.Registry", "5.0.0" },
            { "Newtonsoft.Json", "12.0.3" },
            { "NuGet.Common", "5.8.0" },
            { "NuGet.Frameworks", "5.8.0" },
            { "NuGet.Packaging", "5.8.0" },
            { "NuGet.Protocol", "5.8.0" },
            { "NuGet.Resolver", "5.8.0" },
            { "NuGet.Versioning", "5.8.0" },
            { "System.Collections.Immutable", "5.0.0" },
            { "System.Reflection.Metadata", "5.0.0" },
            { "xunit", "2.4.1" },
        };

        // parsed from Cake: v2.0.0
        private static readonly Dictionary<string, string> CakeV20 = new Dictionary<string, string>
        {
            { "Autofac", "6.3.0" },
            { "Microsoft.CodeAnalysis.CSharp.Scripting", "4.0.1" },
            { "Microsoft.CSharp", "4.7.0" },
            { "Microsoft.DotNet.PlatformAbstractions", "3.1.6" },
            { "Microsoft.Extensions.DependencyInjection", "6.0.0" },
            { "Microsoft.NETCore.Platforms", "6.0.0" },
            { "Microsoft.Win32.Registry", "5.0.0" },
            { "Newtonsoft.Json", "13.0.1" },
            { "NuGet.Common", "5.11.0" },
            { "NuGet.Frameworks", "5.11.0" },
            { "NuGet.Packaging", "5.11.0" },
            { "NuGet.Protocol", "5.11.0" },
            { "NuGet.Resolver", "5.11.0" },
            { "NuGet.Versioning", "5.11.0" },
            { "System.Collections.Immutable", "6.0.0" },
            { "System.Reflection.Metadata", "6.0.0" },
            { "xunit", "2.4.1" },
        };

        // parsed from Cake: v3.0.0
        private static readonly Dictionary<string, string> CakeV30 = new Dictionary<string, string>
        {
            { "Autofac", "6.4.0" },
            { "Microsoft.CodeAnalysis.CSharp.Scripting", "4.4.0-4.final" },
            { "Microsoft.CSharp", "4.7.0" },
            { "Microsoft.Extensions.DependencyInjection", "7.0.0" },
            { "Microsoft.NETCore.Platforms", "7.0.0" },
            { "Microsoft.Win32.Registry", "5.0.0" },
            { "Newtonsoft.Json", "13.0.1" },
            { "NuGet.Common", "6.3.1" },
            { "NuGet.Frameworks", "6.3.1" },
            { "NuGet.Packaging", "6.3.1" },
            { "NuGet.Protocol", "6.3.1" },
            { "NuGet.Resolver", "6.3.1" },
            { "NuGet.Versioning", "6.3.1" },
            { "System.Collections.Immutable", "7.0.0" },
            { "System.Reflection.Metadata", "7.0.0" },
            { "xunit", "2.4.2" },
        };

        // parsed from Cake: v4.0
        private static readonly Dictionary<string, string> CakeV40 = new Dictionary<string, string>
        {
            { "Autofac", "7.1.0" },
            { "Microsoft.CodeAnalysis.CSharp.Scripting", "4.8.0-3.final" },
            { "Microsoft.CSharp", "4.7.0" },
            { "Microsoft.Extensions.DependencyInjection", "8.0.0" },
            { "Microsoft.NETCore.Platforms", "7.0.4" },
            { "Microsoft.Win32.Registry", "5.0.0" },
            { "Newtonsoft.Json", "13.0.3" },
            { "NuGet.Common", "6.7.0" },
            { "NuGet.Frameworks", "6.7.0" },
            { "NuGet.Packaging", "6.7.0" },
            { "NuGet.Protocol", "6.7.0" },
            { "NuGet.Resolver", "6.7.0" },
            { "NuGet.Versioning", "6.7.0" },
            { "System.Collections.Immutable", "8.0.0" },
            { "System.Reflection.Metadata", "8.0.0" },
            { "xunit", "2.6.1" },
        };

        private readonly Dictionary<Predicate<Version>, Dictionary<string, string>> allInternalReferences =
            new Dictionary<Predicate<Version>, Dictionary<string, string>>
            {
                {
                    x => x.LessThan(CakeVersions.V1),
                    CakeV038
                },
                {
                    x => x.GreaterEqual(CakeVersions.V1) && x.LessThan(CakeVersions.V2),
                    CakeV10
                },
                {
                    x => x.GreaterEqual(CakeVersions.V2) && x.LessThan(CakeVersions.V3),
                    CakeV20
                },
                {
                    x => x.GreaterEqual(CakeVersions.V3) && x.LessThan(CakeVersions.V4),
                    CakeV30
                },
                {
                    x => x.GreaterEqual(CakeVersions.V4) && x.LessThan(CakeVersions.VNext),
                    CakeV40
                },
            };

        /// <summary>
        /// Gets or sets the ProjectType.
        /// </summary>
        [Required]
        public string ProjectType { get; set; }

        /// <summary>
        /// Gets or sets the References.
        /// </summary>
        [Required]
        public ITaskItem[] References { get; set; }

        /// <summary>
        /// Gets or sets the warnings that are suppressed.
        /// </summary>
        public string[] NoWarn { get; set; }

        /// <summary>
        /// Gets or sets the warnings that should be raised as errors.
        /// </summary>
        public string[] WarningsAsErrors { get; set; }

        /// <summary>
        /// Gets or sets the project file.
        /// </summary>
        public string ProjectFile { get; set; }

        /// <summary>
        /// Gets or sets an explicit Cake version instead of doing Cake.Core detection.
        /// </summary>
        public string CakeVersion { get; set; }

        /// <inheritdoc />
        public override bool Execute()
        {
            if (!CakeProjectType.IsOneOf(ProjectType, CakeProjectType.Addin, CakeProjectType.Module))
            {
                Log.LogMessage(
                    LogLevel,
                    $"Internal Cake references will not be checked for {ProjectType} projects.");
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
                        "Could not find Cake.Core reference. Internal Cake references will not be checked.");
                    return true;
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
                Log.CcgTrace($"Cake version was {CakeVersion} which is not a valid version.");
                return true;
            }

            var internalReferences = allInternalReferences
                .Where(x => x.Key.Invoke(version))
                .Select(x => x.Value)
                .FirstOrDefault();

            if (internalReferences == null)
            {
                Log.CcgWarning(
                    CcgRule,
                    ProjectFile,
                    $"Cake version was {CakeVersion} but no matching list of Cake provided references could be found.",
                    NoWarn,
                    WarningsAsErrors);
                return true;
            }

            var referencesInProject = References.Select(x => new
            {
                Name = x.ToString(),
                Version = x.GetMetadata("version"),
                IsPrivate = (x.GetMetadata("PrivateAssets")?.ToLower() ?? string.Empty) == "all",
            }).ToArray();

            foreach (var reference in referencesInProject)
            {
                var internalRef = internalReferences
                    .Where(x => reference.Name.Equals(x.Key, StringComparison.OrdinalIgnoreCase))
                    .Select(x => new
                    {
                        Name = x.Key,
                        Version = x.Value,
                    })
                    .FirstOrDefault();

                if (internalRef == null)
                {
                    continue;
                }

                if (!Version.TryParse(reference.Version, out var localVersion))
                {
                    Log.LogMessage(
                        LogLevel,
                        $"The reference {reference.Name} had version {reference.Version}. The version could not be parsed!");
                    continue;
                }

                if (!Version.TryParse(internalRef.Version, out var cakeRefVersion))
                {
                    Log.LogMessage(
                        LogLevel,
                        $"The Cake v{CakeVersion} reference {internalRef.Name} had version {internalRef.Version}. The version could not be parsed!");
                    continue;
                }

                if (!localVersion.Equals(cakeRefVersion))
                {
                    Log.CcgWarning(
                        CcgRule,
                        ProjectFile,
                        $"{internalRef.Name} is provided by Cake {CakeVersion} in version {cakeRefVersion}. Do not reference a different version.",
                        NoWarn,
                        WarningsAsErrors);
                }

                if (!reference.IsPrivate)
                {
                    Log.CcgWarning(
                        CcgRule,
                        ProjectFile,
                        $"{internalRef.Name} is provided by Cake. It should have `PrivateAssets=\"all\"` set",
                        NoWarn,
                        WarningsAsErrors);
                }
            }

            return true;
        }
    }
}
