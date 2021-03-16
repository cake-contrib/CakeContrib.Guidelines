#load nuget:?package=Cake.Recipe&version=2.2.1

Environment.SetVariableNames();

var standardNotificationMessage = "Version {0} of {1} has just been released, this will be available here https://www.nuget.org/packages/{1}, once package indexing is complete.";

BuildParameters.SetParameters(
    context: Context,
    buildSystem: BuildSystem,
    sourceDirectoryPath: "./src",
    masterBranchName: "main",
    title: "CakeContrib.Guidelines",
    repositoryName: "CakeContrib.Guidelines", // workaround for https://github.com/cake-contrib/Cake.Recipe/issues/687
    shouldRunInspectCode: false, // not sure how to resolve all the false-positives
    shouldRunDotNetCorePack: true,
    shouldDocumentSourceFiles: false,
    testFilePattern: "/**/*.Tests.csproj", // omit integration-tests in CI-Build 
    repositoryOwner: "cake-contrib",
    gitterMessage: "@/all " + standardNotificationMessage,
    twitterMessage: standardNotificationMessage);

BuildParameters.PrintParameters(Context);

ToolSettings.SetToolSettings(
    context: Context,
    dupFinderExcludePattern: new string[] 
    {
        $"{BuildParameters.RootDirectoryPath}/{BuildParameters.SourceDirectoryPath}/**/*.AssemblyInfo.cs",
        $"{BuildParameters.RootDirectoryPath}/{BuildParameters.SourceDirectoryPath}/Tasks.IntegrationTests/**/*.cs",
        $"{BuildParameters.RootDirectoryPath}/{BuildParameters.SourceDirectoryPath}/Tasks.Tests/**/*.cs"
    });

Build.RunDotNetCore();
