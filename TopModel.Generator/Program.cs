using System.CommandLine;
using System.CommandLine.Help;
using System.Text.RegularExpressions;
using Spectre.Console;
using TopModel.Core;
using TopModel.Core.Loaders;
using TopModel.Generator;
using TopModel.Utils.Cli;

var excludeOption = new Option<IEnumerable<string>>("--exclude", "-e")
{
    Description = GeneratorMessage.ExcludeOptionDescription.GetMessage(),
};
var updateOption = new Option<string>("--update", "-u")
{
    Description = GeneratorMessage.UpdateOptionDescription.GetMessage(),
};
var schemaOption = new Option<bool>("--schema", "-s")
{
    Description = GeneratorMessage.SchemaOptionDescription.GetMessage(),
};

var command = new RootCommand(GeneratorMessage.RootCommandDescription.GetMessage())
{
    TopModelCli.FileOption,
    excludeOption,
    TopModelCli.WatchOption,
    TopModelCli.CheckOption,
    updateOption,
    schemaOption,
};

var helpOption = command.Options.OfType<HelpOption>().Single();
var versionOption = command.Options.OfType<VersionOption>().Single();

var result = command.Parse(args);

if (result.GetResult(helpOption) != null || result.GetResult(versionOption) != null)
{
    return await result.InvokeAsync();
}

var files = result.GetValue(TopModelCli.FileOption) ?? [];
var watchMode = result.GetValue(TopModelCli.WatchOption);
var excludedTags = result.GetValue(excludeOption)?.ToArray() ?? [];
var checkMode = result.GetValue(TopModelCli.CheckOption);
var updateMode = result.GetValue(updateOption);
var schemaMode = result.GetValue(schemaOption);

using var cts = new CancellationTokenSource();
await TopModelCli.StartPackage("TopModel.Generator", cts.Token);
var configs = new List<FileInfo>();
var pattern = new Regex("topmodel\\.?([a-zA-Z-_.]*)\\.config$");
foreach (var file in TopModelCli.ResolveFiles(files, pattern))
{
    configs.Add(file);
}

if (configs.Count == 0)
{
    AnsiConsole.LogError(CliMessage.NoConfigFileFound);
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

if (watchMode)
{
    AnsiConsole.LogInformation(CliMessage.WatchModeEnabled);
}

if (checkMode)
{
    AnsiConsole.LogInformation(CliMessage.CheckModeEnabled);
}

TopModelCli.ListFoundFiles(configs.Select(c => c.FullName));

var loggerProvider = new LoggerProvider();
Console.CancelKeyPress += (sender, eventArgs) =>
{
    eventArgs.Cancel = true;
    cts.Cancel();
};

FileChecker FileChecker = new("schema.config.json");

TopModelWorker CreateWorker(ModelConfig config, FileInfo configInfo, int configIndex)
{
    return new TopModelWorker(config, configInfo.FullName, loggerProvider, configIndex, FileChecker)
    {
        UpdateMode = updateMode,
        SchemaMode = schemaMode,
        WatchMode = watchMode,
        ExcludedTags = excludedTags,
    };
}
IList<ConfigObserver> ConfigObservers = [];
for (var i = 0; i < configs.Count; i++)
{
    var config = configs[i];
    ConfigObservers.Add(new ConfigObserver(config, FileChecker, i, CreateWorker));
}

try
{
    foreach (var configObserver in ConfigObservers)
    {
        await configObserver.Start(cts.Token);
    }

    if (watchMode)
    {
        cts.Token.WaitHandle.WaitOne();
    }

    if (ConfigObservers.Any(w => w.HasError))
    {
        return 1;
    }

    if (checkMode && loggerProvider.Changes > 0)
    {
        AnsiConsole.WriteLine();
        AnsiConsole.LogError(
            loggerProvider.Changes == 1
                ? CliMessage.OneFileModifiedInCheckMode
                : CliMessage.MultipleFilesModifiedInCheckMode,
            loggerProvider.Changes
        );

        return 1;
    }

    return 0;
}
finally
{
    foreach (var configObserver in ConfigObservers)
    {
        configObserver.Dispose();
    }
}
