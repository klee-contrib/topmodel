using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TopModel.Core.FileModel;
using Microsoft.Extensions.Logging;

namespace TopModel.Generator.Javascript
{
    /// <summary>
    /// Générateur de définitions Typescript.
    /// </summary>
    public class TypescriptDefinitionGenerator : GeneratorBase
    {
        private readonly JavascriptConfig _config;
        private readonly ILogger<TypescriptDefinitionGenerator> _logger;
        private readonly IDictionary<FileName, ModelFile> _files = new Dictionary<FileName, ModelFile>();

        public TypescriptDefinitionGenerator(ILogger<TypescriptDefinitionGenerator> logger, JavascriptConfig config)
            : base(logger, config)
        {
            _config = config;
            _logger = logger;
        }

        public override string Name => "TSDefinitionGen";

        protected override void HandleFiles(IEnumerable<ModelFile> files)
        {
            if (_config.ModelOutputDirectory == null)
            {
                return;
            }

            foreach (var file in files)
            {
                _files[file.Name] = file;
                GenerateClasses(file);
            }

            var modules = files.SelectMany(f => f.Classes.Select(c => c.Namespace.Module)).Distinct();

            foreach (var module in modules)
            {
                GenerateReferences(module);
            }
        }

        private void GenerateClasses(ModelFile file)
        {
            if (_config.ModelOutputDirectory == null)
            {
                return;
            }

            var count = 0;
            foreach (var classe in file.Classes)
            {
                if (!classe.Reference || classe.PrimaryKey!.Domain.Name == "DO_ID")
                {
                    var fileName = classe.Name.ToDashCase();

                    fileName = $"{_config.ModelOutputDirectory}/{file.Descriptor.Module.ToDashCase()}/{fileName}.ts";
                    var fileInfo = new FileInfo(fileName);

                    var isNewFile = !fileInfo.Exists;

                    var directoryInfo = fileInfo.Directory;
                    if (!directoryInfo.Exists)
                    {
                        Directory.CreateDirectory(directoryInfo.FullName);
                    }

                    GenerateClassFile(fileName, classe);
                    count++;
                }
            }
        }

        private void GenerateReferences(string module)
        {
            var classes = _files.Values.SelectMany(f => f.Classes).Where(c => c.Namespace.Module == module && c.Reference && c.PrimaryKey!.Domain.Name != "DO_ID");

            if (_config.ModelOutputDirectory != null && classes.Any())
            {
                var fileName = module != null
                    ? $"{_config.ModelOutputDirectory}/{module.ToDashCase()}/references.ts"
                    : $"{_config.ModelOutputDirectory}/references.ts";

                var fileInfo = new FileInfo(fileName);

                var isNewFile = !fileInfo.Exists;

                var directoryInfo = fileInfo.Directory;
                if (!directoryInfo.Exists)
                {
                    Directory.CreateDirectory(directoryInfo.FullName);
                }

                GenerateReferenceFile(fileName, classes.OrderBy(r => r.Name));
            }
        }

        private void GenerateClassFile(string fileName, Class classe)
        {
            using var fw = new FileWriter(fileName, _logger, false);

            fw.Write("import {EntityToType, ");

            if (classe.Properties.Any(p => p is IFieldProperty))
            {
                fw.Write("FieldEntry2, ");
            }

            if (classe.Properties.Any(p => p is CompositionProperty { Kind: Composition.List } cp && cp.Class == classe))
            {
                fw.Write("ListEntry, ");
            }

            if (classe.Properties.Any(p => p is CompositionProperty { Kind: Composition.Object }))
            {
                fw.Write("ObjectEntry, ");
            }

            if (classe.Properties.Any(p => p is CompositionProperty { Kind: Composition.List } cp && cp.Composition == classe))
            {
                fw.Write("RecursiveListEntry, ");
            }

            fw.Write("StoreNode} from \"@focus4/stores\";");
            fw.Write("\r\nimport {");
            fw.Write(string.Join(", ", GetDomainList(classe)));
            fw.Write("} from \"../../domains\";\r\n");

            var imports = GetImportList(classe);
            foreach (var import in imports)
            {
                fw.Write("\r\nimport {");
                fw.Write(import.Import);
                fw.Write("} from \"");
                fw.Write(import.Path);
                fw.Write("\";");
            }

            if (imports.Any())
            {
                fw.Write("\r\n");
            }

            fw.Write("\r\nexport type ");
            fw.Write(classe.Name);
            fw.Write(" = EntityToType<");
            fw.Write(classe.Name);
            fw.Write("EntityType>;\r\nexport type ");
            fw.Write(classe.Name);
            fw.Write("Node = StoreNode<");
            fw.Write(classe.Name);
            fw.Write("EntityType>;\r\n");

            fw.Write($"export interface {classe.Name}EntityType ");

            if (classe.Extends != null)
            {
                fw.Write($"extends {classe.Extends.Name}EntityType ");
            }

            fw.Write("{\r\n");

            foreach (var property in classe.Properties)
            {
                fw.Write($"    {property.Name.ToFirstLower()}: ");

                if (property is CompositionProperty cp)
                {
                    if (cp.Kind == Composition.List)
                    {
                        if (cp.Composition.Name == classe.Name)
                        {
                            fw.Write($"RecursiveListEntry");
                        }
                        else
                        {
                            fw.Write($"ListEntry<{cp.Composition.Name}EntityType>");
                        }
                    }
                    else
                    {
                        fw.Write($"ObjectEntry<{cp.Composition.Name}EntityType>");
                    }
                }
                else if (property is IFieldProperty field)
                {
                    fw.Write($"FieldEntry2<typeof {field.Domain.Name}, {field.TSType}>");
                }

                if (property != classe.Properties.Last())
                {
                    fw.Write(",");
                }

                fw.Write("\r\n");
            }

            fw.Write("}\r\n\r\n");

            fw.Write($"export const {classe.Name}Entity: {classe.Name}EntityType = {{\r\n");

            if (classe.Extends != null)
            {
                fw.Write("    ...");
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
                            fw.Write("\"list\",");
                        }
                    }
                    else
                    {
                        fw.Write("\"object\",");
                    }
                }
                else
                {
                    fw.Write("\"field\",");
                }

                fw.Write("\r\n");

                if (property is IFieldProperty field)
                {
                    fw.Write("        name: \"");
                    fw.Write(field.Name.ToFirstLower());
                    fw.Write("\"");
                    fw.Write(",\r\n        domain: ");
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

            fw.Write("}\r\n");

            if (classe.Reference)
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
        private IEnumerable<(string Import, string Path)> GetImportList(Class classe)
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
                    import: $"{name}Entity, {name}EntityType",
                    path: $"{module}/{name.ToDashCase()}");
            }).Distinct().ToList();

            var references = classe.Properties
                .Select(p => p is AliasProperty alp ? alp.Property : p)
                .OfType<IFieldProperty>()
                .Select(prop => (prop, classe: prop is AssociationProperty ap ? ap.Association : prop.Class))
                .Where(pc => pc.prop.TSType != pc.prop.Domain.CsharpType && pc.prop.Domain.CsharpType == "string" && pc.classe.Reference)
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
            using var fw = new FileWriter(fileName, _logger, false);

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
                    ? string.Join(" | ", reference.ReferenceValues.Select(r => $@"""{r.Value[reference.PrimaryKey]}""").OrderBy(x => x))
                    : "string");
                fw.WriteLine(";");

                if (reference.FlagProperty != null && reference.ReferenceValues != null)
                {
                    fw.Write($"export enum {reference.Name}Flag {{\r\n");

                    foreach (var refValue in reference.ReferenceValues)
                    {
                        var flag = int.Parse((string)refValue.Value[reference.Properties.OfType<IFieldProperty>().Single(rp => rp.Name == reference.FlagProperty)]);
                        fw.Write($"    {refValue.Name} = 0b{Convert.ToString(flag, 2)}");
                        if (reference.ReferenceValues.IndexOf(refValue) != reference.ReferenceValues.Count - 1)
                        {
                            fw.WriteLine(",");
                        }
                    }

                    fw.WriteLine("\r\n}");
                }

                fw.Write("export interface ");
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
