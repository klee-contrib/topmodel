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
        private readonly Class _class;

        public TypescriptTemplate(Class @class)
        {
            _class = @class;
        }

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
            Write(_class.Name);
            Write(" = EntityToType<typeof ");
            Write(_class.Name);
            Write("Entity>;\r\nexport type ");
            Write(_class.Name);
            Write("Node = StoreNode<typeof ");
            Write(_class.Name);
            Write("Entity>;\r\n\r\n");

            Write("export const ");
            Write(_class.Name);
            Write("Entity = {\r\n");

            if (_class.Extends != null)
            {
                Write("        ...");
                Write(_class.Extends.Name);
                Write("Entity,\r\n");
            }

            foreach (var property in _class.Properties)
            {
                Write("    ");
                Write(property.Name.ToFirstLower());
                Write(": {\r\n");
                Write("        type: ");

                if (property is CompositionProperty cp)
                {
                    if (cp.Kind == Composition.List)
                    {
                        if (cp.Composition.Name == _class.Name)
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
                    Write(field.TSType switch
                    {
                        "{}" => "{}",
                        string t when t != "string" && field.Domain.CsharpType == "string" => $"\"string\" as {field.TSType}",
                        "string" => "\"string\"",
                        "number" => "\"number\"",
                        "boolean" => "\"boolean\"",
                        string t => $"{{}} as {t}"
                    });

                    Write(",\r\n");
                    Write("        domain: ");
                    Write(field.Domain.Name);
                    Write(",\r\n        isRequired: ");
                    Write((field.Required && !field.PrimaryKey).ToString().ToFirstLower());
                    Write(",\r\n        label: \"");
                    Write(TSUtils.ToNamespace(_class.Namespace.Module));
                    Write(".");
                    Write(_class.Name.ToFirstLower());
                    Write(".");
                    Write(property.Name.ToFirstLower());
                    Write("\"\r\n");
                }
                else if (property is CompositionProperty cp2 && cp2.Composition.Name != _class.Name)
                {
                    Write("        entity: ");
                    Write(cp2.Composition.Name);
                    Write("Entity");
                    Write("\r\n");
                }

                Write("    }");

                if (property != _class.Properties.Last())
                {
                    Write(",");
                }

                Write("\r\n");
            }

            Write("} as const;\r\n");

            if (_class.Stereotype == Stereotype.Reference)
            {
                Write("\r\nexport const ");
                Write(_class.Name.ToFirstLower());
                Write(" = {type: {} as ");
                Write(_class.Name);
                Write(", valueKey: \"");
                Write(_class.PrimaryKey!.Name.ToFirstLower());
                Write("\", labelKey: \"");
                Write(_class.DefaultProperty?.ToFirstLower() ?? "libelle");
                Write("\"} as const;\r\n");
            }

            return GenerationEnvironment.ToString();
        }

        private IEnumerable<string> GetDomainList()
        {
            return _class.Properties
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
            var types = _class.Properties
                .OfType<CompositionProperty>()
                .Select(property => property.Composition)
                .Where(c => c.Name != _class.Name);

            if (_class.Extends != null)
            {
                types = types.Concat(new[] { _class.Extends });
            }

            var currentModule = _class.Namespace.Module;

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

            var references = _class.Properties
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
    }
}