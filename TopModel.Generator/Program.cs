using System.CommandLine;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using TopModel.Core;
using TopModel.Core.Loaders;
using TopModel.Generator;
using TopModel.Utils.Cli;

var excludeOption = new Option<IEnumerable<string>>("--exclude", "-e")
{
    Description = GeneratorMessage.ExcludeOptionDescription.GetMessage(),
};
var schemaOption = new Option<bool>("--schema", "-s")
{
    Description = GeneratorMessage.SchemaOptionDescription.GetMessage(),
};
var updateOption = new Option<string>("--update", "-u")
{
    Description = GeneratorMessage.UpdateOptionDescription.GetMessage(),
};

using var command = new TopModelCommand<GeneratorMessage, ModelConfig, FileChecker, ModgenWorker>(
    GeneratorMessage.RootCommandDescription,
    args,
    excludeOption,
    updateOption,
    schemaOption
);

if (await command.IsHelpOrVersionRequested())
{
    return 0;
}

var excludedTags = command.Args.GetValue(excludeOption)?.ToArray() ?? [];
var schemaMode = command.Args.GetValue(schemaOption);
var updateMode = command.Args.GetValue(updateOption);

if (await command.CheckVersionAndFindConfigs("TopModel.Generator", new Regex("topmodel\\.?([a-zA-Z-_.]*)\\.config$")))
{
    return 1;
}

if (excludedTags.Length > 0)
{
    AnsiConsole.LogInformation(GeneratorMessage.ExcludedTags, string.Join(", ", excludedTags));
}

if (updateMode != null)
{
    AnsiConsole.LogInformation(GeneratorMessage.UpdateModeEnabled, updateMode);
    await NugetUtils.ClearAsync(command.CancellationToken);
}

var modelFileLoader = new ServiceCollection()
    .AddLogging(builder => builder.AddProvider(command.LoggerProvider))
    .AddModelFileLoader(command.FileChecker)
    .BuildServiceProvider()
    .GetRequiredService<ModelFileLoader>();

return await command.RunConfigs(worker =>
{
    worker.Services.AddSingleton(modelFileLoader);

    worker.UpdateMode = updateMode;
    worker.ExcludedTags = excludedTags;
    worker.SchemaMode = schemaMode;
});
