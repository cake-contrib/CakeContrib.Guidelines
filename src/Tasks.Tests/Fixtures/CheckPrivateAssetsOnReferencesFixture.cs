using System.Collections.Generic;

using Microsoft.Build.Framework;

using Moq;

namespace CakeContrib.Guidelines.Tasks.Tests.Fixtures
{
    public class CheckPrivateAssetsOnReferencesFixture
    {
        public MockBuildEngine BuildEngine { get; }

        private readonly CheckPrivateAssetsOnReferences task;
        private readonly List<ITaskItem> references;
        private readonly List<ITaskItem> packagesToCheck;

        public CheckPrivateAssetsOnReferencesFixture()
        {
            BuildEngine = new MockBuildEngine();
            task = new CheckPrivateAssetsOnReferences
            {
                BuildEngine = BuildEngine
            };
            packagesToCheck = new List<ITaskItem>();
            references = new List<ITaskItem>();
        }

        public bool Execute()
        {
            task.References = references.ToArray();
            task.PackagesToCheck = packagesToCheck.ToArray();
            return task.Execute();
        }

        public void WithProjectFile(string fileName)
        {
            task.ProjectFile = fileName;
        }

        public void WithPackageToCheck(string packageName)
        {
            var packageToCheck = new Mock<ITaskItem>();
            packageToCheck.Setup(x => x.ToString()).Returns(packageName);
            packagesToCheck.Add(packageToCheck.Object);
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
