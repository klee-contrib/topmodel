using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Utils;

namespace TopModel.Generator.Javascript;

using static JavascriptUtils;

/// <summary>
/// Générateur de définitions Typescript.
/// </summary>
public class TypescriptDefinitionGenerator : ClassGeneratorBase
{
    private readonly JavascriptConfig _config;
    private readonly ILogger<TypescriptDefinitionGenerator> _logger;

    public TypescriptDefinitionGenerator(ILogger<TypescriptDefinitionGenerator> logger, JavascriptConfig config)
        : base(logger, config)
    {
        _config = config;
        _logger = logger;
    }

    public override string Name => "JSDefinitionGen";

    protected override bool FilterClass(Class classe)
    {
        return !classe.IsJSReference();
    }

    protected override string GetFileName(Class classe, string tag)
    {
        return _config.GetClassFileName(classe, tag);
    }

    protected override void HandleClass(string fileName, Class classe, string tag)
    {
        using var fw = new FileWriter(fileName, _logger, false);

        if (_config.TargetFramework == TargetFramework.FOCUS)
        {
            fw.WriteLine($"import {{{string.Join(", ", GetFocusStoresImports(classe).OrderBy(x => x))}}} from \"@focus4/stores\";");
        }

        if (classe.DomainDependencies.Any())
        {
            var domainImport = _config.DomainPath.StartsWith("@")
                ? _config.DomainPath
                : Path.GetRelativePath(string.Join('/', fileName.Split('/').SkipLast(1)), Path.Combine(_config.OutputDirectory, _config.ResolveVariables(_config.DomainPath, tag))).Replace("\\", "/");
            fw.WriteLine($"import {{{string.Join(", ", classe.DomainDependencies.OrderBy(d => d.Domain.Name).Select(d => d.Domain.Name).Distinct())}}} from \"{domainImport}\";");
        }

        var imports = classe.ClassDependencies
            .Select(dep => (
                Import: dep is { Source: CompositionProperty { DomainKind: not null } }
                    ? dep.Classe.Name
                    : dep is { Source: IFieldProperty fp }
                    ? fp.GetPropertyTypeName(Classes).Replace("[]", string.Empty)
                    : $"{dep.Classe.Name}Entity, {dep.Classe.Name}{(_config.TargetFramework == TargetFramework.FOCUS ? "EntityType" : string.Empty)}",
                Path: _config.GetImportPathForClass(dep, tag, Classes)!))
            .Concat(classe.DomainDependencies.Select(p => (Import: p.Domain.TS!.Type.ParseTemplate(p.Source).Replace("[]", string.Empty).Split("<").First(), Path: p.Domain.TS.Import!.ParseTemplate(p.Source))))
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

        if (_config.TargetFramework == TargetFramework.FOCUS)
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
            fw.Write($"    {property.Name.ToFirstLower()}{(_config.TargetFramework == TargetFramework.FOCUS ? string.Empty : "?")}: ");

            if (_config.TargetFramework == TargetFramework.FOCUS)
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

        fw.Write($"export const {classe.Name}Entity");

        if (_config.TargetFramework == TargetFramework.FOCUS)
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
                fw.WriteLine($"        name: \"{field.Name.ToFirstLower()}\",");
                fw.WriteLine($"        domain: {field.Domain.Name},");
                fw.WriteLine($"        isRequired: {(field.Required && !field.PrimaryKey).ToString().ToFirstLower()},");

                var defaultValue = _config.GetDefaultValue(field);
                if (defaultValue != "undefined")
                {
                    fw.WriteLine($"        defaultValue: {defaultValue},");
                }

                fw.WriteLine($"        label: \"{field.ResourceKey}\"{(_config.GenerateComments ? "," : string.Empty)}");

                if (_config.GenerateComments)
                {
                    fw.WriteLine($"        comment: \"{field.CommentResourceKey}\"");
                }
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
                fw.Write(classe.Namespace.ModuleFirstLower);
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

        fw.Write($"}}{(_config.TargetFramework == TargetFramework.FOCUS ? string.Empty : " as const")}\r\n");

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

        foreach (var p in classe.Properties.OfType<CompositionProperty>().Where(p => p.DomainKind?.TS?.Import == "@focus4/stores"))
        {
            yield return p.DomainKind!.TS!.Type.ParseTemplate(p).Replace("[]", string.Empty).Split('<').First();
        }

        yield return "EntityToType";
        yield return "StoreNode";
    }
}