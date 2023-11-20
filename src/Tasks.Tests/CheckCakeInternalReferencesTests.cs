using System;
using System.Linq;

using CakeContrib.Guidelines.Tasks.Tests.Fixtures;

using Shouldly;

using Xunit;

namespace CakeContrib.Guidelines.Tasks.Tests
{
    public class CheckCakeInternalReferencesTests
    {
        private const string CcgRule10 = "CCG0010";

        [Fact]
        public void Should_Warn_If_NuGet_Is_Referenced_In_The_Wrong_Version()
        {
            // given
            var fixture = new CheckCakeInternalReferencesFixture();
            fixture.WithReference("NuGet.Common", "5.11.0");

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.WarningEvents.Count.ShouldBe(1);
            fixture.BuildEngine.WarningEvents.First().Code.ShouldBe(CcgRule10);
            fixture.BuildEngine.WarningEvents.First().Message.ShouldContain("6.3.1"); // the required version for NuGet.Common
        }

        [Fact]
        public void Should_Not_Warn_If_NuGet_Is_Referenced_In_The_Correct_Version()
        {
            // given
            var fixture = new CheckCakeInternalReferencesFixture();
            fixture.WithReference("NuGet.Common", "6.3.1");

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.WarningEvents.Count.ShouldBe(0);
        }

        [Fact]
        public void Should_Warn_If_NuGet_Is_Referenced_Not_Private()
        {
            // given
            var fixture = new CheckCakeInternalReferencesFixture();
            fixture.WithReference("NuGet.Common", "6.3.1", string.Empty);

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.WarningEvents.Count.ShouldBe(1);
            fixture.BuildEngine.WarningEvents.First().Code.ShouldBe(CcgRule10);
            fixture.BuildEngine.WarningEvents.First().Message.ShouldContain("privateAssets");
        }

        [Fact]
        public void Should_Not_Warn_If_NuGet_Is_Referenced_In_The_Wrong_Version_But_Cake_Is_Not_Referenced()
        {
            // given
            var fixture = new CheckCakeInternalReferencesFixture();
            fixture.WithReference("NuGet.Common", "5.11.0");
            fixture.WithCakeVersion(string.Empty);

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.WarningEvents.Count.ShouldBe(0);
        }

        [Fact]
        public void Should_Not_Warn_If_NuGet_Is_Referenced_In_The_Wrong_Version_But_Project_Is_Recipe()
        {
            // given
            var fixture = new CheckCakeInternalReferencesFixture();
            fixture.WithReference("NuGet.Common", "5.11.0");
            fixture.WithProjectType(CakeProjectType.Recipe);

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.WarningEvents.Count.ShouldBe(0);
        }

        [Fact]
        public void Should_Not_Warn_If_NuGet_Is_Referenced_In_The_Wrong_Version_But_Cake_Version_Is_Invalid()
        {
            // given
            var fixture = new CheckCakeInternalReferencesFixture();
            fixture.WithReference("NuGet.Common", "5.11.0");
            fixture.WithCakeVersion("a.b.c");

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.WarningEvents.Count.ShouldBe(0);
        }

        [Fact]
        public void Should_Not_Warn_If_NuGet_Is_Referenced_With_A_Broken_Version()
        {
            // given
            var fixture = new CheckCakeInternalReferencesFixture();
            fixture.WithReference("NuGet.Common", "a.b.c");

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.WarningEvents.Count.ShouldBe(0);
        }

        [Fact]
        public void Should_Warn_If_NuGet_Is_Referenced_In_The_Wrong_Version_Even_If_Cake_Is_A_Preview_Version()
        {
            // given
            var fixture = new CheckCakeInternalReferencesFixture();
            fixture.WithReference("NuGet.Common", "5.11.0");
            fixture.WithReference("Cake.Core", "3.0.0-preview1");
            fixture.WithCakeVersion(string.Empty);

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.WarningEvents.Count.ShouldBe(1);
            fixture.BuildEngine.WarningEvents.First().Code.ShouldBe(CcgRule10);
            fixture.BuildEngine.WarningEvents.First().Message.ShouldContain("6.3.1"); // the required version for NuGet.Common
        }

        [Fact]
        public void Should_Warn_If_Cake_Version_is_Out_Of_Range()
        {
            // given
            var fixture = new CheckCakeInternalReferencesFixture();
            fixture.WithReference("NuGet.Common", "5.11.0");
            fixture.WithReference("Cake.Core", "99.0.0");
            fixture.WithCakeVersion(string.Empty);

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.WarningEvents.Count.ShouldBe(1);
            fixture.BuildEngine.WarningEvents.First().Code.ShouldBe(CcgRule10);
            fixture.BuildEngine.WarningEvents.First().Message.ShouldNotContain("6.3.1");
            fixture.BuildEngine.WarningEvents.First().Message.ShouldContain("no matching list of Cake provided references could be found");
        }

        [Fact]
        public void Should_Warn_If_Newtonsoft_Is_Wrong_For_Cake_4()
        {
            // given
            var fixture = new CheckCakeInternalReferencesFixture();
            fixture.WithReference("Newtonsoft.Json", "13.0.1");
            fixture.WithReference("Cake.Core", "4.0.0");
            fixture.WithCakeVersion(string.Empty);

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.WarningEvents.Count.ShouldBe(1);
            fixture.BuildEngine.WarningEvents.First().Code.ShouldBe(CcgRule10);
            fixture.BuildEngine.WarningEvents.First().Message.ShouldContain("13.0.3"); // the required version for NuGet.Common
        }
    }
}
