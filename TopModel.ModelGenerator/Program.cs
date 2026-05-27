using System.CommandLine;
using System.CommandLine.Help;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using SharpYaml.Serialization;
using Spectre.Console;
using TopModel.Core;
using TopModel.ModelGenerator;
using TopModel.ModelGenerator.Database;
using TopModel.ModelGenerator.OpenApi;
using TopModel.Utils;
using TopModel.Utils.Cli;

var command = new RootCommand("Lance le générateur de fichiers tmd.")
{
    TopModelCli.FileOption,
    TopModelCli.WatchOption,
    TopModelCli.CheckOption,
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
var checkMode = result.GetValue(TopModelCli.CheckOption);

var configs = new List<(string FullPath, string DirectoryName)>();
var serializer = new Serializer(new() { NamingConvention = new CamelCaseNamingConvention() });

void HandleFile(FileInfo file)
{
    configs.Add((file.FullName, file.DirectoryName!));
}

var tmdgenPattern = new Regex(@"tmdgen[^/\\]*\.config$");
foreach (var file in TopModelCli.ResolveFiles(files, tmdgenPattern))
{
    HandleFile(file);
}

if (!configs.Any())
{
    AnsiConsole.MarkupLine($"[red]{LocalizeUtils.Localize(CliMessage.NoConfigFileFound)}[/]");
    return 1;
}

await TopModelCli.StartPackage("TopModel.ModelGenerator", CancellationToken.None);

if (watchMode)
{
    AnsiConsole.MarkupLine(LocalizeUtils.Localize(CliMessage.WatchModeEnabled));
}

if (checkMode)
{
    AnsiConsole.MarkupLine(LocalizeUtils.Localize(CliMessage.CheckModeEnabled));
}

TopModelCli.ListFoundFiles(configs.Select(c => c.FullPath));

var disposables = new List<IDisposable>();
var loggerProvider = new LoggerProvider();

var fsCache = new MemoryCache(new MemoryCacheOptions());
Dictionary<string, string> passwords = [];

async Task StartGeneration(string filePath, string directoryName, int i)
{
    AnsiConsole.WriteLine();

    var configFile = new FileInfo(filePath);
    await using var stream = configFile.OpenRead();
    var config = serializer.Deserialize<ModelGeneratorConfig>(stream)!;

    config.ConfigRoot = directoryName;
    config.ModelRoot ??= "./";
    config.LockFileName ??= "tmdgen.lock";
    ModelUtils.CombinePath(directoryName, config, c => c.ModelRoot);

    var services = new ServiceCollection()
        .AddLogging(builder => builder.AddProvider(loggerProvider))
        .AddSingleton<IFileWriterProvider>(new GeneratedFileWriterProvider(config));

    foreach (var conf in config.OpenApi)
    {
        ModelUtils.TrimSlashes(conf, c => c.OutputDirectory);
        services.AddSingleton<TmdGenerator>(p => new OpenApiTmdGenerator(
            p.GetRequiredService<ILogger<OpenApiTmdGenerator>>(),
            conf,
            p.GetRequiredService<IFileWriterProvider>()
        )
        {
            DirectoryName = directoryName,
            ModelRoot = config.ModelRoot,
            Number = config.OpenApi.IndexOf(conf) + 1,
        });
    }

    foreach (var conf in config.Database)
    {
        ModelUtils.TrimSlashes(conf, c => c.OutputDirectory);
        if (conf.Source.DbType == DbType.ORACLE)
        {
            services.AddSingleton<TmdGenerator>(p => new DatabaseOraTmdGenerator(
                p.GetRequiredService<ILogger<DatabaseOraTmdGenerator>>(),
                conf,
                p.GetRequiredService<IFileWriterProvider>()
            )
            {
                DirectoryName = directoryName,
                ModelRoot = config.ModelRoot,
                Number = config.Database.IndexOf(conf) + 1,
                Passwords = passwords,
            });
        }
        else if (conf.Source.DbType == DbType.POSTGRESQL)
        {
            services.AddSingleton<TmdGenerator>(p => new DatabasePgTmdGenerator(
                p.GetRequiredService<ILogger<DatabasePgTmdGenerator>>(),
                conf,
                p.GetRequiredService<IFileWriterProvider>()
            )
            {
                DirectoryName = directoryName,
                ModelRoot = config.ModelRoot,
                Number = config.Database.IndexOf(conf) + 1,
                Passwords = passwords,
            });
        }
        else if (conf.Source.DbType == DbType.MYSQL)
        {
            services.AddSingleton<TmdGenerator>(p => new DatabaseMySqlTmdGenerator(
                p.GetRequiredService<ILogger<DatabaseMySqlTmdGenerator>>(),
                conf,
                p.GetRequiredService<IFileWriterProvider>()
            )
            {
                DirectoryName = directoryName,
                ModelRoot = config.ModelRoot,
                Number = config.Database.IndexOf(conf) + 1,
                Passwords = passwords,
            });
        }
        else if (conf.Source.DbType == DbType.MSSQL)
        {
            services.AddSingleton<TmdGenerator>(p => new DatabaseMsSqlTmdGenerator(
                p.GetRequiredService<ILogger<DatabaseMsSqlTmdGenerator>>(),
                conf,
                p.GetRequiredService<IFileWriterProvider>()
            )
            {
                DirectoryName = directoryName,
                ModelRoot = config.ModelRoot,
                Number = config.Database.IndexOf(conf) + 1,
                Passwords = passwords,
            });
        }
    }

    await using var provider = services.BuildServiceProvider();

    var mainLogger = provider.GetRequiredService<ILogger<TmdGenerator>>();
    var loggingScope = new LoggingScope(i + 1, TopModelCli.Colors[i % TopModelCli.Colors.Length]);
    using var scope = mainLogger.BeginScope(loggingScope);

    var generators = provider.GetRequiredService<IEnumerable<TmdGenerator>>();

    mainLogger.LogInformation(
        LocalizeUtils.Localize(
            ModelGeneratorMessage.RegisteredGenerators,
            $"\n                          {string.Join("\n                          ", generators.Select(g => $"- {g.Name}@{{{g.Number}}}"))}"
        )
    );

    var tmdLock = new TopModelLock(config, mainLogger);
    var generatedFiles = new List<string>();

    foreach (var generator in generators)
    {
        generatedFiles.AddRange(await generator.Generate(loggingScope));
    }

    tmdLock.UpdateFiles(generatedFiles);

    mainLogger.LogInformation(LocalizeUtils.Localize(ModelGeneratorMessage.UpdateCompleted));
}

foreach (var config in configs)
{
    await StartGeneration(config.FullPath, config.DirectoryName, configs.IndexOf(config));

    if (watchMode)
    {
        var fsWatcher = new FileSystemWatcher(config.DirectoryName, "tmdgen*.config");
        fsWatcher.Changed += (sender, args) =>
        {
            fsCache.Set(
                args.FullPath,
                args,
                new MemoryCacheEntryOptions()
                    .AddExpirationToken(
                        new CancellationChangeToken(new CancellationTokenSource(TimeSpan.FromMilliseconds(500)).Token)
                    )
                    .RegisterPostEvictionCallback(
                        async (k, v, r, a) =>
                        {
                            if (r != EvictionReason.TokenExpired)
                            {
                                return;
                            }

                            await StartGeneration(args.FullPath, config.DirectoryName, configs.IndexOf(config));
                        }
                    )
            );
        };
        fsWatcher.IncludeSubdirectories = true;
        fsWatcher.EnableRaisingEvents = true;
        disposables.Add(fsWatcher);
    }
}

if (watchMode)
{
    var autoResetEvent = new AutoResetEvent(initialState: false);
    Console.CancelKeyPress += (sender, eventArgs) =>
    {
        eventArgs.Cancel = true;
        autoResetEvent.Set();
    };
    autoResetEvent.WaitOne();

    foreach (var disposable in disposables)
    {
        disposable.Dispose();
    }
}

if (checkMode && loggerProvider.Changes > 0)
{
    AnsiConsole.WriteLine();
    AnsiConsole.MarkupLine(
        loggerProvider.Changes == 1
            ? $"[red]{LocalizeUtils.Localize(CliMessage.OneFileModifiedInCheckMode)}[/]"
            : $"[red]{LocalizeUtils.Localize(CliMessage.MultipleFilesModifiedInCheckMode, loggerProvider.Changes)}[/]"
    );

    return 1;
}

return 0;
