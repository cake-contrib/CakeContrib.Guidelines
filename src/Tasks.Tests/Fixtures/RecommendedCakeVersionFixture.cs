using System.Collections.Generic;
using System.Linq;

using Microsoft.Build.Framework;

namespace CakeContrib.Guidelines.Tasks.Tests.Fixtures
{
    public class RecommendedCakeVersionFixture : BaseBuildFixture<RecommendedCakeVersion>
    {
        private readonly List<ITaskItem> references;
        private readonly List<ITaskItem> omitted;
        private readonly List<ITaskItem> referencesToCheck;

        public RecommendedCakeVersionFixture()
        {
            references = new List<ITaskItem>();
            omitted = new List<ITaskItem>();
            referencesToCheck = new List<ITaskItem>();
            Task.ProjectType = CakeProjectType.Addin.ToString();
        }

        public override bool Execute()
        {
            Task.Omitted = omitted.ToArray();
            Task.References = references.ToArray();
            Task.ReferencesToCheck = referencesToCheck.ToArray();
            return base.Execute();
        }

        private ITaskItem[] Convert(IEnumerable<string> tags)
        {
            return tags.Select(x => GetMockTaskItem(x).Object).ToArray();
        }

        public void WithOmittedReferences(params string[] referenceNames)
        {
            omitted.AddRange(Convert(referenceNames));
        }

        public void WithReference(string referenceName, string version)
        {
            var reference = GetMockTaskItem(
                referenceName,
                new Dictionary<string, string>
                {
                    { "version", version }
                });
            references.Add(reference.Object);
        }

        public void WithReferencesToCheck(params string[] referenceNames)
        {
            referencesToCheck.AddRange(Convert(referenceNames));
        }

        public void WithRecommendedVersion(string version)
        {
            Task.RecommendedVersion = version;
        }

        public void WithProjectFile(string fileName)
        {
            Task.ProjectFile = fileName;
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
