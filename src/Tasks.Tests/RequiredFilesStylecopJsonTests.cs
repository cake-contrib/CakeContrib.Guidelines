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
    }
}
