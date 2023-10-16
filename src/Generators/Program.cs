using Generators.Commands;

using Spectre.Console.Cli;

var app = new CommandApp();
app.Configure(config =>
{
    config.AddCommand<PackageReferencesCommand>("package-references")
        .WithAlias("packagereferences")
        .WithDescription("parses package references of Cake (Core/Common) and creates a dictionary to copy into the code.");
});

return app.Run(args);
