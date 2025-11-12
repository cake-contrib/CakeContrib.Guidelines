using System;
using System.Linq;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace CakeContrib.Guidelines.Tasks
{
    internal static class ITaskItemExtensions
    {
        internal static string GetVersion(
            this ITaskItem item,
            ITaskItem[] centralPackageManagementVersions,
            TaskLoggingHelper log)
        {
            var name = item.ToString();
            var version = item.GetMetadata("version");

            if (string.IsNullOrEmpty(version))
            {
                // check Central Package Management
                var cpmItem = centralPackageManagementVersions.FirstOrDefault(x => x.ToString().Equals(name, StringComparison.OrdinalIgnoreCase));
                if (cpmItem != null)
                {
                    version = cpmItem.GetMetadata("version");
                }

                // this IS a CPM reference, but now it could be overridden.
                var versionOverride = item.GetMetadata("VersionOverride");
                if (!string.IsNullOrEmpty(versionOverride))
                {
                    version = versionOverride;
                }
            }

            if (string.IsNullOrEmpty(version))
            {
                // we have a reference, without a "version" that is not a CPM reference.
                // set version to "null" as string, so we can generate a nicer warning.
                version = "null";
            }

            log.LogMessage(MessageImportance.Low, $"Version of {name}: {version}");
            return version;
        }
    }
}
