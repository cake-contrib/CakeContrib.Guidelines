using System.Linq;

using CakeContrib.Guidelines.Tasks.Tests.Fixtures;

using FluentAssertions;

using Xunit;

namespace CakeContrib.Guidelines.Tasks.Tests
{
    public class CalculateProjectTypeTests
    {
        private const string ExpectedTypeAddin = "Addin";
        private const string ExpectedTypeModule = "Module";

        [Fact]
        public void Should_Return_The_ProjectType_If_Set()
        {
            // given
            var fixture = new CalculateProjectTypeFixture();
            const string existingType = "some-type-manually-set";
            fixture.WithExistinType(existingType);

            // when
            fixture.Execute();

            // then
            fixture.Output.Should().Be(existingType);
        }

        [Fact]
        public void Should_Return_Addin_If_Nothing_Is_Set()
        {
            // given
            var fixture = new CalculateProjectTypeFixture();

            // when
            fixture.Execute();

            // then
            fixture.Output.Should().Be(ExpectedTypeAddin);
        }

        [Fact]
        public void Should_Return_Addin_Names_Point_To_Addin()
        {
            // given
            var fixture = new CalculateProjectTypeFixture();
            fixture.WithProjectNames("foo","bar","Cake.7zip","baz");

            // when
            fixture.Execute();

            // then
            fixture.Output.Should().Be(ExpectedTypeAddin);
        }

        [Fact]
        public void Should_Return_Module_If_Names_Point_To_Module()
        {
            // given
            var fixture = new CalculateProjectTypeFixture();
            fixture.WithProjectNames("foo","bar","Cake.BuildSystems.Module","baz");

            // when
            fixture.Execute();

            // then
            fixture.Output.Should().Be(ExpectedTypeModule);
        }
    }
}
