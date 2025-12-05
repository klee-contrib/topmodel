using System.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using TopModel.Utils;

namespace TopModel.ModelGenerator.Database;

public class DatabaseMsSqlTmdGenerator(
    ILogger<DatabaseTmdGenerator> logger,
    DatabaseConfig config,
    IFileWriterProvider writerProvider
) : DatabaseTmdGenerator(logger, config, writerProvider)
{
    private readonly DatabaseConfig _config = config;

    public override string Name => "DatabaseMsSqlGen";

    protected override string GetColumnsQuery()
    {
        return @$"
                SELECT
                    t.name                                  AS TableName,
                    c.name                                  AS ColumnName,
                    ty.name                                 AS DataType,
                    isnull(c.is_nullable , 0)				AS Nullable,
                    CASE 
                        WHEN ty.name IN ('decimal', 'numeric') THEN c.[precision]
                        WHEN ty.name IN ('time', 'datetime2', 'datetimeoffset') THEN c.scale
                        ELSE NULL
                    END                                      AS [Precision],
                    CASE
                        WHEN ty.name IN ('char','varchar')                    THEN c.max_length
                        WHEN ty.name IN ('nchar','nvarchar')                  THEN (CASE WHEN c.max_length = -1 THEN -1 ELSE c.max_length / 2 END)
                        WHEN ty.name IN ('binary','varbinary')                THEN c.max_length
                        WHEN ty.name IN ('decimal','numeric')                 THEN c.scale
                        ELSE NULL
                    END                                      AS [Scale]
                FROM sys.columns AS c
                JOIN sys.tables  AS t  ON t.object_id = c.object_id
                JOIN sys.schemas AS s  ON s.schema_id = t.schema_id
                JOIN sys.types   AS ty ON ty.user_type_id = c.user_type_id
                WHERE s.name = '{_config.Source.Schema}'
                ORDER BY t.name, c.column_id;
            ";
    }

    protected override DbConnection GetConnection()
    {
        return new SqlConnection(_config.ConnectionString);
    }

    protected override string GetForeignKeysQuery()
    {
        return $@"
               SELECT
                    t_parent.[name]                     AS TableName,
                    c_parent.[name]                     AS ColumnName,
                    t_ref.[name]                        AS ForeignTableName,
                    c_ref.[name]                        AS ForeignColumnName
                FROM sys.foreign_keys AS fk
                JOIN sys.foreign_key_columns AS fkc
                    ON fk.object_id = fkc.constraint_object_id
                JOIN sys.tables AS t_parent
                    ON t_parent.object_id = fk.parent_object_id
                JOIN sys.schemas AS sch_parent
                    ON sch_parent.schema_id = t_parent.schema_id
                JOIN sys.columns AS c_parent
                    ON c_parent.object_id = t_parent.object_id
                    AND c_parent.column_id = fkc.parent_column_id
                JOIN sys.tables AS t_ref
                    ON t_ref.object_id = fk.referenced_object_id
                JOIN sys.schemas AS sch_ref
                    ON sch_ref.schema_id = t_ref.schema_id
                JOIN sys.columns AS c_ref
                    ON c_ref.object_id = t_ref.object_id
                    AND c_ref.column_id = fkc.referenced_column_id
                WHERE sch_parent.[name] = '{_config.Source.Schema}'
                    AND sch_ref.[name]    = '{_config.Source.Schema}'
                ORDER BY t_parent.[name], fk.[name], fkc.constraint_column_id;
            ";
    }

    protected override string GetPrimaryKeysQuery()
    {
        return GetConstraintKeyQuery("PK");
    }

    protected override string GetUniqueKeysQuery()
    {
        return GetConstraintKeyQuery("UQ");
    }

    private string GetConstraintKeyQuery(string typeName)
    {
        return $@"
                SELECT
                    kc.name                      AS [Name],
                    t.name                       AS TableName,
                    c.name                       AS ColumnName
                FROM sys.key_constraints AS kc
                JOIN sys.tables          AS t   ON t.object_id        = kc.parent_object_id
                JOIN sys.schemas         AS sch ON sch.schema_id      = t.schema_id
                JOIN sys.indexes         AS i   ON i.object_id        = kc.parent_object_id
                                               AND i.index_id         = kc.unique_index_id
                JOIN sys.index_columns   AS ic  ON ic.object_id       = i.object_id
                                               AND ic.index_id        = i.index_id
                JOIN sys.columns         AS c   ON c.object_id        = ic.object_id
                                               AND c.column_id        = ic.column_id
                WHERE kc.type = '{typeName}'
                  AND sch.name = '{_config.Source.Schema}'
                ORDER BY t.name, kc.name, ic.key_ordinal;
            ";
    }
}
