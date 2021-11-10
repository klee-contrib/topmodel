using System;
using System.IO;
using System.Threading;
using TopModel.Generator.CSharp;
using TopModel.Generator.Javascript;
using TopModel.Generator.Kasper;
using TopModel.Generator.ProceduralSql;
using TopModel.Generator.Ssdt;
using TopModel.Core.Loaders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using static TopModel.Core.ModelUtils;

namespace TopModel.Generator
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var watch = false;
            string configFileName;
            if (args.Length != 1 && args.Length != 2)
            {
                throw new ArgumentException("1 ou 2 paramètres sont attendus.");
            }

            if (args.Length == 1)
            {
                configFileName = args[0];
            }
            else if (args[0] == "watch" || args[0] == "-w" || args[0] == "--watch")
            {
                watch = true;
                configFileName = args[1];
            }
            else if (args[1] == "watch" || args[1] == "-w" || args[1] == "--watch")
            {
                watch = true;
                configFileName = args[0];
            }
            else
            {
                throw new ArgumentException("Arguments invalides. Seuls 'watch', '-w' ou '--watch' peuvent être passés en plus de la config.");
            }

            var fileChecker = new FileChecker("schema.config.json");
            var configFile = new FileInfo(configFileName);
            var config = fileChecker.Deserialize<FullConfig>(configFile.OpenText().ReadToEnd());

            if (config.AllowCompositePrimaryKey && config.Csharp != null)
            {
                throw new ArgumentException("Impossible de spécifier 'allowCompositePrimaryKey' avec 'csharp'.");
            }

            var dn = configFile.DirectoryName!;

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

            modelStore.LoadFromConfig(watch);

            if (watch)
            {
                var autoResetEvent = new AutoResetEvent(false);
                Console.CancelKeyPress += (sender, eventArgs) =>
                {
                    eventArgs.Cancel = true;
                    autoResetEvent.Set();
                };
                autoResetEvent.WaitOne();
            }
        }
    }
}