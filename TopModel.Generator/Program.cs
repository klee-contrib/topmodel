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
using TopModel.Generator.Kasper;
using TopModel.Generator.ProceduralSql;
using TopModel.Generator.Ssdt;
using static TopModel.Utils.ModelUtils;

var fileChecker = new FileChecker("schema.config.json");

var configs = new List<(FullConfig Config, string FullPath, string DirectoryName)>();
var watchMode = false;

var command = new RootCommand("Lance le générateur topmodel.") { Name = "modgen" };

var fileOption = new Option<IEnumerable<FileInfo>>(new[] { "-f", "--file" }, "Chemin vers un fichier de config.");
var watchOption = new Option<bool>(new[] { "-w", "--watch" }, "Lance le générateur en mode 'watch'");
command.AddOption(fileOption);
command.AddOption(watchOption);
command.SetHandler(
    (files, watch) =>
    {
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

if (!configs.Any())
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Aucun fichier de config trouvé.");
    return;
}

var fullVersion = System.Reflection.Assembly.GetEntryAssembly()!.GetName().Version!;
var version = $"{fullVersion.Major}.{fullVersion.Minor}.{fullVersion.Build}";

var colors = new[] { ConsoleColor.DarkCyan, ConsoleColor.DarkYellow, ConsoleColor.Cyan, ConsoleColor.Yellow };

Console.WriteLine($"========= TopModel v{version} =========");
Console.WriteLine();
Console.WriteLine("Configuration trouvées :");

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

    if (config.AllowCompositePrimaryKey && config.Csharp != null)
    {
        throw new ArgumentException("Impossible de spécifier 'allowCompositePrimaryKey' avec 'csharp'.");
    }

    var services = new ServiceCollection()
        .AddModelStore(fileChecker, config, dn)
        .AddLogging(builder => builder.AddProvider(new LoggerProvider()));

    if (config.ProceduralSql != null)
    {
        foreach (var pSqlConfig in config.ProceduralSql)
        {
            CombinePath(dn, pSqlConfig, c => c.CrebasFile);
            CombinePath(dn, pSqlConfig, c => c.IndexFKFile);
            CombinePath(dn, pSqlConfig, c => c.InitListFile);
            CombinePath(dn, pSqlConfig, c => c.TypeFile);
            CombinePath(dn, pSqlConfig, c => c.UniqueKeysFile);

            services.AddSingleton<IModelWatcher>(p =>
                new ProceduralSqlGenerator(p.GetRequiredService<ILogger<ProceduralSqlGenerator>>(), pSqlConfig));
        }
    }

    if (config.Ssdt != null)
    {
        foreach (var ssdtConfig in config.Ssdt)
        {
            CombinePath(dn, ssdtConfig, c => c.InitListScriptFolder);
            CombinePath(dn, ssdtConfig, c => c.TableScriptFolder);
            CombinePath(dn, ssdtConfig, c => c.TableTypeScriptFolder);

            services.AddSingleton<IModelWatcher>(p =>
                new SsdtGenerator(p.GetRequiredService<ILogger<SsdtGenerator>>(), ssdtConfig));
        }
    }

    if (config.Csharp != null)
    {
        foreach (var csharpConfig in config.Csharp)
        {
            CombinePath(dn, csharpConfig, c => c.OutputDirectory);

            services.AddSingleton<IModelWatcher>(p =>
                new CSharpGenerator(p.GetRequiredService<ILogger<CSharpGenerator>>(), csharpConfig, config.App));

            if (csharpConfig.ApiGeneration == ApiGeneration.Server)
            {
                services.AddSingleton<IModelWatcher>(p =>
                    new CSharpApiServerGenerator(p.GetRequiredService<ILogger<CSharpApiServerGenerator>>(), csharpConfig));
            }
            else if (csharpConfig.ApiGeneration == ApiGeneration.Client)
            {
                services.AddSingleton<IModelWatcher>(p =>
                    new CSharpApiClientGenerator(p.GetRequiredService<ILogger<CSharpApiClientGenerator>>(), csharpConfig));
            }
        }
    }

    if (config.Javascript != null)
    {
        foreach (var jsConfig in config.Javascript)
        {
            CombinePath(dn, jsConfig, c => c.ModelOutputDirectory);
            CombinePath(dn, jsConfig, c => c.ResourceOutputDirectory);
            CombinePath(dn, jsConfig, c => c.ApiClientOutputDirectory);

            if (jsConfig.ModelOutputDirectory != null)
            {
                services.AddSingleton<IModelWatcher>(p =>
                    new TypescriptDefinitionGenerator(p.GetRequiredService<ILogger<TypescriptDefinitionGenerator>>(), jsConfig));

                if (jsConfig.ApiClientOutputDirectory != null)
                {
                    if (jsConfig.TargetFramework == TargetFramework.ANGULAR)
                    {
                        services.AddSingleton<IModelWatcher>(p =>
                           new AngularApiClientGenerator(p.GetRequiredService<ILogger<AngularApiClientGenerator>>(), jsConfig));
                    }
                    else
                    {
                        services.AddSingleton<IModelWatcher>(p =>
                           new JavascriptApiClientGenerator(p.GetRequiredService<ILogger<JavascriptApiClientGenerator>>(), jsConfig));
                    }
                }
            }

            if (jsConfig.ResourceOutputDirectory != null)
            {
                services.AddSingleton<IModelWatcher>(p =>
                    new JavascriptResourceGenerator(p.GetRequiredService<ILogger<JavascriptResourceGenerator>>(), jsConfig));
            }
        }
    }

    if (config.Jpa != null)
    {
        foreach (var jpaConfig in config.Jpa)
        {
            CombinePath(dn, jpaConfig, c => c.ModelOutputDirectory);
            CombinePath(dn, jpaConfig, c => c.ApiOutputDirectory);

            if (jpaConfig.EntitiesPackageName != null || jpaConfig.DtosPackageName != null)
            {
                services
                    .AddSingleton<IModelWatcher>(p =>
                        new JpaModelGenerator(p.GetRequiredService<ILogger<JpaModelGenerator>>(), jpaConfig));
                services
                    .AddSingleton<IModelWatcher>(p =>
                        new JpaModelInterfaceGenerator(p.GetRequiredService<ILogger<JpaModelInterfaceGenerator>>(), jpaConfig));
            }

            if (jpaConfig.DaosPackageName != null)
            {
                services
                    .AddSingleton<IModelWatcher>(p =>
                        new JpaDaoGenerator(p.GetRequiredService<ILogger<JpaDaoGenerator>>(), jpaConfig));
            }

            if (jpaConfig.ApiOutputDirectory != null)
            {
                if (jpaConfig.ApiGeneration == ApiGeneration.Server)
                {
                    services
                        .AddSingleton<IModelWatcher>(p =>
                            new SpringServerApiGenerator(p.GetRequiredService<ILogger<SpringServerApiGenerator>>(), jpaConfig));
                }
                else if (jpaConfig.ApiGeneration == ApiGeneration.Client)
                {
                    services
                        .AddSingleton<IModelWatcher>(p =>
                            new SpringClientApiGenerator(p.GetRequiredService<ILogger<SpringClientApiGenerator>>(), jpaConfig));
                }
            }
        }
    }

    if (config.Kasper != null)
    {
        foreach (var kasperConfig in config.Kasper)
        {
            CombinePath(dn, kasperConfig, c => c.SourcesDirectory);

            services.AddSingleton<IModelWatcher>(p =>
                new KasperGenerator(p.GetRequiredService<ILogger<KasperGenerator>>(), kasperConfig));
        }
    }

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