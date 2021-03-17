using System;
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
#if DEBUG
        private const MessageImportance LogLevel = MessageImportance.High;
#else
        private const MessageImportance LogLevel = MessageImportance.Low;
#endif

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
                Log.LogMessage(
                    LogLevel,
                    $"No Cake reference required for {ProjectType} projects.");
                return true;
            }

            var omitted = Omitted.Select(x => x.ToString()).ToArray();
            var toCheck = ReferencesToCheck.Select(x => x.ToString()).ToArray();
            foreach (var r in References)
            {
                var package = r.ToString();
                var version = r.GetMetadata("version");
                if (!toCheck.Any(x => x.Equals(package, StringComparison.OrdinalIgnoreCase)))
                {
                    // not a cake reference
                    continue;
                }

                if (omitted.Any(x => x.Equals(package, StringComparison.OrdinalIgnoreCase)))
                {
                    Log.LogMessage(LogLevel, $"Package '{package}' is set to omit.");
                    continue;
                }

                Log.LogMessage(LogLevel, $"Checking {package} version {version} against recommended version of {RecommendedVersion}");

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

            return true;
        }
    }
}
