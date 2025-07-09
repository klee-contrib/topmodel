#nullable disable

using TopModel.Utils;

namespace TopModel.Core;

public class ModelConfig : ConfigBase
{
    public string App { get; set; }

    public bool PluralizeTableNames { get; set; }

    public bool UseLegacyRoleNames { get; set; }

    public bool UseLegacyAssociationCompositionMappers { get; set; }

    public I18nConfig I18n { get; set; } = new();

    public Dictionary<string, IEnumerable<IDictionary<string, object>>> Generators { get; } = [];

    public List<string> CustomGenerators { get; set; } = [];

    public Dictionary<string, WatcherConfigBase> Configs { get; } = [];

    public Dictionary<string, Variable> GlobalVariables => Configs
        .SelectMany(c => c.Value.GlobalVariableNames
            .Select(v => new { Name = v, ConfigName = c.Key, c.Value.Tags, c.Value.Languages }))
        .GroupBy(v => v.Name)
        .ToDictionary(g => g.Key, g => new Variable
        {
            Configs = g.Select(g => g.ConfigName).ToList(),
            Tags = g.SelectMany(g => g.Tags).Distinct().ToList(),
            Languages = g.SelectMany(g => g.Languages).Distinct().ToList()
        });

    public Dictionary<string, Variable> TagVariables => Configs
        .SelectMany(c => c.Value.TagVariableNames
            .Select(v => new { Name = v, ConfigName = c.Key, c.Value.Tags, c.Value.Languages }))
        .GroupBy(v => v.Name)
        .ToDictionary(g => g.Key, g => new Variable
        {
            ByTag = true,
            Configs = g.Select(g => g.ConfigName).ToList(),
            Tags = g.SelectMany(g => g.Tags).Distinct().ToList(),
            Languages = g.SelectMany(g => g.Languages).Distinct().ToList()
        });

    public string GetFileName(string filePath)
    {
        return Path.GetRelativePath(Path.Combine(Directory.GetCurrentDirectory(), ModelRoot), filePath)
            .Replace(".tmd", string.Empty)
            .Replace("\\", "/");
    }
}