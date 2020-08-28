#load nuget:?package=Cake.Recipe&version=1.1.2

Environment.SetVariableNames();

BuildParameters.SetParameters(
    context: Context,
    buildSystem: BuildSystem,
    sourceDirectoryPath: "./src",
    nuspecFilePath: "./src/CakeContrib.Guidelines.nuspec",
    masterBranchName: "main",
    title: "CakeContrib.Guidelines",
    repositoryOwner: "nils-a",
    repositoryName: "CakeContrib.Guidelines",
    shouldRunGitVersion: true);

BuildParameters.PrintParameters(Context);

ToolSettings.SetToolSettings(context: Context);

Build.RunNuGet();
