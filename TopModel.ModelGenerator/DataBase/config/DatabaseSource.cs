using SharpYaml.Serialization;

namespace TopModel.ModelGenerator.Database;

public class DatabaseSource
{
    [YamlMember("host")]
    public string Host { get; set; } = "localhost";

    [YamlMember("port")]
    public string Port { get; set; } = "5432";

    [YamlMember("dbName")]
    public string DbName { get; set; } = "postgres";

    [YamlMember("user")]
    public string User { get; set; } = "postgres";

    [YamlMember("password")]
    public string? Password { get; set; }

    [YamlMember("schema")]
    public string Schema { get; set; } = "public";
}