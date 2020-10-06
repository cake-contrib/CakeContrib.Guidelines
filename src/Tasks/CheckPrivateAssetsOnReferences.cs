using System;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace CakeContrib.Guidelines.Tasks
{
    /// <summary>
    /// The Task for the guideline <see href="https://cake-contrib.github.io/CakeContrib.Guidelines/guidelines/PrivateAssets"/>.
    /// </summary>
    public class CheckPrivateAssetsOnReferences : Task
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
        public ITaskItem[] PackagesToCheck { get; set; }

        /// <summary>
        /// Gets or sets the project file.
        /// </summary>
        public string ProjectFile { get; set; }

        /// <inheritdoc />
        public override bool Execute()
        {
            foreach (var r in References)
            {
                var privateAssets = r.GetMetadata("PrivateAssets");
                foreach (var pack in PackagesToCheck)
                {
                    if (!pack.ToString().Equals(r.ToString(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        continue;
                    }

                    Log.LogMessage(MessageImportance.Low, $"Package {r} for PrivateAssets: {privateAssets}");

                    if (!privateAssets.Equals("all", StringComparison.InvariantCultureIgnoreCase))
                    {
                        Log.LogError(
                            null,
                            "CCG0004",
                            string.Empty, // TODO: Can we get HelpLink like in roslyn analysers?
                            ProjectFile ?? string.Empty,
                            0,
                            0,
                            0,
                            0,
                            $"{pack} should have PrivateAssets=\"All\"");
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
