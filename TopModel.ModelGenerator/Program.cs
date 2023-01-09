using System.CommandLine;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using SharpYaml.Serialization;
using TopModel.ModelGenerator;
using TopModel.ModelGenerator.OpenApi;
using TopModel.Utils;

var command = new RootCommand("Lance le générateur de fichiers tmd.") { Name = "tmdgen" };
var watchMode = false;
var regularCommand = false;
var configs = new List<(ModelGeneratorConfig Config, string FullPath, string DirectoryName)>();

var fileOption = new Option<IEnumerable<FileInfo>>(new[] { "-f", "--file" }, "Chemin vers un fichier de config.");
var watchOption = new Option<bool>(new[] { "-w", "--watch" }, "Lance le générateur en mode 'watch'");
command.AddOption(fileOption);
command.AddOption(watchOption);
command.SetHandler(
    (files, watch) =>
    {
        regularCommand = true;
        watchMode = watch;

        if (files.Count() > 0)
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
                    configs.Add((new Serializer().Deserialize<ModelGeneratorConfig>(stream), file.FullName, file.DirectoryName!));
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
                    configs.Add((new Serializer().Deserialize<ModelGeneratorConfig>(stream.ReadToEnd()), fileName, foundFile.DirectoryName!));
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


var fullVersion = System.Reflection.Assembly.GetEntryAssembly()!.GetName().Version!;
var version = $"{fullVersion.Major}.{fullVersion.Minor}.{fullVersion.Build}";
var colors = new[] { ConsoleColor.DarkCyan, ConsoleColor.DarkYellow, ConsoleColor.Cyan, ConsoleColor.Yellow };

Console.WriteLine($"========= ModelGenerator v{version} =========");
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

foreach (var (config, FullPath, DirectoryName) in configs)
{
    if (config.OpenApi is null)
    {
        return;
    }
    ModelUtils.CombinePath(DirectoryName, config, c => c.ModelRoot);

    using var loggerFactory = LoggerFactory.Create(l => l.AddSimpleConsole(c => c.SingleLine = true));
    var logger = loggerFactory.CreateLogger("ModelGenerator");
    await OpenApiTmdGenerator.GenerateOpenApi(config.OpenApi, logger, DirectoryName, config.ModelRoot);
    logger.LogInformation("Mise à jour terminée avec succès.");
    if (watchMode)
    {
        var fsWatcher = new FileSystemWatcher(DirectoryName, "tmdgen*.config");
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
                   var file = new FileInfo(args.FullPath);
                   using var stream = file.OpenRead();
                   var newConfig = new Serializer().Deserialize<ModelGeneratorConfig>(stream);
                   if (newConfig.OpenApi != null)
                   {
                       await OpenApiTmdGenerator.GenerateOpenApi(newConfig.OpenApi, logger, DirectoryName, config.ModelRoot);
                   }
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

