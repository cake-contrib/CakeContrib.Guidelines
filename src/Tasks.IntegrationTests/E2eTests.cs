using System;
using System.IO;
using System.Linq;

using CakeContrib.Guidelines.Tasks.IntegrationTests.Fixtures;

using FluentAssertions;

using Xunit;
using Xunit.Abstractions;

namespace CakeContrib.Guidelines.Tasks.IntegrationTests
{
    // TODO: Writing things to disk is not deterministic...
    // TODO: Running Code-Coverage on the integration-tests breaks all tests

    //
    //  !! DO NOT RUN CODE-CONVERAGE ON E2E-TESTS !!
    //

    public class E2eTests : IDisposable
    {
        private readonly E2eTestFixture fixture;

        public E2eTests(ITestOutputHelper output)
        {
            string tempDir;
            do
            {
                tempDir = Path.Combine(Path.GetTempPath(), "CCG-" + Guid.NewGuid().ToString("n"));
            } while (Directory.Exists(tempDir));
            Directory.CreateDirectory(tempDir);
            fixture = new E2eTestFixture(tempDir, output);
        }

        public void Dispose()
        {
            fixture.Dispose();
        }

        [Fact]
        public void PackageIcon_Tag_missing_results_in_CCG0001_error()
        {
            // given
            fixture.WithoutPackageIcon();

            // when
            var result = fixture.Run();

            // then
            result.IsErrorExitCode.Should().BeTrue();
            result.ErrorLines.Should().Contain(l => l.IndexOf("CCG0001", StringComparison.Ordinal) > -1);
        }

        [Fact]
        public void PackageIconUrl_Tag_missing_results_in_CCG0002_warning()
        {
            // given
            fixture.WithoutPackageIconUrl();

            // when
            var result = fixture.Run();

            // then
            result.IsErrorExitCode.Should().BeFalse();
            result.WarningLines.Should().Contain(l => l.IndexOf("CCG0002", StringComparison.Ordinal) > -1);
        }

        [Fact]
        public void PackageIcon_Tag_with_non_standard_value_results_in_CCG0003_warning()
        {
            // given
            fixture.WithPackageIcon("coolIcon.jpeg");

            // when
            var result = fixture.Run();

            // then
            result.IsErrorExitCode.Should().BeFalse();
            result.WarningLines.Should().Contain(l => l.IndexOf("CCG0003", StringComparison.Ordinal) > -1);
        }

        [Fact]
        public void PackageIcon_Tag_with_modified_CakeContribGuidelinesIconDestinationLocation_results_not_in_CCG0003_warning()
        {
            const string icon = "coolIcon.jpeg";

            // given
            fixture.WithPackageIcon(icon);
            fixture.WithCustomContent($@"
<PropertyGroup>
    <CakeContribGuidelinesIconDestinationLocation>{icon}</CakeContribGuidelinesIconDestinationLocation>
</PropertyGroup>");

            // when
            var result = fixture.Run();

            // then
            result.IsErrorExitCode.Should().BeFalse();
            result.WarningLines.Should().BeEmpty();
        }

        [Fact]
        public void Referencing_Cake_Core_with_PrivateAssets_results_in_no_error()
        {
            // given
            fixture.WithPackageReference("Cake.Core", "0.38.5", "all");

            // when
            var result = fixture.Run();

            // then
            result.IsErrorExitCode.Should().BeFalse();
            result.ErrorLines.Should().BeEmpty();
        }

        [Fact]
        public void Referencing_Cake_Core_without_PrivateAssets_results_in_CCG0004_error()
        {
            // given
            fixture.WithPackageReference("Cake.Core", "0.38.5");

            // when
            var result = fixture.Run();

            // then
            result.IsErrorExitCode.Should().BeTrue();
            result.ErrorLines.Should().Contain(l => l.IndexOf("CCG0004", StringComparison.Ordinal) > -1);
        }

        [Fact]
        public void Not_referencing_StyleCop_Analyzers_results_in_CCG0005_warning()
        {
            // given
            fixture.WithoutStylecopReference();

            // when
            var result = fixture.Run();

            // then
            result.IsErrorExitCode.Should().BeFalse();
            result.WarningLines.Should().Contain(l => l.IndexOf("CCG0005", StringComparison.Ordinal) > -1);
        }

        [Fact]
        public void Not_referencing_StyleCop_Analyzers_and_setting_CakeContribGuidelinesOmitRecommendedReference_results_in_no_warning()
        {
            // given
            fixture.WithoutStylecopReference();
            fixture.WithCustomContent($@"
<ItemGroup>
      <CakeContribGuidelinesOmitRecommendedReference Include=""StyleCop.Analyzers"" />
</ItemGroup>");

            // when
            var result = fixture.Run();

            // then
            result.IsErrorExitCode.Should().BeFalse();
            result.WarningLines.Should().BeEmpty();
        }

        [Fact]
        public void Missing_file_stylecopJson_results_in_CCG0006_warning()
        {
            // given
            fixture.WithoutFileStylecopJson();

            // when
            var result = fixture.Run();

            // then
            result.IsErrorExitCode.Should().BeFalse();
            result.WarningLines.Should().Contain(l => l.IndexOf("CCG0006", StringComparison.Ordinal) > -1);
            result.WarningLines.Should().Contain(l => l.IndexOf("stylecop.json", StringComparison.Ordinal) > -1);
        }

        [Fact]
        public void Missing_file_editorconfig_results_in_CCG0006_warning()
        {
            // given
            fixture.WithoutFileEditorconfig();

            // when
            var result = fixture.Run();

            // then
            result.IsErrorExitCode.Should().BeFalse();
            result.WarningLines.Should().Contain(l => l.IndexOf("CCG0006", StringComparison.Ordinal) > -1);
            result.WarningLines.Should().Contain(l => l.IndexOf(".editorconfig", StringComparison.Ordinal) > -1);
        }

        [Fact]
        public void Missing_Required_Target_results_in_CCG0007_error()
        {
            // given
            fixture.WithTargetFrameworks("net47");

            // when
            var result = fixture.Run();

            // then
            result.IsErrorExitCode.Should().BeTrue();
            result.ErrorLines.Should().Contain(l => l.IndexOf("CCG0007", StringComparison.Ordinal) > -1);
            result.ErrorLines.Should().Contain(l => l.IndexOf("netstandard2.0", StringComparison.Ordinal) > -1);
        }

        [Fact]
        public void Missing_Suggested_Target_results_in_CCG0007_warning()
        {
            // given
            fixture.WithTargetFrameworks("netstandard2.0");

            // when
            var result = fixture.Run();

            // then
            result.IsErrorExitCode.Should().BeFalse();
            result.WarningLines.Should().Contain(l => l.IndexOf("CCG0007", StringComparison.Ordinal) > -1);
            result.WarningLines.Should().Contain(l => l.IndexOf("net461", StringComparison.Ordinal) > -1);
        }

        [Fact]
        public void Missing_Suggested_Target_net5_for_Cake_v100_results_in_CCG0007_warning()
        {
            // given
            fixture.WithPackageReference("Cake.Core", "1.0.0", "all");
            fixture.WithTargetFrameworks("netstandard2.0;net461");

            // when
            var result = fixture.Run();

            // then
            result.IsErrorExitCode.Should().BeFalse();
            result.WarningLines.Should().Contain(l => l.IndexOf("CCG0007", StringComparison.Ordinal) > -1);
            result.WarningLines.Should().Contain(l => l.IndexOf("net5.0", StringComparison.Ordinal) > -1);
        }

        [Fact]
        public void Referencing_CakeCore_With_all_targets_raises_no_warning()
        {
            // given
            fixture.WithPackageReference("Cake.Core", "1.0.0", "all");
            fixture.WithTargetFrameworks("netstandard2.0;net461;net5.0");

            // when
            var result = fixture.Run();

            // then
            result.IsErrorExitCode.Should().BeFalse();
            result.WarningLines.Should().BeEmpty();
        }

        [Fact]
        public void ProjectType_Default_Is_Addin()
        {
            // given
            fixture.WithCustomContent(@"
<Target Name=""ForTest""
  AfterTargets=""BeforeBuild""
  BeforeTargets=""CoreBuild""
  DependsOnTargets=""EnsureProjectTypeIsSet"">

  <Warning Text=""!FOR-TEST!:$(CakeContribGuidelinesProjectType)"" />
</Target>");

            // when
            var result = fixture.Run();

            // then
            result.IsErrorExitCode.Should().BeFalse();
            var output = result.WarningLines
                .First(x => x.IndexOf("!FOR-TEST!:", StringComparison.OrdinalIgnoreCase) > -1);
            output = output.Substring(output.IndexOf("!FOR-TEST!:", StringComparison.OrdinalIgnoreCase)+11);
            output.Should().Be("Addin");
        }

        [Fact]
        public void ProjectType_Manually_Set_Is_Not_Overridden()
        {
            // given
            fixture.WithCustomContent(@"
<PropertyGroup>
  <CakeContribGuidelinesProjectType>MyCustomProjectType</CakeContribGuidelinesProjectType>
</PropertyGroup>

<Target Name=""ForTest""
  AfterTargets=""BeforeBuild""
  BeforeTargets=""CoreBuild""
  DependsOnTargets=""EnsureProjectTypeIsSet"">

  <Warning Text=""!FOR-TEST!:$(CakeContribGuidelinesProjectType)"" />
</Target>");

            // when
            var result = fixture.Run();

            // then
            result.IsErrorExitCode.Should().BeFalse();
            var output = result.WarningLines
                .First(x => x.IndexOf("!FOR-TEST!:", StringComparison.OrdinalIgnoreCase) > -1);
            output = output.Substring(output.IndexOf("!FOR-TEST!:", StringComparison.OrdinalIgnoreCase)+11);
            output.Should().Be("MyCustomProjectType");
        }

        [Fact]
        public void ProjectType_When_Assembly_Is_Module_Is_Module()
        {
            // given
            fixture.WithCustomContent(@"
<PropertyGroup>
  <AssemblyName>Cake.Buildsystems.Module</AssemblyName>
</PropertyGroup>

<Target Name=""ForTest""
  AfterTargets=""BeforeBuild""
  BeforeTargets=""CoreBuild""
  DependsOnTargets=""EnsureProjectTypeIsSet"">

  <Warning Text=""!FOR-TEST!:$(CakeContribGuidelinesProjectType)"" />
</Target>");

            // when
            var result = fixture.Run();

            // then
            result.IsErrorExitCode.Should().BeFalse();
            var output = result.WarningLines
                .First(x => x.IndexOf("!FOR-TEST!:", StringComparison.OrdinalIgnoreCase) > -1);
            output = output.Substring(output.IndexOf("!FOR-TEST!:", StringComparison.OrdinalIgnoreCase)+11);
            output.Should().Be("Module");
        }

        [Fact]
        public void ProjectType_When_PackageId_Is_Module_Is_Module()
        {
            // given
            fixture.WithCustomContent(@"
<PropertyGroup>
  <PackageId>Cake.Buildsystems.Module</PackageId>
</PropertyGroup>

<Target Name=""ForTest""
  AfterTargets=""BeforeBuild""
  BeforeTargets=""CoreBuild""
  DependsOnTargets=""EnsureProjectTypeIsSet"">

  <Warning Text=""!FOR-TEST!:$(CakeContribGuidelinesProjectType)"" />
</Target>");

            // when
            var result = fixture.Run();

            // then
            result.IsErrorExitCode.Should().BeFalse();
            var output = result.WarningLines
                .First(x => x.IndexOf("!FOR-TEST!:", StringComparison.OrdinalIgnoreCase) > -1);
            output = output.Substring(output.IndexOf("!FOR-TEST!:", StringComparison.OrdinalIgnoreCase)+11);
            output.Should().Be("Module");
        }

        [Fact]
        public void ProjectType_When_Assembly_Is_Not_Module_Is_Addin()
        {
            // given
            fixture.WithCustomContent(@"
<PropertyGroup>
  <AssemblyName>Cake.7zip</AssemblyName>
</PropertyGroup>

<Target Name=""ForTest""
  AfterTargets=""BeforeBuild""
  BeforeTargets=""CoreBuild""
  DependsOnTargets=""EnsureProjectTypeIsSet"">

  <Warning Text=""!FOR-TEST!:$(CakeContribGuidelinesProjectType)"" />
</Target>");

            // when
            var result = fixture.Run();

            // then
            result.IsErrorExitCode.Should().BeFalse();
            var output = result.WarningLines
                .First(x => x.IndexOf("!FOR-TEST!:", StringComparison.OrdinalIgnoreCase) > -1);
            output = output.Substring(output.IndexOf("!FOR-TEST!:", StringComparison.OrdinalIgnoreCase)+11);
            output.Should().Be("Addin");
        }
    }
}
