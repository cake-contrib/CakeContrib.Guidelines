using System;
using System.Linq;

using CakeContrib.Guidelines.Tasks.Extensions;
using CakeContrib.Guidelines.Tasks.Testability;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace CakeContrib.Guidelines.Tasks
{
    /// <summary>
    /// The Task to check for editorconfig for the guideline <see href="https://cake-contrib.github.io/CakeContrib.Guidelines/guidelines/Analysers"/>.
    /// </summary>
    public class RequiredFileEditorconfig : Task
    {
#if DEBUG
        private const MessageImportance LogLevel = MessageImportance.High;
#else
        private const MessageImportance LogLevel = MessageImportance.Low;
#endif
        private const string FileName = ".editorconfig";

        /// <summary>
        /// Initializes a new instance of the <see cref="RequiredFileEditorconfig"/> class.
        /// </summary>
        public RequiredFileEditorconfig()
        {
            FileSearcher = new FileSearcher();
        }

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

        /// <summary>
        /// Gets or sets the ProjectType.
        /// </summary>
        [Required]
        public string ProjectType { get; set; }

        /// <summary>
        /// Sets the FileSearcher. INTERNAL USE. replaced in unit-tests.
        /// </summary>
        internal IFileSearcher FileSearcher { private get; set; }

        /// <inheritdoc />
        public override bool Execute()
        {
            if (!CakeProjectType.IsOneOf(ProjectType, CakeProjectType.Addin, CakeProjectType.Module))
            {
                Log.LogMessage(
                    LogLevel,
                    $".editorconfig not required for {ProjectType} projects.");
                return true;
            }

            if (OmitFiles
                .Select(x => x.GetMetadata("Identity"))
                .Where(x => !string.IsNullOrEmpty(x))
                .Any(x => x.Equals(FileName, StringComparison.OrdinalIgnoreCase)))
            {
                Log.LogMessage(LogLevel, $"Recommended file '{FileName}' is set to omit.");
                return true;
            }

            if (FileSearcher.HasFileInFolderStructure(ProjectFile, FileName))
            {
                return true;
            }

            Log.CcgWarning(
                6,
                ProjectFile,
                $"No '{FileName}' found in folder-structure. Usage of '{FileName}' is strongly recommended.");
            return true;
        }
    }
}
