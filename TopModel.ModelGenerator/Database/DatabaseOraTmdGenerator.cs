using System.Data.Common;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;
using TopModel.Utils;

namespace TopModel.ModelGenerator.Database;

public class DatabaseOraTmdGenerator(
    ILogger<DatabaseTmdGenerator> logger,
    DatabaseConfig config,
    IFileWriterProvider writerProvider
) : DatabaseTmdGenerator(logger, config, writerProvider)
{
    private readonly DatabaseConfig _config = config;

    public override string Name => "DatabaseOraGen";

    protected override string GetColumnCommentsQuery()
    {
        return @$"
                SELECT
                    table_name  AS TableName,
                    column_name AS ColumnName,
                    comments    AS Comment
                FROM all_col_comments
                WHERE owner = '{_config.Source.Schema}'
                    AND comments IS NOT NULL
            ";
    }

    protected override string GetColumnsQuery()
    {
        return @$"
            select                 
                table_name                                  AS TableName,
                column_name                                 AS ColumnName,
                data_type                                   AS DataType,
                case when nullable = 'Y' then 1 else 0 end  AS Nullable,   
                data_precision                              AS ""Precision"",
                coalesce(char_length, data_scale)           AS Scale
            from all_tab_columns
            where owner = '{_config.Source.Schema}'
            order by table_name, column_id
            ";
    }

    protected override DbConnection GetConnection()
    {
        return new OracleConnection(_config.ConnectionString);
    }

    protected override string GetForeignKeysQuery()
    {
        return @$"
                Select 
                    uc.constraint_name      AS ""Name"",
                    uc.table_name           AS TableName,
                    cols.column_name        AS ColumnName,
                    uc_pk.table_name        AS ForeignTableName,
                    cols_pk.column_name     AS ForeignColumnName
                From 
                    user_constraints uc
                    JOIN user_cons_columns cols ON uc.constraint_name = cols.constraint_name
                    JOIN user_constraints uc_pk ON uc.r_constraint_name = uc_pk.constraint_name
                    JOIN user_cons_columns cols_pk ON uc_pk.constraint_name = cols_pk.constraint_name
                Where 
                    uc.owner = '{_config.Source.Schema}'
                    and uc.constraint_type = 'R'
                Order by
                    uc.table_name,
                    cols.position
            ";
    }

    protected override string GetIndexesQuery()
    {
        return $@"
                SELECT
                    ai.index_name       AS ""Name"",
                    ai.table_name       AS TableName,
                    aic.column_name     AS ColumnName
                FROM all_indexes ai
                JOIN all_ind_columns aic
                    ON  aic.index_owner = ai.owner
                    AND aic.index_name  = ai.index_name
                    AND aic.table_name  = ai.table_name
                WHERE ai.owner          = '{_config.Source.Schema}'
                  AND ai.uniqueness     = 'NONUNIQUE'
                  AND NOT EXISTS (
                    SELECT 1
                    FROM all_constraints ac
                    WHERE ac.owner      = ai.owner
                      AND ac.index_name = ai.index_name
                  )
                ORDER BY ai.table_name, ai.index_name, aic.column_position
            ";
    }

    protected override string GetPrimaryKeysQuery()
    {
        return GetConstraintKeyQuery("P");
    }

    protected override string GetTableCommentsQuery()
    {
        return @$"
                SELECT
                    table_name  AS TableName,
                    comments    AS Comment
                FROM all_tab_comments
                WHERE owner = '{_config.Source.Schema}'
                    AND table_type = 'TABLE'
                    AND comments IS NOT NULL
            ";
    }

    protected override string GetUniqueKeysQuery()
    {
        return GetConstraintKeyQuery("U");
    }

    private string GetConstraintKeyQuery(string name)
    {
        return @$"
                select 
                    uc.constraint_name AS ""Name"",
                    uc.table_name      AS TableName,
                    cols.column_name   AS ColumnName
                from 
                    user_constraints uc
                    join user_cons_columns cols ON uc.constraint_name = cols.constraint_name
                where
                    uc.owner = '{_config.Source.Schema}'
                    and uc.constraint_type = '{name}'
                order by
                    uc.table_name, 
                    cols.position
            ";
    }
}
