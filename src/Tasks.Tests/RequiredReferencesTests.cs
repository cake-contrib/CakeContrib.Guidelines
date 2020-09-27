using System.Linq;

using CakeContrib.Guidelines.Tasks.Tests.Fixtures;

using FluentAssertions;

using Xunit;

namespace CakeContrib.Guidelines.Tasks.Tests
{
    public class RequiredReferencesTests
    {
        [Fact]
        public void Should_Not_Warn_If_RequiredPackage_Is_Referenced()
        {
            // given
            const string required = "Some.Analyser";
            var fixture = new RequiredReferencesFixture();
            fixture.WithReferencedPackage(required);
            fixture.WithRequiredReferences(required);

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.WarningEvents.Should().HaveCount(0);
        }

        [Fact]
        public void Should_Warn_If_RequiredPackage_Is_Not_Referenced()
        {
            // given
            var fixture = new RequiredReferencesFixture();
            fixture.WithReferencedPackage("Cool.Ref.Project");
            fixture.WithRequiredReferences("Some.Analyser");

            // when
            var actual = fixture.Execute();

            // then
            fixture.BuildEngine.WarningEvents.Should().HaveCount(1);
            var theEvent = fixture.BuildEngine.WarningEvents.Single();
            theEvent.Code.Should().Be("CCG0005");
        }

        [Fact]
        public void Should_Not_Warn_If_RequiredPackage_Is_Omitted()
        {
            // given
            const string required = "Some.Analyser";
            var fixture = new RequiredReferencesFixture();
            fixture.WithReferencedPackage("Cool.Ref.Project");
            fixture.WithRequiredReferences(required);
            fixture.WithOmittedReferences(required);

            // when
            var actual = fixture.Execute();

            // then
            fixture.BuildEngine.WarningEvents.Should().HaveCount(0);
        }
    }
}
