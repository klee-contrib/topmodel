using Microsoft.Extensions.Logging;
using TopModel.Utils;

namespace TopModel.Generator.Sql.Procedural.Postgres;

public class PostgresValuesGenerator(ILogger<PostgresValuesGenerator> logger, IFileWriterProvider writerProvider)
    : AbstractSqlValuesGenerator(logger, writerProvider) { }
