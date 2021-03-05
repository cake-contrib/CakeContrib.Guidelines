using System.Linq;

using CakeContrib.Guidelines.Tasks.Tests.Fixtures;

using FluentAssertions;

using Xunit;

namespace CakeContrib.Guidelines.Tasks.Tests
{
    public class EnsureCakeContribIconTests
    {
        [Fact]
        public void Should_Output_DefaultPackageIcon_If_None_Is_Specified()
        {
            // given
            var fixture = new EnsureCakeContribIconFixture();
            fixture.WithPackageIcon(string.Empty);

            // when
            fixture.Execute();

            // then
            fixture.PackageIconOutput.Should().NotBeEmpty();
        }

        [Fact]
        public void Should_Output_PackageIcon_If_One_Is_Specified()
        {
            // given
            const string iconPath = "some/cool-icon.png";
            var fixture = new EnsureCakeContribIconFixture();
            fixture.WithPackageIcon(iconPath);

            // when
            fixture.Execute();

            // then
            fixture.PackageIconOutput.Should().Be(iconPath);
        }

        [Fact]
        public void Should_Output_NewNoneRef_If_No_None_Is_Specified()
        {
            // given
            var fixture = new EnsureCakeContribIconFixture();

            // when
            fixture.Execute();

            // then
            fixture.AdditionalNoneRefOutput.Should().NotBeNull();
        }

        [Fact]
        public void Should_Not_Output_NewNoneRef_If_None_Is_Specified()
        {
            // given
            var fixture = new EnsureCakeContribIconFixture();
            fixture.WithIconFileReference("temp\\logo.png", "");

            // when
            fixture.Execute();

            // then
            fixture.AdditionalNoneRefOutput.Should().BeNull();
        }

        [Fact]
        public void Should_Output_NewNoneRef_If_WrongRef_Is_Specified()
        {
            // given
            var fixture = new EnsureCakeContribIconFixture();
            fixture.WithPackageIcon("cool-logo.png");
            fixture.WithIconFileReference("temp\\logo.png", "");

            // when
            fixture.Execute();

            // then
            fixture.AdditionalNoneRefOutput.Should().NotBeNull();
        }

        [Fact]
        public void Should_Output_NewNoneRef_If_Ref_Is_Specified_For_Wrong_Folder()
        {
            // given
            var fixture = new EnsureCakeContribIconFixture();
            fixture.WithPackageIcon("icons/logo.png");
            fixture.WithIconFileReference("temp\\logo.png", "logos");

            // when
            fixture.Execute();

            // then
            fixture.AdditionalNoneRefOutput.Should().NotBeNull();
        }

        [Fact]
        public void Should_Output_NewNoneRef_If_Ref_Is_Specified_For_No_Pack()
        {
            // given
            var fixture = new EnsureCakeContribIconFixture();
            fixture.WithPackageIcon("icons/logo.png");
            fixture.WithIconFileReference("temp\\logo.png", "icons", false);

            // when
            fixture.Execute();

            // then
            fixture.AdditionalNoneRefOutput.Should().NotBeNull();
        }

        [Fact]
        public void Should_Not_Output_NewNoneRef_If_Ref_Is_Specified_And_Renamed()
        {
            // given
            var fixture = new EnsureCakeContribIconFixture();
            fixture.WithPackageIcon("icons/cool-logo.png");
            fixture.WithIconFileReference("icons\\logo.png", "icons\\cool-logo.png", false);

            // when
            fixture.Execute();

            // then
            fixture.AdditionalNoneRefOutput.Should().NotBeNull();
        }

        [Fact]
        public void Should_Not_Output_NewNoneRef_If_Ref_Is_Specified_For_Correct_Folder()
        {
            // given
            var fixture = new EnsureCakeContribIconFixture();
            fixture.WithPackageIcon("logos/cool-logo.png");
            fixture.WithIconFileReference("images\\cool-logo.png", "logos");

            // when
            fixture.Execute();

            // then
            fixture.AdditionalNoneRefOutput.Should().BeNull();
        }

        [Fact]
        public void Should_Copy_New_Logo_If_existing_Is_old()
        {
            // given
            var fixture = new EnsureCakeContribIconFixture();
            fixture.WithPackageIcon("logos/cool-logo.png");
            fixture.WithIconFileReference("images\\cool-logo.png", "logos");
            fixture.WithCompareIconResult(false);

            // when
            fixture.Execute();

            // then
            fixture.AssertCopyWasCalled(1);
        }

        [Fact]
        public void Should_Not_Copy_New_Logo_If_existing_Is_current()
        {
            // given
            var fixture = new EnsureCakeContribIconFixture();
            fixture.WithPackageIcon("logos/cool-logo.png");
            fixture.WithIconFileReference("images\\cool-logo.png", "logos");
            fixture.WithCompareIconResult(true);

            // when
            fixture.Execute();

            // then
            fixture.AssertCopyWasCalled(0);
        }

        [Fact]
        public void Should_Error_If_PackageIcon_Is_Specified_With_Wrong_extension()
        {
            // given
            var fixture = new EnsureCakeContribIconFixture();
            fixture.WithPackageIcon("logos/cool-logo.jpeg");

            // when
            var actual = fixture.Execute();

            // then
            actual.Should().BeFalse();
            fixture.BuildEngine.ErrorEvents.Should().Contain(x => x.Code == "CCG0003");
        }

        [Fact]
        public void Should_Warn_CCG0003_If_PackageIcon_Is_Specified_And_Referenced_But_With_With_Wrong_extension()
        {
            // given
            var fixture = new EnsureCakeContribIconFixture();
            fixture.WithPackageIcon("logos/cool-logo.jpeg");
            fixture.WithIconFileReference("temp\\foo.jpeg", "logos\\cool-logo.jpeg");

            // when
            var actual = fixture.Execute();

            // then
            actual.Should().BeTrue();
            fixture.BuildEngine.WarningEvents.Should().Contain(x => x.Code == "CCG0003");
        }

        [Fact]
        public void Should_Error_CCG0001_If_PackageIcon_Is_Not_Specified_And_Omit_Is_Set()
        {
            // given
            var fixture = new EnsureCakeContribIconFixture();
            fixture.WithPackageIcon("");
            fixture.WithOmitIconImport();

            // when
            var actual = fixture.Execute();

            // then
            actual.Should().BeFalse();
            fixture.BuildEngine.ErrorEvents.Should().Contain(x => x.Code == "CCG0001");
        }

        [Fact]
        public void Should_Not_Error_Or_Warn_If_PackageIconReference_Is_Not_Specified_And_Omit_Is_Set()
        {
            // given
            var fixture = new EnsureCakeContribIconFixture();
            fixture.WithPackageIcon("logo.png");
            fixture.WithOmitIconImport();

            // when
            var actual = fixture.Execute();

            // then
            actual.Should().BeTrue();
            fixture.BuildEngine.ErrorEvents.Should().BeEmpty();
            fixture.BuildEngine.WarningEvents.Should().BeEmpty();
        }

        [Fact]
        public void Should_Log_Correct_ErrorSourceFile_On_Error_With_Given_ProjectFile()
        {
            // given
            const string projectFileName = "some.project.csproj";
            var fixture = new EnsureCakeContribIconFixture();
            fixture.WithPackageIcon("");
            fixture.WithOmitIconImport();
            fixture.WithProjectFile(projectFileName);

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.ErrorEvents.Should().HaveCount(1);
            var theEvent = fixture.BuildEngine.ErrorEvents.Single();
            theEvent.File.Should().Be(projectFileName);
        }

        [Fact]
        public void Should_Warn_CCG0003_On_Omit_Set_But_Wrong_Icon()
        {
            // given
            const string projectFileName = "some.project.csproj";
            var fixture = new EnsureCakeContribIconFixture();
            fixture.WithPackageIcon("foo.png");
            fixture.WithIconFileReference("foo.png", "");
            fixture.WithOmitIconImport();
            fixture.WithCompareIconResult(false);

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.WarningEvents.Should().HaveCount(1);
            fixture.BuildEngine.WarningEvents.Should().Contain(x => x.Code == "CCG0003");
        }
    }
}
