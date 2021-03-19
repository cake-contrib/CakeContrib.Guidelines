using System;
using System.Collections.Generic;
using System.Linq;

using CakeContrib.Guidelines.Tasks.Extensions;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace CakeContrib.Guidelines.Tasks
{
    /// <summary>
    /// The Task to check for References for the guideline <see href="https://cake-contrib.github.io/CakeContrib.Guidelines/guidelines/RecommendedTags"/>.
    /// </summary>
    public class RecommendedCakeVersion : Task
    {
        /// <summary>
        /// Gets or sets the Recommended Version.
        /// </summary>
        [Required]
        public string RecommendedVersion { get; set; }

        /// <summary>
        /// Gets or sets the References.
        /// </summary>
        [Required]
        public ITaskItem[] References { get; set; }

        /// <summary>
        /// Gets or sets the references to omit.
        /// </summary>
        [Required]
        public ITaskItem[] Omitted { get; set; }

        /// <summary>
        /// Gets or sets the references to be checked.
        /// </summary>
        [Required]
        public ITaskItem[] ReferencesToCheck { get; set; }

        /// <summary>
        /// Gets or sets the project file.
        /// </summary>
        public string ProjectFile { get; set; }

        /// <summary>
        /// Gets or sets the ProjectType.
        /// </summary>
        [Required]
        public string ProjectType { get; set; }

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
                Log.CcgTrace($"No Cake reference required for {ProjectType} projects.");
                return true;
            }

            var omitted = Omitted.Select(x => x.ToString()).ToArray();
            var toCheck = ReferencesToCheck.Select(x => x.ToString()).ToArray();
            var referencedVersions = new HashSet<string>();
            foreach (var r in References)
            {
                var package = r.ToString();
                var version = r.GetMetadata("version");
                if (!toCheck.Any(x => x.Equals(package, StringComparison.OrdinalIgnoreCase)))
                {
                    // not a cake reference
                    continue;
                }

                referencedVersions.Add(version);
                if (omitted.Any(x => x.Equals(package, StringComparison.OrdinalIgnoreCase)))
                {
                    Log.CcgTrace($"Package '{package}' is set to omit.");
                    continue;
                }

                Log.CcgTrace($"Checking {package} version {version} against recommended version of {RecommendedVersion}");

                if (!RecommendedVersion.Equals(version, StringComparison.OrdinalIgnoreCase))
                {
                    Log.CcgWarning(
                        9,
                        ProjectFile,
                        $"{package} is referenced in version {version}. Recommended version is {RecommendedVersion}.",
                        NoWarn,
                        WarningsAsErrors);
                }
            }

            if (referencedVersions.Count > 1)
            {
                Log.CcgSuggestion(
                    9,
                    ProjectFile,
                    $"{referencedVersions.Count} different versions of Cake were referenced. It is suggested to reference the same version for all Cake references.");
            }

            return true;
        }
    }
}
