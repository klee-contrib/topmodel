using Microsoft.Extensions.Logging;
using TopModel.Utils;

namespace TopModel.Generator.Sql.Procedural.Oracle;

public class OracleValuesGenerator(ILogger<OracleValuesGenerator> logger, IFileWriterProvider writerProvider)
    : AbstractSqlValuesGenerator(logger, writerProvider)
{
    protected override bool ExplicitSequenceNextVal => true;

    protected override string GetNextValCall(string sequenceName)
    {
        return $"{sequenceName}.nextval";
    }
}
