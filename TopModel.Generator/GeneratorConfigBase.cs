#nullable disable
namespace TopModel.Generator;

public abstract class GeneratorConfigBase<T>
{
    /// <summary>
    /// Racine du répertoire de génération.
    /// </summary>
    public string OutputDirectory { get; set; }

    /// <summary>
    /// Tags du générateur.
    /// </summary>
    public IList<string> Tags { get; set; }

    /// <summary>
    /// Surcharge par tag du token {tag} pour les différents paramètres du générateur.
    /// </summary>
    public Dictionary<string, T> TagConfigOverrides { get; set; } = new();

    /// <summary>
    /// Récupère la valeur du token {tag} pour une propriété donnée.
    /// </summary>
    /// <param name="tag">Valeur du tag.</param>
    /// <param name="property">Propriété.</param>
    /// <returns>Valeur.</returns>
    public string GetTagValue(string tag, Func<T, string> property)
    {
        if (TagConfigOverrides.TryGetValue(tag, out var value))
        {
            var tagOverride = property(value);
            if (tagOverride != null)
            {
                return tagOverride;
            }
        }

        return tag;
    }
}