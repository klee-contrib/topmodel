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
    /// SGBD cible ("sqlserver" ou "postgres").
    /// </summary>
    public TargetDBMS TargetDBMS { get; set; } = TargetDBMS.Postgre;

    public override bool CanClassUseEnums(Class classe, IEnumerable<Class>? availableClasses = null, IFieldProperty? prop = null)
    {
        return false;
    }

    protected override string GetEnumType(string className, string propName, bool asList = false, bool isPrimaryKeyDef = false)
    {
        throw new NotImplementedException();
    }

    protected override string GetListType(string name, bool useIterable = true)
    {
        throw new NotImplementedException();
    }
}
