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

        public void WithTargetFramwork(string packageName)
        {
            targetFramework = GetMockTaskItem(packageName).Object;
        }

        public void WithTargetFramworks(params string[] packageNames)
        {
            targetFrameworks.AddRange(packageNames.Select(n => GetMockTaskItem(n).Object));
        }

        public void WithOmittedTargetFramework(string targetFramework)
        {
            omittedTargets.Add(GetMockTaskItem(targetFramework).Object);
        }

        public void WithCakeCoreReference(int major = 0, int minor = 0, int patch = 0)
        {
            var cakeRef = GetMockTaskItem("Cake.Core");
            cakeRef.Setup(x => x.GetMetadata("Version")).Returns($"{major}.{minor}.{patch}");
            references.Add(cakeRef.Object);
        }

        public void WithProjectFile(string fileName)
        {
            Task.ProjectFile = fileName;
        }
    }
}
