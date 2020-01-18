using System;
using System.Linq;
using System.Threading;
using TopModel.Generator.CSharp;
using TopModel.Generator.Javascript;
using TopModel.Generator.ProceduralSql;
using TopModel.Generator.Ssdt;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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
            } else if (args[1] == "watch" || args[1] == "-w" || args[1] == "--watch")
            {
                watch = true;
                configFileName = args[0];
            } else
            {
                throw new ArgumentException("Arguments invalides. Seuls 'watch', '-w' ou '--watch' peuvent être passés en plus de la config.");
            }

                using var provider = new ServiceCollection()
                .AddModelStore(args[0])
                .AddSingleton<IGenerator, SsdtGenerator>()
                .AddSingleton<IGenerator, ProceduralSqlGenerator>()
                .AddSingleton<IGenerator, CSharpGenerator>()
                .AddSingleton<IGenerator, TypescriptDefinitionGenerator>()
                .AddSingleton<IGenerator, JavascriptResourceGenerator>()
                .AddLogging(builder => builder.AddProvider(new LoggerProvider()))
                .BuildServiceProvider();

            var logger = provider.GetService<ILogger<IGenerator>>();
            var modelStore = provider.GetService<ModelStore>();

            logger.LogInformation("Début de la génération...");

            logger.LogInformation(string.Empty);
            logger.LogInformation("Chargement du modèle...");
            modelStore.LoadFromConfig();
            logger.LogInformation("Chargement du modèle terminé.");

            foreach (var generator in provider.GetServices<IGenerator>().Where(g => g.CanGenerate))
            {
                logger.LogInformation(string.Empty);
                logger.LogInformation($"Lancement de la génération {generator.Name}...");
                generator.GenerateAll();
                logger.LogInformation($"Génération {generator.Name} terminée.");
            }

            logger.LogInformation(string.Empty);
            logger.LogInformation("Génération terminée.");

            if (watch)
            {
                modelStore.BeginWatch();

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