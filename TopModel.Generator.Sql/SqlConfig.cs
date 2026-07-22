using TopModel.Core.Model;
using TopModel.Generator.Core;
using TopModel.Generator.Sql.Procedural;
using TopModel.Generator.Sql.Ssdt;

namespace TopModel.Generator.Sql;

public class SqlConfig : GeneratorConfigBase
{
    private static readonly HashSet<string> ReservedSqlKeywords = new(StringComparer.OrdinalIgnoreCase)
    {
        "add",
        "all",
        "alter",
        "and",
        "any",
        "as",
        "asc",
        "authorization",
        "between",
        "by",
        "case",
        "check",
        "column",
        "constraint",
        "create",
        "cross",
        "current_date",
        "current_time",
        "current_timestamp",
        "delete",
        "desc",
        "distinct",
        "drop",
        "else",
        "end",
        "except",
        "exists",
        "false",
        "for",
        "foreign",
        "from",
        "full",
        "grant",
        "group",
        "having",
        "in",
        "inner",
        "insert",
        "intersect",
        "into",
        "is",
        "join",
        "key",
        "left",
        "like",
        "not",
        "null",
        "on",
        "or",
        "order",
        "outer",
        "primary",
        "references",
        "right",
        "select",
        "set",
        "table",
        "then",
        "true",
        "union",
        "unique",
        "update",
        "user",
        "using",
        "values",
        "when",
        "where",
        "with",
    };

    /// <summary>
    /// Config pour la génération en mode procédural.
    /// </summary>
    public virtual ProceduralSqlConfig? Procedural { get; set; }

    /// <summary>
    /// Config pour la génération en mode SSDT.
    /// </summary>
    public virtual SsdtConfig? Ssdt { get; set; }

    public override string? DefaultLanguage => "sql";

    /// <summary>
    /// Désactive la génération des valeurs par défaut des propriétés dans les classes et endpoints générés avec cette configuration.
    /// </summary>
    public override bool IgnoreDefaultValues { get; set; } = true;

    /// <summary>
    /// Si le langage cible de la configuration supporte les enums.
    /// </summary
    /// >
    public override bool HasEnumSupport => true;

    /// <summary>
    /// Retourne ou définit le nom du tablespace pour les tables (Postgres ou Oracle).
    /// </summary>
    public virtual string? TableTablespace { get; set; }

    /// <summary>
    /// Retourne ou définit le nom du tablespace pour les index (Postgres ou Oracle).
    /// </summary>
    public virtual string? IndexTablespace { get; set; }

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
    public override UniqueValueGenerationMode UniqueValueGeneration => UniqueValueGenerationMode.None;

    protected override bool UseValueNameForValues => false;

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

    public override bool FilterClass(Class classe)
    {
        return classe.IsPersistent && (TargetDBMS == TargetDBMS.Postgre || classe.Enum != EnumMode.Enum);
    }

    public override bool FilterEndpoint(Endpoint endpoint)
    {
        return false;
    }

    public override string GetEnumType(IProperty prop, bool internalReference = false)
    {
        if (prop.UniqueValuedProperty == null)
        {
            return string.Empty;
        }

        return prop.UniqueValuedProperty?.Class != null
            ? GetSqlName(prop.UniqueValuedProperty!.Class, string.Empty)
            : string.Empty;
    }

    /// <summary>
    /// Renvoie le SQL pour appeler la valeur suivante de la séquence associée à une propriété.
    /// </summary>
    /// <param name="property">Propriété.</param>
    /// <param name="tag">Tag.</param>
    /// <returns>SQL.</returns>
    public virtual string GetNextValCall(IProperty property, string tag)
    {
        return TargetDBMS switch
        {
            TargetDBMS.Oracle => $"{GetSequenceName(property, tag)}.nextval",
            TargetDBMS.Postgre => $"nextval('{GetSequenceName(property, tag)}')",
            TargetDBMS.Sqlserver => $"next value for {GetSequenceName(property, tag)}",
            _ => throw new InvalidOperationException(),
        };
    }

    public override IEnumerable<IProperty> GetProperties(Class? classe)
    {
        if (classe?.Extends != null && classe.Extends.InheritanceStrategy == InheritanceStrategy.DistinctTables)
        {
            foreach (var prop in GetProperties(classe.Extends))
            {
                yield return prop;
            }
        }

        // On enlève les multiples (directes ou reverse) + les reverses de oneToOne.
        foreach (
            var prop in base.GetProperties(classe)
                .Where(p => !p.AssociationMultiple && (!p.IsReverseProperty || p.ReverseProperty!.AssociationMultiple))
        )
        {
            yield return prop;
        }

        if (classe?.ParentAssociationProperty != null)
        {
            yield return classe.ParentAssociationProperty!;
        }

        if (classe?.DiscriminatorProperty != null && !classe.Properties.Contains(classe.DiscriminatorProperty))
        {
            yield return classe.DiscriminatorProperty;
        }

        if (classe?.InheritanceStrategy == InheritanceStrategy.SingleTable)
        {
            foreach (var prop in Classes.Where(c => c.Extends == classe).SelectMany(GetProperties))
            {
                yield return prop;
            }
        }
    }

    public virtual string GetSqlPrimaryKeyName(Class classe, string tag, bool noQuote = false)
    {
        var pkName = $"PK_{classe.SqlName}";
        return FixSqlIdentifier(UseLowerCaseSqlNames(tag) ? pkName.ToLower() : pkName, noQuote);
    }

    public virtual string GetSqlTableTypeName(Class classe, string tag, bool noQuote = false)
    {
        var typeName = classe.SqlName + "_TABLE_TYPE";
        return FixSqlIdentifier(UseLowerCaseSqlNames(tag) ? typeName.ToLower() : typeName, noQuote);
    }

    public string GetType(IProperty property)
    {
        var type = GetType(property, forceAssociationPropertyType: true);

        if (type.ToLower().Equals("varchar") && property.Domain?.Length != null)
        {
            type = $"{type}({property.Domain.Length})";
        }

        if ((type.ToLower().Equals("numeric") || type.ToLower().Equals("decimal")) && property.Domain?.Length != null)
        {
            type =
                $"{type}({property.Domain.Length}{(property.Domain.Scale != null ? $", {property.Domain.Scale}" : string.Empty)})";
        }

        return type;
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
        var type = GetImplementation(property.Domain)?.Type?.ToLower();
        return property.Domain == null
            || (type ?? string.Empty).Contains("varchar")
            || type == "text"
            || type == "uniqueidentifier"
            || type == "uuid"
            || type == "bit"
            || (type ?? string.Empty).Contains("date")
            || (type ?? string.Empty).Contains("time");
    }

    protected override string FixSqlIdentifier(string identifier, bool noQuote = false)
    {
        identifier = base.FixSqlIdentifier(identifier, noQuote);

        var quoteS = TargetDBMS == TargetDBMS.Sqlserver ? "[" : "\"";
        var quoteE = TargetDBMS == TargetDBMS.Sqlserver ? "]" : "\"";

        if (
            noQuote
            || !ReservedSqlKeywords.Contains(identifier)
                && identifier.All(i => char.IsLetterOrDigit(i) || i == '_')
                && (
                    TargetDBMS == TargetDBMS.Postgre && identifier.ToLower() == identifier
                    || TargetDBMS == TargetDBMS.Oracle && identifier.ToUpper() == identifier
                    || TargetDBMS == TargetDBMS.Sqlserver
                )
        )
        {
            return identifier;
        }

        return $"{quoteS}{identifier}{quoteE}";
    }

    protected override string QuoteValue(string value)
    {
        return $@"{(TargetDBMS == TargetDBMS.Sqlserver ? "N" : string.Empty)}'{value.Replace("'", "''")}'";
    }
}
