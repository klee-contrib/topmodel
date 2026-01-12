using System.CommandLine;
using System.CommandLine.Help;
using System.Reflection;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using SharpYaml.Serialization;
using Spectre.Console;
using TopModel.ModelGenerator;
using TopModel.ModelGenerator.Database;
using TopModel.ModelGenerator.OpenApi;
using TopModel.Utils;

var fileOption = new Option<IEnumerable<FileInfo>>("--file", "-f")
{
    Description = "Chemin vers un fichier de config.",
};
var watchOption = new Option<bool>("--watch", "-w") { Description = "Lance le générateur en mode 'watch'" };
var checkOption = new Option<bool>("--check", "-c")
{
    Description = "Vérifie que le modèle généré est conforme aux sources.",
};

var command = new RootCommand("Lance le générateur de fichiers tmd.") { fileOption, watchOption, checkOption };

var helpOption = command.Options.OfType<HelpOption>().Single();
var versionOption = command.Options.OfType<VersionOption>().Single();

var result = command.Parse(args);

if (result.GetResult(helpOption) != null || result.GetResult(versionOption) != null)
{
    return await result.InvokeAsync();
}

var files = result.GetValue(fileOption) ?? [];
var watchMode = result.GetValue(watchOption);
var checkMode = result.GetValue(checkOption);

var configs = new List<(string FullPath, string DirectoryName)>();
var serializer = new Serializer(new() { NamingConvention = new CamelCaseNamingConvention() });

void HandleFile(FileInfo file)
{
    configs.Add((file.FullName, file.DirectoryName!));
}

if (files.Any())
{
    foreach (var file in files)
    {
        if (!file.Exists)
        {
            AnsiConsole.MarkupLine($"[red]Le fichier '{file.FullName}' est introuvable.[/]");
        }
        else
        {
            HandleFile(file);
        }
    }
}
else
{
    var dir = Directory.GetCurrentDirectory();
    var pattern = "tmdgen*.config";
    foreach (var fileName in Directory.GetFiles(dir, pattern, SearchOption.AllDirectories))
    {
        var foundFile = new FileInfo(fileName);
        if (foundFile != null)
        {
            HandleFile(foundFile);
        }
    }

    if (!configs.Any())
    {
        var found = false;
        while (!found && dir != null)
        {
            dir = Directory.GetParent(dir)?.FullName;
            if (dir != null)
            {
                foreach (var fileName in Directory.GetFiles(dir, pattern))
                {
                    HandleFile(new FileInfo(fileName));
                    found = true;
                }
            }
        }
    }
}

if (!configs.Any())
{
    AnsiConsole.MarkupLine($"[red]Aucun fichier de configuration trouvé.[/]");
    return 1;
}

var fullVersion = Assembly.GetEntryAssembly()!.GetName().Version!;
var version = $"{fullVersion.Major}.{fullVersion.Minor}.{fullVersion.Build}";
var colors = new[] { "teal", "olive", "yellow", "aqua" };

AnsiConsole.MarkupLine($"========= TopModel.ModelGenerator v{version} =========");
AnsiConsole.WriteLine();

if (watchMode)
{
    AnsiConsole.MarkupLine("Mode [darkcyan]watch[/] activé.");
}

if (checkMode)
{
    AnsiConsole.MarkupLine("Mode [darkcyan]check[/] activé.");
}

AnsiConsole.WriteLine("Fichiers de configuration trouvés :");

for (var i = 0; i < configs.Count; i++)
{
    var (fullName, _) = configs[i];
    var color = colors[i % colors.Length];
    AnsiConsole.MarkupLine($"[{color}]#{i + 1} - {Path.GetRelativePath(Directory.GetCurrentDirectory(), fullName)}[/]");
}

var disposables = new List<IDisposable>();
var loggerProvider = new LoggerProvider();

var fsCache = new MemoryCache(new MemoryCacheOptions());
Dictionary<string, string> passwords = [];

async Task StartGeneration(string filePath, string directoryName, int i)
{
    AnsiConsole.WriteLine();

    var configFile = new FileInfo(filePath);
    using var stream = configFile.OpenRead();
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

    using var provider = services.BuildServiceProvider();

    var mainLogger = provider.GetRequiredService<ILogger<TmdGenerator>>();
    var loggingScope = new LoggingScope(i + 1, colors[i]);
    using var scope = mainLogger.BeginScope(loggingScope);

    var generators = provider.GetRequiredService<IEnumerable<TmdGenerator>>();

    mainLogger.LogInformation(
        $"Générateurs enregistrés :\n                          {string.Join("\n                          ", generators.Select(g => $"- {g.Name}@{{{g.Number}}}"))}"
    );

    var tmdLock = new TopModelLock(config, mainLogger);
    var generatedFiles = new List<string>();

    foreach (var generator in generators)
    {
        generatedFiles.AddRange(await generator.Generate(loggingScope));
    }

    tmdLock.UpdateFiles(generatedFiles);

    mainLogger.LogInformation("Mise à jour terminée avec succès.");
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
    Console.ForegroundColor = ConsoleColor.Red;
    AnsiConsole.WriteLine();
    if (loggerProvider.Changes == 1)
    {
        AnsiConsole.MarkupLine(
            $"[red]1 fichier généré a été modifié ou supprimé. Le code généré n'était pas à jour.[/]"
        );
    }
    else
    {
        AnsiConsole.MarkupLine(
            $"[red]{loggerProvider.Changes} fichiers générés ont été modifiés ou supprimés. Le code généré n'était pas à jour.[/]"
        );
    }

    return 1;
}

return 0;
