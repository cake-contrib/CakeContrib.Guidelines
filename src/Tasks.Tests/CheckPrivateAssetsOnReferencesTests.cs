using System.Linq;

using CakeContrib.Guidelines.Tasks.Tests.Fixtures;

using FluentAssertions;

using Xunit;

namespace CakeContrib.Guidelines.Tasks.Tests
{
    public class CheckPrivateAssetsOnReferencesTests
    {
        [Fact]
        public void Should_Not_Error_If_Package_Is_Unknown()
        {
            // given
            var fixture = new CheckPrivateAssetsOnReferencesFixture();
            fixture.WithReferencedPackage("Cool.Ref.Project");
            fixture.WithPackageToCheck("Some.Other.Project");

            // when
            var actual = fixture.Execute();

            // then
            actual.Should().BeTrue();
        }

        [Fact]
        public void Should_Not_Error_If_Package_Has_PrivateAssets()
        {
            // given
            var fixture = new CheckPrivateAssetsOnReferencesFixture();
            fixture.WithReferencedPackage("Cake.Core", "all");
            fixture.WithPackageToCheck("Cake.Core");

            // when
            var actual = fixture.Execute();

            // then
            actual.Should().BeTrue();
        }

        [Fact]
        public void Should_Error_If_Package_Has_Not_PrivateAssets()
        {
            // given
            var fixture = new CheckPrivateAssetsOnReferencesFixture();
            fixture.WithReferencedPackage("Cake.Core", "");
            fixture.WithPackageToCheck("Cake.Core");

            // when
            var actual = fixture.Execute();

            // then
            actual.Should().BeFalse();
        }

        [Fact]
        public void Should_Log_Correct_ErrorCode_On_Error()
        {
            // given
            var fixture = new CheckPrivateAssetsOnReferencesFixture();
            fixture.WithReferencedPackage("Cake.Core", "");
            fixture.WithPackageToCheck("Cake.Core");

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.ErrorEvents.Should().HaveCount(1);
            var theEvent = fixture.BuildEngine.ErrorEvents.Single();
            theEvent.Code.Should().Be("CCG0004");
        }

        [Fact]
        public void Should_Log_Correct_ErrorSourceFile_On_Error_With_Given_ProjectFile()
        {
            // given
            const string projectFileName = "some.project.csproj";
            var fixture = new CheckPrivateAssetsOnReferencesFixture();
            fixture.WithReferencedPackage("Cake.Core", "");
            fixture.WithPackageToCheck("Cake.Core");
            fixture.WithProjectFile(projectFileName);

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.ErrorEvents.Should().HaveCount(1);
            var theEvent = fixture.BuildEngine.ErrorEvents.Single();
            theEvent.File.Should().Be(projectFileName);
        }
    }
}
