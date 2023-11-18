using System.Collections.Generic;

using Microsoft.Build.Framework;

namespace CakeContrib.Guidelines.Tasks.Tests.Fixtures
{
    public class CheckCakeInternalReferencesFixture : BaseBuildFixture<CheckCakeInternalReferences>
    {
        private readonly List<ITaskItem> references;

        public CheckCakeInternalReferencesFixture()
        {
            Task.ProjectType = CakeProjectType.Addin.ToString();
            Task.CakeVersion = "3.0.0";
            Task.ProjectFile = "some.project.csproj";
            references = new List<ITaskItem>();
        }

        public void WithNoWarn(params string[] rules)
        {
            Task.NoWarn = rules;
        }

        public void WithWarningsAsErrors(params string[] rules)
        {
            Task.WarningsAsErrors = rules;
        }

        public void WithReference(string referenceName, string version, string privateAssets = "all")
        {
            var metadata = new Dictionary<string, string>
            {
                { "version", version },
                { "privateAssets", privateAssets },
            };

            var reference = GetMockTaskItem(referenceName, metadata);
            references.Add(reference.Object);
        }

        public override bool Execute()
        {
            Task.References = references.ToArray();
            return base.Execute();
        }

        public void WithProjectType(CakeProjectType type)
        {
            Task.ProjectType = type.ToString();
        }

        public void WithCakeVersion(string version)
        {
            Task.CakeVersion = version;
        }
    }
}
