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

    protected override object? GetDomainType(Domain domain)
    {
        return domain.TS;
    }

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

        if (Config.TargetFramework == TargetFramework.FOCUS)
        {
            fw.WriteLine($"import {{{string.Join(", ", GetFocusStoresImports(classe).OrderBy(x => x))}}} from \"@focus4/stores\";");
        }

        if (classe.DomainDependencies.Any())
        {
            var domainImport = Config.DomainPath.StartsWith("@")
                ? Config.DomainPath
                : Path.GetRelativePath(string.Join('/', fileName.Split('/').SkipLast(1)), Path.Combine(Config.OutputDirectory, Config.ResolveVariables(Config.DomainPath, tag))).Replace("\\", "/");
            fw.WriteLine($"import {{{string.Join(", ", classe.DomainDependencies.OrderBy(d => d.Domain.Name).Select(d => d.Domain.Name).Distinct())}}} from \"{domainImport}\";");
        }

        var imports = classe.ClassDependencies
            .Select(dep => (
                Import: dep is { Source: CompositionProperty { DomainKind: not null } }
                    ? dep.Classe.NamePascal
                    : dep is { Source: IFieldProperty fp }
                    ? fp.GetPropertyTypeName(Classes).Replace("[]", string.Empty)
                    : $"{dep.Classe.NamePascal}Entity, {dep.Classe.NamePascal}{(Config.TargetFramework == TargetFramework.FOCUS ? "EntityType" : string.Empty)}",
                Path: Config.GetImportPathForClass(dep, GetClassTags(dep.Classe).Contains(tag) ? tag : GetClassTags(dep.Classe).Intersect(Config.Tags).FirstOrDefault() ?? tag, tag, Classes)!))
            .Concat(classe.DomainDependencies.SelectMany(dep => dep.Domain.TS!.Imports.Select(import => (Import: dep.Domain.TS!.Type.ParseTemplate(dep.Source).Replace("[]", string.Empty).Split("<").First(), Path: import.ParseTemplate(dep.Source)))))
            .Where(p => p.Path != null && p.Path != "@focus4/stores")
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

        if (Config.TargetFramework == TargetFramework.FOCUS)
        {
            fw.Write("export type ");
            fw.Write(classe.NamePascal);
            fw.Write(" = EntityToType<");
            fw.Write(classe.NamePascal);
            fw.Write("EntityType>;\r\nexport type ");
            fw.Write(classe.NamePascal);
            fw.Write("Node = StoreNode<");
            fw.Write(classe.NamePascal);
            fw.Write("EntityType>;\r\n");

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
            fw.Write($"    {property.NameCamel}{(Config.TargetFramework == TargetFramework.FOCUS ? string.Empty : "?")}: ");

            if (Config.TargetFramework == TargetFramework.FOCUS)
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
                            fw.Write($"ListEntry<{cp.Composition.NamePascal}EntityType>");
                        }
                    }
                    else if (cp.Kind == "object")
                    {
                        fw.Write($"ObjectEntry<{cp.Composition.NamePascal}EntityType>");
                    }
                    else
                    {
                        fw.Write($"FieldEntry2<typeof {cp.Kind}, {cp.GetPropertyTypeName(Classes)}>");
                    }
                }
                else if (property is IFieldProperty field)
                {
                    fw.Write($"FieldEntry2<typeof {field.Domain.Name}, {field.GetPropertyTypeName(Classes)}>");
                }
            }
            else
            {
                fw.Write(property.GetPropertyTypeName(Classes));
            }

            if (property != classe.Properties.Last())
            {
                fw.Write(",");
            }

            fw.Write("\r\n");
        }

        fw.Write("}\r\n\r\n");

        fw.Write($"export const {classe.NamePascal}Entity");

        if (Config.TargetFramework == TargetFramework.FOCUS)
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
                fw.WriteLine($"        name: \"{field.NameCamel}\",");
                fw.WriteLine($"        domain: {field.Domain.Name},");
                fw.WriteLine($"        isRequired: {(field.Required && !field.PrimaryKey).ToString().ToFirstLower()},");

                var defaultValue = Config.GetDefaultValue(field);
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
            else if (property is CompositionProperty cp3 && cp3.DomainKind != null)
            {
                fw.Write("        name: \"");
                fw.Write(cp3.NameCamel);
                fw.Write("\"");
                fw.Write(",\r\n        domain: ");
                fw.Write(cp3.DomainKind.Name);
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

        fw.Write($"}}{(Config.TargetFramework == TargetFramework.FOCUS ? string.Empty : " as const")}\r\n");

        if (classe.Reference)
        {
            fw.WriteLine();
            WriteReferenceDefinition(fw, classe);
        }
    }

    private static IEnumerable<string> GetFocusStoresImports(Class classe)
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

        foreach (var p in classe.Properties.OfType<CompositionProperty>().Where(p => p.DomainKind?.TS?.Imports.Contains("@focus4/stores") ?? false))
        {
            yield return p.DomainKind!.TS!.Type.ParseTemplate(p).Replace("[]", string.Empty).Split('<').First();
        }

        yield return "EntityToType";
        yield return "StoreNode";
    }
}