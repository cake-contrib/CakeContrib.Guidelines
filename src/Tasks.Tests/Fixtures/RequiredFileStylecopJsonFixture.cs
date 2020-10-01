using System.Collections.Generic;

using Microsoft.Build.Framework;

using Moq;

namespace CakeContrib.Guidelines.Tasks.Tests.Fixtures
{
    public class RequiredFileStylecopJsonFixture : BaseBuildFixture<RequiredFileStylecopJson>
    {
        private readonly List<ITaskItem> additionalFiles;
        private readonly List<ITaskItem> omittedFiles;

        public RequiredFileStylecopJsonFixture()
        {
            additionalFiles = new List<ITaskItem>();
            omittedFiles = new List<ITaskItem>();
        }

        public override bool Execute()
        {
            Task.AdditionalFiles = additionalFiles.ToArray();
            Task.OmitFiles = omittedFiles.ToArray();
            return base.Execute();
        }

        public void WithAdditionalFile(string filePath)
        {
            var item = GetMockTaskItem(filePath);
            item.Setup(x => x.GetMetadata("FullPath")).Returns(filePath);
            additionalFiles.Add(item.Object);
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
