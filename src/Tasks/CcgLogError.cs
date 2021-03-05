using CakeContrib.Guidelines.Tasks.Extensions;

using JetBrains.Annotations;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

// ReSharper disable UnusedMember.Global
namespace CakeContrib.Guidelines.Tasks
{
    /// <summary>
    /// This is a convenience-Task to call CcgError from inside the msbuild.
    /// </summary>
    [UsedImplicitly]
    public class CcgLogError : Task
    {
        /// <summary>
        /// Gets or sets the CCG-Id.
        /// </summary>
        [Required]
        public int CcgId { get; set; }

        /// <summary>
        /// Gets or sets the project file.
        /// </summary>
        public string File { get; set; }

        /// <summary>
        /// Gets or sets the message text to be rendered.
        /// </summary>
        [Required]
        public string Text { get; set; }

        /// <inheritdoc />
        public override bool Execute()
        {
            Log.CcgError(
                CcgId,
                File,
                Text);

            return false;
        }
    }
}
