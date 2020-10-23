using System;
using System.IO;
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
            fixture.WithCustomContent($@"
<ItemGroup>
      <PackageReference Include=""Cake.Core"" Version=""0.38.5"" PrivateAssets=""all"" />
</ItemGroup>");

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
            fixture.WithCustomContent($@"
<ItemGroup>
      <PackageReference Include=""Cake.Core"" Version=""0.38.5"" />
</ItemGroup>");

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
    }
}
