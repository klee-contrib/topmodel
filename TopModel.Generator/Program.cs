#pragma warning disable SA1516

using System.CommandLine;
using System.CommandLine.Invocation;
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

FullConfig? config = null;
string dn = null!;
var watchMode = false;

var command = new RootCommand
{
    new Argument<FileInfo>("configFile", () => new FileInfo("topmodel.config"), "Chemin vers le fichier de config."),
    new Option<bool>(new[] { "-w", "--watch" }, "Lance le générateur en mode 'watch'")
};

command.Name = "modgen";

command.Handler = CommandHandler.Create<FileInfo, bool>((configFile, watch) =>
{
    if (!configFile.Exists)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Le fichier '{configFile.FullName}' est introuvable.");
    }
    else
    {
        config = fileChecker.Deserialize<FullConfig>(configFile.OpenText().ReadToEnd());
        dn = configFile.DirectoryName!;
        watchMode = watch;
    }
});

command.Invoke(args);

if (config == null)
{
    return;
}

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
            new CSharpGenerator(p.GetRequiredService<ILogger<CSharpGenerator>>(), csharpConfig));

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
                services.AddSingleton<IModelWatcher>(p =>
                    new JavascriptApiClientGenerator(p.GetRequiredService<ILogger<JavascriptApiClientGenerator>>(), jsConfig));
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

using var provider = services.BuildServiceProvider();

var modelStore = provider.GetRequiredService<ModelStore>();

modelStore.LoadFromConfig(watchMode);

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
