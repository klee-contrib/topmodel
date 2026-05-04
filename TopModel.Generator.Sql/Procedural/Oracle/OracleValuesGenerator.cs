using Microsoft.Extensions.Logging;
using TopModel.Utils;

namespace TopModel.Generator.Sql.Procedural.Oracle;

public class OracleValuesGenerator(ILogger<OracleValuesGenerator> logger, IFileWriterProvider writerProvider)
    : AbstractSqlValuesGenerator(logger, writerProvider) { }
