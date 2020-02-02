using System;
using System.IO;
using System.Threading;
using TopModel.Generator.CSharp;
using TopModel.Generator.Javascript;
using TopModel.Generator.ProceduralSql;
using TopModel.Generator.Ssdt;
using TopModel.Core.Loaders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TopModel.Generator
{
    using static ModelUtils;

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
            var dn = configFile.DirectoryName;

            var services = new ServiceCollection()
                .AddModelStore(fileChecker, config, dn)
                .AddLogging(builder => builder.AddProvider(new LoggerProvider()));

            if (config.ProceduralSql != null)
            {
                CombinePath(dn, config.ProceduralSql, c => c.CrebasFile);
                CombinePath(dn, config.ProceduralSql, c => c.IndexFKFile);
                CombinePath(dn, config.ProceduralSql, c => c.InitListFile);
                CombinePath(dn, config.ProceduralSql, c => c.TypeFile);
                CombinePath(dn, config.ProceduralSql, c => c.UKFile);

                services
                    .AddSingleton(config.ProceduralSql)
                    .AddSingleton<IModelWatcher, ProceduralSqlGenerator>();
            }

            if (config.Ssdt != null)
            {
                CombinePath(dn, config.Ssdt, c => c.InitListScriptFolder);
                CombinePath(dn, config.Ssdt, c => c.TableScriptFolder);
                CombinePath(dn, config.Ssdt, c => c.TableTypeScriptFolder);

                services
                    .AddSingleton(config.Ssdt)
                    .AddSingleton<IModelWatcher, SsdtGenerator>();
            }

            services.AddSingleton<IModelWatcher, YamlReferenceListGenerator>();

            if (config.Csharp != null)
            {
                CombinePath(dn, config.Csharp, c => c.OutputDirectory);

                services
                    .AddSingleton(config.Csharp)
                    .AddSingleton<IModelWatcher, CSharpGenerator>();
            }

            if (config.Javascript != null)
            {
                CombinePath(dn, config.Javascript, c => c.ModelOutputDirectory);
                CombinePath(dn, config.Javascript, c => c.ResourceOutputDirectory);

                services
                    .AddSingleton(config.Javascript)
                    .AddSingleton<IModelWatcher, TypescriptDefinitionGenerator>()
                    .AddSingleton<IModelWatcher, JavascriptResourceGenerator>();
            }

            using var provider = services.BuildServiceProvider();
            var modelStore = provider.GetService<ModelStore>();

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