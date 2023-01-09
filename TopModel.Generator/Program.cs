#pragma warning disable SA1516

using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Core.Loaders;
using TopModel.Generator;
using TopModel.Generator.CSharp;
using TopModel.Generator.Javascript;
using TopModel.Generator.Jpa;
using TopModel.Generator.ProceduralSql;
using TopModel.Generator.Ssdt;
using TopModel.Generator.Translation;

var fileChecker = new FileChecker("schema.config.json");

var configs = new List<(FullConfig Config, string FullPath, string DirectoryName)>();
var watchMode = false;
var regularCommand = false;

var command = new RootCommand("Lance le générateur topmodel.") { Name = "modgen" };

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
                    configs.Add((fileChecker.Deserialize<FullConfig>(file.OpenText().ReadToEnd()), file.FullName, file.DirectoryName!));
                }
            }
        }
        else
        {
            foreach (var fileName in Directory.GetFiles(Directory.GetCurrentDirectory(), "topmodel*.config", SearchOption.AllDirectories))
            {
                var foundFile = new FileInfo(fileName);
                if (foundFile != null)
                {
                    configs.Add((fileChecker.Deserialize<FullConfig>(foundFile.OpenText().ReadToEnd()), fileName, foundFile.DirectoryName!));
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
    Console.WriteLine("Aucun fichier de configuration trouvé.");
    return;
}

var fullVersion = System.Reflection.Assembly.GetEntryAssembly()!.GetName().Version!;
var version = $"{fullVersion.Major}.{fullVersion.Minor}.{fullVersion.Build}";

var colors = new[] { ConsoleColor.DarkCyan, ConsoleColor.DarkYellow, ConsoleColor.Cyan, ConsoleColor.Yellow };

Console.WriteLine($"========= TopModel v{version} =========");
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

for (var i = 0; i < configs.Count; i++)
{
    var (_, fullName, _) = configs[i];
    Console.ForegroundColor = colors[i % colors.Length];
    Console.Write($"#{i + 1} - ");
    Console.WriteLine(Path.GetRelativePath(Directory.GetCurrentDirectory(), fullName));
}

var disposables = new List<IDisposable>();

for (var i = 0; i < configs.Count; i++)
{
    var (config, _, dn) = configs[i];

    Console.WriteLine();

    var services = new ServiceCollection()
        .AddLogging(builder => builder.AddProvider(new LoggerProvider()))
        .AddModelStore(fileChecker, config, dn)
        .AddProceduralSql(dn, config.ProceduralSql)
        .AddSsdt(dn, config.Ssdt)
        .AddCSharp(dn, config.App, config.Csharp)
        .AddJavascript(dn, config.Javascript)
        .AddJpa(dn, config.Jpa)
        .AddTranslationOut(dn, config.Translation);

    var provider = services.BuildServiceProvider();
    disposables.Add(provider);

    var modelStore = provider.GetRequiredService<ModelStore>();
    var watcher = modelStore.LoadFromConfig(watchMode, new(i + 1, colors[i % colors.Length]));
    if (watcher != null)
    {
        disposables.Add(watcher);
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
}

foreach (var provider in disposables)
{
    provider.Dispose();
}