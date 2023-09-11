using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Javascript;

using static JavascriptUtils;

/// <summary>
/// Générateur de définitions Typescript.
/// </summary>
public class TypescriptDefinitionGenerator : ClassGeneratorBase<JavascriptConfig>
{
    private readonly ILogger<TypescriptDefinitionGenerator> _logger;

    public TypescriptDefinitionGenerator(ILogger<TypescriptDefinitionGenerator> logger)
        : base(logger)
    {
        _logger = logger;
    }

    public override string Name => "JSDefinitionGen";

    protected override bool FilterClass(Class classe)
    {
        return !classe.IsJSReference();
    }

    protected override string GetFileName(Class classe, string tag)
    {
        return Config.GetClassFileName(classe, tag);
    }

    protected override void HandleClass(string fileName, Class classe, string tag)
    {
        using var fw = new FileWriter(fileName, _logger, false);

        if (Config.EntityMode == EntityMode.TYPED)
        {
            fw.WriteLine($"import {{{string.Join(", ", GetFocusStoresImports(fileName, classe, tag).OrderBy(x => x))}}} from \"{Config.EntityTypesPath}\";");
        }

        if (classe.Properties.Any(c => c is IFieldProperty or CompositionProperty { Domain: not null } && !Config.IsListComposition(c)))
        {
            var domainImport = Config.GetRelativePath(Config.ResolveVariables(Config.DomainPath, tag), fileName);
            fw.WriteLine($"import {{{string.Join(", ", classe.Properties.Select(p => p is IFieldProperty fp ? fp.Domain : p is CompositionProperty cp && !Config.IsListComposition(cp) ? cp.Domain! : null!).Where(d => d != null).OrderBy(d => d.Name).Select(d => d.Name).Distinct())}}} from \"{domainImport}\";");
        }

        var imports = classe.ClassDependencies
            .Select(dep => (
                Import: dep is { Source: CompositionProperty { Domain: not null } cp } && !Config.IsListComposition(cp)
                    ? dep.Classe.NamePascal
                    : dep is { Source: IFieldProperty fp }
                    ? Config.GetEnumType(fp)
                    : $"{dep.Classe.NamePascal}Entity, {dep.Classe.NamePascal}{(Config.EntityMode == EntityMode.TYPED ? "EntityType" : string.Empty)}",
                Path: Config.GetImportPathForClass(dep, dep.Classe.Tags.Contains(tag) ? tag : dep.Classe.Tags.Intersect(Config.Tags).FirstOrDefault() ?? tag, tag, Classes)!))
            .Concat(classe.Properties.SelectMany(dep => Config.GetDomainImportPaths(fileName, dep, tag)))
            .Concat(classe.Properties.OfType<IFieldProperty>().SelectMany(dep => Config.GetValueImportPaths(fileName, dep)))
            .Where(p => p.Path != null && p.Path != Config.EntityTypesPath)
            .GroupAndSort();

        fw.WriteLine();

        foreach (var import in imports)
        {
            fw.WriteLine($"import {{{import.Import}}} from \"{import.Path}\";");
        }

        if (imports.Any())
        {
            fw.WriteLine();
        }

        if (Config.EntityMode == EntityMode.TYPED)
        {
            fw.WriteLine($"export type {classe.NamePascal} = EntityToType<{classe.NamePascal}EntityType>");

            fw.Write($"export interface {classe.NamePascal}EntityType ");

            if (classe.Extends != null)
            {
                fw.Write($"extends {classe.Extends.NamePascal}EntityType ");
            }
        }
        else
        {
            fw.Write("export interface ");
            fw.Write($"{classe.NamePascal} ");

            if (classe.Extends != null)
            {
                fw.Write($"extends {classe.Extends.NamePascal} ");
            }
        }

        fw.Write("{\r\n");

        foreach (var property in classe.Properties)
        {
            fw.Write($"    {property.NameCamel}{(Config.EntityMode == EntityMode.TYPED ? string.Empty : "?")}: ");

            if (Config.EntityMode == EntityMode.TYPED)
            {
                switch (property)
                {
                    case CompositionProperty { Domain: null } cp:
                        fw.Write($"ObjectEntry<{cp.Composition.NamePascal}EntityType>");
                        break;
                    case CompositionProperty cp2 when Config.IsListComposition(cp2):
                        if (cp2.Composition.Name == classe.Name)
                        {
                            fw.Write($"RecursiveListEntry");
                        }
                        else
                        {
                            fw.Write($"ListEntry<{cp2.Composition.NamePascal}EntityType>");
                        }

                        break;
                    default:
                        fw.Write($"FieldEntry2<typeof {property.Domain.Name}, {Config.GetType(property, Classes)}>");
                        break;
                }
            }
            else
            {
                fw.Write(Config.GetType(property, Classes));
            }

            if (property != classe.Properties.Last())
            {
                fw.Write(",");
            }

            fw.Write("\r\n");
        }

        fw.Write("}\r\n\r\n");

        fw.Write($"export const {classe.NamePascal}Entity");

        if (Config.EntityMode == EntityMode.TYPED)
        {
            fw.Write($": {classe.NamePascal}EntityType");
        }

        fw.Write(" = {\r\n");

        if (classe.Extends != null)
        {
            fw.Write("    ...");
            fw.Write(classe.Extends.NamePascal);
            fw.Write("Entity,\r\n");
        }

        foreach (var property in classe.Properties)
        {
            fw.Write("    ");
            fw.Write(property.NameCamel);
            fw.Write(": {\r\n");
            fw.Write("        type: ");

            switch (property)
            {
                case CompositionProperty { Domain: null } cp:
                    fw.Write("\"object\",");
                    break;
                case CompositionProperty cp2 when Config.IsListComposition(cp2):
                    if (cp2.Composition.Name == classe.Name)
                    {
                        fw.Write("\"recursive-list\"");
                    }
                    else
                    {
                        fw.Write("\"list\",");
                    }

                    break;
                default:
                    fw.Write("\"field\",");
                    break;
            }

            fw.Write("\r\n");

            if (property is IFieldProperty field)
            {
                fw.WriteLine($"        name: \"{field.NameCamel}\",");
                fw.WriteLine($"        domain: {field.Domain.Name},");
                fw.WriteLine($"        isRequired: {(field.Required && !(field.PrimaryKey && field.Domain.AutoGeneratedValue)).ToString().ToFirstLower()},");

                var defaultValue = Config.GetValue(field, Classes);
                if (defaultValue != "undefined")
                {
                    fw.WriteLine($"        defaultValue: {defaultValue},");
                }

                fw.WriteLine($"        label: \"{field.ResourceKey}\"{(Config.GenerateComments ? "," : string.Empty)}");

                if (Config.GenerateComments)
                {
                    fw.WriteLine($"        comment: \"{field.CommentResourceKey}\"");
                }
            }
            else if (property is CompositionProperty cp3 && cp3.Domain != null && !Config.IsListComposition(cp3))
            {
                fw.Write("        name: \"");
                fw.Write(cp3.NameCamel);
                fw.Write("\"");
                fw.Write(",\r\n        domain: ");
                fw.Write(cp3.Domain.Name);
                fw.Write(",\r\n        isRequired: true");
                fw.Write(",\r\n        label: \"");
                fw.Write(classe.Namespace.ModuleCamel);
                fw.Write(".");
                fw.Write(classe.NameCamel);
                fw.Write(".");
                fw.Write(property.NameCamel);
                fw.Write("\"\r\n");
            }
            else if (property is CompositionProperty cp2 && cp2.Composition.Name != classe.Name)
            {
                fw.Write("        entity: ");
                fw.Write(cp2.Composition.NamePascal);
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

        fw.Write($"}}{(Config.EntityMode == EntityMode.TYPED ? string.Empty : " as const")}\r\n");

        if (classe.Reference)
        {
            fw.WriteLine();
            WriteReferenceDefinition(fw, classe);
        }
    }

    private IEnumerable<string> GetFocusStoresImports(string fileName, Class classe, string tag)
    {
        if (classe.Properties.Any(p => p is IFieldProperty || p is CompositionProperty { Domain: not null } && !Config.IsListComposition(p)))
        {
            yield return "FieldEntry2";
        }

        if (classe.Properties.Any(p => p is CompositionProperty { Domain: null }))
        {
            yield return "ObjectEntry";
        }

        if (classe.Properties.Any(p => p is CompositionProperty cp && cp.Class == classe && Config.IsListComposition(p)))
        {
            yield return "ListEntry";
        }

        if (classe.Properties.Any(p => p is CompositionProperty cp && cp.Composition == classe && Config.IsListComposition(p)))
        {
            yield return "RecursiveListEntry";
        }

        foreach (var p in classe.Properties.SelectMany(dep => Config.GetDomainImportPaths(fileName, dep, tag)).Where(p => p.Path == Config.EntityTypesPath))
        {
            yield return p.Import;
        }

        yield return "EntityToType";
    }
}