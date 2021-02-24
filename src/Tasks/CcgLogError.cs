using System.Diagnostics.CodeAnalysis;

using CakeContrib.Guidelines.Tasks.Extensions;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace CakeContrib.Guidelines.Tasks
{
    /// <summary>
    /// This is a convenience-Task to call CcgError from inside the msbuild.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Needed in Tasks")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "Needed in Tasks")]
    [SuppressMessage("ReSharper", "UnusedType.Global", Justification = "Used as Task")]
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
