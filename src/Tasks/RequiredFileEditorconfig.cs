using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace CakeContrib.Guidelines.Tasks
{
    /// <summary>
    /// The Task to check for editorconfig for the guideline <see href="https://cake-contrib.github.io/CakeContrib.Guidelines/guidelines/Analysers"/>.
    /// </summary>
    public class RequiredFileEditorconfig : Task
    {
        private const string FileName = ".editorconfig";

        /// <summary>
        /// Gets or sets the project file.
        /// </summary>
        [Required]
        public string ProjectFile { get; set; }

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
                .Any(x => x.Equals(FileName, StringComparison.OrdinalIgnoreCase)))
            {
                Log.LogMessage(MessageImportance.Low, $"Recommended file '{FileName}' is set to omit.");
                return true;
            }

            var folder = Path.GetDirectoryName(ProjectFile);
            var found = false;
            while (folder != null && !found)
            {
                found = File.Exists(Path.Combine(folder, FileName));
                if (found)
                {
                    return true;
                }

                folder = Path.GetDirectoryName(folder);
            }

            Log.LogWarning(
                null,
                "CCG0006",
                string.Empty, // TODO: Can we get HelpLink like in roslyn analysers?
                ProjectFile,
                0,
                0,
                0,
                0,
                $"No '{FileName}' found in folder-structure. Usage of '{FileName}' is strongly recommended.");
            return true;
        }
    }
}
