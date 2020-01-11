using System;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using YamlDotNet.Serialization;
using Microsoft.Extensions.DependencyInjection;
using YamlDotNet.Serialization.NamingConventions;

namespace TopModel.Core.Config
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddConfig(this IServiceCollection services, string filePath)
        {            
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            services.AddSingleton(deserializer);

            var configFile = new FileInfo(filePath);
            var config = deserializer.Deserialize<Config>(configFile.OpenText().ReadToEnd());
            config.ModelRoot ??= string.Empty;
            config.Domains ??= "domains.yml";

            CombinePath(config, c => c.ModelRoot);
            CombinePath(config, c => c.Domains);
            CombinePath(config, c => c.StaticLists);
            CombinePath(config, c => c.ReferenceLists);

            services.AddSingleton<RootConfig>(config);

            if (config.ProceduralSql != null)
            {
                CombinePath(config.ProceduralSql, c => c.CrebasFile);
                CombinePath(config.ProceduralSql, c => c.IndexFKFile);
                CombinePath(config.ProceduralSql, c => c.ReferenceListFile);
                CombinePath(config.ProceduralSql, c => c.StaticListFile);
                CombinePath(config.ProceduralSql, c => c.TypeFile);
                CombinePath(config.ProceduralSql, c => c.UKFile);

                services.AddSingleton(config.ProceduralSql);
            }

            if (config.Ssdt != null)
            {
                CombinePath(config.Ssdt, c => c.InitReferenceListScriptFolder);
                CombinePath(config.Ssdt, c => c.InitStaticListScriptFolder);
                CombinePath(config.Ssdt, c => c.TableScriptFolder);
                CombinePath(config.Ssdt, c => c.TableTypeScriptFolder);

                services.AddSingleton(config.Ssdt);
            }

            if (config.Csharp != null)
            {
                CombinePath(config.Csharp, c => c.OutputDirectory);

                services.AddSingleton(config.Csharp);
            }

            if (config.Javascript != null)
            {
                CombinePath(config.Javascript, c => c.ModelOutputDirectory);
                CombinePath(config.Javascript, c => c.ResourceOutputDirectory);

                services.AddSingleton(config.Javascript);
            }

            void CombinePath<T>(T classe, Expression<Func<T, string?>> getter)
            {
                var property = (PropertyInfo)((MemberExpression)getter.Body).Member;

                if (property.GetValue(classe) != null)
                {
                    property.SetValue(classe, Path.GetFullPath(Path.Combine(configFile.DirectoryName, (string)property.GetValue(classe)!)));
                }
            }

            return services;
        }
    }
}
