using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TopModel.Core.FileModel;
using Microsoft.Extensions.Logging;

namespace TopModel.Generator.CSharp
{
    using static CSharpUtils;

    public class CSharpClassGenerator
    {
        private readonly CSharpConfig _config;
        private readonly ILogger _logger;

        public CSharpClassGenerator(CSharpConfig config, ILogger logger)
        {
            _config = config;
            _logger = logger;
        }

        /// <summary>
        /// Méthode générant le code d'une classe.
        /// </summary>
        /// <param name="item">Classe concernée.</param>
        public void Generate(Class item)
        {
            if (_config.OutputDirectory == null)
            {
                return;
            }

            var fileName = Path.Combine(GetDirectoryForModelClass(_config.LegacyProjectPaths, _config.OutputDirectory, item.Namespace.Kind == Kind.Data, item.Namespace.App, item.Namespace.CSharpName), item.Name + ".cs");

            using var w = new CSharpWriter(fileName, _logger);

            GenerateUsings(w, item);
            w.WriteLine();
            w.WriteNamespace($"{item.Namespace.App}.{item.Namespace.CSharpName}");
            w.WriteSummary(1, item.Comment);
            GenerateClassDeclaration(w, item);
            w.WriteLine("}");
        }

        /// <summary>
        /// Génère le constructeur par recopie d'un type base.
        /// </summary>
        /// <param name="w">Writer.</param>
        /// <param name="item">Classe générée.</param>
        private void GenerateBaseCopyConstructor(CSharpWriter w, Class item)
        {
            if (item.Extends != null)
            {
                w.WriteLine();
                w.WriteSummary(2, "Constructeur par base class.");
                w.WriteParam("bean", "Source.");
                w.WriteLine(2, "public " + item.Name + "(" + item.Extends.Name + " bean)");
                w.WriteLine(3, ": base(bean)");
                w.WriteLine(2, "{");
                w.WriteLine(3, "OnCreated();");
                w.WriteLine(2, "}");
            }
        }

        /// <summary>
        /// Génération de la déclaration de la classe.
        /// </summary>
        /// <param name="w">Writer</param>
        /// <param name="item">Classe à générer.</param>
        private void GenerateClassDeclaration(CSharpWriter w, Class item)
        {
            if (item.Stereotype == Stereotype.Reference)
            {
                w.WriteAttribute(1, "Reference");
            }
            else if (item.Stereotype == Stereotype.Statique)
            {
                w.WriteAttribute(1, "Reference", "true");
            }

            if (!string.IsNullOrEmpty(item.DefaultProperty))
            {
                w.WriteAttribute(1, "DefaultProperty", $@"""{item.DefaultProperty}""");
            }

            if (item.Namespace.Kind == Kind.Data)
            {
                if (_config.DbSchema != null)
                {
                    w.WriteAttribute(1, "Table", $@"""{item.SqlName}""", $@"Schema = ""{_config.DbSchema}""");
                }
                else
                {
                    w.WriteAttribute(1, "Table", $@"""{item.SqlName}""");
                }
            }

            ICollection<string> interfaces = new List<string>();

            if (_config.IsWithEntityInterface && item.Namespace.Kind == Kind.Data)
            {
                if (item.PrimaryKey != null)
                {
                    var name = item.PrimaryKey.Name;
                    var type = item.PrimaryKey.Domain.CsharpType;

                    if (name == "Id" && type == "int?")
                    {
                        interfaces.Add("IIdEntity");
                    }
                    else if (name == "Code" && type == "string")
                    {
                        interfaces.Add("ICodeEntity");
                    }
                }

                interfaces.Add("IEntity");
            }

            w.WriteClassDeclaration(item.Name, item.Extends?.Name, interfaces);

            GenerateConstProperties(w, item);
            GenerateConstructors(w, item);

            if (_config.DbContextProjectPath == null && item.Namespace.Kind == Kind.Data)
            {
                w.WriteLine();
                w.WriteLine(2, "#region Meta données");
                GenerateEnumCols(w, item);
                w.WriteLine();
                w.WriteLine(2, "#endregion");
            }

            GenerateProperties(w, item);
            GenerateExtensibilityMethods(w, item);
            w.WriteLine(1, "}");

            if (_config.UseTypeSafeConstValues)
            {
                GenerateConstPropertiesClass(w, item);
            }
        }

        /// <summary>
        /// Génération des constantes statiques.
        /// </summary>
        /// <param name="w">Writer.</param>
        /// <param name="item">La classe générée.</param>
        private void GenerateConstProperties(CSharpWriter w, Class item)
        {
            if (item.ReferenceValues?.Any() ?? false)
            {
                var i = 0;
                foreach (var refValue in item.ReferenceValues.OrderBy(x => x.Name, StringComparer.Ordinal))
                {
                    ++i;
                    var code = item.Stereotype == Stereotype.Statique
                        ? (string)refValue.Value[item.PrimaryKey]
                        : (string)refValue.Value[item.Properties.OfType<RegularProperty>().Single(rp => rp.Unique)];
                    var label = item.LabelProperty != null
                        ? (string)refValue.Value[item.LabelProperty]
                        : refValue.Name;
                    IFieldProperty? property = null;
                    if (item.Stereotype == Stereotype.Reference)
                    {
                        foreach (var prop in item.Properties.OfType<RegularProperty>())
                        {
                            if (prop.Unique)
                            {
                                property = prop;
                                break;
                            }
                        }
                    }
                    else
                    {
                        property = item.PrimaryKey;
                    }

                    w.WriteSummary(2, label);

                    if (_config.UseTypeSafeConstValues)
                    {
                        w.WriteLine(2, string.Format("public readonly {2}Code {0} = new {2}Code({1});", refValue.Name, code, item.Name));
                    }
                    else
                    {
                        w.WriteLine(2, string.Format("public const string {0} = \"{1}\";", refValue.Name, code));
                    }

                    w.WriteLine();
                }
            }
        }

        /// <summary>
        /// Génération des constantes statiques.
        /// </summary>
        /// <param name="w">Writer.</param>
        /// <param name="item">La classe générée.</param>
        private void GenerateConstPropertiesClass(CSharpWriter w, Class item)
        {
            if (item.ReferenceValues?.Any() ?? false)
            {
                w.WriteLine();
                w.WriteLine("#pragma warning disable SA1402");
                w.WriteLine();
                w.WriteSummary(1, $"Type des valeurs pour {item.Name}");
                w.WriteLine(1, $"public sealed class {item.Name}Code : TypeSafeEnum {{");
                w.WriteLine();

                w.WriteLine(2, $"private readonly Dictionary<string, {item.Name}Code> Instance = new Dictionary<string, {item.Name}Code>();");
                w.WriteLine();

                w.WriteSummary(2, "Constructeur");
                w.WriteParam("value", "Valeur");
                w.WriteLine(2, $"public {item.Name}Code(string value)");
                w.WriteLine(3, ": base(value) {");
                w.WriteLine(3, "Instance[value] = this;");
                w.WriteLine(2, "}");
                w.WriteLine();

                w.WriteLine(2, $"public explicit operator {item.Name}Code(string value) {{");
                w.WriteLine(3, $"System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof({item.Name}).TypeHandle);");
                w.WriteLine(3, "if (Instance.TryGetValue(value, out var result)) {");
                w.WriteLine(4, "return result;");
                w.WriteLine(3, "} else {");
                w.WriteLine(4, "throw new InvalidCastException();");
                w.WriteLine(3, "}");
                w.WriteLine(2, "}");
                w.WriteLine(1, "}");
            }
        }

        /// <summary>
        /// Génère les constructeurs.
        /// </summary>
        /// <param name="w">Writer.</param>
        /// <param name="item">La classe générée.</param>
        private void GenerateConstructors(CSharpWriter w, Class item)
        {
            GenerateDefaultConstructor(w, item);
            GenerateCopyConstructor(w, item);
            GenerateBaseCopyConstructor(w, item);
        }

        /// <summary>
        /// Génère le constructeur par recopie.
        /// </summary>
        /// <param name="w">Writer.</param>
        /// <param name="item">Classe générée.</param>
        private void GenerateCopyConstructor(CSharpWriter w, Class item)
        {
            w.WriteLine();
            w.WriteSummary(2, "Constructeur par recopie.");
            w.WriteParam("bean", "Source.");
            if (item.Extends != null)
            {
                w.WriteLine(2, "public " + item.Name + "(" + item.Name + " bean)");
                w.WriteLine(3, ": base(bean)");
                w.WriteLine(2, "{");
            }
            else
            {
                w.WriteLine(2, "public " + item.Name + "(" + item.Name + " bean)");
                w.WriteLine(2, "{");
            }

            w.WriteLine(3, "if (bean == null)");
            w.WriteLine(3, "{");
            w.WriteLine(4, "throw new ArgumentNullException(nameof(bean));");
            w.WriteLine(3, "}");
            w.WriteLine();

            var initd = new List<string>();

            foreach (var property in item.Properties.OfType<IFieldProperty>().Where(t => t.Domain.CsharpType.Contains("ICollection")))
            {
                initd.Add(property.Name);
                var strip = property.Domain.CsharpType.Replace("ICollection<", string.Empty).Replace(">", string.Empty);
                w.WriteLine(3, property.Name + " = new List<" + strip + ">(bean." + property.Name + ");");
            }

            foreach (var property in item.Properties.OfType<CompositionProperty>().Where(p => p.Kind == Composition.Object))
            {
                w.WriteLine(3, property.Name + " = new " + property.Composition.Name + "(bean." + property.Name + ");");
            }

            foreach (var property in item.Properties.OfType<CompositionProperty>().Where(p => p.Kind == Composition.List))
            {
                w.WriteLine(3, property.Name + " = new List<" + property.Composition.Name + ">(bean." + property.Name + ");");
            }

            foreach (var property in item.Properties.Where(p => !(p is CompositionProperty) && !initd.Contains(p.Name)))
            {
                w.WriteLine(3, property.Name + " = bean." + property.Name + ";");
            }

            w.WriteLine();
            w.WriteLine(3, "OnCreated(bean);");
            w.WriteLine(2, "}");
        }

        /// <summary>
        /// Génère le constructeur par défaut.
        /// </summary>
        /// <param name="w">Writer.</param>
        /// <param name="item">Classe générée.</param>
        private void GenerateDefaultConstructor(CSharpWriter w, Class item)
        {
            w.WriteSummary(2, "Constructeur.");
            w.WriteLine(2, $@"public {item.Name}()");

            if (item.Extends != null)
            {
                w.WriteLine(3, ": base()");
            }

            w.WriteLine(2, "{");

            var line = false;
            foreach (var property in item.Properties.OfType<IFieldProperty>().Where(t => t.Domain.CsharpType.Contains("ICollection")))
            {
                line = true;
                var strip = property.Domain.CsharpType.Replace("ICollection<", string.Empty).Replace(">", string.Empty);
                w.WriteLine(3, LoadPropertyInit(property.Name, "List<" + strip + ">"));
            }

            foreach (var property in item.Properties.OfType<CompositionProperty>().Where(p => p.Kind == Composition.Object))
            {
                line = true;
                w.WriteLine(3, LoadPropertyInit(property.Name, property.Composition.Name));
            }

            foreach (var property in item.Properties.OfType<CompositionProperty>().Where(p => p.Kind == Composition.List))
            {
                line = true;
                w.WriteLine(3, LoadPropertyInit(property.Name, "List<" + property.Composition.Name + ">"));
            }

            if (line)
            {
                w.WriteLine();
            }

            w.WriteLine(3, "OnCreated();");
            w.WriteLine(2, "}");
        }

        /// <summary>
        /// Génère les méthodes d'extensibilité.
        /// </summary>
        /// <param name="w">Writer.</param>
        /// <param name="item">Classe générée.</param>
        private void GenerateExtensibilityMethods(CSharpWriter w, Class item)
        {
            w.WriteLine();
            w.WriteSummary(2, "Methode d'extensibilité possible pour les constructeurs.");
            w.WriteLine(2, "partial void OnCreated();");
            w.WriteLine();
            w.WriteSummary(2, "Methode d'extensibilité possible pour les constructeurs par recopie.");
            w.WriteParam("bean", "Source.");
            w.WriteLine(2, $"partial void OnCreated({item.Name} bean);");
        }

        /// <summary>
        /// Génère les propriétés.
        /// </summary>
        /// <param name="w">Writer.</param>
        /// <param name="item">La classe générée.</param>
        private void GenerateProperties(CSharpWriter w, Class item)
        {
            foreach (var property in item.Properties)
            {
                w.WriteLine();
                GenerateProperty(w, property);
            }
        }

        /// <summary>
        /// Génère la propriété concernée.
        /// </summary>
        /// <param name="w">Writer.</param>
        /// <param name="property">La propriété générée.</param>
        private void GenerateProperty(CSharpWriter w, IProperty property)
        {
            w.WriteSummary(2, property.Comment);

            if (property is IFieldProperty fp)
            {
                var prop = fp is AliasProperty alp ? alp.Property : fp;
                if ((!_config.NoColumnOnAlias || !(fp is AliasProperty)) && prop.Class.Trigram != null)
                {
                    if (prop.Domain.UseTypeName)
                    {
                        w.WriteAttribute(2, "Column", $@"""{prop.SqlName}""", $@"TypeName = ""{prop.Domain.SqlType}""");
                    }
                    else
                    {
                        w.WriteAttribute(2, "Column", $@"""{prop.SqlName}""");
                    }
                }

                if (fp.Required && !fp.PrimaryKey || fp is AliasProperty { Property: { PrimaryKey: true } })
                {
                    w.WriteAttribute(2, "Required");
                }

                if (prop is AssociationProperty ap)
                {
                    w.WriteAttribute(2, "ReferencedType", $"typeof({ap.Association.Name})");
                }
                else if (fp is AliasProperty alp2 && !alp2.PrimaryKey && alp2.Property.PrimaryKey)
                {
                    w.WriteAttribute(2, "ReferencedType", $"typeof({alp2.Property.Class.Name})");
                }

                w.WriteAttribute(2, "Domain", $@"""{prop.Domain.Name}""");

                if (!string.IsNullOrEmpty(prop.Domain.CustomAnnotation))
                {
                    w.WriteLine(2, prop.Domain.CustomAnnotation);
                }

                if (fp.DefaultValue != null)
                {
                    w.WriteAttribute(2, "DatabaseGenerated", "DatabaseGeneratedOption.Identity");
                }
            }
            else
            {
                w.WriteAttribute(2, "NotMapped");
            }

            if (property.PrimaryKey && property is RegularProperty)
            {
                w.WriteAttribute(2, "Key");
            }

            switch (property)
            {
                case CompositionProperty { Kind: Composition.Object } ocp:
                    w.WriteLine(2, $"public {ocp.Composition.Name} {property.Name} {{ get; set; }}");
                    break;
                case CompositionProperty { Kind: Composition.List } lcp:
                    w.WriteLine(2, $"public ICollection<{lcp.Composition.Name}> {property.Name} {{ get; set; }}");
                    break;
                case IFieldProperty ifp:
                    w.WriteLine(2, $"public {ifp.Domain.CsharpType} {property.Name} {{ get; set; }}");
                    break;
            }
        }

        /// <summary>
        /// Génération des imports.
        /// </summary>
        /// <param name="w">Writer.</param>
        /// <param name="item">Classe concernée.</param>
        private void GenerateUsings(CSharpWriter w, Class item)
        {
            var usings = new List<string> { "System" };

            if (item.Properties.Any(p => p is CompositionProperty { Kind: Composition.List }) || _config.UseTypeSafeConstValues && (item.ReferenceValues?.Any() ?? false))
            {
                usings.Add("System.Collections.Generic");
            }

            if (!string.IsNullOrEmpty(item.DefaultProperty))
            {
                usings.Add("System.ComponentModel");
            }

            if (item.Properties.OfType<IFieldProperty>()
                .Select(p => p is AliasProperty alp ? alp.Property : p)
                .Any(p => p.Required || p.PrimaryKey))
            {
                usings.Add("System.ComponentModel.DataAnnotations");
            }

            if (item.Properties.Any(p => p is CompositionProperty) ||
                item.Properties.OfType<IFieldProperty>().Any(fp =>
            {
                var prop = fp is AliasProperty alp ? alp.Property : fp;
                return (!_config.NoColumnOnAlias || !(fp is AliasProperty)) && prop.Class.Trigram != null;
            }))
            {
                usings.Add("System.ComponentModel.DataAnnotations.Schema");
            }

            if (item.Properties.OfType<IFieldProperty>().Any() || item.Extends == null)
            {
                if (_config.Kinetix == KinetixVersion.Core)
                {
                    usings.Add("Kinetix.ComponentModel.Annotations");
                }
                else if (_config.Kinetix == KinetixVersion.Framework)
                {
                    usings.Add("Kinetix.ComponentModel");
                }
                else
                {
                    usings.Add("Fmk.ComponentModel");
                }
            }

            if (_config.IsWithEntityInterface && item.Trigram != null)
            {
                usings.Add("Kinetix.ComponentModel.Entity");
            }

            foreach (var property in item.Properties)
            {
                if (property is IFieldProperty fp && !string.IsNullOrEmpty(fp.Domain.CustomUsings))
                {
                    usings.AddRange(fp.Domain.CustomUsings.Split(',').Select(u => u.Trim()));
                }

                switch (property)
                {
                    case AssociationProperty ap:
                        usings.Add($"{item.Namespace.App}.{ap.Association.Namespace.CSharpName}");
                        break;
                    case AliasProperty { Property: AssociationProperty ap2 }:
                        usings.Add($"{item.Namespace.App}.{ap2.Association.Namespace.CSharpName}");
                        break;
                    case AliasProperty { PrimaryKey: false, Property: RegularProperty { PrimaryKey: true } rp }:
                        usings.Add($"{item.Namespace.App}.{rp.Class.Namespace.CSharpName}");
                        break;
                    case CompositionProperty cp:
                        usings.Add($"{item.Namespace.App}.{cp.Composition.Namespace.CSharpName}");
                        break;
                }
            }

            w.WriteUsings(usings
                .Where(u => u != $"{item.Namespace.App}.{item.Namespace.CSharpName}")
                .Distinct()
                .ToArray());
        }

        /// <summary>
        /// Retourne le code associé à l'instanciation d'une propriété.
        /// </summary>
        /// <param name="fieldName">Nom de la variable membre privée.</param>
        /// <param name="dataType">Type de données.</param>
        /// <returns>Code généré.</returns>
        private string LoadPropertyInit(string fieldName, string dataType)
        {
            var res = $"{fieldName} = ";
            if (IsCSharpBaseType(dataType))
            {
                res += GetCSharpDefaultValueBaseType(dataType) + ";";
            }
            else
            {
                res += $"new {dataType}();";
            }

            return res;
        }

        /// <summary>
        /// Génère le type énuméré présentant les colonnes persistentes.
        /// </summary>
        /// <param name="w">Writer.</param>
        /// <param name="item">La classe générée.</param>
        private void GenerateEnumCols(CSharpWriter w, Class item)
        {
            w.WriteLine();
            w.WriteSummary(2, "Type énuméré présentant les noms des colonnes en base.");

            if (item.Extends == null)
            {
                w.WriteLine(2, "public enum Cols");
            }
            else
            {
                w.WriteLine(2, "public new enum Cols");
            }

            w.WriteLine(2, "{");

            var cols = item.Properties.OfType<IFieldProperty>().ToList();
            foreach (var property in cols)
            {
                w.WriteSummary(3, "Nom de la colonne en base associée à la propriété " + property.Name + ".");
                w.WriteLine(3, $"{property.SqlName},");
                if (cols.IndexOf(property) != cols.Count - 1)
                {
                    w.WriteLine();
                }
            }

            w.WriteLine(2, "}");
        }
    }
}
