using System.Collections.Generic;

using CakeContrib.Guidelines.Tasks.Testability;

using Microsoft.Build.Framework;

using Moq;

namespace CakeContrib.Guidelines.Tasks.Tests.Fixtures
{
    public class RequiredFileEditorconfigFixture : BaseBuildFixture<RequiredFileEditorconfig>
    {
        private readonly List<ITaskItem> omittedFiles;
        private readonly Mock<IFileSearcher> fileSearcher;

        public RequiredFileEditorconfigFixture()
        {
            omittedFiles = new List<ITaskItem>();
            fileSearcher = new Mock<IFileSearcher>();
            Task.FileSearcher = fileSearcher.Object;
        }

        public void WithExistingEditorconfig()
        {
            fileSearcher.Setup(x => x.HasFileInFolderStructure(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
        }

        public void WithNonExistingEditorconfig()
        {
            fileSearcher.Setup(x => x.HasFileInFolderStructure(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
        }

        public override bool Execute()
        {
            Task.OmitFiles = omittedFiles.ToArray();
            return base.Execute();
        }

        public void WithOmittedFile(string id)
        {
            omittedFiles.Add(GetMockTaskItem(id).Object);
        }

        public void WithProjectFile(string fileName)
        {
            Task.ProjectFile = fileName;
        }
    }
}
