using TopModel.Core.Model;
using TopModel.Utils;

namespace TopModel.Core;

/// <summary>
/// Configuration globale de TopModel.
/// </summary>
public class ModelConfig : ConfigBase
{
    /// <summary>
    /// Nom de l'application.
    /// </summary>
    public string App { get; internal set; } = null!;

    /// <summary>
    /// Utilise le nom au pluriel des classes pour générer leurs noms de table SQL.
    /// </summary>
    public bool PluralizeTableNames { get; internal set; }

    /// <summary>
    /// Détermine la valeur par défaut de <c>useClass</c> sur les propriétés d'association du modèle.
    /// </summary>
    public bool DefaultAssociationUseClass { get; internal set; }

    /// <summary>
    /// Transforme les noms de rôles d'associations en majuscules au lieu du constant case pour déterminer le nom SQL des propriétés.
    /// </summary>
    public bool UseLegacyRoleNames { get; internal set; }

    /// <summary>
    /// Configuration de la localisation de l'application.
    /// </summary>
    public I18nConfig I18n { get; internal set; } = new();

    /// <summary>
    /// Configuration des générateurs.
    /// </summary>
    public IDictionary<string, IEnumerable<IDictionary<string, object>>> Generators { get; } =
        new Dictionary<string, IEnumerable<IDictionary<string, object>>>();

    /// <summary>
    /// Chemins vers des projets C# contenant des générateurs personnalisés.
    /// </summary>
    public IList<string> CustomGenerators { get; } = [];

    /// <summary>
    /// Configurations des générateurs.
    /// </summary>
    public IDictionary<string, WatcherConfigBase> Configs { get; } = new Dictionary<string, WatcherConfigBase>();

    public IDictionary<string, Variable> GlobalVariables =>
        Configs
            .SelectMany(c =>
                c.Value.GlobalVariableNames.Select(v => new
                {
                    Name = v,
                    ConfigName = c.Key,
                    c.Value.Tags,
                    Language = c
                        .Value.Tags.SelectMany(tag => c.Value.Language.Select(l => c.Value.ResolveTagVariables(l, tag)))
                        .Distinct(),
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
                    Language = c
                        .Value.Tags.SelectMany(tag => c.Value.Language.Select(l => c.Value.ResolveTagVariables(l, tag)))
                        .Distinct(),
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
