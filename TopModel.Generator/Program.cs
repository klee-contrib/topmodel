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
using TopModel.Utils;

var fileChecker = new FileChecker("schema.config.json");

var configs = new List<(FullConfig Config, string FullPath, string DirectoryName)>();
var watchMode = false;
var checkMode = false;
var regularCommand = false;
var returnCode = 0;

var command = new RootCommand("Lance le générateur topmodel.") { Name = "modgen" };

var fileOption = new Option<IEnumerable<FileInfo>>(new[] { "-f", "--file" }, "Chemin vers un fichier de config.");
var watchOption = new Option<bool>(new[] { "-w", "--watch" }, "Lance le générateur en mode 'watch'");
var checkOption = new Option<bool>(new[] { "-c", "--check" }, "Vérifie que le code généré est conforme au modèle.");
command.AddOption(fileOption);
command.AddOption(watchOption);
command.AddOption(checkOption);
command.SetHandler(
    (files, watch, check) =>
    {
        regularCommand = true;
        watchMode = watch;
        checkMode = check;

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
                    try
                    {
                        fileChecker.CheckConfigFile(file.FullName);
                        configs.Add((fileChecker.Deserialize<FullConfig>(file.OpenText().ReadToEnd()), file.FullName, file.DirectoryName!));
                    }
                    catch (ModelException me)
                    {
                        returnCode = 1;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(me.Message);
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
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
                    try
                    {
                        fileChecker.CheckConfigFile(fileName);
                        configs.Add((fileChecker.Deserialize<FullConfig>(foundFile.OpenText().ReadToEnd()), fileName, foundFile.DirectoryName!));
                    }
                    catch (ModelException me)
                    {
                        returnCode = 1;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(me.Message);
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                }
            }
        }
    },
    fileOption,
    watchOption,
    checkOption);

await command.InvokeAsync(args);

if (!regularCommand)
{
    return returnCode;
}

if (!configs.Any())
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Aucun fichier de configuration trouvé.");
    Console.ForegroundColor = ConsoleColor.Gray;
    return 1;
}

var fullVersion = System.Reflection.Assembly.GetEntryAssembly()!.GetName().Version!;
var version = $"{fullVersion.Major}.{fullVersion.Minor}.{fullVersion.Build}";

var colors = new[] { ConsoleColor.DarkCyan, ConsoleColor.DarkYellow, ConsoleColor.Cyan, ConsoleColor.Yellow };

Console.WriteLine($"========= TopModel.Generator v{version} =========");
Console.WriteLine();

if (watchMode)
{
    Console.Write("Mode");
    Console.ForegroundColor = ConsoleColor.DarkCyan;
    Console.Write(" watch ");
    Console.ForegroundColor = ConsoleColor.Gray;
    Console.WriteLine("activé.");
}

if (checkMode)
{
    Console.Write("Mode");
    Console.ForegroundColor = ConsoleColor.DarkCyan;
    Console.Write(" check ");
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
var loggerProvider = new LoggerProvider();
var hasErrors = Enumerable.Range(0, configs.Count).Select(_ => false).ToArray();

for (var i = 0; i < configs.Count; i++)
{
    var (config, _, dn) = configs[i];

    Console.WriteLine();

    var services = new ServiceCollection()
        .AddLogging(builder => builder.AddProvider(loggerProvider))
        .AddModelStore(fileChecker, config, dn)
        .AddProceduralSql(dn, config.ProceduralSql)
        .AddSsdt(dn, config.Ssdt)
        .AddCSharp(dn, config.Csharp)
        .AddJavascript(dn, config.Javascript)
        .AddJpa(dn, config.Jpa)
        .AddTranslationOut(dn, config.Translation);

    var provider = services.BuildServiceProvider();
    disposables.Add(provider);

    var modelStore = provider.GetRequiredService<ModelStore>();
    modelStore.OnResolve += hasError => hasErrors[i] = hasError;

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

if (hasErrors.Any(he => he))
{
    return 1;
}

if (checkMode && loggerProvider.Changes > 0)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine();
    if (loggerProvider.Changes == 1)
    {
        Console.WriteLine($"1 fichier généré a été modifié ou supprimé. Le code généré n'était pas à jour.");
    }
    else
    {
        Console.WriteLine($"{loggerProvider.Changes} fichiers générés ont été modifiés ou supprimés. Le code généré n'était pas à jour.");
    }

    Console.ForegroundColor = ConsoleColor.Gray;

    return 1;
}

return 0;