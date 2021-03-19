using System;
using System.IO;
using System.Linq;

using CakeContrib.Guidelines.Tasks.Extensions;
using CakeContrib.Guidelines.Tasks.Testability;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace CakeContrib.Guidelines.Tasks
{
    /// <summary>
    /// The Task for the guideline <see href="https://cake-contrib.github.io/CakeContrib.Guidelines/guidelines/CakeContribIcon"/>.
    /// </summary>
    public class EnsureCakeContribIcon : Task
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnsureCakeContribIcon"/> class.
        /// </summary>
        public EnsureCakeContribIcon()
        {
            FileFacade = new FileFacade();
        }

        /// <summary>
        /// Gets or sets the PackageIcon.
        /// </summary>
        public string PackageIcon { get; set; }

        /// <summary>
        /// Gets the PackageIcon output.
        /// </summary>
        [Output]
        public string PackageIconOutput { get; private set; }

        /// <summary>
        /// Gets or sets the None-References.
        /// </summary>
        [Required]
        public ITaskItem[] NoneReferences { get; set; }

        /// <summary>
        /// Gets the AdditionalNoneRefOutput output.
        /// </summary>
        [Output]
        public ITaskItem AdditionalNoneRefOutput { get; private set; }

        /// <summary>
        /// Gets or sets the path to the default CakeContrib icon.
        /// </summary>
        [Required]
        public string CakeContribIconPath { get; set; }

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

        /// <summary>
        /// Sets the FileComparer. INTERNAL USE. replaced in unit-tests.
        /// </summary>
        internal IFileFacade FileFacade { private get; set; }

        /// <inheritdoc />
        public override bool Execute()
        {
            // check Icon in package
            if (!string.IsNullOrEmpty(PackageIcon))
            {
                PackageIconOutput = PackageIcon;
            }
            else
            {
                if (OmitIconImport)
                {
                    Log.CcgError(
                        1,
                        ProjectFile,
                        "PackageIcon is empty.");
                    return false;
                }

                PackageIconOutput = Path.GetFileName(CakeContribIconPath.NormalizePathSeparators());
                Log.CcgTrace($"PackageIcon was not set. Setting it to {PackageIconOutput}.");
            }

            PackageIconOutput = PackageIconOutput.NormalizePathSeparators();
            var packageIconOutputPath =
                (Path.GetDirectoryName(PackageIconOutput) ?? string.Empty).NormalizePathSeparators();

            // find icon in FileSystem from References
            var mappedReferences = NoneReferences.Select(MapToDataStructure).ToList();
            var iconRef = mappedReferences
                .FirstOrDefault(x =>
                    x.Pack && (
                    x.DestinationInPackage.Equals(PackageIconOutput, StringComparison.OrdinalIgnoreCase)
                    || x.DestinationInPackage.Equals(packageIconOutputPath, StringComparison.OrdinalIgnoreCase)));

            var srcExtension = Path.GetExtension(CakeContribIconPath);
            if (iconRef == null)
            {
                if (OmitIconImport)
                {
                    // we found no Icon that matches the PackageIcon and
                    // OmitIconImport is set, so we can't add one.
                    // DO NOT add an error/warning here, MSBuild will
                    // error out anyway if the required icon is not there.
                    return true;
                }

                var packageIconExtension = Path.GetExtension(PackageIconOutput);
                if (!packageIconExtension.Equals(srcExtension, StringComparison.OrdinalIgnoreCase))
                {
                    Log.CcgError(
                        3,
                        ProjectFile,
                        $"The PackageIcon source ({PackageIconOutput}) has an extension of {packageIconExtension}. It can not be set from the CakeContrib-Icon.");
                    return false;
                }

                AdditionalNoneRefOutput = new TaskItem(CakeContribIconPath);
                AdditionalNoneRefOutput.SetMetadata("Link", Path.GetFileName(CakeContribIconPath));
                AdditionalNoneRefOutput.SetMetadata("Pack", "True");
                AdditionalNoneRefOutput.SetMetadata("PackagePath", PackageIconOutput);

                Log.CcgTrace($"PackageIcon ({PackageIconOutput}) was not referenced in Project. Referencing {CakeContribIconPath}.");

                return true;
            }

            // check file format
            var dstExtension = Path.GetExtension(iconRef.SourceInFileSystem);
            Log.CcgTrace($"Comparing compatibility of {iconRef.SourceInFileSystem} ({dstExtension}) with {CakeContribIconPath} ({srcExtension}) .");
            if (!dstExtension.Equals(srcExtension, StringComparison.OrdinalIgnoreCase))
            {
                Log.CcgWarning(
                    3,
                    ProjectFile,
                    $"The PackageIcon source ({iconRef.SourceInFileSystem}) has an extension of {dstExtension}. It can not be updated from the CakeContrib-Icon.",
                    NoWarn,
                    WarningsAsErrors);
                return true;
            }

            // if a file exists override, if different.
            if (FileFacade.AreFilesSame(iconRef.SourceInFileSystem, CakeContribIconPath))
            {
                Log.CcgTrace($"Package icon at {iconRef.SourceInFileSystem} is already up-to-date.");
                return true;
            }

            if (OmitIconImport)
            {
                Log.CcgWarning(
                    3,
                    ProjectFile,
                    $"The PackageIcon source ({iconRef.SourceInFileSystem}) is outdated and should be replaced.",
                    NoWarn,
                    WarningsAsErrors);
                return true;
            }

            Log.CcgTrace($"Copying CakeContrib-Icon to {iconRef.SourceInFileSystem}.");
            FileFacade.Copy(CakeContribIconPath, iconRef.SourceInFileSystem);

            return true;
        }

        private PackDataStructure MapToDataStructure(ITaskItem item)
        {
            var pack = item.GetMetadata("Pack");
            var data = new PackDataStructure
            {
                SourceInFileSystem = item.ToString().NormalizePathSeparators(),
                DestinationInPackage = item.GetMetadata("PackagePath") ?? string.Empty,
                Pack = !string.IsNullOrEmpty(pack) && Convert.ToBoolean(pack),
            };

            if (!data.Pack)
            {
                return data;
            }

            data.DestinationInPackage = data.DestinationInPackage.NormalizePathSeparators();
            var fileName = Path.GetFileName(data.SourceInFileSystem);
            var fileExtension = Path.GetExtension(fileName);
            if (!data.DestinationInPackage.EndsWith(fileExtension, StringComparison.OrdinalIgnoreCase))
            {
                data.DestinationInPackage =
                    string.IsNullOrEmpty(data.DestinationInPackage)
                        ? fileName
                        : Path.Combine(data.DestinationInPackage, fileName);
            }

            return data;
        }

        private class PackDataStructure
        {
            public string SourceInFileSystem { get; set; }

            public string DestinationInPackage { get; set; }

            public bool Pack { get; set; }
        }
    }
}
