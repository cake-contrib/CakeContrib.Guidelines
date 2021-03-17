using System;
using System.Collections.Generic;

using Microsoft.Build.Framework;

using Moq;

namespace CakeContrib.Guidelines.Tasks.Tests.Fixtures
{
    public class RequiredReferencesFixture : BaseBuildFixture<RequiredReferences>
    {
        private readonly List<ITaskItem> requiredReferences;
        private readonly List<ITaskItem> omittedReferences;
        private readonly List<ITaskItem> references;

        public RequiredReferencesFixture()
        {
            references = new List<ITaskItem>();
            requiredReferences = new List<ITaskItem>();
            omittedReferences = new List<ITaskItem>();
            Task.ProjectType = CakeProjectType.Addin.ToString();
        }

        public override bool Execute()
        {
            Task.References = references.ToArray();
            Task.Required = requiredReferences.ToArray();
            Task.Omitted = omittedReferences.ToArray();
            return base.Execute();
        }

        public void WithRequiredReferences(string packageName)
        {
            var reference = new Mock<ITaskItem>();
            reference.Setup(x => x.ToString()).Returns(packageName);
            requiredReferences.Add(reference.Object);
        }

        public void WithOmittedReferences(string packageName)
        {
            var reference = new Mock<ITaskItem>();
            reference.Setup(x => x.ToString()).Returns(packageName);
            omittedReferences.Add(reference.Object);
        }

        public void WithProjectFile(string fileName)
        {
            Task.ProjectFile = fileName;
        }

        public void WithReferencedPackage(string packageName, string privateAssets = "")
        {
            var referencedPackage = GetMockTaskItem(packageName);
            referencedPackage.Setup(x => x.GetMetadata(
                It.Is<string>(y => "PrivateAssets".Equals(y, StringComparison.OrdinalIgnoreCase))))
                .Returns(privateAssets);
            references.Add(referencedPackage.Object);
        }

        public void WithProjectTypeRecipe()
        {
            Task.ProjectType = "Recipe";
        }

        public void WithNoWarn(params string[] rules)
        {
            Task.NoWarn = rules;
        }

        public void WithWarningsAsErrors(params string[] rules)
        {
            Task.WarningsAsErrors = rules;
        }
    }
}
