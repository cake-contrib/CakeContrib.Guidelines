using System;
using System.Collections.Generic;
using System.Linq;

using CakeContrib.Guidelines.Tasks.Tests.Fixtures;

using Shouldly;

using Xunit;

namespace CakeContrib.Guidelines.Tasks.Tests
{
    public class TargetFrameworkVersionsTests
    {
        private const string NetStandard20 = "netstandard2.0";
        private const string Net46 = "net46";
        private const string Net461 = "net461";
        private const string NetCore31 = "netcoreapp3.1";
        private const string Net50 = "net5.0";
        private const string Net60 = "net6.0";
        private const string Net70 = "net7.0";

        [Fact]
        public void Should_Error_If_RequiredTargetFramework_Is_Not_Targeted()
        {
            // given
            var fixture = new TargetFrameworkVersionsFixture();
            fixture.WithCakeCoreReference(1);

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.ErrorEvents.Count.ShouldBe(1);
            fixture.BuildEngine.ErrorEvents.First().Message.ShouldContain(NetStandard20);
        }

        [Fact]
        public void Should_Not_Error_If_RequiredTargetFramework_Is_Not_Targeted_But_Omitted()
        {
            // given
            var fixture = new TargetFrameworkVersionsFixture();
            fixture.WithCakeCoreReference(1);
            fixture.WithOmittedTargetFramework(NetStandard20);

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.ErrorEvents.Count.ShouldBe(0);
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
            fixture.BuildEngine.ErrorEvents.Count().ShouldBe(0);
            fixture.BuildEngine.WarningEvents.Count().ShouldBe(1);
            fixture.BuildEngine.WarningEvents.First().Message.ShouldContain(Net46);
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
            fixture.BuildEngine.ErrorEvents.Count().ShouldBe(0);
            fixture.BuildEngine.WarningEvents.Count().ShouldBe(0);
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
            fixture.BuildEngine.ErrorEvents.Count().ShouldBe(0);
            fixture.BuildEngine.WarningEvents.Count().ShouldBe(0);
        }

        [Fact]
        public void Should_Warn_For_Broken_CakeCore_Reference()
        {
            // given
            const string brokenVersion = "1.2.3.4.5.6.7.8.9";
            var fixture = new TargetFrameworkVersionsFixture();
            fixture.WithCakeCoreReference(brokenVersion);
            fixture.WithTargetFrameworksMatchingDefault();

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.ErrorEvents.Count.ShouldBe(0);
            fixture.BuildEngine.WarningEvents.Count.ShouldBe(1);
            fixture.BuildEngine.WarningEvents.ShouldContain(x => x.Message.Contains(brokenVersion));
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
            fixture.BuildEngine.ErrorEvents.Count().ShouldBe(0);
            fixture.BuildEngine.WarningEvents.Count().ShouldBe(0);
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
            fixture.BuildEngine.ErrorEvents.Count().ShouldBe(0);
            fixture.BuildEngine.WarningEvents.Count().ShouldBe(1);
            fixture.BuildEngine.WarningEvents.First().Message.ShouldContain(Net50);
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
            fixture.BuildEngine.ErrorEvents.Count().ShouldBe(0);
            fixture.BuildEngine.WarningEvents.Count().ShouldBe(0);
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
            fixture.BuildEngine.ErrorEvents.Count().ShouldBe(1);
        }

        [Theory]
        [MemberData(nameof(Should_Not_Warn_If_ProjectType_Is_Module_And_Reference_Is_Correct_Data))]
        public void Should_Not_Warn_If_ProjectType_Is_Module_And_Reference_Is_Correct(int cakeCoreReferenceMajor, string targetFramework)
        {
            // given
            var fixture = new TargetFrameworkVersionsFixture();
            fixture.WithCakeCoreReference(cakeCoreReferenceMajor);
            fixture.WithTargetFramework(targetFramework);
            fixture.WithProjectType("module");

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.ErrorEvents.Count().ShouldBe(0);
            fixture.BuildEngine.WarningEvents.Count().ShouldBe(0);
        }

        public static IEnumerable<object[]> Should_Not_Warn_If_ProjectType_Is_Module_And_Reference_Is_Correct_Data()
        {
            yield return new object[] { 1, NetStandard20 };
            yield return new object[] { 2, NetCore31 };
            yield return new object[] { 3, Net60 };
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
            fixture.BuildEngine.ErrorEvents.Count.ShouldBe(1);
            fixture.BuildEngine.ErrorEvents.ShouldContain(x => x.File == projectFileName);
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
            fixture.BuildEngine.ErrorEvents.Count().ShouldBe(0);
            fixture.BuildEngine.WarningEvents.Count().ShouldBe(0);
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
            fixture.BuildEngine.WarningEvents.Count().ShouldBe(0);
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
            fixture.BuildEngine.ErrorEvents.Count().ShouldBe(1);
        }

        [Theory]
        [MemberData(nameof(Should_Error_If_RequiredTargetFramework_Is_Not_Targeted_Cake_2_Data))]
        public void Should_Error_If_RequiredTargetFramework_Is_Not_Targeted_Cake_2(string[] targetFrameworks, bool expectedError, string missingTargetFramework)
        {
            // given
            var fixture = new TargetFrameworkVersionsFixture();
            fixture.WithCakeCoreReference(2);
            fixture.WithTargetFrameworks(targetFrameworks);

            // when
            fixture.Execute();

            // then
            if (expectedError)
            {
                fixture.BuildEngine.ErrorEvents.Count.ShouldBe(1);
                fixture.BuildEngine.ErrorEvents.First().Message.ShouldContain(missingTargetFramework);
            }
            else
            {
                fixture.BuildEngine.ErrorEvents.Count.ShouldBe(0);
            }
        }

        public static IEnumerable<object[]> Should_Error_If_RequiredTargetFramework_Is_Not_Targeted_Cake_2_Data()
        {
            yield return new object[] { Array.Empty<string>(), true, NetCore31 };
            yield return new object[] { new[]{ NetCore31 }, true, Net50 };
            yield return new object[] { new[]{ NetCore31, Net50 }, true, Net60 };
            yield return new object[] { new[]{ NetCore31, Net50, Net60 }, false, string.Empty };
        }

        [Theory]
        [MemberData(nameof(Should_Error_If_RequiredTargetFramework_Is_Not_Targeted_Cake_3_Data))]
        public void Should_Error_If_RequiredTargetFramework_Is_Not_Targeted_Cake_3(string[] targetFrameworks, bool expectedError, string missingTargetFramework)
        {
            // given
            var fixture = new TargetFrameworkVersionsFixture();
            fixture.WithCakeCoreReference(3);
            fixture.WithTargetFrameworks(targetFrameworks);

            // when
            fixture.Execute();

            // then
            if (expectedError)
            {
                fixture.BuildEngine.ErrorEvents.Count.ShouldBe(1);
                fixture.BuildEngine.ErrorEvents.First().Message.ShouldContain(missingTargetFramework);
            }
            else
            {
                fixture.BuildEngine.ErrorEvents.Count.ShouldBe(0);
            }
        }

        public static IEnumerable<object[]> Should_Error_If_RequiredTargetFramework_Is_Not_Targeted_Cake_3_Data()
        {
            yield return new object[] { Array.Empty<string>(), true, Net60 };
            yield return new object[] { new[] { Net60 }, true, Net70 };
            yield return new object[] { new[] { Net70 }, true, Net60 };
            yield return new object[] { new[] { Net60, Net70 }, false, string.Empty };
        }

        [Fact]
        public void Should_Error_If_RequiredTargetFramework_Is_Not_Targeted_Cake_2_Module()
        {
            // given
            var fixture = new TargetFrameworkVersionsFixture();
            fixture.WithProjectType("module");
            fixture.WithCakeCoreReference("2.0.0-rc0001");

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.ErrorEvents.Count.ShouldBe(1);
            fixture.BuildEngine.ErrorEvents.First().Message.ShouldContain(NetCore31);
        }

        [Fact]
        public void Should_Error_If_RequiredTargetFramework_Is_Not_Targeted_Cake_3_Module()
        {
            // given
            var fixture = new TargetFrameworkVersionsFixture();
            fixture.WithProjectType("module");
            fixture.WithCakeCoreReference("3.0.0");

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.ErrorEvents.Count.ShouldBe(1);
            fixture.BuildEngine.ErrorEvents.First().Message.ShouldContain(Net60);
        }

        [Fact]
        public void Should_Error_If_RequiredTargetFramework_Is_Not_Targeted_Module_Explicit_Version()
        {
            // given
            var fixture = new TargetFrameworkVersionsFixture();
            fixture.WithProjectType("module");
            fixture.WithExplicitCakeVersion("1.0.0");

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.ErrorEvents.Count.ShouldBe(1);
            fixture.BuildEngine.ErrorEvents.First().Message.ShouldContain(NetStandard20);
        }

        [Fact]
        public void Should_Error_If_Additional_TargetFrameworks_Are_Supplied_For_Module()
        {
            // given
            var fixture = new TargetFrameworkVersionsFixture();
            fixture.WithProjectType("module");
            fixture.WithCakeCoreReference("3.0.0");
            fixture.WithTargetFrameworks("net6.0", "net7.0");

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.ErrorEvents.Count.ShouldBe(1);
            fixture.BuildEngine.ErrorEvents.First().Message.ShouldContain(Net70);
        }
    }
}
