using System.Collections.Generic;

using Microsoft.Build.Framework;

using Moq;

namespace CakeContrib.Guidelines.Tasks.Tests.Fixtures
{
    public class CheckPrivateAssetsOnReferencesFixture : BaseBuildFixture<CheckPrivateAssetsOnReferences>
    {
        private readonly List<ITaskItem> packagesToCheck;
        private readonly List<ITaskItem> references;

        public CheckPrivateAssetsOnReferencesFixture()
        {
            references = new List<ITaskItem>();
            packagesToCheck = new List<ITaskItem>();
        }

        public override bool Execute()
        {
            Task.References = references.ToArray();
            Task.PackagesToCheck = packagesToCheck.ToArray();
            return base.Execute();
        }

        public void WithPackageToCheck(string packageName)
        {
            var packageToCheck = new Mock<ITaskItem>();
            packageToCheck.Setup(x => x.ToString()).Returns(packageName);
            packagesToCheck.Add(packageToCheck.Object);
        }

        public void WithProjectFile(string fileName)
        {
            Task.ProjectFile = fileName;
        }

        public void WithReferencedPackage(string packageName, string privateAssets = "")
        {
            var referencedPackage = new Mock<ITaskItem>();
            referencedPackage.Setup(x => x.ToString()).Returns(packageName);
            referencedPackage.Setup(x => x.GetMetadata("PrivateAssets")).Returns(privateAssets);
            references.Add(referencedPackage.Object);
        }
    }
}
