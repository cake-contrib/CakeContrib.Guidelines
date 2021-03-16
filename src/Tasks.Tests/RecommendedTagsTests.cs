using System;

using CakeContrib.Guidelines.Tasks.Tests.Fixtures;

using FluentAssertions;

using Xunit;

namespace CakeContrib.Guidelines.Tasks.Tests
{
    public class RequiredTagTests
    {
        [Fact]
        public void Should_Not_Warn_If_RecommendedTags_Are_Used()
        {
            // given
            var fixture = new RequiredTagsFixture();
            fixture.WithRecommendedTags("cake", "cake-build");
            fixture.WithGivenTags("cake", "cake-build");

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.WarningEvents.Should().HaveCount(0);
        }

        [Fact]
        public void Should_Warn_If_RequiredTags_Are_Not_Used()
        {
            // given
            var fixture = new RequiredTagsFixture();
            fixture.WithRecommendedTags("cake", "cake-build");
            fixture.WithGivenTags("cake");

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.WarningEvents.Should().HaveCount(1);
            fixture.BuildEngine.WarningEvents.Should().Contain(x => x.Code == "CCG0008");
        }

        [Fact]
        public void Should_Not_Warn_If_RequiredTags_Are_Omitted()
        {
            // given
            var fixture = new RequiredTagsFixture();
            fixture.WithRecommendedTags("cake", "cake-build");
            fixture.WithOmittedTags("cake-build");
            fixture.WithGivenTags("cake");

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.WarningEvents.Should().HaveCount(0);
        }

        [Fact]
        public void Should_Log_Correct_WarningSourceFile_On_Warning()
        {
            // given
            const string projectFileName = "some.project.csproj";
            var fixture = new RequiredTagsFixture();
            fixture.WithProjectFile(projectFileName);
            fixture.WithRecommendedTags("not-used");

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.WarningEvents.Should().HaveCount(1);
            fixture.BuildEngine.WarningEvents.Should().Contain(x => x.File == projectFileName);
        }

        [Fact]
        public void Should_Warn_If_Tags_Contain_Comma()
        {
            // given
            var fixture = new RequiredTagsFixture();
            fixture.WithGivenTags("one, two, three");

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.WarningEvents.Should().HaveCount(1)
                .And.Contain(x => x.Code == "CCG0008")
                .And.Contain(x => x.Message.IndexOf("comma", StringComparison.OrdinalIgnoreCase) > -1);
        }

    }
}
