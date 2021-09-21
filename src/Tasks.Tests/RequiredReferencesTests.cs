using System.Linq;

using CakeContrib.Guidelines.Tasks.Tests.Fixtures;

using Shouldly;

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
            fixture.BuildEngine.WarningEvents.Count().ShouldBe(0);
        }

        [Fact]
        public void Should_Warn_If_RequiredPackage_Is_Not_Referenced()
        {
            // given
            var fixture = new RequiredReferencesFixture();
            fixture.WithReferencedPackage("Cool.Ref.Project");
            fixture.WithRequiredReferences("Some.Analyser");

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.WarningEvents.Count().ShouldBe(1);
            var theEvent = fixture.BuildEngine.WarningEvents.Single();
            theEvent.Code.ShouldBe("CCG0005");
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
            fixture.Execute();

            // then
            fixture.BuildEngine.WarningEvents.Count().ShouldBe(0);
        }

        [Fact]
        public void Should_Log_Correct_SourceFile_On_Warning_With_Given_ProjectFile()
        {
            // given
            const string projectFileName = "some.project.csproj";
            var fixture = new RequiredReferencesFixture();
            fixture.WithReferencedPackage("Cool.Ref.Project");
            fixture.WithRequiredReferences("Some.Analyser");
            fixture.WithProjectFile(projectFileName);

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.WarningEvents.Count.ShouldBe(1);
            fixture.BuildEngine.WarningEvents.ShouldContain(x => x.File == projectFileName);
        }

        [Fact]
        public void RequiredStylecopJson_Should_Not_Warn_If_ProjectType_Is_Recipe()
        {
            // given
            var fixture = new RequiredReferencesFixture();
            fixture.WithReferencedPackage("Cool.Ref.Project");
            fixture.WithRequiredReferences("Some.Analyser");
            fixture.WithProjectTypeRecipe();

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.WarningEvents.Count().ShouldBe(0);
        }

        [Fact]
        public void Should_Not_Log_If_NoWarn_Is_Set()
        {
            // given
            var fixture = new RequiredReferencesFixture();
            fixture.WithRequiredReferences("Some.Analyser");
            fixture.WithNoWarn("ccg0005");

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.WarningEvents.Count().ShouldBe(0);
        }

        [Fact]
        public void Should_Log_Error_If_WarnAsError_Is_Set()
        {
            // given
            var fixture = new RequiredReferencesFixture();
            fixture.WithRequiredReferences("Some.Analyser");
            fixture.WithWarningsAsErrors("ccg0005");

            // when
            fixture.Execute();

            // then
            fixture.BuildEngine.ErrorEvents.Count().ShouldBe(1);
        }
    }
}
