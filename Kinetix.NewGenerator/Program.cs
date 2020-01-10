using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Kinetix.NewGenerator.Config;
using Kinetix.NewGenerator.CSharp;
using Kinetix.NewGenerator.Javascript;
using Kinetix.NewGenerator.Loaders;
using Kinetix.NewGenerator.Model;
using Kinetix.NewGenerator.ProceduralSql;
using Kinetix.NewGenerator.Ssdt;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Kinetix.NewGenerator
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var deserializer = new DeserializerBuilder()
               .WithNamingConvention(CamelCaseNamingConvention.Instance)
               .Build();

            var configFile = new FileInfo(args[0]);
            var config = deserializer.Deserialize<RootConfig>(configFile.OpenText().ReadToEnd());
            config.ModelRoot ??= string.Empty;
            config.Domains ??= "domains.yml";

            void CombinePath<T>(T classe, Expression<Func<T, string?>> getter)
            {
                var property = (PropertyInfo)((MemberExpression)getter.Body).Member;

                if (property.GetValue(classe) != null)
                {
                    property.SetValue(classe, Path.Combine(configFile.DirectoryName, (string)property.GetValue(classe)!));
                }
            }

            CombinePath(config, c => c.ModelRoot);
            CombinePath(config, c => c.Domains);
            CombinePath(config, c => c.StaticLists);
            CombinePath(config, c => c.ReferenceLists);

            var files = Directory.EnumerateFiles(config.ModelRoot, "*.yml", SearchOption.AllDirectories)
                .Where(f => f != config.Domains && f != configFile.FullName);

            var domains = DomainsLoader.LoadDomains(config.Domains, deserializer)
                .ToLookup(f => f.Name, f => f)
                .ToDictionary(f => f.Key, f => f.First());

            var classFiles = files
                .Select(file => ClassesLoader.GetFileDescriptor(file, deserializer))
                .ToDictionary(
                    file => (file.descriptor.Module, file.descriptor.Kind, file.descriptor.File),
                    file => file);

            var apps = classFiles.Select(f => f.Value.descriptor.App).Distinct();
            if (apps.Count() != 1)
            {
                throw new Exception("Tous les fichiers doivent être liés à la même 'app'.");
            }
            var rootNamespace = apps.Single();

            var classes = new Dictionary<string, Class>();

            foreach (var (_, (descriptor, parser)) in classFiles)
            {
                ClassesLoader.LoadClasses(descriptor, parser, classes, classFiles, domains, deserializer);
            }

            foreach (var kvp in classes)
            {
                var classe = kvp.Value;
                if (classe.Properties.Count(p => p.PrimaryKey) > 1)
                {
                    throw new Exception($"La classe {classe.Name} doit avoir une seule clé primaire ({string.Join(", ", classe.Properties.Where(p => p.PrimaryKey).Select(p => p.Name))} trouvés)");
                }
            }

            var staticLists = ReferenceListsLoader.LoadReferenceLists(config.StaticLists);
            var referenceLists = ReferenceListsLoader.LoadReferenceLists(config.ReferenceLists);

            if (staticLists != null)
            {
                foreach (var (className, referenceValues) in staticLists)
                {
                    ReferenceListsLoader.AddReferenceValues(classes[className], referenceValues);
                }
            }

            if (referenceLists != null)
            {
                foreach (var (className, referenceValues) in referenceLists)
                {
                    ReferenceListsLoader.AddReferenceValues(classes[className], referenceValues);
                }
            }

            if (config.ProceduralSql != null)
            {
                CombinePath(config.ProceduralSql, c => c.CrebasFile);
                CombinePath(config.ProceduralSql, c => c.IndexFKFile);
                CombinePath(config.ProceduralSql, c => c.ReferenceListFile);
                CombinePath(config.ProceduralSql, c => c.StaticListFile);
                CombinePath(config.ProceduralSql, c => c.TypeFile);
                CombinePath(config.ProceduralSql, c => c.UKFile);

                var schemaGenerator = config.ProceduralSql.TargetDBMS == TargetDBMS.Postgre
                    ? new PostgreSchemaGenerator(rootNamespace, config.ProceduralSql)
                    : (AbstractSchemaGenerator)new SqlServerSchemaGenerator(rootNamespace, config.ProceduralSql);

                schemaGenerator.GenerateSchemaScript(classes.Values);

                if (staticLists != null)
                {
                    schemaGenerator.GenerateListInitScript(
                        staticLists.ToDictionary(s => classes[s.className], s => s.values),
                        isStatic: true);
                }

                if (referenceLists != null)
                {
                    schemaGenerator.GenerateListInitScript(
                        referenceLists.ToDictionary(s => classes[s.className], s => s.values),
                        isStatic: false);
                }
            }

            if (config.Ssdt != null)
            {
                CombinePath(config.Ssdt, c => c.InitReferenceListScriptFolder);
                CombinePath(config.Ssdt, c => c.InitStaticListScriptFolder);
                CombinePath(config.Ssdt, c => c.TableScriptFolder);
                CombinePath(config.Ssdt, c => c.TableTypeScriptFolder);

                if (config.Ssdt.TableScriptFolder != null && config.Ssdt.TableTypeScriptFolder != null)
                {
                    // Génération pour déploiement SSDT.
                    new SqlServerSsdtSchemaGenerator().GenerateSchemaScript(
                        classes.Values,
                        config.Ssdt.TableScriptFolder,
                        config.Ssdt.TableTypeScriptFolder);
                }

                var ssdtInsertGenerator = new SqlServerSsdtInsertGenerator(config.Ssdt);

                if (staticLists != null && config.Ssdt.InitStaticListMainScriptName != null && config.Ssdt.InitStaticListScriptFolder != null)
                {
                    ssdtInsertGenerator.GenerateListInitScript(
                       staticLists.ToDictionary(s => classes[s.className], s => s.values),
                       config.Ssdt.InitStaticListScriptFolder,
                       config.Ssdt.InitStaticListMainScriptName,
                       "delta_static_lists.sql",
                       true);
                }

                if (referenceLists != null && config.Ssdt.InitReferenceListMainScriptName != null && config.Ssdt.InitReferenceListScriptFolder != null)
                {
                    ssdtInsertGenerator.GenerateListInitScript(
                       referenceLists.ToDictionary(s => classes[s.className], s => s.values),
                       config.Ssdt.InitReferenceListScriptFolder,
                       config.Ssdt.InitReferenceListMainScriptName,
                       "delta_reference_lists.sql",
                       false);
                }
            }

            if (config.Csharp != null)
            {
                CombinePath(config.Csharp, c => c.OutputDirectory);
                CSharpCodeGenerator.Generate(rootNamespace, config.Csharp, classes.Values);
            }

            if (config.Javascript != null)
            {
                CombinePath(config.Javascript, c => c.ModelOutputDirectory);
                CombinePath(config.Javascript, c => c.ResourceOutputDirectory);
                TypescriptDefinitionGenerator.Generate(config.Javascript, classes.Values.ToList());
                JavascriptResourceGenerator.Generate(config.Javascript, classes.Values.ToList());
            }
        }
    }
}
