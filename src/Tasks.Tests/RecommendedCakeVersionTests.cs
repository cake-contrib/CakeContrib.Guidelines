using System;

using CakeContrib.Guidelines.Tasks.Tests.Fixtures;

using FluentAssertions;

using Xunit;

namespace CakeContrib.Guidelines.Tasks.Tests
{
    public class RequiredCakeVersionTests
    {
        [Fact]
        public void Should_Warn_If_One_Version_Is_Not_Correct()
        {
            // given
            var fixture = new RecommendedCakeVersionFixture();
            fixture.WithReference("cake.core", "0.38.5");
            fixture.WithReference("cake.common", "1.0.0");
            fixture.WithReferencesToCheck("cake.core", "cake.common");
            fixture.WithRecommendedVersion("1.0.0");

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.WarningEvents.Should()
                .HaveCount(1)
                .And
                .Contain(x => x.Code == "CCG0009")
                .And
                .Contain(x => x.Message.IndexOf("cake.core", StringComparison.InvariantCulture) > -1);
        }

        [Fact]
        public void Should_Not_Warn_If_Incorrect_Reference_Is_Omitted()
        {
            // given
            var fixture = new RecommendedCakeVersionFixture();
            fixture.WithReference("cake.core", "0.38.5");
            fixture.WithReference("cake.common", "1.0.0");
            fixture.WithReferencesToCheck("cake.core", "cake.common");
            fixture.WithOmittedReferences("cake.core");
            fixture.WithRecommendedVersion("1.0.0");

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.WarningEvents.Should()
                .HaveCount(0);
        }

        [Fact]
        public void Should_Not_Warn_For_None_Cake_Versions()
        {
            // given
            var fixture = new RecommendedCakeVersionFixture();
            fixture.WithReference("some.lib", "0.0.1");
            fixture.WithReferencesToCheck("cake.core", "cake.common");
            fixture.WithRecommendedVersion("1.0.0");

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.WarningEvents.Should()
                .HaveCount(0);
        }


        [Fact]
        public void Should_Log_Correct_WarningSourceFile_On_Warning()
        {
            // given
            const string projectFileName = "some.project.csproj";
            var fixture = new RecommendedCakeVersionFixture();
            fixture.WithReference("cake.core", "0.38.5");
            fixture.WithReferencesToCheck("cake.core");
            fixture.WithRecommendedVersion("1.0.0");
            fixture.WithProjectFile(projectFileName);

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.WarningEvents.Should()
                .HaveCount(1)
                .And
                .Contain(x => x.File == projectFileName);
        }

        [Fact]
        public void Should_Not_Warn_For_None_Modules_Or_Addins()
        {
            // given
            var fixture = new RecommendedCakeVersionFixture();
            fixture.WithReference("cake.core", "0.38.5");
            fixture.WithReferencesToCheck("cake.core");
            fixture.WithRecommendedVersion("1.0.0");
            fixture.WithProjectTypeRecipe();

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.WarningEvents.Should().HaveCount(0);
        }
    }
}
