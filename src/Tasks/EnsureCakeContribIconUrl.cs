using CakeContrib.Guidelines.Tasks.Extensions;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace CakeContrib.Guidelines.Tasks
{
    /// <summary>
    /// The Task for the guideline <see href="https://cake-contrib.github.io/CakeContrib.Guidelines/guidelines/CakeContribIcon"/>.
    /// </summary>
    public class EnsureCakeContribIconUrl : Task
    {
#if DEBUG
        private const MessageImportance LogLevel = MessageImportance.High;
#else
        private const MessageImportance LogLevel = MessageImportance.Low;
#endif

        /// <summary>
        /// Gets or sets the PackageIconUrl.
        /// </summary>
        public string PackageIconUrl { get; set; }

        /// <summary>
        /// Gets the PackageIconUrl output.
        /// </summary>
        [Output]
        public string PackageIconUrlOutput { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether to omit importing an icon.
        /// </summary>
        [Required]
        public string CakeContribIconUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to omit importing an icon.
        /// </summary>
        public bool OmitIconImport { get; set; }

        /// <summary>
        /// Gets or sets the project file.
        /// </summary>
        [Required]
        public string ProjectFile { get; set; }

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
            PackageIconUrlOutput = string.Empty;
            if (!string.IsNullOrEmpty(PackageIconUrl))
            {
                return true;
            }

            if (OmitIconImport)
            {
                Log.CcgWarning(
                    2,
                    ProjectFile,
                    "PackageIconUrl is empty. For compatibility it should be set.",
                    NoWarn,
                    WarningsAsErrors);
                return true;
            }

            PackageIconUrlOutput = CakeContribIconUrl;
            Log.LogMessage(
                LogLevel,
                $"PackageIconUrl was not set. Setting it to {PackageIconUrlOutput}.");
            return true;
        }
    }
}
