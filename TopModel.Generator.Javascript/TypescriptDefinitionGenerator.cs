using Microsoft.Extensions.Logging;
using TopModel.Core.Model;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Javascript;

using static JavascriptUtils;

/// <summary>
/// Générateur de définitions Typescript.
/// </summary>
public class TypescriptDefinitionGenerator(
    ILogger<TypescriptDefinitionGenerator> logger,
    IFileWriterProvider writerProvider
) : ClassGeneratorBase<JavascriptConfig>(logger, writerProvider)
{
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
        using var fw = OpenFileWriter(fileName, encoderShouldEmitUTF8Identifier: false);

        var commonImports = new List<(string Import, string Path)>();

        var entityTypesPath =
            Config.EntityTypesPath ?? (Config.EntityMode == EntityMode.FOCUS ? "@focus4/entities" : "@focus4/stores");

        if (Config.EntityMode == EntityMode.TYPED || Config.EntityMode == EntityMode.FOCUS)
        {
            var storeImport = Config.GetRelativePath(entityTypesPath, fileName);
            commonImports.AddRange(
                GetEntityImports(fileName, classe, tag, entityTypesPath).Select(import => (import, storeImport))
            );
        }

        if (
            (Config.EntityMode != EntityMode.NONE)
            && classe.Properties.Any(c => c.Domain is not null && !Config.IsListComposition(c))
        )
        {
            var domainImport = Config.GetRelativePath(Config.ResolveVariables(Config.DomainPath, tag), fileName);
            commonImports.AddRange(
                classe
                    .Properties.Select(p =>
                        p is not CompositionProperty and not AliasProperty { Property: CompositionProperty } ? p.Domain
                        : p is CompositionProperty or AliasProperty { Property: CompositionProperty }
                        && !Config.IsListComposition(p)
                            ? p.Domain
                        : null!
                    )
                    .Where(d => d != null)
                    .Distinct()
                    .Select(domain => (domain.Name.Value, domainImport))
            );
        }

        foreach (var import in commonImports.GroupAndSort())
        {
            fw.WriteLine($"import {{{import.Import}}} from \"{import.Path}\";");
        }

        var dependencyImports = classe
            .ClassDependencies.Select(dep =>
                (
                    Import: (
                        dep is { Source: CompositionProperty { Domain: not null } cp } && !Config.IsListComposition(cp)
                    )
                    || (
                        dep is { Source: AliasProperty { Property: CompositionProperty { Domain: not null } cp2 } }
                        && !Config.IsListComposition(cp2)
                    )
                        ? dep.Classe.NamePascal
                    : dep
                        is
                    {
                        Source: IProperty fp
                                and not CompositionProperty
                                and not AliasProperty { Property: CompositionProperty }
                    }
                        ? Config.GetEnumType(fp)
                    : $"{(Config.EntityMode == EntityMode.TYPED || Config.EntityMode == EntityMode.UNTYPED ? dep.Classe.NamePascal + "Entity, " : string.Empty)}{dep.Classe.NamePascal}{(Config.EntityMode == EntityMode.TYPED ? "EntityType" : Config.EntityMode == EntityMode.FOCUS ? "Entity" : string.Empty)}",
                    Path: Config.GetImportPathForClass(
                        dep,
                        dep.Classe.Tags.Contains(tag)
                            ? tag
                            : dep.Classe.Tags.Intersect(Config.Tags).FirstOrDefault() ?? tag,
                        tag
                    )!
                )
            )
            .Concat(classe.Properties.SelectMany(dep => Config.GetDomainImportPaths(fileName, dep, tag)))
            .Concat(classe.Properties.SelectMany(dep => Config.GetValueImportPaths(fileName, dep)))
            .Where(p => p.Path != null && p.Path != entityTypesPath)
            .GroupAndSort();

        fw.WriteLine();

        foreach (var import in dependencyImports)
        {
            fw.WriteLine($"import {{{import.Import}}} from \"{import.Path}\";");
        }

        if (dependencyImports.Count > 0)
        {
            fw.WriteLine();
        }

        if (Config.EntityMode == EntityMode.TYPED)
        {
            fw.WriteLine($"export type {classe.NamePascal} = EntityToType<{classe.NamePascal}EntityType>;");

            fw.Write($"export interface {classe.NamePascal}EntityType ");

            if (classe.Extends != null)
            {
                fw.Write($"extends {classe.Extends.NamePascal}EntityType ");
            }
        }
        else if (Config.EntityMode == EntityMode.FOCUS)
        {
            fw.WriteLine($"export type {classe.NamePascal} = EntityToType<{classe.NamePascal}EntityType>;");
            fw.WriteLine($"export type {classe.NamePascal}EntityType = typeof {classe.NamePascal}Entity;");
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
        if (Config.EntityMode != EntityMode.FOCUS)
        {
            fw.Write("{\r\n");

            foreach (var property in classe.Properties)
            {
                fw.Write($"    {property.NameCamel}{(Config.EntityMode == EntityMode.TYPED ? string.Empty : "?")}: ");

                if (Config.EntityMode == EntityMode.TYPED)
                {
                    switch (property)
                    {
                        case CompositionProperty { Domain: null } cp:
                            fw.Write($"ObjectEntry<{cp.Composition.NamePascal}EntityType>;");
                            break;
                        case AliasProperty { Property: CompositionProperty { Domain: null } cp }:
                            fw.Write($"ObjectEntry<{cp.Composition.NamePascal}EntityType>;");
                            break;
                        case CompositionProperty cp when Config.IsListComposition(cp):
                            if (cp.Composition.Name == classe.Name)
                            {
                                fw.Write($"RecursiveListEntry");
                            }
                            else
                            {
                                fw.Write($"ListEntry<{cp.Composition.NamePascal}EntityType>;");
                            }

                            break;
                        case AliasProperty { Property: CompositionProperty cp } when Config.IsListComposition(cp):
                            if (cp.Composition.Name == classe.Name)
                            {
                                fw.Write($"RecursiveListEntry;");
                            }
                            else
                            {
                                fw.Write($"ListEntry<{cp.Composition.NamePascal}EntityType>;");
                            }

                            break;
                        default:
                            fw.Write($"FieldEntry2<typeof {property.Domain.Name}, {Config.GetType(property)}>;");
                            break;
                    }
                }
                else
                {
                    fw.Write($"{Config.GetType(property)};");
                }

                fw.Write("\r\n");
            }

            fw.Write("}\r\n\r\n");
        }

        if (Config.EntityMode == EntityMode.TYPED || Config.EntityMode == EntityMode.UNTYPED)
        {
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
                    case CompositionProperty { Domain: null }:
                    case AliasProperty { Property: CompositionProperty { Domain: null } }:
                        fw.Write("\"object\",");
                        break;
                    case CompositionProperty cp1
                        when Config.IsListComposition(cp1) && cp1.Composition.Name == classe.Name:
                    case AliasProperty { Property: CompositionProperty cp2 }
                        when Config.IsListComposition(cp2) && cp2.Composition.Name == classe.Name:
                        fw.Write("\"recursive-list\"");
                        if (Config.ExtendedCompositions)
                        {
                            fw.Write(",");
                        }

                        break;
                    case CompositionProperty when Config.IsListComposition(property):
                    case AliasProperty { Property: CompositionProperty } when Config.IsListComposition(property):
                        fw.Write("\"list\",");
                        break;
                    default:
                        fw.Write("\"field\",");
                        break;
                }

                fw.Write("\r\n");

                var cp = property switch
                {
                    CompositionProperty c => c,
                    AliasProperty { Property: CompositionProperty c } => c,
                    _ => null,
                };

                if (cp == null || cp.Domain != null && !Config.IsListComposition(cp))
                {
                    fw.WriteLine(2, $"name: \"{property.NameCamel}\",");
                    fw.WriteLine(2, $"domain: {property.Domain.Name},");

                    var defaultValue = Config.GetValue(property);
                    if (defaultValue != "undefined")
                    {
                        fw.WriteLine(2, $"defaultValue: {defaultValue},");
                    }
                }
                else if (cp.Composition.Name != classe.Name)
                {
                    fw.Write(2, $"entity: {cp.Composition.NamePascal}Entity");

                    if (Config.ExtendedCompositions)
                    {
                        fw.Write(",");
                    }

                    fw.WriteLine();
                }

                if (cp == null || cp.Domain != null && !Config.IsListComposition(cp) || Config.ExtendedCompositions)
                {
                    fw.WriteLine(
                        2,
                        $"isRequired: {(property.Required && !((property.PrimaryKey || property is AliasProperty { AliasedPrimaryKey: true }) && property.Domain.AutoGeneratedValue)).ToString().ToFirstLower()},"
                    );

                    if (Config.TranslateProperties == true)
                    {
                        fw.WriteLine(
                            2,
                            $"label: \"{property.ResourceKey}\"{(Config.GenerateComments ? "," : string.Empty)}"
                        );
                    }
                    else
                    {
                        fw.WriteLine(2, $"label: \"{property.Label}\"{(Config.GenerateComments ? "," : string.Empty)}");
                    }

                    if (Config.GenerateComments)
                    {
                        if (Config.TranslateProperties == true)
                        {
                            fw.WriteLine(2, $"comment: \"{property.CommentResourceKey}\"");
                        }
                        else
                        {
                            fw.WriteLine(2, $"comment: \"{property.Comment}\"");
                        }
                    }
                }

                fw.Write(1, "}");

                if (property != classe.Properties[^1])
                {
                    fw.Write(",");
                }

                fw.WriteLine();
            }

            fw.WriteLine($"}}{(Config.EntityMode == EntityMode.TYPED ? string.Empty : " as const")};");
        }
        else if (Config.EntityMode == EntityMode.FOCUS)
        {
            fw.WriteLine();
            fw.WriteLine($"export const {classe.NamePascal}Entity = entity({{");

            if (classe.Extends != null)
            {
                fw.WriteLine(1, $"...{classe.Extends.NamePascal}Entity,");
            }

            foreach (var property in classe.Properties)
            {
                fw.Write(1, $"{property.NameCamel}: e.");

                var cp = property switch
                {
                    CompositionProperty c => c,
                    AliasProperty { Property: CompositionProperty c } => c,
                    _ => null,
                };

                switch (property)
                {
                    case CompositionProperty { Domain: null }:
                    case AliasProperty { Property: CompositionProperty { Domain: null } }:
                        fw.Write("object");
                        break;
                    case CompositionProperty cp1
                        when Config.IsListComposition(cp1) && cp1.Composition.Name == classe.Name:
                    case AliasProperty { Property: CompositionProperty cp2 }
                        when Config.IsListComposition(cp2) && cp2.Composition.Name == classe.Name:
                        fw.Write("recursiveList");
                        break;
                    case CompositionProperty when Config.IsListComposition(property):
                    case AliasProperty { Property: CompositionProperty } when Config.IsListComposition(property):
                        fw.Write("list");
                        break;
                    default:
                        fw.Write("field");
                        break;
                }
                fw.Write("(");

                if (cp != null && (Config.IsListComposition(cp) || cp.Domain == null))
                {
                    fw.Write($"{cp.Composition.NamePascal}Entity");
                }
                else
                {
                    fw.Write(property.Domain.Name);
                }

                fw.Write(", f => f");

                var type = Config.GetType(property);
                if (
                    cp == null && type != Config.GetImplementation(property.Domain)?.Type
                    || cp?.Domain != null && !Config.IsListComposition(cp)
                )
                {
                    fw.Write($".type<{type}>()");
                }

                var defaultValue = Config.GetValue(property);
                if (defaultValue != "undefined")
                {
                    fw.Write($".defaultValue({defaultValue})");
                }

                if (
                    !(
                        property.Required
                        && !(
                            (property.PrimaryKey || property is AliasProperty { AliasedPrimaryKey: true })
                            && property.Domain.AutoGeneratedValue
                        )
                    )
                )
                {
                    fw.WriteLine(".optional()");
                }
                else
                {
                    fw.WriteLine();
                }

                fw.WriteLine(
                    2,
                    $".label(\"{(Config.TranslateProperties == true ? property.ResourceKey : property.Label)}\")"
                );

                if (Config.GenerateComments)
                {
                    fw.WriteLine(
                        2,
                        $".comment(\"{(Config.TranslateProperties == true ? property.CommentResourceKey : property.Comment)}\")"
                    );
                }

                fw.Write(1, ")");
                fw.WriteLine(property == classe.Properties[^1] ? "" : ",");
            }

            fw.WriteLine("});");
        }

        if (classe.Reference)
        {
            fw.WriteLine();
            WriteReferenceDefinition(fw, classe);
        }
    }

    private IEnumerable<string> GetEntityImports(string fileName, Class classe, string tag, string entityTypesPath)
    {
        if (Config.EntityMode == EntityMode.FOCUS)
        {
            yield return "e";
            yield return "entity";
            yield return "EntityToType";
            yield break;
        }

        if (classe.Properties.Any(p => p.Domain is not null && !Config.IsListComposition(p)))
        {
            yield return "FieldEntry2";
        }

        if (
            classe.Properties.Any(p =>
                p
                    is CompositionProperty { Domain: null }
                        or AliasProperty { Property: CompositionProperty { Domain: null } }
            )
        )
        {
            yield return "ObjectEntry";
        }

        if (
            classe.Properties.Any(p =>
                (p is CompositionProperty && p.Class == classe && Config.IsListComposition(p))
                || (
                    p is AliasProperty { Property: CompositionProperty }
                    && p.Class == classe
                    && Config.IsListComposition(p)
                )
            )
        )
        {
            yield return "ListEntry";
        }

        if (
            classe.Properties.Any(p =>
                (p is CompositionProperty cp && cp.Composition == classe && Config.IsListComposition(p))
                || (
                    p is AliasProperty { Property: CompositionProperty cp2 }
                    && cp2.Composition == classe
                    && Config.IsListComposition(p)
                )
            )
        )
        {
            yield return "RecursiveListEntry";
        }

        foreach (
            var p in classe
                .Properties.SelectMany(dep => Config.GetDomainImportPaths(fileName, dep, tag))
                .Where(p => p.Path == entityTypesPath)
        )
        {
            yield return p.Import;
        }

        yield return "EntityToType";
    }
}
