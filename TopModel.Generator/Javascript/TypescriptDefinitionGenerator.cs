using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Core.FileModel;

namespace TopModel.Generator.Javascript;

/// <summary>
/// Générateur de définitions Typescript.
/// </summary>
public class TypescriptDefinitionGenerator : GeneratorBase
{
    private readonly JavascriptConfig _config;
    private readonly ILogger<TypescriptDefinitionGenerator> _logger;
    private readonly IDictionary<string, ModelFile> _files = new Dictionary<string, ModelFile>();

    public TypescriptDefinitionGenerator(ILogger<TypescriptDefinitionGenerator> logger, JavascriptConfig config)
        : base(logger, config)
    {
        _config = config;
        _logger = logger;
    }

    public override string Name => "TSDefinitionGen";

    protected override void HandleFiles(IEnumerable<ModelFile> files)
    {
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
        var count = 0;
        foreach (var classe in file.Classes)
        {
            if (!(classe.Reference || classe.ReferenceValues != null) || classe.PrimaryKey?.Domain.TS?.Type == "number")
            {
                var fileName = classe.Name.ToDashCase();

                fileName = $"{_config.ModelOutputDirectory}/{file.Module.Replace(".", "/").ToDashCase()}/{fileName}.ts";
                var fileInfo = new FileInfo(fileName);

                var isNewFile = !fileInfo.Exists;

                var directoryInfo = fileInfo.Directory!;
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
        var classes = _files.Values
            .SelectMany(f => f.Classes)
            .Distinct()
            .Where(c => c.Namespace.Module == module && (c.Reference || c.ReferenceValues != null) && c.PrimaryKey?.Domain.Name != "DO_ID");

        if (_config.ModelOutputDirectory != null && classes.Any())
        {
            var fileName = module != null
                ? $"{_config.ModelOutputDirectory}/{module.ToDashCase()}/references.ts"
                : $"{_config.ModelOutputDirectory}/references.ts";

            var fileInfo = new FileInfo(fileName);

            var isNewFile = !fileInfo.Exists;

            var directoryInfo = fileInfo.Directory!;
            if (!directoryInfo.Exists)
            {
                Directory.CreateDirectory(directoryInfo.FullName);
            }

            GenerateReferenceFile(fileName, classes.OrderBy(r => r.Name));
        }
    }

    private IEnumerable<string> GetFocusStoresImports(Class classe)
    {
        if (classe.Properties.Any(p => p is IFieldProperty || p is CompositionProperty cp && cp.DomainKind != null))
        {
            yield return "FieldEntry2";
        }

        if (classe.Properties.Any(p => p is CompositionProperty { Kind: "list" } cp && cp.Class == classe))
        {
            yield return "ListEntry";
        }

        if (classe.Properties.Any(p => p is CompositionProperty { Kind: "object" }))
        {
            yield return "ObjectEntry";
        }

        if (classe.Properties.Any(p => p is CompositionProperty { Kind: "list" } cp && cp.Composition == classe))
        {
            yield return "RecursiveListEntry";
        }

        foreach (var p in classe.Properties.OfType<CompositionProperty>().Where(p => p.DomainKind?.TS?.Import == "@focus4/stores"))
        {
            yield return p.DomainKind!.TS!.Type;
        }

        yield return "EntityToType";
        yield return "StoreNode";
    }

    private void GenerateClassFile(string fileName, Class classe)
    {
        using var fw = new FileWriter(fileName, _logger, false);

        if (_config.Focus)
        {
            fw.WriteLine($"import {{{string.Join(", ", GetFocusStoresImports(classe).OrderBy(x => x))}}} from \"@focus4/stores\";");
        }

        if (GetDomainList(classe).Any())
        {
            var arbo = String.Join("/", classe.ModelFile.Module.Split(".").Select(e => ".."));
            fw.WriteLine($"import {{{string.Join(", ", GetDomainList(classe))}}} from \"{arbo}/../domains\";");
        }

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

        fw.Write("\r\n");

        if (_config.Focus)
        {
            fw.Write("export type ");
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
        }
        else
        {
            fw.Write("export interface ");
            fw.Write($"{classe.Name} ");

            if (classe.Extends != null)
            {
                fw.Write($"extends {classe.Extends.Name} ");
            }
        }

        fw.Write("{\r\n");

        foreach (var property in classe.Properties)
        {
            fw.Write($"    {property.Name.ToFirstLower()}{(_config.Focus ? string.Empty : "?")}: ");

            if (_config.Focus)
            {
                if (property is CompositionProperty cp)
                {
                    if (cp.Kind == "list")
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
                    else if (cp.Kind == "object")
                    {
                        fw.Write($"ObjectEntry<{cp.Composition.Name}EntityType>");
                    }
                    else
                    {
                        fw.Write($"FieldEntry2<typeof {cp.Kind}, {cp.DomainKind!.TS!.Type}<{cp.Composition.Name}>>");
                    }
                }
                else if (property is IFieldProperty field)
                {
                    var domain = (field as AliasProperty)?.ListDomain ?? field.Domain;
                    fw.Write($"FieldEntry2<typeof {domain.Name}, {field.TS.Type}>");
                }
            }
            else
            {
                if (property is CompositionProperty cp)
                {
                    if (cp.Kind == "list")
                    {
                        fw.Write($"{cp.Composition.Name}[]");
                    }
                    else if (cp.Kind == "object")
                    {
                        fw.Write(cp.Composition.Name);
                    }
                    else
                    {
                        fw.Write($"{cp.DomainKind!.TS!.Type}<{cp.Composition.Name}>");
                    }
                }
                else if (property is IFieldProperty field)
                {
                    fw.Write(field.TS.Type);
                }
            }

            if (property != classe.Properties.Last())
            {
                fw.Write(",");
            }

            fw.Write("\r\n");
        }

        fw.Write("}\r\n\r\n");

        fw.Write($"export const {classe.Name}Entity");

        if (_config.Focus)
        {
            fw.Write($": {classe.Name}EntityType");
        }

        fw.Write(" = {\r\n");

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
                if (cp.Kind == "list")
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
                else if (cp.Kind == "object")
                {
                    fw.Write("\"object\",");
                }
                else
                {
                    fw.Write("\"field\",");
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
                var domain = (field as AliasProperty)?.ListDomain ?? field.Domain;
                fw.Write(domain.Name);
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
            else if (property is CompositionProperty cp3 && cp3.DomainKind != null)
            {
                fw.Write("        name: \"");
                fw.Write(cp3.Name.ToFirstLower());
                fw.Write("\"");
                fw.Write(",\r\n        domain: ");
                fw.Write(cp3.DomainKind.Name);
                fw.Write(",\r\n        isRequired: true");
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
            fw.WriteLine();
            WriteReferenceDefinition(fw, classe);
        }
    }

    private IEnumerable<string> GetDomainList(Class classe)
    {
        return classe.Properties
            .OfType<IFieldProperty>()
            .Select(property => property is AliasProperty { ListDomain: Domain ld } ? ld.Name : property.Domain.Name)
            .Concat(classe.Properties.OfType<CompositionProperty>().Where(cp => cp.DomainKind != null).Select(cp => cp.DomainKind!.Name))
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
            .Select(property => (property.Composition, property.DomainKind))
            .Where(c => c.Composition.Name != classe.Name);

        if (classe.Extends != null)
        {
            types = types.Concat(new[] { (classe.Extends, (Domain?)null) });
        }

        var currentModule = classe.Namespace.Module;

        var imports = types.Select(type =>
        {
            var module = type.Composition.Namespace.Module;
            var name = type.Composition.Name;

            module = module == currentModule
                ? $"."
                : $"../{module.ToLower()}";

            return (
                import: type.DomainKind == null ? $"{name}Entity, {name}{(_config.Focus ? "EntityType" : string.Empty)}" : name,
                path: $"{module}/{name.ToDashCase()}");
        }).Distinct().ToList();

        var references = classe.Properties
            .Select(p => p is AliasProperty alp ? alp.Property : p)
            .OfType<IFieldProperty>()
            .Select(prop => (prop, classe: prop is AssociationProperty ap ? ap.Association : prop.Class))
            .Where(pc => pc.prop.TS.Type != pc.prop.Domain.TS!.Type && pc.prop.Domain.TS.Type == "string" && pc.classe.Reference)
            .Select(pc => (Code: pc.prop.TS.Type, pc.classe.Namespace.Module))
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

        imports.AddRange(
            classe.Properties.OfType<IFieldProperty>()
                .Where(p => p.Domain.TS?.Import != null)
                .Select(p => (p.Domain.TS!.Type, p.Domain.TS.Import!))
                .Distinct());

        imports.AddRange(
            classe.Properties.OfType<CompositionProperty>()
                .Where(p => p.DomainKind != null && p.DomainKind.TS!.Import != "@focus4/stores")
                .Select(p => (p.DomainKind!.TS!.Type, p.DomainKind.TS.Import!))
                .Distinct());

        return imports
            .GroupBy(i => i.path)
            .Select(i => (import: string.Join(", ", i.Select(l => l.import)), path: i.Key))
            .OrderBy(i => i.path.StartsWith(".") ? i.path : $"...{i.path}")
            .ToList();
    }

    /// <summary>
    /// Create the template output
    /// </summary>
    private void GenerateReferenceFile(string fileName, IEnumerable<Class> references)
    {
        using var fw = new FileWriter(fileName, _logger, false);

        var imports = references
            .SelectMany(classe => classe.Properties.OfType<IFieldProperty>().Select(fp => (fp.TS.Type, fp.TS.Import)))
            .Where(type => type.Import != null)
            .Distinct()
            .OrderBy(fp => fp.Import)
            .ToList();

        foreach (var import in imports)
        {
            fw.Write("import {");
            fw.Write(import.Type);
            fw.Write("} from \"");
            fw.Write(import.Import);
            fw.Write("\";\r\n");
        }

        if (imports.Any())
        {
            fw.Write("\r\n");
        }

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
                ? string.Join(" | ", reference.ReferenceValues.Select(r => $@"""{r.Value[reference.PrimaryKey ?? reference.Properties.OfType<IFieldProperty>().First()]}""").OrderBy(x => x, StringComparer.Ordinal))
                : "string");
            fw.WriteLine(";");

            if (reference.FlagProperty != null && reference.ReferenceValues != null)
            {
                fw.Write($"export enum {reference.Name}Flag {{\r\n");

                var flagProperty = reference.Properties.OfType<IFieldProperty>().Single(rp => rp.Name == reference.FlagProperty);
                var flagValues = reference.ReferenceValues.Where(refValue => int.TryParse((string)refValue.Value[flagProperty], out var _)).ToList();
                foreach (var refValue in flagValues)
                {
                    var flag = int.Parse((string)refValue.Value[flagProperty]);
                    fw.Write($"    {refValue.Name} = 0b{Convert.ToString(flag, 2)}");
                    if (flagValues.IndexOf(refValue) != flagValues.Count - 1)
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

            WriteReferenceDefinition(fw, reference);
        }
    }

    private void WriteReferenceDefinition(FileWriter fw, Class classe)
    {
        fw.Write("export const ");
        fw.Write(classe.Name.ToFirstLower());
        fw.Write(" = {type: {} as ");
        fw.Write(classe.Name);
        fw.Write(", valueKey: \"");
        fw.Write((classe.PrimaryKey ?? classe.Properties.OfType<IFieldProperty>().First()).Name.ToFirstLower());
        fw.Write("\", labelKey: \"");
        fw.Write(classe.DefaultProperty?.ToFirstLower() ?? "libelle");
        fw.Write("\"} as const;\r\n");
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

        return property.TS?.Type ?? throw new ModelException(property.Class.ModelFile, $"Le type Typescript du domaine {property.Domain.Name} doit être renseigné.");
    }
}