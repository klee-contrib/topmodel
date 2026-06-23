using Microsoft.Extensions.Logging;
using TopModel.Utils;

namespace TopModel.Generator.Sql.Procedural.Postgres;

public class PostgresTablesGenerator(ILogger<PostgresTablesGenerator> logger, IFileWriterProvider writerProvider)
    : AbstractSqlTablesGenerator(logger, writerProvider)
{
    protected override string JsonType => "jsonb";

    protected override bool SupportsClusteredKey => false;
}
