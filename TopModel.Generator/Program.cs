﻿using System.Linq;
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

            logger.LogInformation("Début de la génération...");

            logger.LogInformation(string.Empty);
            logger.LogInformation("Chargement du modèle...");
            provider.GetService<ModelStore>().LoadFromConfig();
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
        }
    }
}