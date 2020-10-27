#load nuget:?package=Cake.Recipe&version=2.0.1

Environment.SetVariableNames();

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
    repositoryOwner: "cake-contrib");

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
