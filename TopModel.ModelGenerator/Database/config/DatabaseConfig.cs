namespace TopModel.ModelGenerator.Database;

public class DatabaseConfig
{
    public string OutputDirectory { get; set; } = "./";

    public IList<DomainMapping> Domains { get; set; } = [];

    public DatabaseSource Source { get; set; } = new();

    public IList<string> Exclude { get; set; } = [];

    public IList<string> Tags { get; set; } = [];

    public IList<string> ExtractValues { get; set; } = [];

    public IDictionary<string, string> ClassNameOverrides { get; set; } = new Dictionary<string, string>();

    public IList<ModuleConfig> Modules { get; set; } = [];

    public string ConnectionString =>
        Source.DbType switch
        {
            DbType.POSTGRESQL => PgConnectionString,
            DbType.ORACLE => OracleConnectionString,
            DbType.MYSQL => MySqlConnectionString,
            DbType.MSSQL => MsSqlConnectionString,
            _ => string.Empty,
        };

    private string OracleConnectionString =>
        $@"DATA SOURCE={Source.Host}:{Source.Port}/{Source.DbName};USER ID={Source.User};password={Source.Password}";

    private string PgConnectionString =>
        @$"Host={Source.Host};Port={Source.Port};Database={Source.DbName};Username={Source.User}{(Source.Password != null ? $";Password={Source.Password}" : string.Empty)}";

    private string MySqlConnectionString =>
        $@"Server={Source.Host};Port={Source.Port};User ID={Source.User};Password={Source.Password};Database={Source.DbName}";

    private string MsSqlConnectionString =>
        $@"Server={Source.Host};Port={Source.Port};User ID={Source.User};Password={Source.Password};Database={Source.DbName}";
}
