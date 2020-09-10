#load nuget:?package=Cake.Recipe&version=1.1.2

Environment.SetVariableNames();

BuildParameters.SetParameters(
    context: Context,
    buildSystem: BuildSystem,
    sourceDirectoryPath: "./src",
    nuspecFilePath: "./src/CakeContrib.Guidelines.nuspec",
    masterBranchName: "main",
    title: "CakeContrib.Guidelines",
    shouldPublishMyGet: false, // currently broken
    repositoryOwner: "cake-contrib",
    repositoryName: "CakeContrib.Guidelines",
    shouldDeployGraphDocumentation: false,
    shouldRunGitVersion: true);

BuildParameters.PrintParameters(Context);

ToolSettings.SetToolSettings(context: Context);

Build.RunNuGet();
