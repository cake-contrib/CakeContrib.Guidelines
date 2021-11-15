using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Build.Framework;

using Moq;

namespace CakeContrib.Guidelines.Tasks.Tests.Fixtures
{
    public class TargetFrameworkVersionsFixture : BaseBuildFixture<TargetFrameworkVersions>
    {
        private readonly List<ITaskItem> references;
        private readonly List<ITaskItem> targetFrameworks;
        private ITaskItem targetFramework;
        private readonly List<ITaskItem> omittedTargets;

        public TargetFrameworkVersionsFixture()
        {
            Task.ProjectType = CakeProjectType.Addin.ToString();
            references = new List<ITaskItem>();
            omittedTargets = new List<ITaskItem>();
            targetFrameworks = new List<ITaskItem>();
            targetFramework = null;
        }

        public override bool Execute()
        {
            Task.References = references.ToArray();
            Task.TargetFramework = targetFramework;
            Task.TargetFrameworks = targetFrameworks.ToArray();
            Task.Omitted = omittedTargets.ToArray();
            return base.Execute();
        }

        public void WithTargetFramework(string packageName)
        {
            targetFramework = GetMockTaskItem(packageName).Object;
        }

        public void WithTargetFrameworksMatchingDefault()
        {
            targetFrameworks.Clear();
            WithTargetFrameworks("netcoreapp3.1", "net5.0", "net6.0"); // matches the "Default" of CakeContrib.Guidelines.Tasks.TargetFrameworkVersions
        }

        public void WithTargetFrameworks(params string[] packageNames)
        {
            targetFrameworks.AddRange(packageNames.Select(n => GetMockTaskItem(n).Object));
        }

        public void WithOmittedTargetFramework(string targetFrameworkToOmit)
        {
            omittedTargets.Add(GetMockTaskItem(targetFrameworkToOmit).Object);
        }

        public void WithCakeCoreReference(int major = 0, int minor = 0, int patch = 0)
        {
            WithCakeCoreReference($"{major}.{minor}.{patch}");
        }

        public void WithCakeCoreReference(string reference)
        {
            var cakeRef = GetMockTaskItem("Cake.Core");
            cakeRef.Setup(x => x.GetMetadata(
                    It.Is<string>(y => "version".Equals(y, StringComparison.OrdinalIgnoreCase))))
                .Returns(reference);
            references.Add(cakeRef.Object);
        }

        public void WithProjectType(string projectType)
        {
            Task.ProjectType = projectType;
        }

        public void WithProjectFile(string fileName)
        {
            Task.ProjectFile = fileName;
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
