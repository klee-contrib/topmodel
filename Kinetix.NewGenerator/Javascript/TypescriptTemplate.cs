using System.Collections.Generic;
using System.Linq;
using Kinetix.NewGenerator.Model;
using Kinetix.Tools.Common;

namespace Kinetix.NewGenerator.Javascript
{
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    public partial class TypescriptTemplate : TemplateBase
    {
        /// <summary>
        /// Objet de modèle.
        /// </summary>
        public Class Class { get; set; }

        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            Write("/*\r\n    Ce fichier a été généré automatiquement.\r\n    Toute modification sera per" +
                    "due.\r\n*/\r\n\r\n");

            Write("import {EntityToType, StoreNode} from \"@focus4/stores\";");
            Write("\r\nimport {");
            Write(string.Join(", ", GetDomainList()));
            Write("} from \"../../domains\";\r\n");

            var imports = GetImportList();
            foreach (var import in imports)
            {
                Write("\r\nimport {");
                Write(import.import);
                Write("} from \"");
                Write(import.path);
                Write("\";");
            }
            if (imports.Any())
            {
                Write("\r\n");
            }

            Write("\r\nexport type ");
            Write(Class.Name);
            Write(" = EntityToType<typeof ");
            Write(Class.Name);
            Write("Entity>;\r\nexport type ");
            Write(Class.Name);
            Write("Node = StoreNode<typeof ");
            Write(Class.Name);
            Write("Entity>;\r\n\r\n");

            Write("export const ");
            Write(Class.Name);
            Write("Entity = {\r\n");

            if (Class.Extends != null)
            {
                Write("        ...");
                Write(Class.Extends.Name);
                Write("Entity,\r\n");
            }

            foreach (var property in Class.Properties)
            {
                Write("    ");
                Write(property.Name.ToFirstLower());
                Write(": {\r\n");
                Write("        type: ");

                if (property is CompositionProperty cp)
                {
                    if (cp.Kind == "list")
                    {
                        if (cp.Composition.Name == Class.Name)
                        {
                            Write("\"recursive-list\"");
                        }
                        else
                        {
                            Write("\"list\"");
                        }
                    }
                    else
                    {
                        Write("\"object\"");
                    }
                }
                else
                {
                    Write("\"field\"");
                }

                Write(",\r\n");

                if (property is IFieldProperty field)
                {
                    Write("        name: \"");
                    Write(field.Name.ToFirstLower());
                    Write("\",\r\n        fieldType: ");
                    var propType = field.GetTSType();
                    if (propType == "string")
                    {
                        Write("\"string\"");
                    }
                    else if (propType == "boolean")
                    {
                        Write("\"boolean\"");
                    }
                    else if (propType == "number")
                    {
                        Write("\"number\"");
                    }
                    else if (propType.EndsWith("Code"))
                    {
                        Write($"\"string\" as {propType}");
                    }
                    else
                    {
                        Write($"{{}} as {propType}");
                    }

                    Write(",\r\n");
                    Write("        domain: ");
                    Write(field.Domain.Name);
                    Write(",\r\n        isRequired: ");
                    Write((field.Required && (!field.PrimaryKey || field.Domain.CsharpType != "int?")).ToString().ToFirstLower());
                    Write(",\r\n        label: \"");
                    Write(TSUtils.ToNamespace(Class.Namespace.Module));
                    Write(".");
                    Write(Class.Name.ToFirstLower());
                    Write(".");
                    Write(property.Name.ToFirstLower());
                    Write("\"\r\n");
                }
                else if (property is CompositionProperty cp2)
                {
                    Write("        entity: ");
                    Write(cp2.Composition.Name);
                    Write("Entity");
                    Write("\r\n");
                }

                Write("    }");

                if (property != Class.Properties.Last())
                {
                    Write(",");
                }

                Write("\r\n");
            }

            Write("} as const;\r\n");

            if (Class.Stereotype == "Reference")
            {
                Write("\r\nexport const ");
                Write(Class.Name.ToFirstLower());
                Write(" = {type: {} as ");
                Write(Class.Name);
                Write(", valueKey: \"");
                Write(Class.Properties.Single(p => p.PrimaryKey).Name.ToFirstLower());
                Write("\", labelKey: \"");
                Write(Class.DefaultProperty?.ToFirstLower() ?? "libelle");
                Write("\"} as const;\r\n");
            }

            return GenerationEnvironment.ToString();
        }

        private IEnumerable<string> GetDomainList()
        {
            return Class.Properties
                .OfType<IFieldProperty>()
                .Select(property => property.Domain.Name)
                .Distinct()
                .OrderBy(x => x);
        }

        /// <summary>
        /// Récupère la liste d'imports de types pour les services.
        /// </summary>
        /// <returns>La liste d'imports (type, chemin du module, nom du fichier).</returns>
        private IEnumerable<(string import, string path)> GetImportList()
        {
            var types = Class.Properties
                .OfType<CompositionProperty>()
                .Select(property => property.Composition);

            if (Class.Extends != null)
            {
                types = types.Concat(new[] { Class.Extends });
            }

            var currentModule = Class.Namespace.Module;

            var imports = types.Select(type =>
            {
                var module = type.Namespace.Module;
                var name = type.Name;

                module = module == currentModule
                    ? $"."
                    : $"../{module}";

                return (
                    import: $"{name}Entity",
                    path: $"{module}/{name.ToDashCase()}");
            }).Distinct().ToList();

            var references = Class.Properties
                .OfType<AssociationProperty>()
                .Where(property => property.Association.Stereotype == "Statique")
                .Select(property => (Code: $"{property.Association.Name}Code", property.Association.Namespace.Module))
                .Distinct();

            if (references.Any())
            {
                var referenceTypeMap = references.GroupBy(t => t.Module);
                foreach (var refModule in referenceTypeMap)
                {
                    var module = refModule.Key == currentModule
                    ? $"."
                    : $"../{refModule.Key}";

                    imports.Add((string.Join(", ", refModule.Select(r => r.Code).OrderBy(x => x)), $"{module}/references"));
                }
            }

            return imports.OrderBy(i => i.path);
        }
    }
}