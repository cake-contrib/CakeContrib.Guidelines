#load nuget:?package=Cake.Recipe&version=4.0.0

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
    twitterMessage: standardNotificationMessage,
    shouldRunCodecov: false,
    preferredBuildProviderType: BuildProviderType.GitHubActions);

BuildParameters.PrintParameters(Context);

ToolSettings.SetToolSettings(context: Context);

Build.RunDotNetCore();
