#load nuget:https://pkgs.dev.azure.com/cake-contrib/Home/_packaging/addins%40Local/nuget/v3/index.json?package=Cake.Recipe&version=2.0.0-alpha0471&prerelease

Environment.SetVariableNames();

BuildParameters.SetParameters(
    context: Context,
    buildSystem: BuildSystem,
    sourceDirectoryPath: "./src",
    nuspecFilePath: "./src/CakeContrib.Guidelines.nuspec",
    masterBranchName: "main",
    title: "CakeContrib.Guidelines",
    shouldRunInspectCode: false, // not sure how to resolve all the false-positives
    shouldRunDotNetCorePack: true,
    repositoryOwner: "cake-contrib");

BuildParameters.PrintParameters(Context);

ToolSettings.SetToolSettings(
    context: Context,
    dupFinderExcludePattern: new string[] 
    {
        $"{BuildParameters.RootDirectoryPath}/{BuildParameters.SourceDirectoryPath}/**/*.AssemblyInfo.cs"
    });

Build.RunDotNetCore();
