using SharpYaml.Serialization;

namespace TopModel.ModelGenerator.Database;

public class DatabaseConfig
{
    [YamlMember("outputDirectory")]
    public string OutputDirectory { get; set; } = "./";

    [YamlMember("domains")]
    public IList<DomainMapping> Domains { get; set; } = new List<DomainMapping>();

    [YamlMember("source")]
    public DatabaseSource Source { get; set; } = new();

    [YamlMember("exclude")]
    public List<string> Exclude { get; set; } = new();

    [YamlMember("tags")]
    public List<string> Tags { get; set; } = new();

    [YamlMember("extractValues")]
    public List<string> ExtractValues { get; set; } = new();

    [YamlMember("modules")]
    public List<ModuleConfig> Modules { get; set; } = new();

    public string ConnectionString => @$"Host={Source.Host};Port={Source.Port};Database={Source.DbName};Username={Source.User}{(Source.Password != null ? $";Password={Source.Password}" : string.Empty)}";
}
