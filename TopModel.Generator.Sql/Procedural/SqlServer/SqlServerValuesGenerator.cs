using Microsoft.Extensions.Logging;
using TopModel.Utils;

namespace TopModel.Generator.Sql.Procedural.SqlServer;

public class SqlServerValuesGenerator(ILogger<SqlServerValuesGenerator> logger, IFileWriterProvider writerProvider)
    : AbstractSqlValuesGenerator(logger, writerProvider)
{
    protected override void WriteInsertEnd(IFileWriter writerInsert)
    {
        writerInsert.WriteLine("set nocount off;");
        writerInsert.WriteLine();
    }

    protected override void WriteInsertStart(IFileWriter writerInsert)
    {
        writerInsert.WriteLine("set nocount on;");
        writerInsert.WriteLine();
    }
}
