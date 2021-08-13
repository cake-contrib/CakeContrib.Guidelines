using System.Linq;

using CakeContrib.Guidelines.Tasks.Tests.Fixtures;

using Shouldly;

using Xunit;

namespace CakeContrib.Guidelines.Tasks.Tests
{
    public class EnsureCakeContribIconUrlTests
    {
        [Fact]
        public void Should_Output_DefaultPackageIconUrl_If_None_Is_Specified()
        {
            // given
            var fixture = new EnsureCakeContribIconUrlFixture();
            fixture.WithPackageIconUrl(string.Empty);

            // when
            fixture.Execute();

            // then
            fixture.PackageIconUrlOutput.ShouldNotBeEmpty();
        }

        [Fact]
        public void Should_Not_Output_DefaultPackageIconUrl_If_One_Is_Specified()
        {
            // given
            var fixture = new EnsureCakeContribIconUrlFixture();
            fixture.WithPackageIconUrl("http://my.custom/icon.png");

            // when
            fixture.Execute();

            // then
            fixture.PackageIconUrlOutput.ShouldBeEmpty();
        }

        [Fact]
        public void Should_OutputWarning_CCG0002_If_Omit_is_set_and_No_PackageIconUrl_Is_Specified()
        {
            // given
            var fixture = new EnsureCakeContribIconUrlFixture();
            fixture.WithPackageIconUrl(string.Empty);
            fixture.WithOmitIconImport();

            // when
            fixture.Execute();

            // then
            fixture.PackageIconUrlOutput.ShouldBeEmpty();
            fixture.BuildEngine.WarningEvents.ShouldContain(x => x.Code == "CCG0002");
        }

        [Fact]
        public void Should_Log_Correct_ErrorSourceFile_On_Error_With_Given_ProjectFile()
        {
            // given
            const string projectFileName = "some.project.csproj";
            var fixture = new EnsureCakeContribIconUrlFixture();
            fixture.WithPackageIconUrl(string.Empty);
            fixture.WithOmitIconImport();
            fixture.WithProjectFile(projectFileName);

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.WarningEvents.Count().ShouldBe(1);
            var theEvent = fixture.BuildEngine.WarningEvents.Single();
            theEvent.File.ShouldBe(projectFileName);
        }

        [Fact]
        public void Should_Not_Log_If_NoWarn_Is_Set()
        {
            // given
            var fixture = new EnsureCakeContribIconUrlFixture();
            fixture.WithPackageIconUrl(string.Empty);
            fixture.WithOmitIconImport();
            fixture.WithNoWarn("ccg0002");

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.WarningEvents.Count().ShouldBe(0);
        }

        [Fact]
        public void Should_Log_Error_If_WarnAsError_Is_Set()
        {
            // given
            var fixture = new EnsureCakeContribIconUrlFixture();
            fixture.WithPackageIconUrl(string.Empty);
            fixture.WithOmitIconImport();
            fixture.WithWarningsAsErrors("ccg0002");

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.ErrorEvents.Count().ShouldBe(1);
        }
    }
}
