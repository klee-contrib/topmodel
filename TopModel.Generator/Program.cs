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

            using var provider = new ServiceCollection()
                .AddModelStore(args[0])
                .AddSingleton<IModelWatcher, SsdtGenerator>()
                .AddSingleton<IModelWatcher, ProceduralSqlGenerator>()
                .AddSingleton<IModelWatcher, CSharpGenerator>()
                .AddSingleton<IModelWatcher, TypescriptDefinitionGenerator>()
                .AddSingleton<IModelWatcher, JavascriptResourceGenerator>()
                .AddLogging(builder => builder.AddProvider(new LoggerProvider()))
                .BuildServiceProvider();

            var modelStore = provider.GetService<ModelStore>();

            if (watch)
            {
                modelStore.BeginWatch();
            }

            modelStore.LoadFromConfig();
            
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