using Microsoft.Extensions.Logging;
using TopModel.Core.Model;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Javascript;

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
        return classe.Enum == null;
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
            && Config.GetProperties(classe).Any(c => c.Domain is not null && !Config.IsListComposition(c))
        )
        {
            var domainImport = Config.GetRelativePath(Config.ResolveVariables(Config.DomainPath, tag), fileName);
            commonImports.AddRange(
                Config
                    .GetProperties(classe)
                    .Select(p =>
                        p is not { Composition: not null } ? p.Domain
                        : p is { Composition: not null } && !Config.IsListComposition(p) ? p.Domain
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

        var dependencyImports = Config
            .GetClassDependencies(classe)
            .Select(dep =>
                (
                    Import: (
                        dep is { Source: IProperty { Composition: Class cpc } cp }
                        && cpc.NamePascal != Config.GetType(cp)
                        && !Config.IsListComposition(cp)
                    )
                        ? dep.Classe.NamePascal
                    : dep is { Source: IProperty fp and not { Composition: not null } } ? Config.GetEnumType(fp)
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
            .Concat(
                Config
                    .GetProperties(classe)
                    .Where(p =>
                        Config.EntityMode != EntityMode.FOCUS
                        || p.Composition == null
                            && Config.GetType(p, forceAssociationPropertyType: true)
                                != Config.GetImplementation(p.Domain)?.Type
                        || p.Composition != null
                            && Config.GetType(p) != p.Composition!.NamePascal
                            && !Config.IsListComposition(p)
                    )
                    .SelectMany(dep => Config.GetDomainImportPaths(fileName, dep, tag))
            )
            .Concat(Config.GetProperties(classe).SelectMany(dep => Config.GetValueImportPaths(fileName, dep)))
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
            fw.WriteLine("{");

            foreach (var property in Config.GetProperties(classe))
            {
                fw.Write($"    {property.NameCamel}{(Config.EntityMode == EntityMode.TYPED ? string.Empty : "?")}: ");
                var type = Config.GetType(property, forceAssociationPropertyType: true);

                if (Config.EntityMode == EntityMode.TYPED)
                {
                    switch (property)
                    {
                        case { Composition: Class cpc } when cpc.NamePascal == type:
                            fw.Write($"ObjectEntry<{cpc.NamePascal}EntityType>;");
                            break;
                        case { Composition: Class cpc } cp when Config.IsListComposition(cp):
                            if (cpc.Name == classe.Name)
                            {
                                fw.Write($"RecursiveListEntry");
                            }
                            else
                            {
                                fw.Write($"ListEntry<{cpc.NamePascal}EntityType>;");
                            }

                            break;
                        default:
                            fw.Write($"FieldEntry2<typeof {property.Domain!.Name}, {type}>;");
                            break;
                    }
                }
                else
                {
                    fw.Write($"{Config.GetType(property, forceAssociationPropertyType: true)};");
                }

                fw.WriteLine();
            }

            fw.WriteLine("}");
            fw.WriteLine();
        }

        if (Config.EntityMode == EntityMode.TYPED || Config.EntityMode == EntityMode.UNTYPED)
        {
            fw.Write($"export const {classe.NamePascal}Entity");

            if (Config.EntityMode == EntityMode.TYPED)
            {
                fw.Write($": {classe.NamePascal}EntityType");
            }

            fw.WriteLine(" = {");

            if (classe.Extends != null)
            {
                fw.Write("    ...");
                fw.Write(classe.Extends.NamePascal);
                fw.WriteLine("Entity,");
            }

            foreach (var property in Config.GetProperties(classe))
            {
                fw.Write("    ");
                fw.Write(property.NameCamel);
                fw.WriteLine(": {");
                fw.Write("        type: ");

                var type = Config.GetType(property, forceAssociationPropertyType: true);
                switch (property)
                {
                    case { Composition: Class cpc } when cpc.NamePascal == type:
                        fw.Write("\"object\",");
                        break;
                    case { Composition: Class cpc } when Config.IsListComposition(property) && cpc.Name == classe.Name:
                        fw.Write("\"recursive-list\"");
                        if (Config.ExtendedCompositions)
                        {
                            fw.Write(",");
                        }

                        break;
                    case { Composition: not null } when Config.IsListComposition(property):
                        fw.Write("\"list\",");
                        break;
                    default:
                        fw.Write("\"field\",");
                        break;
                }

                fw.WriteLine();

                if (
                    property.Composition == null
                    || property.Composition!.NamePascal != type && !Config.IsListComposition(property)
                )
                {
                    fw.WriteLine(2, $"name: \"{property.NameCamel}\",");
                    fw.WriteLine(2, $"domain: {property.Domain!.Name},");

                    var defaultValue = Config.GetValue(property);
                    if (defaultValue != "undefined")
                    {
                        fw.WriteLine(2, $"defaultValue: {defaultValue},");
                    }
                }
                else if (property.Composition!.NamePascal != classe.NamePascal)
                {
                    fw.Write(2, $"entity: {property.Composition!.NamePascal}Entity");

                    if (Config.ExtendedCompositions)
                    {
                        fw.Write(",");
                    }

                    fw.WriteLine();
                }

                if (
                    property.Composition == null
                    || property.Composition!.NamePascal != type && !Config.IsListComposition(property)
                    || Config.ExtendedCompositions
                )
                {
                    fw.WriteLine(
                        2,
                        $"isRequired: {(property.Required && !(property.PrimaryKeyish && property.GeneratedValue != null)).ToString().ToFirstLower()},"
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
                        fw.WriteLine(
                            2,
                            $"label: \"{property.Label ?? property.Name}\"{(Config.GenerateComments ? "," : string.Empty)}"
                        );
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

                if (property != Config.GetProperties(classe).Last())
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

            foreach (var property in Config.GetProperties(classe))
            {
                fw.Write(1, $"{property.NameCamel}: e.");
                var type = Config.GetType(property, forceAssociationPropertyType: true);

                switch (property)
                {
                    case { Composition: Class cpc } when cpc.NamePascal == type:
                        fw.Write("object");
                        break;
                    case { Composition: Class cpc } when Config.IsListComposition(property) && cpc.Name == classe.Name:
                        fw.Write("recursiveList");
                        if (Config.ExtendedCompositions)
                        {
                            fw.Write(",");
                        }

                        break;
                    case { Composition: not null } when Config.IsListComposition(property):
                        fw.Write("list");
                        break;
                    default:
                        fw.Write("field");
                        break;
                }

                fw.Write("(");

                if (
                    property.Composition != null
                    && (
                        Config.IsListComposition(property) && property.Composition!.Name != classe.Name
                        || type == property.Composition!.NamePascal
                    )
                )
                {
                    fw.Write($"{property.Composition!.NamePascal}Entity, ");
                }
                else if (!Config.IsListComposition(property))
                {
                    fw.Write($"{property.Domain!.Name}, ");
                }

                fw.Write("f => f");

                if (
                    property.Composition == null && type != Config.GetImplementation(property.Domain)?.Type
                    || property.Composition != null
                        && type != property.Composition!.NamePascal
                        && !Config.IsListComposition(property)
                )
                {
                    fw.Write($".type<{type}>()");
                }

                var defaultValue = Config.GetValue(property);
                if (defaultValue != "undefined")
                {
                    fw.Write($".defaultValue({defaultValue})");
                }

                if (!(property.Required && !(property.PrimaryKeyish && property.GeneratedValue != null)))
                {
                    fw.WriteLine(".optional()");
                }
                else
                {
                    fw.WriteLine();
                }

                fw.WriteLine(
                    2,
                    $".label(\"{(Config.TranslateProperties == true ? property.ResourceKey : (property.Label ?? property.Name))}\")"
                );

                if (Config.GenerateComments)
                {
                    fw.WriteLine(
                        2,
                        $".comment(\"{(Config.TranslateProperties == true ? property.CommentResourceKey : property.Comment)}\")"
                    );
                }

                fw.Write(1, ")");
                fw.WriteLine(property == Config.GetProperties(classe).Last() ? "" : ",");
            }

            fw.WriteLine("});");
        }

        if (classe.Reference)
        {
            fw.WriteLine();
            fw.WriteReferenceDefinition(classe, Config);
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

        if (Config.GetProperties(classe).Any(p => p.Domain is not null && !Config.IsListComposition(p)))
        {
            yield return "FieldEntry2";
        }

        if (
            Config
                .GetProperties(classe)
                .Any(p => p is { Composition: Class cpc } && cpc.NamePascal == Config.GetType(p))
        )
        {
            yield return "ObjectEntry";
        }

        if (
            Config
                .GetProperties(classe)
                .Any(p => (p is { Composition: not null } && p.Class == classe && Config.IsListComposition(p)))
        )
        {
            yield return "ListEntry";
        }

        if (Config.GetProperties(classe).Any(p => p.Composition == classe && Config.IsListComposition(p)))
        {
            yield return "RecursiveListEntry";
        }

        foreach (
            var p in Config
                .GetProperties(classe)
                .SelectMany(dep => Config.GetDomainImportPaths(fileName, dep, tag))
                .Where(p => p.Path == entityTypesPath)
        )
        {
            yield return p.Import;
        }

        yield return "EntityToType";
    }
}
