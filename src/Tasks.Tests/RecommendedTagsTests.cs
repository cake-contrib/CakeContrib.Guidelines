using System;

using CakeContrib.Guidelines.Tasks.Tests.Fixtures;

using Shouldly;

using Xunit;

namespace CakeContrib.Guidelines.Tasks.Tests
{
    public class RequiredTagTests
    {
        [Fact]
        public void Should_Not_Warn_If_RecommendedTags_Are_Used()
        {
            // given
            var fixture = new RecommendedTagsFixture();
            fixture.WithRecommendedTags("cake", "cake-build");
            fixture.WithGivenTags("cake", "cake-build");

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.WarningEvents.Count.ShouldBe(0);
        }

        [Fact]
        public void Should_Warn_If_RequiredTags_Are_Not_Used()
        {
            // given
            var fixture = new RecommendedTagsFixture();
            fixture.WithRecommendedTags("cake", "cake-build");
            fixture.WithGivenTags("cake");

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.WarningEvents.Count.ShouldBe(1);
            fixture.BuildEngine.WarningEvents.ShouldContain(x => x.Code == "CCG0008");
        }

        [Fact]
        public void Should_Not_Warn_If_RequiredTags_Are_Omitted()
        {
            // given
            var fixture = new RecommendedTagsFixture();
            fixture.WithRecommendedTags("cake", "cake-build");
            fixture.WithOmittedTags("cake-build");
            fixture.WithGivenTags("cake");

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.WarningEvents.Count.ShouldBe(0);
        }

        [Fact]
        public void Should_Log_Correct_WarningSourceFile_On_Warning()
        {
            // given
            const string projectFileName = "some.project.csproj";
            var fixture = new RecommendedTagsFixture();
            fixture.WithProjectFile(projectFileName);
            fixture.WithRecommendedTags("not-used");

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.WarningEvents.Count.ShouldBe(1);
            fixture.BuildEngine.WarningEvents.ShouldContain(x => x.File == projectFileName);
        }

        [Fact]
        public void Should_Warn_If_Tags_Contain_Comma()
        {
            // given
            var fixture = new RecommendedTagsFixture();
            fixture.WithGivenTags("one, two, three");

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.WarningEvents.Count.ShouldBe(1);
            fixture.BuildEngine.WarningEvents.ShouldContain(x => x.Code == "CCG0008");
            fixture.BuildEngine.WarningEvents.ShouldContain(x =>
                x.Message.IndexOf("comma", StringComparison.OrdinalIgnoreCase) > -1);
        }

        [Fact]
        public void Should_Not_Log_If_NoWarn_Is_Set()
        {
            // given
            var fixture = new RecommendedTagsFixture();
            fixture.WithRecommendedTags("cake");
            fixture.WithGivenTags("");
            fixture.WithNoWarn("ccg0008");

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.WarningEvents.Count.ShouldBe(0);
        }

        [Fact]
        public void Should_Log_Error_If_WarnAsError_Is_Set()
        {
            // given
            var fixture = new RecommendedTagsFixture();
            fixture.WithRecommendedTags("cake");
            fixture.WithGivenTags("");
            fixture.WithWarningsAsErrors("ccg0008");

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.ErrorEvents.Count.ShouldBe(1);
        }
    }
}
