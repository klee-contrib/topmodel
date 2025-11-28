using TopModel.Core;
using TopModel.Core.FileModel;
using TopModel.Core.Model;
using TopModel.Generator.Core;
using TopModel.Generator.Sql.Procedural;
using TopModel.Generator.Sql.Ssdt;
using TopModel.Utils;

namespace TopModel.Generator.Sql;

public class SqlConfig : GeneratorConfigBase
{
    /// <summary>
    /// Config pour la génération en mode procédural.
    /// </summary>
    public virtual ProceduralSqlConfig? Procedural { get; set; }

    /// <summary>
    /// Config pour la génération en mode SSDT.
    /// </summary>
    public virtual SsdtConfig? Ssdt { get; set; }

    public override string? DefaultLanguage => "sql";

    public override Dictionary<string, List<string>> TemplateAttributes =>
        new()
        {
            { nameof(ForeignKeyConstraintNamePattern), ["tableName", "trigram", "columnName"] },
            { nameof(UniqueConstraintNamePattern), ["tableName", "columnNames", "propertyNames"] },
        };

    /// <summary>
    /// Désactive la génération des valeurs par défaut des propriétés dans les classes et endpoints générés avec cette configuration.
    /// </summary>
    public override bool IgnoreDefaultValues { get; set; } = true;

    /// <summary>
    /// Retourne ou définit le nom du tablespace pour les tables (Postgres ou Oracle).
    /// </summary>
    public virtual string? TableTablespace { get; set; }

    /// <summary>
    /// Retourne ou définit le nom du tablespace pour les index (Postgres ou Oracle).
    /// </summary>
    public virtual string? IndexTablespace { get; set; }

    /// <summary>
    /// Retourne ou définit le pattern pour le nom des contraintes de clé étrangère.
    /// Supporte les variables tableName, trigram, columnName.
    /// Valeur par défaut : "FK_{tableName}_{columnName}".
    /// </summary>
    public virtual string ForeignKeyConstraintNamePattern { get; set; } = "FK_{tableName}_{columnName}";

    /// <summary>
    /// Retourne ou définit le pattern pour le nom des contraintes d'unicité.
    /// Supporte les variables tableName, columnNames (avec trigramme), propertyNames (sans le trigramme).
    /// Valeur par défaut : "UK_{tableName}_{columnNames}".
    /// </summary>
    public virtual string UniqueConstraintNamePattern { get; set; } = "UK_{tableName}_{columnNames}";

    /// <summary>
    /// SGBD cible ("sqlserver" ou "postgres" ou "oracle").
    /// </summary>
    public virtual TargetDBMS TargetDBMS { get; set; } = TargetDBMS.Postgre;

    /// <summary>
    /// Indique si le SGBD gère les tablespaces.
    /// </summary>
    public virtual bool AllowTablespace => TargetDBMS != TargetDBMS.Sqlserver;

    public virtual string BatchSeparator =>
        TargetDBMS switch
        {
            TargetDBMS.Oracle => $"{Environment.NewLine}/",
            TargetDBMS.Sqlserver => $"{Environment.NewLine}go",
            _ => ";",
        };

    /// <summary>
    /// Indique la limite de longueur d'un identifiant.
    /// </summary>
    public virtual int IdentifierLengthLimit => 128;

    protected override bool PersistentOnly => true;

    protected override bool UseNamedEnums => false;

    public static bool IsBoolean(IProperty property)
    {
        var domain = property.Domain;
        /* Pour savoir si un domaine est booléen, on regarde grossièrement le mot bool dans le nom du domaine, le label du domaine, un type d'implémeentation de domaine. */
        return domain.Name.Value.Contains("bool", StringComparison.InvariantCultureIgnoreCase)
            || domain.Label.Contains("bool", StringComparison.InvariantCultureIgnoreCase)
            || domain.Implementations.Values.Any(di =>
                di.Type?.Contains("bool", StringComparison.InvariantCultureIgnoreCase) ?? false
            );
    }

    public override bool CanClassUseEnums(Class classe, IProperty? prop = null)
    {
        return false;
    }

    /// <summary>
    /// Lève une ArgumentException si l'identifiant est trop long.
    /// </summary>
    /// <param name="identifier">Identifiant à vérifier.</param>
    /// <returns>Identifiant passé en paramètre.</returns>
    public string CheckIdentifierLength(string identifier)
    {
        return identifier.Length > IdentifierLengthLimit
            ? throw new ModelException(
                $"Le nom {identifier} est trop long ({identifier.Length} caractères). Limite: {IdentifierLengthLimit} caractères."
            )
            : identifier;
    }

    public override IEnumerable<Class> GetExtraClasses(ModelFile file)
    {
        var manyToManyProperties = file
            .Classes.Where(c => c.IsPersistent && !c.Abstract)
            .SelectMany(cl => cl.Properties)
            .OfType<AssociationProperty>()
            .Where(ap => ap.Type == AssociationType.ManyToMany);

        foreach (var ap in manyToManyProperties)
        {
            var traClass = new Class
            {
                Comment = ap.Comment,
                Label = ap.Label,
                SqlName =
                    $"{ap.Class.SqlName}_{ap.Association.SqlName}{(ap.Role != null ? $"_{ap.Role.ToConstantCase()}" : string.Empty)}",
                ModelFile = file,
            };

            traClass.Properties.Add(
                new AssociationProperty
                {
                    Association = ap.Class,
                    Class = traClass,
                    Comment = ap.Comment,
                    Type = AssociationType.ManyToOne,
                    PrimaryKey = true,
                    Required = true,
                    Role = ap.Role,
                    DefaultValue = ap.DefaultValue,
                    Label = ap.Label,
                    Trigram = ap.Class.PrimaryKey.Single().Trigram,
                }
            );

            traClass.Properties.Add(
                new AssociationProperty
                {
                    Association = ap.Association,
                    Class = traClass,
                    Comment = ap.Comment,
                    Type = AssociationType.ManyToOne,
                    PrimaryKey = true,
                    Required = true,
                    Role = ap.Role,
                    DefaultValue = ap.DefaultValue,
                    Label = ap.Label,
                    Trigram = ap.Trigram ?? ap.Property.Trigram ?? ap.Association.Trigram,
                }
            );

            yield return traClass;
        }
    }

    public virtual string GetForeignKeyConstraintName(string tableName, string? trigram, string columnName)
    {
        return ReplaceCustomVariables(
            ForeignKeyConstraintNamePattern,
            new Dictionary<string, string?>
            {
                [nameof(tableName)] = tableName,
                [nameof(trigram)] = trigram,
                [nameof(columnName)] = columnName,
            }
        );
    }

    /// <summary>
    /// Calcule le nom de la séquence associée à une table.
    /// </summary>
    /// <param name="classe">Classe.</param>
    /// <returns>Nom de la séquence.</returns>
    public virtual string GetSequenceName(Class classe)
    {
        return TargetDBMS switch
        {
            TargetDBMS.Oracle => $"{classe.Trigram}_SEQ",
            TargetDBMS.Postgre => $"SEQ_{classe.SqlName}",
            var t => throw new NotSupportedException($"Sequence declaration is not implemented with {t}"),
        };
    }

    public virtual string GetUniqueConstraintName(string tableName, string columnNames, string propertyNames)
    {
        return ReplaceCustomVariables(
            UniqueConstraintNamePattern,
            new Dictionary<string, string?>
            {
                [nameof(tableName)] = tableName,
                [nameof(columnNames)] = columnNames,
                [nameof(propertyNames)] = propertyNames,
            }
        );
    }

    public override string GetValue(IProperty property, string? value = null)
    {
        /* Cas spécifique d'un booléen sous Oracle, typé comme un numeric(1) */
        bool NeedsBooleanConversionToNumeric()
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            return TargetDBMS == TargetDBMS.Oracle && GetType(property) == "number(1)" && IsBoolean(property);
        }

        if (NeedsBooleanConversionToNumeric())
        {
            return bool.Parse(value!) ? "1" : "0";
        }

        return base.GetValue(property, value);
    }

    public override bool ShouldQuoteValue(IProperty property)
    {
        var type = GetType(property);
        return (type ?? string.Empty).Contains("varchar")
            || type == "text"
            || type == "uniqueidentifier"
            || type == "uuid"
            || type == "bit"
            || (type ?? string.Empty).Contains("date")
            || (type ?? string.Empty).Contains("time");
    }

    protected override string GetEnumType(string className, string propName, bool isPrimaryKeyDef = false)
    {
        throw new NotSupportedException();
    }

    protected override string QuoteValue(string value)
    {
        return $@"{(TargetDBMS == TargetDBMS.Sqlserver ? "N" : string.Empty)}'{value.Replace("'", "''")}'";
    }

    /// <summary>
    /// Remplace des variables dans une chaîne.
    /// </summary>
    /// <param name="value">Chaîne templatisé sous la forme de {paramName}.</param>
    /// <param name="variables">Association entre paramName et paramValue.</param>
    /// <returns>Résultat.</returns>
    private static string ReplaceCustomVariables(string value, Dictionary<string, string?> variables)
    {
        var buffer = value;
        foreach (var paramName in variables.Keys)
        {
            buffer = buffer.Replace($"{{{paramName}}}", variables[paramName]);
        }

        return buffer;
    }
}
