using System.Collections.Generic;
using System.Linq;

using Microsoft.Build.Framework;

namespace CakeContrib.Guidelines.Tasks.Tests.Fixtures
{
    public class CalculateProjectTypeFixture : BaseBuildFixture<CalculateProjectType>
    {
        private readonly List<ITaskItem> projectNames;
        private readonly List<ITaskItem> references;
        private string existingType;

        public CalculateProjectTypeFixture()
        {
            projectNames = new List<ITaskItem>();
            references = new List<ITaskItem>(new[]{ GetMockTaskItem("cake.core").Object });
        }

        public string Output { get; private set; }

        public override bool Execute()
        {
            Task.ProjectType = existingType;
            Task.ProjectNames = projectNames.ToArray();
            Task.References = references.ToArray();
            Task.CakeRequiredReference = new[] { GetMockTaskItem("cake.core").Object };
            var result = base.Execute();
            Output = Task.Output;
            return result;
        }

        public void WithExistinType(string type)
        {
            existingType = type;
        }

        public void WithProjectNames(params string[] names)
        {
            projectNames.AddRange(names.Select(n => GetMockTaskItem(n).Object));
        }

        public void WithoutCakeReference()
        {
            references.Clear();
        }

    }
}
