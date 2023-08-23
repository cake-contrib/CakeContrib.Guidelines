#load nuget:?package=Cake.Recipe&version=3.1.1

Environment.SetVariableNames();

var standardNotificationMessage = "Version {0} of {1} has just been released, this will be available here https://www.nuget.org/packages/{1}, once package indexing is complete.";

BuildParameters.SetParameters(
    context: Context,
    buildSystem: BuildSystem,
    sourceDirectoryPath: "./src",
    masterBranchName: "main",
    title: "CakeContrib.Guidelines",
    shouldRunDotNetCorePack: true,
    shouldDocumentSourceFiles: false,
    testFilePattern: "/**/*.Tests.csproj", // omit integration-tests in CI-Build
    repositoryOwner: "cake-contrib",
    gitterMessage: "@/all " + standardNotificationMessage,
    twitterMessage: standardNotificationMessage,
    preferredBuildProviderType: BuildProviderType.GitHubActions);

BuildParameters.PrintParameters(Context);

ToolSettings.SetToolSettings(context: Context);
ToolSettings.SetToolPreprocessorDirectives(
    reSharperTools: "#tool nuget:?package=JetBrains.ReSharper.CommandLineTools&version=2022.2.4");

Build.RunDotNetCore();
