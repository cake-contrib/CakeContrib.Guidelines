using System.Collections.Generic;
using System.Linq;

using Microsoft.Build.Framework;

using Moq;

namespace CakeContrib.Guidelines.Tasks.Tests.Fixtures
{
    public class RequiredTagsFixture : BaseBuildFixture<RecommendedTags>
    {
        private readonly List<ITaskItem> recommendedTags;
        private readonly List<ITaskItem> omittedTags;
        private readonly List<string> givenTags;

        public RequiredTagsFixture()
        {
            recommendedTags = new List<ITaskItem>();
            omittedTags = new List<ITaskItem>();
            givenTags = new List<string>();
        }

        public override bool Execute()
        {
            Task.Omitted = omittedTags.ToArray();
            Task.Tags =  string.Join(";", givenTags);
            Task.CakeContribTags = recommendedTags.ToArray();
            return base.Execute();
        }

        private ITaskItem[] Convert(IEnumerable<string> tags)
        {
            return tags.Select(x => GetMockTaskItem(x).Object).ToArray();
        }

        public void WithOmittedTags(params string[] tags)
        {
            omittedTags.AddRange(Convert(tags));
        }

        public void WithGivenTags(params string[] tags)
        {
            givenTags.AddRange(tags);
        }

        public void WithRecommendedTags(params string[] tags)
        {
            recommendedTags.AddRange(Convert(tags));
        }

        public void WithProjectFile(string fileName)
        {
            Task.ProjectFile = fileName;
        }
    }
}
