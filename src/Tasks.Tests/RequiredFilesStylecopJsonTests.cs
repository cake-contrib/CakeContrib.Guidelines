using System.Linq;

using CakeContrib.Guidelines.Tasks.Tests.Fixtures;

using FluentAssertions;

using Xunit;

namespace CakeContrib.Guidelines.Tasks.Tests
{
    public class RequiredFileStyleCopTests
    {
        private readonly RequiredFileStylecopJsonFixture fixture;

        public RequiredFileStyleCopTests()
        {
            fixture = new RequiredFileStylecopJsonFixture();
        }

        [Fact]
        public void RequiredStylecopJson_Should_Not_Warn_If_StylecopJson_Is_Referenced()
        {
            // given
            fixture.WithAdditionalFile("some/path/to/a/stylecop.json");

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.WarningEvents.Should().HaveCount(0);
        }

        [Fact]
        public void RequiredStylecopJson_Warn_If_Other_File_Than_StylecopJson_Is_Referenced()
        {
            // given
            fixture.WithAdditionalFile("some/path/to/a/other.file");

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.WarningEvents.Should().HaveCount(1);
        }

        [Fact]
        public void RequiredStylecopJson_Warn_And_Not_Throw_If_Path_Is_Empty()
        {
            // given
            fixture.WithAdditionalFile(string.Empty);

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.WarningEvents.Should().HaveCount(1);
        }

        [Fact]
        public void RequiredStylecopJson_Should_Warn_If_No_StylecopJson_Is_Referenced()
        {
            // given

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.WarningEvents.Should().HaveCount(1);
            var theEvent = fixture.BuildEngine.WarningEvents.Single();
            theEvent.Code.Should().Be("CCG0006");
        }

        [Fact]
        public void RequiredStylecopJson_Should_Not_Warn_If_StylecopJson_Is_Omitted()
        {
            // given
            fixture.WithOmittedFile("stylecop.json");

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.WarningEvents.Should().HaveCount(0);
        }

        [Fact]
        public void RequiredStylecopJson_Should_Not_Warn_If_ProjectType_Is_Recipe()
        {
            // given
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
