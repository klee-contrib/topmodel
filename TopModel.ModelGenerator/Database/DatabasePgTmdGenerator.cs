using System.Data.Common;
using Microsoft.Extensions.Logging;
using Npgsql;
using TopModel.Utils;

namespace TopModel.ModelGenerator.Database;

public class DatabasePgTmdGenerator(
    ILogger<DatabaseTmdGenerator> logger,
    DatabaseConfig config,
    IFileWriterProvider writerProvider
) : DatabaseTmdGenerator(logger, config, writerProvider)
{
    private readonly DatabaseConfig _config = config;

    public override string Name => "DatabasePgGen";

    protected override string GetColumnCommentsQuery()
    {
        return @$"
                SELECT
                    c.table_name    AS TableName,
                    c.column_name   AS ColumnName,
                    pgd.description AS Comment
                FROM information_schema.columns c
                JOIN pg_class pgc
                    ON pgc.relname = c.table_name
                JOIN pg_namespace pgn
                    ON pgn.oid = pgc.relnamespace
                    AND pgn.nspname = c.table_schema
                LEFT JOIN pg_description pgd
                    ON pgd.objoid = pgc.oid
                    AND pgd.objsubid = c.ordinal_position
                WHERE c.table_schema = '{_config.Source.Schema}'
                    AND pgd.description IS NOT NULL
            ";
    }

    protected override string GetColumnsQuery()
    {
        // Récupération des colonnes
        return @$"
                select  table_name                                                              as TableName,
                        column_name                                                             as ColumnName,
                        data_type                                                               as DataType,
                        is_nullable = 'YES'                                                     as Nullable,
                        coalesce(numeric_precision, datetime_precision, interval_precision)     as Precision,
                        coalesce(character_maximum_length, numeric_scale, interval_precision)   as Scale
                from information_schema.columns
                where table_schema  = '{_config.Source.Schema}'
                order by ordinal_position
            ";
    }

    protected override DbConnection GetConnection()
    {
        return new NpgsqlConnection(_config.ConnectionString);
    }

    protected override string GetForeignKeysQuery()
    {
        // Récupération des contraintes de clés étrangères
        return GetConstraintKeyQuery("FOREIGN KEY");
    }

    protected override string GetIndexesQuery()
    {
        return $@"
                SELECT
                    i.relname                                   AS Name,
                    t.relname                                   AS TableName,
                    a.attname                                   AS ColumnName
                FROM pg_index ix
                JOIN pg_class t      ON t.oid = ix.indrelid
                JOIN pg_class i      ON i.oid = ix.indexrelid
                JOIN pg_namespace n  ON n.oid = t.relnamespace
                CROSS JOIN LATERAL unnest(ix.indkey) WITH ORDINALITY AS u(attnum, ord)
                JOIN pg_attribute a  ON a.attrelid = t.oid AND a.attnum = u.attnum
                WHERE n.nspname      = '{_config.Source.Schema}'
                  AND ix.indisunique  = false
                  AND ix.indisprimary = false
                ORDER BY t.relname, i.relname, u.ord
            ";
    }

    protected override string GetPrimaryKeysQuery()
    {
        // Récupération des contraintes de clés primaires
        return GetConstraintKeyQuery("PRIMARY KEY");
    }

    protected override string GetTableCommentsQuery()
    {
        return @$"
                SELECT
                    pgc.relname                             AS TableName,
                    obj_description(pgc.oid, 'pg_class')   AS Comment
                FROM pg_class pgc
                JOIN pg_namespace pgn
                    ON pgn.oid = pgc.relnamespace
                WHERE pgn.nspname = '{_config.Source.Schema}'
                    AND pgc.relkind = 'r'
                    AND obj_description(pgc.oid, 'pg_class') IS NOT NULL
            ";
    }

    protected override string GetUniqueKeysQuery()
    {
        // Récupération des contraintes d'unicité
        return $@"
                SELECT
                    tc.constraint_name  AS Name,
                    tc.table_name       AS TableName,
                    kcu.column_name     AS ColumnName
                FROM 
                    information_schema.table_constraints        AS tc 
                    JOIN information_schema.key_column_usage    AS kcu
                        ON  tc.constraint_name      = kcu.constraint_name
                        AND tc.table_schema         = kcu.table_schema
                WHERE       tc.constraint_type      = 'UNIQUE'
                    AND     tc.table_schema         = '{_config.Source.Schema}'
            ";
    }

    private string GetConstraintKeyQuery(string name)
    {
        return @$"
                SELECT
                    tc.table_name   AS TableName, 
                    kcu.column_name AS ColumnName, 
                    ccu.table_name  AS ForeignTableName,
                    ccu.column_name AS ForeignColumnName 
                FROM 
                            information_schema.table_constraints   AS tc 
                    JOIN    information_schema.key_column_usage    AS kcu
                        ON  tc.constraint_name   = kcu.constraint_name
                        AND tc.table_schema      = kcu.table_schema
                    JOIN information_schema.constraint_column_usage AS ccu
                        ON  ccu.constraint_name  = tc.constraint_name
                        AND ccu.table_schema     = tc.table_schema
                WHERE       tc.constraint_type   = '{name}'
                    AND     tc.table_schema      = '{_config.Source.Schema}'
                    AND     ccu.table_schema     = '{_config.Source.Schema}'
            ";
    }
}
