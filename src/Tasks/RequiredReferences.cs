using System;
using System.Linq;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace CakeContrib.Guidelines.Tasks
{
    /// <summary>
    /// The Task to check for References for the guideline <see href="https://cake-contrib.github.io/CakeContrib.Guidelines/guidelines/Analysers"/>.
    /// </summary>
    public class RequiredReferences : Task
    {
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

        /// <inheritdoc />
        public override bool Execute()
        {
            foreach (var r in Required)
            {
                if (Omitted.Any(x => x.ToString().Equals(r.ToString(), StringComparison.OrdinalIgnoreCase)))
                {
                    Log.LogMessage(MessageImportance.Low, $"Required reference '{r}' is set to omit.");
                    continue;
                }

                Log.LogMessage(MessageImportance.Low, $"Checking required reference: {r}");
                if (References.Any(x => x.ToString().Equals(r.ToString(), StringComparison.OrdinalIgnoreCase)))
                {
                    // found.
                    continue;
                }

                Log.LogWarning(
                    null,
                    "CCG0005",
                    string.Empty, // TODO: Can we get HelpLink like in roslyn analysers?
                    ProjectFile ?? string.Empty,
                    0,
                    0,
                    0,
                    0,
                    $"No reference to '{r}' found. Usage of '{r}' is strongly recommended");
            }

            return true;
        }
    }
}
