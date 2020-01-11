using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TopModel.Core.Config;
using Microsoft.Extensions.Logging;

namespace TopModel.Generator.Javascript
{
    /// <summary>
    /// Générateur de définitions Typescript.
    /// </summary>
    public class TypescriptDefinitionGenerator : IGenerator
    {
        private readonly JavascriptConfig? _config;
        private readonly ILogger<TypescriptDefinitionGenerator> _logger;
        private readonly ModelStore _modelStore;

        public TypescriptDefinitionGenerator(ModelStore modelStore, ILogger<TypescriptDefinitionGenerator> logger, JavascriptConfig? config = null)
        {
            _config = config;
            _logger = logger;
            _modelStore = modelStore;
        }

        public bool CanGenerate => _config?.ModelOutputDirectory != null;

        public string Name => "du modèle Typescript";

        /// <summary>
        /// Génère les définitions Typescript.
        /// </summary>
        public void Generate()
        {
            if (_config?.ModelOutputDirectory == null)
            {
                return;
            }

            var nameSpaceMap = _modelStore.Classes.GroupBy(c => c.Namespace.Module).ToDictionary(g => g.Key, g => g.ToList());

            var staticLists = new List<Class>();

            foreach (var entry in nameSpaceMap)
            {
                _logger.LogInformation($"Génération du modèle pour le module {entry.Key}...");

                var count = 0;
                foreach (var model in entry.Value)
                {
                    if (model.Stereotype != Stereotype.Statique)
                    {
                        if (!_config.IsGenerateEntities && model.Trigram != null)
                        {
                            continue;
                        }

                        var fileName = model.Name.ToDashCase();

                        fileName = $"{_config.ModelOutputDirectory}/{entry.Key.ToDashCase()}/{fileName}.ts";
                        var fileInfo = new FileInfo(fileName);

                        var isNewFile = !fileInfo.Exists;

                        var directoryInfo = fileInfo.Directory;
                        if (!directoryInfo.Exists)
                        {
                            Directory.CreateDirectory(directoryInfo.FullName);
                        }

                        GenerateClassFile(fileName, model);
                        count++;
                    }
                    else
                    {
                        staticLists.Add(model);
                    }
                }

                GenerateReferenceLists(_config, staticLists, entry.Key);

                _logger.LogInformation($"{count + (staticLists.Any() ? 1 : 0)} fichiers de modèle générés.");
                staticLists.Clear();
            }
        }

        private void GenerateReferenceLists(JavascriptConfig parameters, IList<Class> staticLists, string namespaceName)
        {
            if (staticLists.Any())
            {
                var fileName = namespaceName != null
                    ? $"{parameters.ModelOutputDirectory}/{namespaceName.ToDashCase()}/references.ts"
                    : $"{parameters.ModelOutputDirectory}/references.ts";

                var fileInfo = new FileInfo(fileName);

                var isNewFile = !fileInfo.Exists;

                var directoryInfo = fileInfo.Directory;
                if (!directoryInfo.Exists)
                {
                    Directory.CreateDirectory(directoryInfo.FullName);
                }

                GenerateReferenceFile(fileName, staticLists.OrderBy(r => r.Name));
            }
        }

        private void GenerateClassFile(string fileName, Class classe)
        {
            using var fw = new FileWriter(fileName, false);

            fw.Write("import {EntityToType, StoreNode} from \"@focus4/stores\";");
            fw.Write("\r\nimport {");
            fw.Write(string.Join(", ", GetDomainList(classe)));
            fw.Write("} from \"../../domains\";\r\n");

            var imports = GetImportList(classe);
            foreach (var import in imports)
            {
                fw.Write("\r\nimport {");
                fw.Write(import.import);
                fw.Write("} from \"");
                fw.Write(import.path);
                fw.Write("\";");
            }
            if (imports.Any())
            {
                fw.Write("\r\n");
            }

            fw.Write("\r\nexport type ");
            fw.Write(classe.Name);
            fw.Write(" = EntityToType<typeof ");
            fw.Write(classe.Name);
            fw.Write("Entity>;\r\nexport type ");
            fw.Write(classe.Name);
            fw.Write("Node = StoreNode<typeof ");
            fw.Write(classe.Name);
            fw.Write("Entity>;\r\n\r\n");

            fw.Write("export const ");
            fw.Write(classe.Name);
            fw.Write("Entity = {\r\n");

            if (classe.Extends != null)
            {
                fw.Write("        ...");
                fw.Write(classe.Extends.Name);
                fw.Write("Entity,\r\n");
            }

            foreach (var property in classe.Properties)
            {
                fw.Write("    ");
                fw.Write(property.Name.ToFirstLower());
                fw.Write(": {\r\n");
                fw.Write("        type: ");

                if (property is CompositionProperty cp)
                {
                    if (cp.Kind == Composition.List)
                    {
                        if (cp.Composition.Name == classe.Name)
                        {
                            fw.Write("\"recursive-list\"");
                        }
                        else
                        {
                            fw.Write("\"list\"");
                        }
                    }
                    else
                    {
                        fw.Write("\"object\"");
                    }
                }
                else
                {
                    fw.Write("\"field\"");
                }

                fw.Write(",\r\n");

                if (property is IFieldProperty field)
                {
                    fw.Write("        name: \"");
                    fw.Write(field.Name.ToFirstLower());
                    fw.Write("\",\r\n        fieldType: ");
                    fw.Write(field.TSType switch
                    {
                        "{}" => "{}",
                        string t when t != "string" && field.Domain.CsharpType == "string" => $"\"string\" as {field.TSType}",
                        "string" => "\"string\"",
                        "number" => "\"number\"",
                        "boolean" => "\"boolean\"",
                        string t => $"{{}} as {t}"
                    });

                    fw.Write(",\r\n");
                    fw.Write("        domain: ");
                    fw.Write(field.Domain.Name);
                    fw.Write(",\r\n        isRequired: ");
                    fw.Write((field.Required && !field.PrimaryKey).ToString().ToFirstLower());
                    fw.Write(",\r\n        label: \"");
                    fw.Write(classe.Namespace.Module.ToFirstLower());
                    fw.Write(".");
                    fw.Write(classe.Name.ToFirstLower());
                    fw.Write(".");
                    fw.Write(property.Name.ToFirstLower());
                    fw.Write("\"\r\n");
                }
                else if (property is CompositionProperty cp2 && cp2.Composition.Name != classe.Name)
                {
                    fw.Write("        entity: ");
                    fw.Write(cp2.Composition.Name);
                    fw.Write("Entity");
                    fw.Write("\r\n");
                }

                fw.Write("    }");

                if (property != classe.Properties.Last())
                {
                    fw.Write(",");
                }

                fw.Write("\r\n");
            }

            fw.Write("} as const;\r\n");

            if (classe.Stereotype == Stereotype.Reference)
            {
                fw.Write("\r\nexport const ");
                fw.Write(classe.Name.ToFirstLower());
                fw.Write(" = {type: {} as ");
                fw.Write(classe.Name);
                fw.Write(", valueKey: \"");
                fw.Write(classe.PrimaryKey!.Name.ToFirstLower());
                fw.Write("\", labelKey: \"");
                fw.Write(classe.DefaultProperty?.ToFirstLower() ?? "libelle");
                fw.Write("\"} as const;\r\n");
            }
        }

        private IEnumerable<string> GetDomainList(Class classe)
        {
            return classe.Properties
                .OfType<IFieldProperty>()
                .Select(property => property.Domain.Name)
                .Distinct()
                .OrderBy(x => x);
        }

        /// <summary>
        /// Récupère la liste d'imports de types pour les services.
        /// </summary>
        /// <returns>La liste d'imports (type, chemin du module, nom du fichier).</returns>
        private IEnumerable<(string import, string path)> GetImportList(Class classe)
        {
            var types = classe.Properties
                .OfType<CompositionProperty>()
                .Select(property => property.Composition)
                .Where(c => c.Name != classe.Name);

            if (classe.Extends != null)
            {
                types = types.Concat(new[] { classe.Extends });
            }

            var currentModule = classe.Namespace.Module;

            var imports = types.Select(type =>
            {
                var module = type.Namespace.Module;
                var name = type.Name;

                module = module == currentModule
                    ? $"."
                    : $"../{module.ToLower()}";

                return (
                    import: $"{name}Entity",
                    path: $"{module}/{name.ToDashCase()}");
            }).Distinct().ToList();

            var references = classe.Properties
                .Select(p => p is AliasProperty alp ? alp.Property : p)
                .OfType<IFieldProperty>()
                .Select(prop => (prop, classe: prop is AssociationProperty ap ? ap.Association : prop.Class))
                .Where(pc => pc.prop.TSType != pc.prop.Domain.CsharpType && pc.prop.Domain.CsharpType == "string" && pc.classe.Stereotype == Stereotype.Statique)
                .Select(pc => (Code: pc.prop.TSType, pc.classe.Namespace.Module))
                .Distinct();

            if (references.Any())
            {
                var referenceTypeMap = references.GroupBy(t => t.Module);
                foreach (var refModule in referenceTypeMap)
                {
                    var module = refModule.Key == currentModule
                    ? $"."
                    : $"../{refModule.Key.ToLower()}";

                    imports.Add((string.Join(", ", refModule.Select(r => r.Code).OrderBy(x => x)), $"{module}/references"));
                }
            }

            return imports.OrderBy(i => i.path);
        }

        /// <summary>
        /// Create the template output
        /// </summary>
        private void GenerateReferenceFile(string fileName, IEnumerable<Class> references)
        {
            using var fw = new FileWriter(fileName, false);

            var first = true;
            foreach (var reference in references)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    fw.WriteLine();
                }

                fw.Write("export type ");
                fw.Write(reference.Name);
                fw.Write("Code = ");
                fw.Write(reference.ReferenceValues != null
                    ? string.Join(" | ", reference.ReferenceValues.Select(r => r.Value.code).OrderBy(x => x))
                    : "string");
                fw.Write(";\r\nexport interface ");
                fw.Write(reference.Name);
                fw.Write(" {\r\n");

                foreach (var property in reference.Properties.OfType<IFieldProperty>())
                {
                    fw.Write("    ");
                    fw.Write(property.Name.ToFirstLower());
                    fw.Write(property.Required || property.PrimaryKey ? string.Empty : "?");
                    fw.Write(": ");
                    fw.Write(GetRefTSType(property, reference));
                    fw.Write(";\r\n");
                }

                fw.Write("}\r\n");

                fw.Write("export const ");
                fw.Write(reference.Name.ToFirstLower());
                fw.Write(" = {type: {} as ");
                fw.Write(reference.Name);
                fw.Write(", valueKey: \"");
                fw.Write(reference.PrimaryKey!.Name.ToFirstLower());
                fw.Write("\", labelKey: \"");
                fw.Write(reference.DefaultProperty?.ToFirstLower() ?? "libelle");
                fw.Write("\"} as const;\r\n");
            }
        }

        /// <summary>
        /// Transforme le type en type Typescript.
        /// </summary>
        /// <param name="property">La propriété dont on cherche le type.</param>
        /// <param name="reference">Classe de la propriété.</param>
        /// <returns>Le type en sortie.</returns>
        private string GetRefTSType(IFieldProperty property, Class reference)
        {
            if (property.Name == "Code")
            {
                return $"{reference.Name}Code";
            }
            else if (property.Name.EndsWith("Code", StringComparison.Ordinal))
            {
                return property.Name.ToFirstUpper();
            }

            return ModelUtils.CSharpToTSType(property.Domain.CsharpType);
        }
    }
}
