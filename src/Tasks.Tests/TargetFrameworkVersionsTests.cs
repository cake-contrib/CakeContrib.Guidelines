using System.Linq;

using CakeContrib.Guidelines.Tasks.Tests.Fixtures;

using FluentAssertions;

using Xunit;

namespace CakeContrib.Guidelines.Tasks.Tests
{
    public class TargetFrameworkVersionsTests
    {
        private const string NetStandard20 = "netstandard2.0";
        private const string Net46 = "net46";
        private const string Net461 = "net461";
        private const string Net50 = "net5.0";

        [Fact]
        public void Should_Error_If_RequiredTargetFramework_Is_Not_Targeted()
        {
            // given
            var fixture = new TargetFrameworkVersionsFixture();

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.ErrorEvents.Should().HaveCount(1);
            fixture.BuildEngine.ErrorEvents.First().Message.Should().Contain(NetStandard20);
        }

        [Fact]
        public void Should_Not_Error_If_RequiredTargetFramework_Is_Not_Targeted_But_Omitted()
        {
            // given
            var fixture = new TargetFrameworkVersionsFixture();
            fixture.WithOmittedTargetFramework(NetStandard20);

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.ErrorEvents.Should().HaveCount(0);
        }

        [Fact]
        public void Should_Warn_If_SuggestedTargetFramework_Is_Not_Targeted()
        {
            // given
            var fixture = new TargetFrameworkVersionsFixture();
            fixture.WithCakeCoreReference(0, 38, 5);
            fixture.WithTargetFramework(NetStandard20);

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.ErrorEvents.Should().HaveCount(0);
            fixture.BuildEngine.WarningEvents.Should().HaveCount(1);
            fixture.BuildEngine.WarningEvents.First().Message.Should().Contain(Net46);
        }

        [Fact]
        public void Should_Not_Warn_If_SuggestedTargetFramework_Is_Not_Targeted_But_Omitted()
        {
            // given
            var fixture = new TargetFrameworkVersionsFixture();
            fixture.WithCakeCoreReference(0, 38, 5);
            fixture.WithTargetFramework(NetStandard20);
            fixture.WithOmittedTargetFramework(Net461);

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.ErrorEvents.Should().HaveCount(0);
            fixture.BuildEngine.WarningEvents.Should().HaveCount(0);
        }

        [Fact]
        public void Should_Not_Warn_If_Required_And_SuggestedTargetFramework_Is_Targeted()
        {
            // given
            var fixture = new TargetFrameworkVersionsFixture();
            fixture.WithCakeCoreReference(0, 38, 5);
            fixture.WithTargetFrameworks(NetStandard20, Net461);

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.ErrorEvents.Should().HaveCount(0);
            fixture.BuildEngine.WarningEvents.Should().HaveCount(0);
        }

        [Fact]
        public void Should_Warn_For_Broken_CakeCore_Reference()
        {
            // given
            const string brokenVersion = "1.2.3.4.5.6.7.8.9";
            var fixture = new TargetFrameworkVersionsFixture();
            fixture.WithCakeCoreReference(brokenVersion);
            fixture.WithTargetFrameworks(NetStandard20, Net461, Net50);

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.ErrorEvents.Should().HaveCount(0);
            fixture.BuildEngine.WarningEvents
                .Should().HaveCount(1)
                .And.Contain(x => x.Message.Contains(brokenVersion));
        }

        [Fact]
        public void Should_Not_Warn_If_Required_And_Alternative_SuggestedTargetFramework_Is_Targeted()
        {
            // given
            var fixture = new TargetFrameworkVersionsFixture();
            fixture.WithCakeCoreReference(0, 38, 5);
            fixture.WithTargetFrameworks(NetStandard20, Net46);

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.ErrorEvents.Should().HaveCount(0);
            fixture.BuildEngine.WarningEvents.Should().HaveCount(0);
        }

        [Fact]
        public void Should_Warn_If_Net5_is_missing_and_Cake_Version_is_1_0_0()
        {
            // given
            var fixture = new TargetFrameworkVersionsFixture();
            fixture.WithCakeCoreReference(1);
            fixture.WithTargetFrameworks(NetStandard20, Net461);

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.ErrorEvents.Should().HaveCount(0);
            fixture.BuildEngine.WarningEvents.Should().HaveCount(1);
            fixture.BuildEngine.WarningEvents.First().Message.Should().Contain(Net50);
        }

        [Fact]
        public void Should_Not_Warn_If_All_References_for_Cake_Version_is_1_0_0_is_present()
        {
            // given
            var fixture = new TargetFrameworkVersionsFixture();
            fixture.WithCakeCoreReference(1);
            fixture.WithTargetFrameworks(NetStandard20, Net461, Net50);

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.ErrorEvents.Should().HaveCount(0);
            fixture.BuildEngine.WarningEvents.Should().HaveCount(0);
        }

        [Fact]
        public void Should_Error_If_ProjectType_Is_Module_And_Reference_Is_Missing()
        {
            // given
            var fixture = new TargetFrameworkVersionsFixture();
            fixture.WithCakeCoreReference(1);
            fixture.WithProjectType("module");

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.ErrorEvents.Should().HaveCount(1);
        }

        [Fact]
        public void Should_Not_Warn_If_ProjectType_Is_Module_And_Reference_Is_NetStandard()
        {
            // given
            var fixture = new TargetFrameworkVersionsFixture();
            fixture.WithCakeCoreReference(1);
            fixture.WithTargetFramework("netstandard2.0");
            fixture.WithProjectType("module");

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.ErrorEvents.Should().HaveCount(0);
            fixture.BuildEngine.WarningEvents.Should().HaveCount(0);
        }

        [Fact]
        public void Should_Log_Correct_SourceFile_On_Error_With_Given_ProjectFile()
        {
            // given
            const string projectFileName = "some.project.csproj";
            var fixture = new TargetFrameworkVersionsFixture();
            fixture.WithCakeCoreReference(1);
            fixture.WithProjectFile(projectFileName);

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.ErrorEvents
                .Should().HaveCount(1)
                .And.Contain(x => x.File == projectFileName);
        }

        [Fact]
        public void Should_Not_Warn_Or_Error_With_No_Reference_And_ProjectType_Recipe()
        {
            // given
            var fixture = new TargetFrameworkVersionsFixture();
            fixture.WithProjectType("recipe");

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.ErrorEvents.Should().HaveCount(0);
            fixture.BuildEngine.WarningEvents.Should().HaveCount(0);
        }

        [Fact]
        public void Should_Not_Log_If_NoWarn_Is_Set()
        {
            // given
            var fixture = new TargetFrameworkVersionsFixture();
            fixture.WithCakeCoreReference(0, 38, 5);
            fixture.WithTargetFramework(NetStandard20);
            fixture.WithNoWarn("ccg0007");

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.WarningEvents.Should().HaveCount(0);
        }

        [Fact]
        public void Should_Log_Error_If_WarnAsError_Is_Set()
        {
            // given
            var fixture = new TargetFrameworkVersionsFixture();
            fixture.WithCakeCoreReference(0, 38, 5);
            fixture.WithTargetFramework(NetStandard20);
            fixture.WithWarningsAsErrors("ccg0007");

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.ErrorEvents.Should().HaveCount(1);
        }
    }
}
