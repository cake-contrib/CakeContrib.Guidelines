using CakeContrib.Guidelines.Tasks.Tests.Fixtures;

using FluentAssertions;

using Xunit;

namespace CakeContrib.Guidelines.Tasks.Tests
{
    public class CalculateProjectTypeTests
    {
        private const string ExpectedTypeOther = "other";
        private const string ExpectedTypeAddin = "addin";
        private const string ExpectedTypeModule = "module";
        private const string ExpectedTypeRecipe = "recipe";

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
            fixture.Output.Should().BeEquivalentTo(existingType);
        }

        [Fact]
        public void Should_Return_Other_If_Nothing_Is_Set()
        {
            // given
            var fixture = new CalculateProjectTypeFixture();

            // when
            fixture.Execute();

            // then
            fixture.Output.Should().BeEquivalentTo(ExpectedTypeOther);
        }

        [Fact]
        public void Should_Return_Addin_If_Names_Point_To_Addin()
        {
            // given
            var fixture = new CalculateProjectTypeFixture();
            fixture.WithProjectNames("foo","bar","Cake.7zip","baz");

            // when
            fixture.Execute();

            // then
            fixture.Output.Should().BeEquivalentTo(ExpectedTypeAddin);
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
            fixture.Output.Should().BeEquivalentTo(ExpectedTypeModule);
        }

        [Fact]
        public void Should_Return_Recipe_If_Names_Point_To_Recipe()
        {
            // given
            var fixture = new CalculateProjectTypeFixture();
            fixture.WithProjectNames("foo","bar","Cake.Recipe","baz");

            // when
            fixture.Execute();

            // then
            fixture.Output.Should().BeEquivalentTo(ExpectedTypeRecipe);
        }

        [Fact]
        public void Should_Return_Other_If_Cake_Is_Not_Referenced()
        {
            // given
            var fixture = new CalculateProjectTypeFixture();
            fixture.WithProjectNames("Cake.7zip","Cake.Buildsystems.Module","foo");
            fixture.WithoutCakeReference();

            // when
            fixture.Execute();

            // then
            fixture.Output.Should().BeEquivalentTo(ExpectedTypeOther);
        }

        [Fact]
        public void Should_Return_Recipe_For_Recipes_Even_If_Cake_Is_Not_Referenced()
        {
            // given
            var fixture = new CalculateProjectTypeFixture();
            fixture.WithProjectNames("Cake.Recipe");
            fixture.WithoutCakeReference();

            // when
            fixture.Execute();

            // then
            fixture.Output.Should().BeEquivalentTo(ExpectedTypeRecipe);
        }
    }
}
