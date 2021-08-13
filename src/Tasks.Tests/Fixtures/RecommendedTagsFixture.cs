using System.Collections.Generic;
using System.Linq;

using Microsoft.Build.Framework;

namespace CakeContrib.Guidelines.Tasks.Tests.Fixtures
{
    public class RecommendedTagsFixture : BaseBuildFixture<RecommendedTags>
    {
        private readonly List<ITaskItem> recommendedTags;
        private readonly List<ITaskItem> omittedTags;
        private readonly List<string> givenTags;

        public RecommendedTagsFixture()
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
