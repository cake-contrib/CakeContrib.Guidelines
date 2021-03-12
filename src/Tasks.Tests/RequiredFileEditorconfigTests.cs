using System.Linq;

using CakeContrib.Guidelines.Tasks.Tests.Fixtures;

using FluentAssertions;

using Xunit;

namespace CakeContrib.Guidelines.Tasks.Tests
{
    public class RequiredFileEditorconfigTests
    {
        private readonly RequiredFileEditorconfigFixture fixture;

        public RequiredFileEditorconfigTests()
        {
            fixture = new RequiredFileEditorconfigFixture();
        }

        [Fact]
        public void RequiredEditorconfig_Should_Not_Warn_If_Editorconfig_exists()
        {
            // given
            fixture.WithExistingEditorconfig();

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.WarningEvents.Should().HaveCount(0);
        }

        [Fact]
        public void RequiredEditorconfig_Should_Warn_If_No_Editorconfig_exists()
        {
            // given
            fixture.WithNonExistingEditorconfig();

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.WarningEvents.Should().HaveCount(1);
            var theEvent = fixture.BuildEngine.WarningEvents.Single();
            theEvent.Code.Should().Be("CCG0006");
        }

        [Fact]
        public void RequiredEditorconfig_Should_Not_Warn_If_Editorconfig_Is_Omitted()
        {
            // given
            fixture.WithNonExistingEditorconfig();
            fixture.WithOmittedFile(".editorconfig");

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.WarningEvents.Should().HaveCount(0);
        }

        [Fact]
        public void RequiredEditorconfig_Should_Not_Warn_If_PojectType_Is_Recipe()
        {
            // given
            fixture.WithNonExistingEditorconfig();
            fixture.WithProjectTypeRecipe();

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.WarningEvents.Should().HaveCount(0);
        }

        [Fact]
        public void Should_Log_Correct_SourceFile_On_Warning_With_Given_ProjectFile()
        {
            // given
            const string projectFileName = "some.project.csproj";
            fixture.WithNonExistingEditorconfig();
            fixture.WithProjectFile(projectFileName);

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.WarningEvents
                .Should().HaveCount(1)
                .And.Contain(x => x.File == projectFileName);
        }
    }
}
