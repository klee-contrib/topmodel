#nullable disable

using TopModel.Core.Model;
using TopModel.Utils;

namespace TopModel.Core;

public class ModelConfig : ConfigBase
{
    public string App { get; set; }

    public bool PluralizeTableNames { get; set; }

    public bool DefaultAssociationUseClass { get; set; }

    public bool UseLegacyRoleNames { get; set; }

    public I18nConfig I18n { get; set; } = new();

    public IDictionary<string, IEnumerable<IDictionary<string, object>>> Generators { get; } =
        new Dictionary<string, IEnumerable<IDictionary<string, object>>>();

    public IList<string> CustomGenerators { get; set; } = [];

    public IDictionary<string, WatcherConfigBase> Configs { get; } = new Dictionary<string, WatcherConfigBase>();

    public IDictionary<string, Variable> GlobalVariables =>
        Configs
            .SelectMany(c =>
                c.Value.GlobalVariableNames.Select(v => new
                {
                    Name = v,
                    ConfigName = c.Key,
                    c.Value.Tags,
                    c.Value.Language,
                })
            )
            .GroupBy(v => v.Name)
            .ToDictionary(
                g => g.Key,
                g => new Variable
                {
                    Configs = g.Select(g => g.ConfigName).ToList(),
                    Tags = g.SelectMany(g => g.Tags).Distinct().ToList(),
                    Languages = g.SelectMany(g => g.Language).Distinct().ToList(),
                }
            );

    public IDictionary<string, Variable> TagVariables =>
        Configs
            .SelectMany(c =>
                c.Value.TagVariableNames.Select(v => new
                {
                    Name = v,
                    ConfigName = c.Key,
                    c.Value.Tags,
                    c.Value.Language,
                })
            )
            .GroupBy(v => v.Name)
            .ToDictionary(
                g => g.Key,
                g => new Variable
                {
                    ByTag = true,
                    Configs = g.Select(g => g.ConfigName).ToList(),
                    Tags = g.SelectMany(g => g.Tags).Distinct().ToList(),
                    Languages = g.SelectMany(g => g.Language).Distinct().ToList(),
                }
            );

    public string GetFileName(string filePath)
    {
        return Path.GetRelativePath(Path.Combine(Directory.GetCurrentDirectory(), ModelRoot), filePath)
            .Replace(".tmd", string.Empty)
            .Replace('\\', '/');
    }

    public override ModelConfig Init(string rootDir)
    {
        ConfigRoot = rootDir;
        ModelRoot ??= string.Empty;
        LockFileName ??= "topmodel.lock";
        ModelUtils.TrimSlashes(this, c => c.ModelRoot);
        ModelUtils.CombinePath(rootDir, this, c => c.ModelRoot);
        ModelUtils.CombinePath(rootDir, I18n, c => c.RootPath);
        return this;
    }
}
