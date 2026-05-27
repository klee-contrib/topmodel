using System.CommandLine;
using System.Text.RegularExpressions;
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

var command = new TopModelCommand<GeneratorMessage>(
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

using var cts = new CancellationTokenSource();
Console.CancelKeyPress += (sender, eventArgs) =>
{
    eventArgs.Cancel = true;
    cts.Cancel();
};

if (await command.FindConfigs("TopModel.Generator", new Regex("topmodel\\.?([a-zA-Z-_.]*)\\.config$"), cts.Token))
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
    await NugetUtils.ClearAsync(cts.Token);
}

var fileChecker = new FileChecker("schema.config.json");
return await command.RunConfigs<ModelConfig, FileChecker, ModgenWorker>(
    fileChecker,
    cts.Token,
    worker =>
    {
        worker.UpdateMode = updateMode;
        worker.ExcludedTags = excludedTags;
        worker.SchemaMode = schemaMode;
    }
);
