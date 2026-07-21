using Microsoft.Extensions.Logging;
using TopModel.Core.Model;
using TopModel.Utils;

namespace TopModel.Generator.Sql.Procedural.Oracle;

public class OracleTablesGenerator(ILogger<OracleTablesGenerator> logger, IFileWriterProvider writerProvider)
    : AbstractSqlTablesGenerator(logger, writerProvider)
{
    protected override bool SupportsClusteredKey => false;

    protected override void WriteBooleanCheckConstraints(IFileWriter writer, IEnumerable<IProperty> properties)
    {
        /* En Oracle, en 2024, il n'y a pas de type booléen. On utilise un numeric(1) et on rajoute une check constraint pour forcer les valeurs 0 et 1. */
        bool IsNumericBoolean(IProperty property)
        {
            var sqlType = Config.GetType(property);
            return sqlType == "number(1)" && SqlConfig.IsBoolean(property);
        }

        foreach (var property in properties)
        {
            if (IsNumericBoolean(property))
            {
                writer.WriteLine(
                    $"\tconstraint CHK_{property.SqlName} check ({Config.GetSqlName(property)} in (0,1)),"
                );
            }
        }
    }
}
