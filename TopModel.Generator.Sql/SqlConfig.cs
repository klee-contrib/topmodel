using TopModel.Core;
using TopModel.Core.Model;
using TopModel.Generator.Core;
using TopModel.Generator.Sql.Procedural;
using TopModel.Generator.Sql.Ssdt;

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

    /// <summary>
    /// Désactive la génération des valeurs par défaut des propriétés dans les classes et endpoints générés avec cette configuration.
    /// </summary>
    public override bool IgnoreDefaultValues { get; set; } = true;

    /// <summary>
    /// Si le langage cible de la configuration supporte les enums.
    /// </summary>
    public override bool HasEnumSupport => false;

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

    /// <summary>
    /// Indique la limite de longueur d'un identifiant.
    /// </summary>
    public virtual int IdentifierLengthLimit => 128;

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

    public override bool FilterClass(Class classe)
    {
        return classe.IsPersistent && classe.Enum != EnumMode.Enum;
    }

    public override bool FilterEndpoint(Endpoint endpoint)
    {
        return false;
    }

    /// <summary>
    /// Renvoie le SQL pour appeler la valeur suivante de la séquence associée à une classe.
    /// </summary>
    /// <param name="classe">Classe.</param>
    /// <returns>SQL.</returns>
    public virtual string GetNextValCall(Class classe)
    {
        return TargetDBMS switch
        {
            TargetDBMS.Oracle => $"{GetSequenceName(classe)}.nextval",
            TargetDBMS.Postgre => $"nextval('{GetSequenceName(classe)}')",
            TargetDBMS.Sqlserver => $"next value for {GetSequenceName(classe)}",
            _ => throw new InvalidOperationException(),
        };
    }

    /// <summary>
    /// Calcule le nom de la séquence associée à une table.
    /// </summary>
    /// <param name="classe">Classe.</param>
    /// <returns>Nom de la séquence.</returns>
    public virtual string GetSequenceName(Class classe)
    {
        return $"SEQ_{classe.SqlName}";
    }

    public string GetType(IProperty property)
    {
        return GetType(property, forceAssociationPropertyType: true);
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

    /// <summary>
    /// Indique si une classe persistée doit utiliser une séquence pour générer sa clé primaire.
    /// </summary>
    /// <param name="classe">Classe.</param>
    /// <returns><c>true</c> si la PK est unique, non-association, auto-générée, non-varchar et que le mode d'identité est <see cref="IdentityMode.SEQUENCE"/>.</returns>
    public virtual bool UsesSequence(Class classe)
    {
        return Procedural?.Identity.Mode == IdentityMode.SEQUENCE
                && classe.PrimaryKey.Count() == 1
                && classe.PrimaryKey.Single().Association == null
                && classe.PrimaryKey.Single().AutoGeneratedValue
                && !GetType(classe.PrimaryKey.Single()).Contains("varchar", StringComparison.CurrentCultureIgnoreCase)
            || classe.InheritanceStrategy == InheritanceStrategy.DistinctTables;
    }

    protected override string QuoteValue(string value)
    {
        return $@"{(TargetDBMS == TargetDBMS.Sqlserver ? "N" : string.Empty)}'{value.Replace("'", "''")}'";
    }
}
