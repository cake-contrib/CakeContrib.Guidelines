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
    public class RecommendedTags : Task
    {
#if DEBUG
        private const MessageImportance LogLevel = MessageImportance.High;
#else
        private const MessageImportance LogLevel = MessageImportance.Low;
#endif

        /// <summary>
        /// Gets or sets the Recommended Tags.
        /// </summary>
        [Required]
        public ITaskItem[] CakeContribTags { get; set; }

        /// <summary>
        /// Gets or sets the Tags.
        /// </summary>
        [Required]
        public string Tags { get; set; }

        /// <summary>
        /// Gets or sets the tags to omit.
        /// </summary>
        [Required]
        public ITaskItem[] Omitted { get; set; }

        /// <summary>
        /// Gets or sets the project file.
        /// </summary>
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
            if (Tags.IndexOf(",", StringComparison.Ordinal) > -1)
            {
                Log.CcgWarning(
                    8,
                    ProjectFile,
                    $"Unsupported delimiter in PackageTags: comma.",
                    NoWarn,
                    WarningsAsErrors);
            }

            var tagsSetInProj = Tags.Split(new[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var r in CakeContribTags)
            {
                var recommended = r.ToString();
                if (Omitted.Any(x => x.ToString().Equals(recommended, StringComparison.OrdinalIgnoreCase)))
                {
                    Log.LogMessage(LogLevel, $"Recommended tag '{recommended}' is set to omit.");
                    continue;
                }

                Log.LogMessage(LogLevel, $"Checking recommended tag: {recommended}");

                if (tagsSetInProj.Any(x => x.ToString().Equals(recommended, StringComparison.OrdinalIgnoreCase)))
                {
                    // found.
                    continue;
                }

                Log.CcgWarning(
                    8,
                    ProjectFile,
                    $"Usage of tag '{recommended}' is recommended.",
                    NoWarn,
                    WarningsAsErrors);
            }

            return true;
        }
    }
}
