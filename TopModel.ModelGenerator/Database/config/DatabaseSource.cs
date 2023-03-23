namespace TopModel.ModelGenerator.Database;

public class DatabaseSource
{
    public string Host { get; set; } = "localhost";

    public string Port { get; set; } = "5432";

    public string DbName { get; set; } = "postgres";

    public string User { get; set; } = "postgres";

    public string? Password { get; set; }

    public string Schema { get; set; } = "public";
}