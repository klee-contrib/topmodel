using System.CommandLine;
using System.Reflection;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using SharpYaml.Serialization;
using TopModel.ModelGenerator;
using TopModel.ModelGenerator.Database;
using TopModel.ModelGenerator.OpenApi;
using TopModel.Utils;

var command = new RootCommand("Lance le générateur de fichiers tmd.") { Name = "tmdgen" };
var watchMode = false;
var regularCommand = false;
var configs = new List<(ModelGeneratorConfig Config, string FullPath, string DirectoryName)>();
var serializer = new Serializer(new() { NamingConvention = new CamelCaseNamingConvention() });

var fileOption = new Option<IEnumerable<FileInfo>>(new[] { "-f", "--file" }, "Chemin vers un fichier de config.");
var watchOption = new Option<bool>(new[] { "-w", "--watch" }, "Lance le générateur en mode 'watch'");
command.AddOption(fileOption);
command.AddOption(watchOption);
command.SetHandler(
    (files, watch) =>
    {
        regularCommand = true;
        watchMode = watch;

        if (files.Any())
        {
            foreach (var file in files)
            {
                if (!file.Exists)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Le fichier '{file.FullName}' est introuvable.");
                }
                else
                {
                    using var stream = file.OpenRead();
                    configs.Add((serializer.Deserialize<ModelGeneratorConfig>(stream)!, file.FullName, file.DirectoryName!));
                }
            }
        }
        else
        {
            foreach (var fileName in Directory.GetFiles(Directory.GetCurrentDirectory(), "tmdgen*.config", SearchOption.AllDirectories))
            {
                var foundFile = new FileInfo(fileName);
                if (foundFile != null)
                {
                    using var stream = foundFile.OpenText();
                    configs.Add((serializer.Deserialize<ModelGeneratorConfig>(stream.ReadToEnd())!, fileName, foundFile.DirectoryName!));
                }
            }
        }
    },
    fileOption,
    watchOption);

await command.InvokeAsync(args);

if (!regularCommand)
{
    return;
}

if (!configs.Any())
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Aucun fichier de config trouvé.");
    return;
}

var fullVersion = Assembly.GetEntryAssembly()!.GetName().Version!;
var version = $"{fullVersion.Major}.{fullVersion.Minor}.{fullVersion.Build}";
var colors = new[] { ConsoleColor.DarkCyan, ConsoleColor.DarkYellow, ConsoleColor.Cyan, ConsoleColor.Yellow };

Console.WriteLine($"======= TopModel.ModelGenerator v{version} =======");
Console.WriteLine();
if (watchMode)
{
    Console.Write("Mode");
    Console.ForegroundColor = ConsoleColor.DarkCyan;
    Console.Write(" watch ");
    Console.ForegroundColor = ConsoleColor.Gray;
    Console.WriteLine("activé.");
}

Console.WriteLine("Fichiers de configuration trouvés :");

for (var p = 0; p < configs.Count; p++)
{
    var (_, fullName, _) = configs[p];
    Console.ForegroundColor = colors[p % colors.Length];
    Console.Write($"#{p + 1} - ");
    Console.WriteLine(Path.GetRelativePath(Directory.GetCurrentDirectory(), fullName));
}

var disposables = new List<IDisposable>();

var fsCache = new MemoryCache(new MemoryCacheOptions());
Dictionary<string, string> passwords = new();

async Task StartGeneration(string filePath, string directoryName, int i)
{
    Console.WriteLine();

    var configFile = new FileInfo(filePath);
    using var stream = configFile.OpenRead();
    var config = serializer.Deserialize<ModelGeneratorConfig>(stream)!;

    ModelUtils.CombinePath(directoryName, config, c => c.ModelRoot);

    var services = new ServiceCollection()
        .AddLogging(builder => builder.AddProvider(new LoggerProvider()));

    foreach (var conf in config.OpenApi)
    {
        ModelUtils.TrimSlashes(conf, c => c.OutputDirectory);
        services.AddSingleton<ModelGenerator>(p => new OpenApiTmdGenerator(p.GetRequiredService<ILogger<OpenApiTmdGenerator>>(), conf)
        {
            DirectoryName = directoryName,
            ModelRoot = config.ModelRoot,
            Number = config.OpenApi.IndexOf(conf) + 1
        });
    }

    foreach (var conf in config.Database)
    {
        ModelUtils.TrimSlashes(conf, c => c.OutputDirectory);
        services.AddSingleton<ModelGenerator>(p => new DatabaseTmdGenerator(p.GetRequiredService<ILogger<DatabaseTmdGenerator>>(), conf)
        {
            DirectoryName = directoryName,
            ModelRoot = config.ModelRoot,
            Number = config.Database.IndexOf(conf) + 1,
            Passwords = passwords
        });
    }

    using var provider = services.BuildServiceProvider();

    var mainLogger = provider.GetRequiredService<ILogger<ModelGenerator>>();
    var loggingScope = new LoggingScope(i + 1, colors[i]);
    using var scope = mainLogger.BeginScope(loggingScope);

    var generators = provider.GetRequiredService<IEnumerable<ModelGenerator>>();

    mainLogger.LogInformation($"Générateurs enregistrés :\n                          {string.Join("\n                          ", generators.Select(g => $"- {g.Name}@{{{g.Number}}}"))}");

    TopModelLock tmdLock = new();
    var lockFile = new FileInfo(Path.Combine(config.ModelRoot, config.LockFileName));
    if (lockFile.Exists)
    {
        try
        {
            using var file = lockFile.OpenText();
            tmdLock = serializer.Deserialize<TopModelLock>(file)!;
        }
        catch
        {
            mainLogger.LogError($"Erreur à la lecture du fichier {config.LockFileName}. Merci de rétablir la version générée automatiquement.");
        }
    }

    tmdLock.Init(mainLogger);

    var generatedFiles = new List<string>();

    foreach (var generator in generators)
    {
        generatedFiles.AddRange(await generator.Generate(loggingScope));
    }

    tmdLock.Update(config.ModelRoot, config.LockFileName, mainLogger, generatedFiles);

    mainLogger.LogInformation("Mise à jour terminée avec succès.");
};


foreach (var config in configs)
{
    ModelUtils.CombinePath(config.DirectoryName, config.Config, c => c.ModelRoot);

    await StartGeneration(config.FullPath, config.DirectoryName, configs.IndexOf(config));

    if (watchMode)
    {
        var fsWatcher = new FileSystemWatcher(config.DirectoryName, "tmdgen*.config");
        fsWatcher.Changed += (sender, args) =>
        {
            fsCache.Set(args.FullPath, args, new MemoryCacheEntryOptions()
                .AddExpirationToken(new CancellationChangeToken(new CancellationTokenSource(TimeSpan.FromMilliseconds(500)).Token))
                .RegisterPostEvictionCallback(async (k, v, r, a) =>
                {
                    if (r != EvictionReason.TokenExpired)
                    {
                        return;
                    }

                    await StartGeneration(args.FullPath, config.DirectoryName, configs.IndexOf(config));

                }));
        };
        fsWatcher.IncludeSubdirectories = true;
        fsWatcher.EnableRaisingEvents = true;
        disposables.Add(fsWatcher);
    }
}


if (watchMode)
{
    var autoResetEvent = new AutoResetEvent(false);
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

