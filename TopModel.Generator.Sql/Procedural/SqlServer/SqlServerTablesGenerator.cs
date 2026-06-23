using Microsoft.Extensions.Logging;
using TopModel.Utils;

namespace TopModel.Generator.Sql.Procedural.SqlServer;

public class SqlServerTablesGenerator(ILogger<SqlServerTablesGenerator> logger, IFileWriterProvider writerProvider)
    : AbstractSqlTablesGenerator(logger, writerProvider)
{
    protected override bool SupportsClusteredKey => true;
}
