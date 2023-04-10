using System.CommandLine;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Core.Loaders;
using TopModel.Generator.Core;
using TopModel.Utils;

var fileChecker = new FileChecker("schema.config.json");

static Type? GetIGenRegInterface(Type t)
{
    return t.GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IGeneratorRegistration<>));
}

var generators = new FileInfo(Assembly.GetEntryAssembly()!.Location).Directory!.GetFiles("TopModel.Generator.*.dll")
    .Select(f => Assembly.LoadFrom(f.FullName))
    .SelectMany(a => a.GetExportedTypes())
    .Where(t => GetIGenRegInterface(t) != null)
    .ToList();

var configs = new List<(ModelConfig Config, string FullPath, string DirectoryName)>();
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

        void HandleFile(FileInfo file)
        {
            try
            {
                fileChecker.CheckConfigFile(file.FullName);
                var text = file.OpenText().ReadToEnd();
                configs.Add((fileChecker.DeserializeConfig(text), file.FullName, file.DirectoryName!));
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
                    HandleFile(file);
                }
            }
        }
        else
        {
            var dir = Directory.GetCurrentDirectory();
            var pattern = "topmodel*.config";
            foreach (var fileName in Directory.GetFiles(dir, pattern, SearchOption.AllDirectories))
            {
                HandleFile(new FileInfo(fileName));
            }

            if (!configs.Any())
            {
                dir = Directory.GetParent(dir)?.FullName;
                while (dir != null)
                {
                    foreach (var fileName in Directory.GetFiles(dir, pattern))
                    {
                        HandleFile(new FileInfo(fileName));
                        dir = null;
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

var fullVersion = Assembly.GetEntryAssembly()!.GetName().Version!;
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
        .AddModelStore(fileChecker, config, dn);

    foreach (var generator in generators)
    {
        var configType = GetIGenRegInterface(generator)!.GetGenericArguments()[0];
        if (config.Generators.TryGetValue(configType.Name.Replace("Config", string.Empty).ToCamelCase(), out var genConfigMaps))
        {
            for (var j = 0; j < genConfigMaps.Count(); j++)
            {
                var genConfigMap = genConfigMaps.ElementAt(j);
                var number = j + 1;

                var genConfig = (GeneratorConfigBase)fileChecker.GetGenConfig(configType, genConfigMap);

                genConfig.InitVariables(config.App, number);

                ModelUtils.CombinePath(dn, genConfig, c => c.OutputDirectory);

                var instance = Activator.CreateInstance(generator);
                instance!.GetType().GetMethod("Register")!
                    .Invoke(
                        instance,
                        new object[] { services, genConfig, number });
            }
        }
    }

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