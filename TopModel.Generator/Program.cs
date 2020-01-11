using TopModel.Generator.CSharp;
using TopModel.Generator.Javascript;
using TopModel.Generator.ProceduralSql;
using TopModel.Generator.Ssdt;
using TopModel.Core;
using TopModel.Core.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TopModel.Generator
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var generators = Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                {
                    services
                        .AddConfig(args[0])
                        .AddSingleton<ModelStore>()
                        .AddSingleton<IGenerator, SsdtGenerator>()
                        .AddSingleton<IGenerator, ProceduralSqlGenerator>()
                        .AddSingleton<IGenerator, CSharpGenerator>()
                        .AddSingleton<IGenerator, TypescriptDefinitionGenerator>()
                        .AddSingleton<IGenerator, JavascriptResourceGenerator>();
                })
                .Build()
                .Services
                .GetServices<IGenerator>();

            foreach (var generator in generators)
            {
                generator.Generate();
            }
        }
    }
}