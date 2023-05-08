using System.Text.RegularExpressions;
using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using TopModel.Utils;

namespace TopModel.ModelGenerator.Database;

public class DatabasePgTmdGenerator : DatabaseTmdGenerator, IDisposable
{
    private readonly Dictionary<string, TmdClass> _classes = new();
    private readonly DatabaseConfig _config;
    private readonly NpgsqlConnection _connection;
    private readonly ILogger<DatabaseTmdGenerator> _logger;

    private int _fileIndice = 10;
    private int _moduleIndice = 0;

#pragma warning disable CS8618
    public DatabasePgTmdGenerator(ILogger<DatabaseTmdGenerator> logger, DatabaseConfig config)
        : base(logger, config)
    {
        _config = config;
        _logger = logger;

        // Connexion à la base de données {_config.Source.DbName}
        _connection = new NpgsqlConnection(config.ConnectionString);
    }
#pragma warning restore CS8618

    public override string Name => "DatabasePgGen";

    public Dictionary<string, string> Passwords { get; init; }

    private IEnumerable<TmdFile> Files => _classes.Select(c => c.Value.File!).Distinct().OrderBy(c => c.Name);


    public void Dispose()
    {
        _connection.Dispose();
    }

    protected override void InitConnection()
    {
        {
            if (Passwords.TryGetValue(_config.Source.DbName, out var password))
            {
                _config.Source.Password = password;
            }
            else
            {
                try
                {
                    using var connection = new NpgsqlConnection(_config.ConnectionString);
                    connection.Open();
                }
                catch (NpgsqlException)
                {
                    _logger.LogInformation($"Mot de passe pour l'utilisateur {_config.Source.User}:  ");
                    while (true)
                    {
                        var key = Console.ReadKey(true);
                        if (key.Key == ConsoleKey.Enter)
                        {
                            break;
                        }

                        password += key.KeyChar;
                    }

                    Passwords.Add(_config.Source.DbName, password ?? string.Empty);
                    _config.Source.Password = password;
                }
            }
        }
    }

    protected override async Task<IEnumerable<DbColumn>> GetColumns()
    {
        // Récupération des colonnes
        var columns = await _connection
            .QueryAsync<DbColumn>(@$"
                select  table_name                                                              as TableName, 
                        column_name                                                             as ColumnName, 
                        data_type                                                               as DataType,
                        is_nullable = 'YES'                                                     as Nullable,
                        coalesce(numeric_precision, datetime_precision, interval_precision)     as Precision,
                        coalesce(character_maximum_length, numeric_scale, interval_precision)   as Scale
                from information_schema.columns 
                where table_schema  = '{_config.Source.Schema}'
                order by ordinal_position 
            ");
        return columns.Where(c => !_config.Exclude.Contains(c.TableName));
    }

    protected override async Task<IEnumerable<ConstraintKey>> GetConstraintKey(string name)
    {
        var constraint = await _connection
            .QueryAsync<ConstraintKey>(@$"
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
            ");

        return constraint.Where(c => !_config.Exclude.Contains(c.TableName));
    }

    protected override Task<IEnumerable<ConstraintKey>> GetForeignKeys()
    {
        // Récupération des contraintes de clés étrangères
        return GetConstraintKey("FOREIGN KEY");
    }

    protected override Task<IEnumerable<ConstraintKey>> GetPrimaryKeys()
    {
        // Récupération des contraintes de clés primaires
        return GetConstraintKey("PRIMARY KEY");
    }

    protected override async Task<IEnumerable<ConstraintKey>> GetUniqueKeys()
    {
        // Récupération des contraintes d'unicité
        var keys = await _connection
            .QueryAsync<ConstraintKey>(@$"
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
            ");

        return keys.Where(c => !_config.Exclude.Contains(c.TableName));
    }

    protected override async Task ReadValues()
    {
        // Extraction des valeurs pour les tables paramétrées
        foreach (var classe in _config.ExtractValues.Select(classe => _classes.FirstOrDefault(c => classe == c.Value.Name)).Where(c => c.Value != null))
        {
            var values = await _connection.QueryAsync(@$"select * from {classe.Key}");
            classe.Value.Values.AddRange(values.Select(r =>
            {
                var d = new Dictionary<string, string?>();
                foreach (var kv in r)
                {
                    d.Add(classe.Value.Properties.First(p => p.SqlName == kv.Key).Name, kv.Value?.ToString());
                }

                return d;
            }));
        }
    }
}