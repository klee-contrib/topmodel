using TopModel.Core;
using TopModel.Generator.Core;
using TopModel.Generator.Sql.Procedural;
using TopModel.Generator.Sql.Ssdt;

namespace TopModel.Generator.Sql;

public class SqlConfig : GeneratorConfigBase
{
    /// <summary>
    /// Config pour la génération en mode procédural.
    /// </summary>
    public ProceduralSqlConfig? Procedural { get; set; }

    /// <summary>
    /// Config pour la génération en mode SSDT.
    /// </summary>
    public SsdtConfig? Ssdt { get; set; }

    /// <summary>
    /// Désactive la génération des valeurs par défaut des propriétés dans les classes et endpoints générés avec cette configuration.
    /// </summary>
    public override bool IgnoreDefaultValues { get; set; } = true;

    /// <summary>
    /// SGBD cible ("sqlserver" ou "postgres").
    /// </summary>
    public TargetDBMS TargetDBMS { get; set; } = TargetDBMS.Postgre;

    protected override bool UseNamedEnums => false;

    public override bool CanClassUseEnums(Class classe, IEnumerable<Class>? availableClasses = null, IFieldProperty? prop = null)
    {
        return false;
    }

    public override bool ShouldQuoteValue(IFieldProperty prop)
    {
        var type = GetType(prop);
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
        throw new NotImplementedException();
    }

    protected override string QuoteValue(string value)
    {
        return $@"{(TargetDBMS == TargetDBMS.Sqlserver ? "N" : string.Empty)}'{value.Replace("'", "''")}'";
    }
}
