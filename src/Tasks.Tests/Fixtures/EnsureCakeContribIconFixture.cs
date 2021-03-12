using System.Collections.Generic;

using CakeContrib.Guidelines.Tasks.Testability;

using Microsoft.Build.Framework;

using Moq;

namespace CakeContrib.Guidelines.Tasks.Tests.Fixtures
{
    public class EnsureCakeContribIconFixture : BaseBuildFixture<EnsureCakeContribIcon>
    {
        private readonly Mock<IFileFacade> fileFacadeMock;
        private readonly List<ITaskItem> noneReferences;
        private readonly string cakeContribDefaultIcon;

        public EnsureCakeContribIconFixture()
        {
            fileFacadeMock = new Mock<IFileFacade>();
            noneReferences = new List<ITaskItem>();
            cakeContribDefaultIcon = "C:\\users\\someUser\\.dotnet\\whatever\\CakeContrib-Guidelines\\data\\logo.png";
        }

        public string PackageIconOutput { get; private set; }
        public ITaskItem AdditionalNoneRefOutput { get; private set; }

        public override bool Execute()
        {
            Task.FileFacade = fileFacadeMock.Object;
            Task.NoneReferences = noneReferences.ToArray();
            Task.CakeContribIconPath = cakeContribDefaultIcon;
            var result = base.Execute();

            PackageIconOutput = Task.PackageIconOutput;
            AdditionalNoneRefOutput = Task.AdditionalNoneRefOutput;
            return result;
        }

        public void WithPackageIcon(string packageIcon)
        {
            Task.PackageIcon = packageIcon;
        }

        public void WithIconFileReference(string imagesLogoPng, string packagePath, bool doPack = true)
        {
            var mock = GetMockTaskItem(imagesLogoPng);
            mock.Setup(x => x.GetMetadata("PackagePath")).Returns(packagePath);
            if (doPack)
            {
                mock.Setup(x => x.GetMetadata("Pack")).Returns("True");
            }

            noneReferences.Add(mock.Object);
        }

        public void WithCompareIconResult(bool result)
        {
            fileFacadeMock
                .Setup(x => x.AreFilesSame(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(result);
        }

        public void AssertCopyWasCalled(int times)
        {
            fileFacadeMock
                .Verify(x =>
                    x.Copy(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(times));
        }

        public void WithOmitIconImport()
        {
            Task.OmitIconImport = true;
        }

        public void WithProjectFile(string fileName)
        {
            Task.ProjectFile = fileName;
        }
    }
}
