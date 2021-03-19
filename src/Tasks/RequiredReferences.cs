using System;
using System.Linq;

using CakeContrib.Guidelines.Tasks.Extensions;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace CakeContrib.Guidelines.Tasks
{
    /// <summary>
    /// The Task to check for References for the guideline <see href="https://cake-contrib.github.io/CakeContrib.Guidelines/guidelines/Analysers"/>.
    /// </summary>
    public class RequiredReferences : Task
    {
#if DEBUG
        private const MessageImportance LogLevel = MessageImportance.High;
#else
        private const MessageImportance LogLevel = MessageImportance.Low;
#endif

        /// <summary>
        /// Gets or sets the References.
        /// </summary>
        [Required]
        public ITaskItem[] References { get; set; }

        /// <summary>
        /// Gets or sets the Packages to check.
        /// </summary>
        [Required]
        public ITaskItem[] Required { get; set; }

        /// <summary>
        /// Gets or sets the Packages to check.
        /// </summary>
        [Required]
        public ITaskItem[] Omitted { get; set; }

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
                // not for recipes!
                Log.LogMessage(
                    LogLevel,
                    $"References are not required for {ProjectType} projects.");
                return true;
            }

            foreach (var r in Required)
            {
                if (Omitted.Any(x => x.ToString().Equals(r.ToString(), StringComparison.OrdinalIgnoreCase)))
                {
                    Log.LogMessage(LogLevel, $"Required reference '{r}' is set to omit.");
                    continue;
                }

                Log.LogMessage(LogLevel, $"Checking required reference: {r}");
                if (References.Any(x => x.ToString().Equals(r.ToString(), StringComparison.OrdinalIgnoreCase)))
                {
                    // found.
                    continue;
                }

                Log.CcgWarning(
                    5,
                    ProjectFile,
                    $"No reference to '{r}' found. Usage of '{r}' is strongly recommended",
                    NoWarn,
                    WarningsAsErrors);
            }

            return true;
        }
    }
}
