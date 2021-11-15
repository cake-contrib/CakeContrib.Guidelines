using System;
using System.IO;
using System.Linq;

using CakeContrib.Guidelines.Tasks.IntegrationTests.Fixtures;

using Shouldly;

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
            fixture.WithCustomContent(@"
<Target Name=""ForTest""
  AfterTargets=""BeforeBuild""
  BeforeTargets=""CoreBuild""
  DependsOnTargets=""_EnsureCakeContribGuidelinesIcon"">

  <Warning Text=""!FOR-TEST!:$(PackageIcon)"" />
</Target>");

            // when
            var result = fixture.Run();

            // then
            result.IsErrorExitCode.ShouldBeFalse();
            var output = result.WarningLines
                .First(x => x.IndexOf("!FOR-TEST!:", StringComparison.OrdinalIgnoreCase) > -1);
            output = output.Substring(output.IndexOf("!FOR-TEST!:", StringComparison.OrdinalIgnoreCase)+11);
            output.ShouldBe("icon.png");
        }

        [Fact]
        public void PackageIconUrl_Tag_missing_And_results_in_CCG0002_warning()
        {
            // given
            fixture.WithPackageIcon("icon.png");
            fixture.WithoutPackageIconUrl();
            fixture.WithCustomContent(@"
<PropertyGroup>
  <CakeContribGuidelinesIconOmitImport>True</CakeContribGuidelinesIconOmitImport>
</PropertyGroup>
");

            // when
            var result = fixture.Run();

            // then
            result.IsErrorExitCode.ShouldBeFalse();
            result.WarningLines.ShouldContain(l => l.IndexOf("CCG0002", StringComparison.Ordinal) > -1);
        }

        [Fact]
        public void PackageIcon_Tag_with_wrong_extension_results_in_CCG0003_error()
        {
            // given
            fixture.WithPackageIcon("coolIcon.jpeg");

            // when
            var result = fixture.Run();

            // then
            result.IsErrorExitCode.ShouldBeTrue();
            result.ErrorLines.ShouldContain(l => l.IndexOf("CCG0003", StringComparison.Ordinal) > -1);
        }

        [Fact]
        public void PackageIcon_Tag_with_wrong_extension_and_custom_Icon_include_results_in_CCG0003_error()
        {
            // given
            fixture.WithPackageIcon("icons/coolIcon.jpeg");
            fixture.WithCustomContent(@"
<ItemGroup>
  <None Include=""c:\\something\\coolIcon.jpeg"" Pack=""True"" PackagePath=""icons/coolIcon.jpeg"" />
</ItemGroup>
");

            // when
            var result = fixture.Run();

            // then
            result.IsErrorExitCode.ShouldBeFalse();
            result.WarningLines.ShouldContain(l => l.IndexOf("CCG0003", StringComparison.Ordinal) > -1);
        }

        [Fact]
        public void Referencing_Cake_Core_with_PrivateAssets_results_in_no_error()
        {
            // given
            fixture.WithoutDefaultCakeReference();
            fixture.WithPackageReference("Cake.Core", "0.38.5", "all");

            // when
            var result = fixture.Run();

            // then
            result.IsErrorExitCode.ShouldBeFalse();
            result.ErrorLines.ShouldBeEmpty();
        }

        [Fact]
        public void Referencing_Cake_Core_without_PrivateAssets_results_in_CCG0004_error()
        {
            // given
            fixture.WithoutDefaultCakeReference();
            fixture.WithPackageReference("Cake.Core", "0.38.5");

            // when
            var result = fixture.Run();

            // then
            result.IsErrorExitCode.ShouldBeTrue();
            result.ErrorLines.ShouldContain(l => l.IndexOf("CCG0004", StringComparison.Ordinal) > -1);
        }

        [Fact]
        public void Not_referencing_StyleCop_Analyzers_results_in_CCG0005_warning()
        {
            // given
            fixture.WithoutStylecopReference();

            // when
            var result = fixture.Run();

            // then
            result.IsErrorExitCode.ShouldBeFalse();
            result.WarningLines.ShouldContain(l => l.IndexOf("CCG0005", StringComparison.Ordinal) > -1);
        }

        [Fact]
        public void Not_referencing_StyleCop_Analyzers_and_setting_CakeContribGuidelinesOmitRecommendedReference_results_in_no_warning()
        {
            // given
            fixture.WithoutStylecopReference();
            fixture.OmitRecommendedCakeVersion();
            fixture.WithCustomContent(@"
<ItemGroup>
      <CakeContribGuidelinesOmitRecommendedReference Include=""StyleCop.Analyzers"" />
</ItemGroup>");

            // when
            var result = fixture.Run();

            // then
            result.IsErrorExitCode.ShouldBeFalse();
            result.WarningLines.ShouldBeEmpty();
        }

        [Fact]
        public void Missing_file_stylecopJson_results_in_CCG0006_warning()
        {
            // given
            fixture.WithoutFileStylecopJson();

            // when
            var result = fixture.Run();

            // then
            result.IsErrorExitCode.ShouldBeFalse();
            result.WarningLines.ShouldContain(l => l.IndexOf("CCG0006", StringComparison.Ordinal) > -1);
            result.WarningLines.ShouldContain(l => l.IndexOf("stylecop.json", StringComparison.Ordinal) > -1);
        }

        [Fact]
        public void Missing_file_editorconfig_results_in_CCG0006_warning()
        {
            // given
            fixture.WithoutFileEditorconfig();

            // when
            var result = fixture.Run();

            // then
            result.IsErrorExitCode.ShouldBeFalse();
            result.WarningLines.ShouldContain(l => l.IndexOf("CCG0006", StringComparison.Ordinal) > -1);
            result.WarningLines.ShouldContain(l => l.IndexOf(".editorconfig", StringComparison.Ordinal) > -1);
        }

        [Fact]
        public void Missing_Required_Target_results_in_CCG0007_error()
        {
            // given
            fixture.WithTargetFrameworks("net47");

            // when
            var result = fixture.Run();

            // then
            result.IsErrorExitCode.ShouldBeTrue();
            result.ErrorLines.ShouldContain(l => l.IndexOf("CCG0007", StringComparison.Ordinal) > -1);
            result.ErrorLines.ShouldContain(l => l.IndexOf("netstandard2.0", StringComparison.Ordinal) > -1);
        }

        [Fact]
        public void Missing_Suggested_Target_results_in_CCG0007_warning()
        {
            // given
            fixture.WithTargetFrameworks("netstandard2.0");

            // when
            var result = fixture.Run();

            // then
            result.IsErrorExitCode.ShouldBeFalse();
            result.WarningLines.ShouldContain(l => l.IndexOf("CCG0007", StringComparison.Ordinal) > -1);
            result.WarningLines.ShouldContain(l => l.IndexOf("net461", StringComparison.Ordinal) > -1);
        }

        [Fact]
        public void Missing_Suggested_Target_net5_for_Cake_v100_results_in_CCG0007_warning()
        {
            // given
            fixture.WithoutDefaultCakeReference();
            fixture.WithPackageReference("Cake.Core", "1.0.0", "all");
            fixture.WithTargetFrameworks("netstandard2.0;net461");

            // when
            var result = fixture.Run();

            // then
            result.IsErrorExitCode.ShouldBeFalse();
            result.WarningLines.ShouldContain(l => l.IndexOf("CCG0007", StringComparison.Ordinal) > -1);
            result.WarningLines.ShouldContain(l => l.IndexOf("net5.0", StringComparison.Ordinal) > -1);
        }

        [Fact]
        public void Referencing_CakeCore_With_all_targets_raises_no_warning()
        {
            // given
            fixture.WithoutDefaultCakeReference();
            fixture.WithPackageReference("Cake.Core", "1.0.0", "all");
            fixture.WithTargetFrameworks("netstandard2.0;net461;net5.0");
            fixture.OmitRecommendedCakeVersion();

            // when
            var result = fixture.Run();

            // then
            result.IsErrorExitCode.ShouldBeFalse();
            result.WarningLines.ShouldBeEmpty();
        }

        [Fact]
        public void Referencing_CakeCore_With_NetStandard_raises_no_warning_If_PackageType_Is_Module()
        {
            // given
            fixture.WithoutDefaultCakeReference();
            fixture.WithPackageReference("Cake.Core", "1.0.0", "all");
            fixture.WithTargetFrameworks("netstandard2.0");
            fixture.OmitRecommendedCakeVersion();
            fixture.WithCustomContent(@"
<PropertyGroup>
  <PackageId>Cake.Buildsystems.Module</PackageId>
</PropertyGroup>");

            // when
            var result = fixture.Run();

            // then
            result.IsErrorExitCode.ShouldBeFalse();
            result.WarningLines.ShouldBeEmpty();
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
            result.IsErrorExitCode.ShouldBeFalse();
            var output = result.WarningLines
                .First(x => x.IndexOf("!FOR-TEST!:", StringComparison.OrdinalIgnoreCase) > -1);
            output = output.Substring(output.IndexOf("!FOR-TEST!:", StringComparison.OrdinalIgnoreCase)+11);
            output.ShouldBe("Addin", StringCompareShould.IgnoreCase);
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
            result.IsErrorExitCode.ShouldBeFalse();
            var output = result.WarningLines
                .First(x => x.IndexOf("!FOR-TEST!:", StringComparison.OrdinalIgnoreCase) > -1);
            output = output.Substring(output.IndexOf("!FOR-TEST!:", StringComparison.OrdinalIgnoreCase)+11);
            output.ShouldBe("MyCustomProjectType");
        }

        [Fact]
        public void ProjectType_When_Assembly_Is_Module_Is_Module()
        {
            // given
            fixture.WithAssemblyName("Cake.Buildsystems.Module");
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
            result.IsErrorExitCode.ShouldBeFalse();
            var output = result.WarningLines
                .First(x => x.IndexOf("!FOR-TEST!:", StringComparison.OrdinalIgnoreCase) > -1);
            output = output.Substring(output.IndexOf("!FOR-TEST!:", StringComparison.OrdinalIgnoreCase)+11);
            output.ShouldBe("Module", StringCompareShould.IgnoreCase);
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
            result.IsErrorExitCode.ShouldBeFalse();
            var output = result.WarningLines
                .First(x => x.IndexOf("!FOR-TEST!:", StringComparison.OrdinalIgnoreCase) > -1);
            output = output.Substring(output.IndexOf("!FOR-TEST!:", StringComparison.OrdinalIgnoreCase)+11);
            output.ShouldBe("Module", StringCompareShould.IgnoreCase);
        }

        [Fact]
        public void ProjectType_When_Assembly_Is_Not_Module_Is_Addin()
        {
            // given
            fixture.WithAssemblyName("Cake.7zip");
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
            result.IsErrorExitCode.ShouldBeFalse();
            var output = result.WarningLines
                .First(x => x.IndexOf("!FOR-TEST!:", StringComparison.OrdinalIgnoreCase) > -1);
            output = output.Substring(output.IndexOf("!FOR-TEST!:", StringComparison.OrdinalIgnoreCase)+11);
            output.ShouldBe("Addin", StringCompareShould.IgnoreCase);
        }

        [Fact]
        public void Packing_Should_Add_PackageIcon_Property()
        {
            // given
            fixture.WithCustomContent(@"
<PropertyGroup>
  <GeneratePackageOnBuild>true</GeneratePackageOnBuild>
</PropertyGroup>

<Target Name=""ForTest""
  BeforeTargets=""SetNuspecProperties;GenerateNuspec""
  DependsOnTargets=""_EnsureCakeContribGuidelinesIcon"">

  <Warning Text=""!FOR-TEST!:$(PackageIcon)"" />
</Target>");

            // when
            var result = fixture.Run();

            // then
            result.IsErrorExitCode.ShouldBeFalse();
            var output = result.WarningLines
                .First(x => x.IndexOf("!FOR-TEST!:", StringComparison.OrdinalIgnoreCase) > -1);
            output = output.Substring(output.IndexOf("!FOR-TEST!:", StringComparison.OrdinalIgnoreCase)+11);
            output.ShouldBe("icon.png");
        }

        [Fact]
        public void Packaging_Should_Add_The_Icon_As_Link_And_Pack_True()
        {
            // given
            fixture.WithoutPackageIcon();
            fixture.WithoutPackageIconUrl();
            fixture.WithCustomContent(@"
<PropertyGroup>
  <GeneratePackageOnBuild>true</GeneratePackageOnBuild>
</PropertyGroup>

<Target Name=""ForTest""
  BeforeTargets=""SetNuspecProperties;GenerateNuspec""
  DependsOnTargets=""_EnsureCakeContribGuidelinesIcon"">

  <Warning Text=""!FOR-TEST!:%(None.Identity) Pack:%(None.Pack) Link:%(None.Link) PackagePath:%(None.PackagePath)"" />
</Target>");

            // when
            var result = fixture.Run();

            // then
            result.IsErrorExitCode.ShouldBeFalse();
            var output = result.WarningLines
                .Where(x => x.IndexOf("!FOR-TEST!:", StringComparison.OrdinalIgnoreCase) > -1)
                .First(x => x.IndexOf("icon.png", StringComparison.OrdinalIgnoreCase) > -1);
            output = output.Substring(output.IndexOf("!FOR-TEST!:", StringComparison.OrdinalIgnoreCase)+11);
            output.ShouldContain("Pack:True");
            output.ShouldContain("Link:cake-contrib-addin-medium.png");
            output.ShouldContain("PackagePath:");
        }

        [Theory]
        [InlineData("Cake.7zip", "/../images/cake-contrib-addin-medium.png")]
        [InlineData("Cake.Recipe", "/../images/cake-contrib-recipe-medium.png")]
        [InlineData("Cake.Buildsystems.Module", "/../images/cake-contrib-module-medium.png")]
        [InlineData("Polly", "/../images/cake-contrib-community-medium.png")]
        public void Packaging_Different_Types_Should_Add_The_Correct_Icon(string assemblyName, string expectedFileName)
        {
            // given
            fixture.WithoutPackageIcon();
            fixture.WithoutPackageIconUrl();
            fixture.WithAssemblyName(assemblyName);
            fixture.WithCustomContent(@"
<PropertyGroup>
  <GeneratePackageOnBuild>true</GeneratePackageOnBuild>
</PropertyGroup>

<Target Name=""ForTest""
  BeforeTargets=""SetNuspecProperties;GenerateNuspec""
  DependsOnTargets=""_EnsureCakeContribGuidelinesIcon"">

  <Warning Text=""!FOR-TEST!:%(None.Identity) Pack:%(None.Pack) Link:%(None.Link) PackagePath:%(None.PackagePath)"" />
</Target>");

            // when
            var result = fixture.Run();

            // then
            result.IsErrorExitCode.ShouldBeFalse();
            var output = result.WarningLines
                .Where(x => x.IndexOf("!FOR-TEST!:", StringComparison.OrdinalIgnoreCase) > -1)
                .First(x => x.IndexOf("icon.png", StringComparison.OrdinalIgnoreCase) > -1);
            output = output.Substring(output.IndexOf("!FOR-TEST!:", StringComparison.OrdinalIgnoreCase)+11);
            output.ShouldContain(expectedFileName);
        }

        [Theory]
        [InlineData("Cake.7zip", "cake-contrib/graphics/png/addin/cake-contrib-addin-medium.png")]
        [InlineData("Cake.Recipe", "cake-contrib/graphics/png/recipe/cake-contrib-recipe-medium.png")]
        [InlineData("Cake.Buildsystems.Module", "cake-contrib/graphics/png/module/cake-contrib-module-medium.png")]
        [InlineData("Polly", "cake-contrib/graphics/png/community/cake-contrib-community-medium.png")]
        public void Packaging_Different_Types_Should_Add_The_Correct_IconUrl_To_Properties(string assemblyName, string expectedUrl)
        {
            // given
            fixture.WithoutPackageIcon();
            fixture.WithoutPackageIconUrl();
            fixture.WithAssemblyName(assemblyName);
            fixture.WithCustomContent(@"
<PropertyGroup>
  <GeneratePackageOnBuild>true</GeneratePackageOnBuild>
</PropertyGroup>

<Target Name=""ForTest""
  BeforeTargets=""SetNuspecProperties;GenerateNuspec""
  DependsOnTargets=""_EnsureCakeContribGuidelinesIcon"">

  <Warning Text=""!FOR-TEST!:$(PackageIconUrl)"" />
</Target>");

            // when
            var result = fixture.Run();

            // then
            result.IsErrorExitCode.ShouldBeFalse();
            var output = result.WarningLines
                .Single(x => x.IndexOf("!FOR-TEST!:", StringComparison.OrdinalIgnoreCase) > -1);
            output = output.Substring(output.IndexOf("!FOR-TEST!:", StringComparison.OrdinalIgnoreCase)+11);
            output.ShouldContain(expectedUrl);
        }

        [Fact]
        public void Missing_Addin_Tag_Should_Raise_CCG0008()
        {
            // given
            fixture.WithTags("cake build cake-build script");

            // when
            var result = fixture.Run();

            // then
            result.IsErrorExitCode.ShouldBeFalse();
            result.WarningLines.ShouldSatisfyAllConditions(
                    x => x.ShouldContain(l => l.IndexOf("CCG0008", StringComparison.Ordinal) > -1),
                    x => x.ShouldContain(l => l.IndexOf("cake-addin", StringComparison.Ordinal) > -1),
                    x => x.ShouldNotContain(l => l.IndexOf("cake-module", StringComparison.Ordinal) > -1),
                    x => x.ShouldNotContain(l => l.IndexOf("cake-recipe", StringComparison.Ordinal) > -1));
        }

        [Fact]
        public void Missing_Module_Tag_Should_Raise_CCG0008_For_Modules()
        {
            // given
            fixture.WithCustomContent(@"
<PropertyGroup>
  <PackageId>Cake.Buildsystems.Module</PackageId>
</PropertyGroup>");
            fixture.WithTags("cake build cake-build script");

            // when
            var result = fixture.Run();

            // then
            result.IsErrorExitCode.ShouldBeFalse();
            result.WarningLines.ShouldSatisfyAllConditions(
                x => x.ShouldContain(l => l.IndexOf("CCG0008", StringComparison.Ordinal) > -1),
                x => x.ShouldContain(l => l.IndexOf("cake-module", StringComparison.Ordinal) > -1),
                x => x.ShouldNotContain(l => l.IndexOf("cake-addin", StringComparison.Ordinal) > -1),
                x => x.ShouldNotContain(l => l.IndexOf("cake-recipe", StringComparison.Ordinal) > -1));
        }

        [Fact]
        public void Missing_Recipe_Tag_Should_Raise_CCG0008_For_Recipes()
        {
            // given
            fixture.WithCustomContent(@"
<PropertyGroup>
  <PackageId>Cake.Recipe</PackageId>
</PropertyGroup>");
            fixture.WithTags("cake build cake-build script");

            // when
            var result = fixture.Run();

            // then
            result.IsErrorExitCode.ShouldBeFalse();
            result.WarningLines.ShouldSatisfyAllConditions(
                    x => x.ShouldContain(l => l.IndexOf("CCG0008", StringComparison.Ordinal) > -1),
                    x => x.ShouldContain(l => l.IndexOf("cake-recipe", StringComparison.Ordinal) > -1),
                    x => x.ShouldNotContain(l => l.IndexOf("cake-addin", StringComparison.Ordinal) > -1),
                    x => x.ShouldNotContain(l => l.IndexOf("cake-module", StringComparison.Ordinal) > -1));
        }

        [Fact]
        public void Delimiting_Tag_By_Comma_Should_Raise_CCG0008()
        {
            // given
            fixture.WithCustomContent(@"
<PropertyGroup>
  <PackageId>Cake.Recipe</PackageId>
</PropertyGroup>");
            fixture.WithTags("cake, build, cake-build, cake-recipe");

            // when
            var result = fixture.Run();

            // then
            result.IsErrorExitCode.ShouldBeFalse();
            result.WarningLines
                .ShouldContain(l => l.IndexOf("CCG0008", StringComparison.Ordinal) > -1);
            result.WarningLines.ShouldContain(l => l.IndexOf("comma", StringComparison.Ordinal) > -1);
        }

        [Fact]
        public void Incorrect_Cake_Reference_Should_Raise_CCG0009()
        {
            // given
            fixture.WithoutDefaultCakeReference();
            fixture.WithPackageReference("cake.core", "0.38.5", "All");

            // when
            var result = fixture.Run();

            // then
            result.IsErrorExitCode.ShouldBeFalse();
            result.WarningLines.ShouldContain(l => l.IndexOf("CCG0009", StringComparison.Ordinal) > -1);
            result.WarningLines.ShouldContain(l => l.IndexOf("cake.core", StringComparison.Ordinal) > -1);
        }

        [Fact]
        public void Incorrect_But_Omitted_Cake_Reference_Should_Not_Raise_CCG0009()
        {
            // given
            fixture.WithoutDefaultCakeReference();
            fixture.WithPackageReference("cake.core", "0.38.5", "All");
            fixture.WithCustomContent(@"
<ItemGroup>
    <CakeContribGuidelinesOmitRecommendedCakeVersion Include=""Cake.Core"" />
</ItemGroup>");

            // when
            var result = fixture.Run();

            // then
            result.IsErrorExitCode.ShouldBeFalse();
            result.WarningLines.ShouldBeEmpty();
        }

        [Fact]
        public void Missing_Suggested_Target_results_not_in_CCG0007_warning_if_NoWarn_is_set()
        {
            // given
            fixture.WithTargetFrameworks("netstandard2.0");
            fixture.WithCustomContent(@"
<PropertyGroup>
    <NoWarn>1701;1702;ccg0007</NoWarn>
</PropertyGroup>");

            // when
            var result = fixture.Run();

            // then
            result.IsErrorExitCode.ShouldBeFalse();
            result.WarningLines.ShouldNotContain(l => l.IndexOf("CCG0007", StringComparison.Ordinal) > -1);
        }

        [Fact]
        public void Missing_Suggested_Target_results_in_CCG0007_error_if_WarningsAsErrors_is_set()
        {
            // given
            fixture.WithTargetFrameworks("netstandard2.0");
            fixture.WithCustomContent(@"
<PropertyGroup>
    <WarningsAsErrors>NU1605;ccg0007</WarningsAsErrors >
</PropertyGroup>");

            // when
            var result = fixture.Run();

            // then
            result.IsErrorExitCode.ShouldBeTrue();
            result.WarningLines.ShouldNotContain(l => l.IndexOf("CCG0007", StringComparison.Ordinal) > -1);
            result.ErrorLines.ShouldContain(l => l.IndexOf("CCG0007", StringComparison.Ordinal) > -1);
        }

    }
}
