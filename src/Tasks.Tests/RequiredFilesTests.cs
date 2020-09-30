using System.Linq;

using CakeContrib.Guidelines.Tasks.Tests.Fixtures;

using FluentAssertions;

using Xunit;

namespace CakeContrib.Guidelines.Tasks.Tests
{
    public class RequiredFilesTests
    {
        // TODO: Use https://github.com/System-IO-Abstractions/System.IO.Abstractions
        // to test the editorconfig-task?

        [Fact]
        public void RequiredStylecopJson_Should_Not_Warn_If_StylecopJson_Is_Referenced()
        {
            // given
            var fixture = new RequiredFileStylecopJsonFixture();
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
            var fixture = new RequiredFileStylecopJsonFixture();

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
            var fixture = new RequiredFileStylecopJsonFixture();
            fixture.WithOmittedFile("stylecop.json");

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.WarningEvents.Should().HaveCount(0);
        }
    }
}
