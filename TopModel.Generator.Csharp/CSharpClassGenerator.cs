using System.Data;
using Microsoft.Extensions.Logging;
using TopModel.Core.Model;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Csharp;

public class CSharpClassGenerator(ILogger<CSharpClassGenerator> logger, IFileWriterProvider writerProvider)
    : ClassGeneratorBase<CsharpConfig>(logger, writerProvider)
{
    public override string Name => "CSharpClassGen";

    /// <summary>
    /// Génération de la déclaration de la classe.
    /// </summary>
    /// <param name="w">Writer</param>
    /// <param name="item">Classe à générer.</param>
    /// <param name="tag">Tag.</param>
    protected virtual void GenerateClassDeclaration(CSharpWriter w, Class item, string tag)
    {
        if (item.Type != ClassType.Interface && item.Enum != EnumMode.Enum)
        {
            if (item.Reference && Config.Kinetix)
            {
                if (item.ReferenceKey?.GeneratedValue == null)
                {
                    w.WriteAttribute("Reference", "true");
                }
                else
                {
                    w.WriteAttribute("Reference");
                }
            }

            if (item.Reference && item.DefaultProperty != null)
            {
                w.WriteAttribute("DefaultProperty", $@"nameof({item.DefaultProperty.NamePascal})");
            }

            if (
                Config.IsPersistent(item, tag)
                && (item.InheritanceStrategy != InheritanceStrategy.DistinctTables || item.Type == ClassType.Regular)
                && item.Extends?.InheritanceStrategy != InheritanceStrategy.SingleTable
            )
            {
                var sqlName = Config.GetSqlName(item, tag);
                if (Config.DbSchema != null)
                {
                    w.WriteAttribute(
                        "Table",
                        $@"""{sqlName}""",
                        $@"Schema = ""{Config.ResolveVariables(Config.DbSchema, tag, module: item.Namespace.Module.ToSnakeCase())}"""
                    );
                }
                else
                {
                    w.WriteAttribute("Table", $@"""{sqlName}""");
                }
            }
        }

        foreach (var (annotation, _) in Config.GetAnnotations(item, tag))
        {
            w.WriteAttribute(annotation);
        }

        if (item.Enum == EnumMode.Enum)
        {
            WriteEnum(w, item.EnumKey!, GetRefs(item), indent: 0);
            return;
        }

        var extends = Config.GetClassExtends(item, tag);
        var implements = Config.GetClassImplements(item, tag);

        w.WriteClassDeclaration(Config.GetTypeName(item), extends, Config.UseRecords, item.Type, implements.ToArray());

        if (item.Type != ClassType.Interface)
        {
            GenerateConstProperties(w, item);

            if (Config.EnumCols && Config.IsPersistent(item, tag))
            {
                GenerateEnumCols(w, item);
            }

            GenerateEnumValues(w, item);

            GenerateFlags(w, item);
        }

        GenerateReadonlyEnumClassInstances(w, item);

        GenerateProperties(w, item, tag);

        GenerateReadonlyEnumKeyMapper(w, item);

        w.WriteLine("}");
    }

    /// <summary>
    /// Génération des constantes statiques.
    /// </summary>
    /// <param name="w">Writer.</param>
    /// <param name="item">La classe générée.</param>
    protected virtual void GenerateConstProperties(CSharpWriter w, Class item)
    {
        if (!Config.UniqueValueGeneration.CanConst)
        {
            return;
        }

        var consts = new List<(IProperty Prop, string Name, string Code, string Label)>();

        foreach (var refValue in Config.GetAllValues(item))
        {
            var label = refValue.GetLabel(item);

            foreach (
                var prop in Config
                    .GetProperties(item)
                    .Where(p =>
                        p.UniqueValuedProperty == p
                        && (
                            p.EnumProperty == null
                            || Config.UniqueValueGeneration == UniqueValueGenerationMode.ConstOnly
                        )
                    )
            )
            {
                if (refValue.Value.TryGetValue(prop, out var code))
                {
                    consts.Add(
                        (prop, Config.GetUniqueValuedName(prop, refValue.Name, internalReference: true), code, label)
                    );
                }
            }
        }

        var orderedConsts = consts
            .OrderBy(x => x.Name.ToPascalCase(strictIfUppercase: true), StringComparer.Ordinal)
            .ToList();

        foreach (var @const in orderedConsts)
        {
            if (orderedConsts.IndexOf(@const) > 0)
            {
                w.WriteLine();
            }

            w.WriteSummary(1, @const.Label);
            w.WriteLine(
                1,
                $"public const {Config.GetType(@const.Prop).TrimEnd('?')} {@const.Name} = {Config.FormatValue(@const.Prop, @const.Code)};"
            );
        }

        if (consts.Any())
        {
            w.WriteLine();
        }
    }

    /// <summary>
    /// Génère le type énuméré présentant les colonnes persistentes.
    /// </summary>
    /// <param name="w">Writer.</param>
    /// <param name="item">La classe générée.</param>
    protected virtual void GenerateEnumCols(CSharpWriter w, Class item)
    {
        w.WriteLine(1, "#region Meta données");
        w.WriteLine();
        w.WriteSummary(1, "Type énuméré présentant les noms des colonnes en base.");

        if (item.Extends == null)
        {
            w.WriteLine(1, "public enum Cols");
        }
        else
        {
            w.WriteLine(1, "public new enum Cols");
        }

        w.WriteLine(1, "{");

        foreach (var property in Config.GetProperties(item))
        {
            w.WriteSummary(2, "Nom de la colonne en base associée à la propriété " + property.NamePascal + ".");
            w.WriteLine(2, $"{property.SqlName},");
            if (property != Config.GetProperties(item).Last())
            {
                w.WriteLine();
            }
        }

        w.WriteLine(1, "}");
        w.WriteLine();
        w.WriteLine(1, "#endregion");
        w.WriteLine();
    }

    /// <summary>
    /// Génère l'enum pour les valeurs statiques de références.
    /// </summary>
    /// <param name="w">Writer.</param>
    /// <param name="item">La classe générée.</param>
    protected virtual void GenerateEnumValues(CSharpWriter w, Class item)
    {
        if (item.Extends?.Enum != null || !Config.UniqueValueGeneration.CanEnum)
        {
            return;
        }

        var hasLine = false;
        var refs = GetRefs(item);

        foreach (var prop in Config.GetProperties(item).Where(p => p.EnumProperty == p))
        {
            hasLine = true;
            w.WriteSummary(1, $"Valeurs possibles de la liste de référence {prop.Class}.");
            WriteEnum(w, prop, refs);
        }

        if (hasLine)
        {
            w.WriteLine();
        }
    }

    /// <summary>
    /// Génère les flags d'une liste de référence statique.
    /// </summary>
    /// <param name="w">Writer.</param>
    /// <param name="item">La classe générée.</param>
    protected virtual void GenerateFlags(CSharpWriter w, Class item)
    {
        if (item.FlagProperty != null && item.Values.Any())
        {
            w.WriteLine(1, "#region Flags");
            w.WriteLine();
            w.WriteSummary(1, "Flags");
            w.WriteLine(1, "public enum Flags");
            w.WriteLine(1, "{");

            var flagValues = item
                .Values.Where(refValue =>
                    refValue.Value.ContainsKey(item.FlagProperty)
                    && int.TryParse(refValue.Value[item.FlagProperty], out var _)
                )
                .ToList();
            foreach (var refValue in flagValues)
            {
                var flag = int.Parse(refValue.Value[item.FlagProperty]);
                w.WriteSummary(2, refValue.GetLabel(item));
                w.WriteLine(2, $"{refValue.Name} = 0b{Convert.ToString(flag, 2)},");
                if (flagValues.IndexOf(refValue) != flagValues.Count - 1)
                {
                    w.WriteLine();
                }
            }

            w.WriteLine(1, "}");
            w.WriteLine();
            w.WriteLine(1, "#endregion");
            w.WriteLine();
        }
    }

    /// <summary>
    /// Génère les propriétés.
    /// </summary>
    /// <param name="w">Writer.</param>
    /// <param name="item">La classe générée.</param>
    /// <param name="tag">Tag.</param>
    protected virtual void GenerateProperties(CSharpWriter w, Class item, string tag)
    {
        var sameColumnSet = new HashSet<string>(
            Config
                .GetProperties(item)
                .Where(p => !p.AssociationMultiple && !p.IsReverseProperty)
                .GroupBy(g => g.SqlName)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
        );

        foreach (
            var property in Config
                .GetProperties(item)
                .Where(p => p is not { Composition: Class cpc } || Config.AvailableClasses.Contains(cpc))
        )
        {
            if (property != Config.GetProperties(item).First())
            {
                w.WriteLine();
            }

            GenerateProperty(w, property, sameColumnSet, tag);
        }
    }

    /// <summary>
    /// Génère la propriété concernée.
    /// </summary>
    /// <param name="w">Writer.</param>
    /// <param name="property">La propriété générée.</param>
    /// <param name="sameColumnSet">Sets des propriétés avec le même nom de colonne, pour ne pas les gérerer (genre alias).</param>
    /// <param name="tag">Tag.</param>
    protected virtual void GenerateProperty(CSharpWriter w, IProperty property, ISet<string> sameColumnSet, string tag)
    {
        w.WriteSummary(1, property.Comment);

        if (property.CustomProperties.TryGetValue("example", out var example))
        {
            w.WriteExample(1, example);
        }

        var type = Config.GetType(
            property,
            nonNullable: property.AssociationMultiple && property.UseClassForAssociation
                || property.Required && (Config.RequiredNonNullable(tag) || property.Composition != null)
        );

        if (property.Class.Type != ClassType.Interface)
        {
            if (
                property.PersistentClass != null
                && (
                    property.PersistentClass == property.Class
                    || !Config.NoColumnOnAlias && property.DomainChain.Count() == 1
                )
                && Config.AvailableClasses.Contains(property.PersistentClass)
                && !Config.NoPersistence(tag)
                && !sameColumnSet.Contains(property.SqlName)
                && !property.AssociationMultiple
                && !property.UseClassForAssociation
                && !Config.GetAnnotations(property, tag).Any(a => a.Annotation.TrimStart('[').StartsWith("Column"))
            )
            {
                w.WriteAttribute(1, "Column", $@"""{Config.GetSqlName(property, tag)}""");
            }

            if (
                property.Required
                    && !Config.RequiredNonNullable(tag)
                    && !property.PrimaryKey
                    && !property.AssociationMultiple
                || property.PrimaryKey && property.Class.PrimaryKey.Count() > 1
            )
            {
                w.WriteAttribute(1, "Required");
            }

            if (
                Config.Kinetix
                && property is { ReferenceClass: Class refClass }
                && (!property.PrimaryKeyish || property.Association != null)
                && Config.AvailableClasses.Contains(refClass)
            )
            {
                w.WriteAttribute(1, "ReferencedType", $"typeof({Config.GetTypeName(refClass)})");
            }

            if (Config.Kinetix && property.Composition == null && !property.UseClassForAssociation)
            {
                w.WriteAttribute(1, "Domain", $@"Domains.{property.Domain.CSharpName}");
            }

            if (type.TrimEnd('?') == "string" && property.Domain.Length != null)
            {
                w.WriteAttribute(1, "StringLength", $"{property.Domain.Length}");
            }

            foreach (var (annotation, _) in Config.GetAnnotations(property, tag))
            {
                w.WriteAttribute(1, annotation);
            }

            if (
                Config.IsPersistent(property.Class, tag)
                && property.AssociationMultiple
                && !property.UseClassForAssociation
            )
            {
                w.WriteAttribute(1, "NotMapped");
            }

            var isPk =
                property.Class.IsPersistent
                && property.PrimaryKey
                && property.Class.PrimaryKey.Count() == 1
                && !property.UseClassForAssociation;

            if (isPk)
            {
                w.WriteAttribute(1, "Key");

                if (
                    Config.DbContextPath != null
                    && (type == "int" || type == "int?" || type == "Guid" || type == "Guid?")
                    && property.GeneratedValue == null
                    && property.Association == null
                )
                {
                    w.WriteAttribute(1, "DatabaseGenerated", "DatabaseGeneratedOption.None");
                }
            }

            var defaultValue = Config.GetDefaultValue(property, tag);

            w.Write(1, "public");

            if (
                Config.RequiredNonNullable(tag)
                && property.Required
                && (!isPk || property.GeneratedValue == null)
                && defaultValue == "null"
            )
            {
                w.Write(" required");
            }

            w.WriteLine(
                $" {type} {property.NamePascal} {{ get; {(property.Readonly ? "init" : "set")}; }}{(defaultValue != "null" ? $" = {defaultValue};" : string.Empty)}"
            );
        }
        else
        {
            w.WriteLine(1, $"{type} {property.NamePascal} {{ get;{(!property.Readonly ? " set;" : string.Empty)} }}");
        }
    }

    protected virtual void GenerateReadonlyEnumClassInstances(CSharpWriter w, Class item)
    {
        if (item.Enum != EnumMode.Class || !item.Readonly)
        {
            return;
        }

        foreach (
            var refValue in item.Values.OrderBy(
                x => x.Name.ToPascalCase(strictIfUppercase: true),
                StringComparer.Ordinal
            )
        )
        {
            w.WriteSummary(1, refValue.GetLabel(item));
            w.Write(
                1,
                $"public static {Config.GetTypeName(item)} {refValue.Name.ToPascalCase(strictIfUppercase: true)} {{ get; }} = new() {{"
            );

            foreach (var refProp in refValue.Value.ToList())
            {
                var value = Config
                    .GetValue(refProp.Key, refProp.Value)
                    .Replace($"{Config.GetTypeName(item)}.", string.Empty);

                if (item.Reference && refProp.Key == item.DefaultProperty && Config.TranslateReferences == true)
                {
                    value = $"\"{refValue.ResourceKey}\"";
                }

                w.Write($" {refProp.Key.NamePascal} = {value}");
                if (refValue.Value.ToList().IndexOf(refProp) < refValue.Value.Count - 1)
                {
                    w.Write(",");
                }
            }

            w.WriteLine(" };");
            w.WriteLine();
        }

        w.WriteSummary(1, "Liste des valeurs");
        w.Write(1, $"public static IList<{Config.GetTypeName(item)}> Values {{ get; }} = ");

        w.Write(Config.DotnetVersion >= 8 ? "[" : "new() { ");

        IEnumerable<ClassValue> values =
            (item.OrderProperty ?? item.DefaultProperty) != null
                ? item.Values.OrderBy(v => v.Value[item.OrderProperty ?? item.DefaultProperty])
                : item.Values;

        w.Write(string.Join(", ", values.Select(r => r.Name.ToPascalCase(strictIfUppercase: true))));
        w.WriteLine(Config.DotnetVersion >= 8 ? "];" : " };");
        w.WriteLine();
    }

    protected virtual void GenerateReadonlyEnumKeyMapper(CSharpWriter w, Class item)
    {
        if (item.Enum != EnumMode.Class || !item.Readonly || item.EnumKey == null)
        {
            return;
        }

        w.WriteLine();

        var key = item.EnumKey;

        w.WriteSummary(1, "Récupère l'instance correspondante à la clé primaire demandée.");
        w.WriteParam(key.NameCamel, key.Comment);
        w.WriteLine(
            1,
            $"public static {Config.GetTypeName(item)} GetValue({Config.GetType(key, nonNullable: true)} {key.NameCamel})"
        );
        w.WriteLine(1, "{");
        w.WriteLine(2, $"return {key.NameCamel} switch");
        w.WriteLine(2, "{");
        foreach (var refValue in item.Values)
        {
            w.WriteLine(
                3,
                $"{Config.GetValue(key, refValue.Value[key]).Replace($"{Config.GetTypeName(item)}.", string.Empty)} => {refValue.Name.ToPascalCase(strictIfUppercase: true)},"
            );
        }
        w.WriteLine(3, "_ => throw new InvalidOperationException()");
        w.WriteLine(2, "};");
        w.WriteLine(1, "}");
    }

    /// <summary>
    /// Génération des imports.
    /// </summary>
    /// <param name="w">Writer.</param>
    /// <param name="item">Classe concernée.</param>
    /// <param name="tag">Tag.</param>
    protected virtual void GenerateUsings(CSharpWriter w, Class item, string tag)
    {
        var usings = new List<string>();

        if (item.Type != ClassType.Interface)
        {
            if (item.Reference && item.DefaultProperty != null)
            {
                usings.Add("System.ComponentModel");
            }

            if (
                Config
                    .GetProperties(item)
                    .Any(p =>
                        p.Required && !Config.RequiredNonNullable(tag)
                        || p.PrimaryKey
                        || Config.GetType(p)?.TrimEnd('?') == "string" && p.Domain.Length != null
                    )
            )
            {
                usings.Add("System.ComponentModel.DataAnnotations");
            }

            if (
                Config
                    .GetProperties(item)
                    .Any(property =>
                        property.PersistentClass != null
                        && (
                            property.PersistentClass == property.Class
                            || !Config.NoColumnOnAlias && property.DomainChain.Count() == 1
                        )
                        && Config.AvailableClasses.Contains(property.PersistentClass)
                        && !Config.NoPersistence(tag)
                    )
                || Config.IsPersistent(item, tag)
                    && (
                        item.InheritanceStrategy != InheritanceStrategy.DistinctTables || item.Type == ClassType.Regular
                    )
                    && item.Extends?.InheritanceStrategy != InheritanceStrategy.SingleTable
            )
            {
                usings.Add("System.ComponentModel.DataAnnotations.Schema");
            }

            if (Config.GetProperties(item).Any(p => p is not { Composition: not null }) && Config.Kinetix)
            {
                usings.Add("Kinetix.Modeling.Annotations");
                usings.Add(Config.DomainNamespace);
            }

            if (item.Extends != null)
            {
                usings.Add(GetNamespace(item.Extends, tag));
            }

            usings.AddRange(item.Implements.Select(implements => GetNamespace(implements, tag)));
        }

        usings.AddRange(Config.GetDecoratorImports(item, tag));
        usings.AddRange(Config.GetAnnotations(item, tag).SelectMany(a => a.Imports));

        foreach (var property in Config.GetProperties(item))
        {
            usings.AddRange(Config.GetDomainImports(property, tag));
            usings.AddRange(Config.GetAnnotations(property, tag).SelectMany(a => a.Imports));
            usings.AddRange(Config.GetValueImports(property));

            switch (property)
            {
                case { Association: Class a, AssociationProperty: IProperty ap }
                    when Config.AvailableClasses.Contains(a)
                        && ap.EnumProperty != null
                        && Config.UniqueValueGeneration.CanEnum:
                    usings.Add(GetNamespace(a, tag));
                    break;
                case { EnumProperty: IProperty ep }
                    when Config.AvailableClasses.Contains(ep.Class) && Config.UniqueValueGeneration.CanEnum:
                    usings.Add(GetNamespace(ep.Class, tag));
                    break;
                case { ReferenceClass: Class refClass }
                    when Config.Kinetix
                        && Config.AvailableClasses.Contains(refClass)
                        && (!property.PrimaryKeyish || property.Association != null):
                    usings.Add(GetNamespace(refClass, tag));
                    break;
                case { Composition: Class cpc } when Config.AvailableClasses.Contains(cpc):
                    usings.Add(GetNamespace(cpc, tag));
                    break;
            }
        }

        w.AddUsings(usings.Where(u => !GetNamespace(item, tag).StartsWith(u)));
    }

    protected override string GetFileName(Class classe, string tag)
    {
        return Config.GetClassFileName(classe, tag);
    }

    protected virtual string GetNamespace(Class classe, string tag)
    {
        return Config.GetNamespace(classe, Config.GetBestClassTag(classe, tag));
    }

    protected virtual IList<ClassValue> GetRefs(Class item)
    {
        return Config.GetAllValues(item).OrderBy(x => x.Name, StringComparer.Ordinal).ToList();
    }

    protected override void HandleClass(string fileName, Class classe, string tag)
    {
        using var w = this.OpenCSharpWriter(fileName);

        if (classe.Enum != EnumMode.Enum)
        {
            GenerateUsings(w, classe, tag);
        }
        w.WriteNamespace(Config.GetNamespace(classe, tag));
        w.WriteSummary(classe.Comment);
        GenerateClassDeclaration(w, classe, tag);
    }

    protected virtual void WriteEnum(CSharpWriter w, IProperty prop, IList<ClassValue> refs, int indent = 1)
    {
        w.WriteLine(indent, $"public enum {Config.GetEnumType(prop, internalReference: true)}");
        w.WriteLine(indent, "{");

        foreach (var refValue in refs)
        {
            w.WriteSummary(indent + 1, refValue.GetLabel(prop.Class));
            w.Write(indent + 1, refValue.Value[prop]);

            if (refs.IndexOf(refValue) != refs.Count - 1)
            {
                w.WriteLine(",");
            }

            w.WriteLine();
        }

        w.WriteLine(indent, "}");
    }
}
