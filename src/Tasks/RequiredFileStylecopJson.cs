using System;
using System.IO;
using System.Linq;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace CakeContrib.Guidelines.Tasks
{
    /// <summary>
    /// The Task to check for stylecop.json for the guideline <see href="https://cake-contrib.github.io/CakeContrib.Guidelines/guidelines/Analysers"/>.
    /// </summary>
    public class RequiredFileStylecopJson : Task
    {
        private const string SettingsFileName = "stylecop.json";
        private const string AltSettingsFileName = ".stylecop.json";

        /// <summary>
        /// Gets or sets the project file.
        /// </summary>
        public string ProjectFile { get; set; }

        /// <summary>
        /// Gets or sets the AdditionalFiles to check.
        /// </summary>
        [Required]
        public ITaskItem[] AdditionalFiles { get; set; }

        /// <summary>
        /// Gets or sets the files to omit.
        /// </summary>
        [Required]
        public ITaskItem[] OmitFiles { get; set; }

        /// <inheritdoc />
        public override bool Execute()
        {
            if (OmitFiles
                .Select(x => x.GetMetadata("Identity"))
                .Where(x => !string.IsNullOrEmpty(x))
                .Any(x => x.Equals(SettingsFileName, StringComparison.OrdinalIgnoreCase) ||
                    x.Equals(AltSettingsFileName, StringComparison.OrdinalIgnoreCase)))
            {
                Log.LogMessage(MessageImportance.Low, $"Recommended file '{SettingsFileName}' is set to omit.");
                return true;
            }

            foreach (var file in AdditionalFiles)
            {
                var fullPath = file.GetMetadata("FullPath");
                if (string.IsNullOrEmpty(fullPath))
                {
                    continue;
                }

                var refFileName = Path.GetFileName(fullPath);
                if (refFileName.Equals(SettingsFileName, StringComparison.OrdinalIgnoreCase) ||
                    refFileName.Equals(AltSettingsFileName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            Log.LogWarning(
                null,
                "CCG0006",
                string.Empty, // TODO: Can we get HelpLink like in roslyn analysers?
                ProjectFile ?? string.Empty,
                0,
                0,
                0,
                0,
                $"No reference to '{SettingsFileName}' found. Usage of '{SettingsFileName}' is strongly recommended.");

            return true;
        }
    }
}
