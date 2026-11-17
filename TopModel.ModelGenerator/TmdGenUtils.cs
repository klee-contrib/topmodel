using System.Text.RegularExpressions;

namespace TopModel.ModelGenerator;

public static class TmdGenUtils
{
    /// <summary>
    /// Recherche le domaine correspondant au nom ou au type donné.
    /// </summary>
    /// <param name="domains">Correspondances de domaines.</param>
    /// <param name="type">Type de la propriété.</param>
    /// <param name="name">Nom de la propriété.</param>
    /// <param name="scale">Taille (sert uniquement à départager les correspondances).</param>
    /// <param name="precision">Précision (sert uniquement à départager les correspondances).</param>
    /// <returns>Le domaine trouvé, ou <c>null</c> si aucune correspondance.</returns>
    public static string? FindDomain(
        IList<DomainMapping> domains,
        string? type = null,
        string? name = null,
        string? scale = null,
        string? precision = null
    )
    {
        if (name == null && string.IsNullOrEmpty(type))
        {
            type = "object";
        }

        return domains
            .Select(d =>
            {
                var score =
                    d.Name != null && name != null ? GetMatchScore(d.Name, name, exactScore: 100000, regexScore: 10000)
                    : d.Type != null && type != null ? GetMatchScore(d.Type, type, exactScore: 1000, regexScore: 100)
                    : 0;

                if (score == 0)
                {
                    return (score, d.Domain);
                }

                if (scale != null && d.Scale == scale)
                {
                    score += 10;
                }

                if (precision != null && d.Precision == precision)
                {
                    score += 1;
                }

                return (score, d.Domain);
            })
            .Where(t => t.score > 0)
            .OrderByDescending(t => t.score)
            .Select(t => t.Domain)
            .FirstOrDefault();
    }

    /// <summary>
    /// Recherche le domaine correspondant au nom ou au type donné, et retourne le nom ou le type à défaut.
    /// </summary>
    /// <param name="domains">Correspondances de domaines.</param>
    /// <param name="type">Type de la propriété.</param>
    /// <param name="name">Nom de la propriété.</param>
    /// <param name="scale">Taille (sert uniquement à départager les correspondances).</param>
    /// <param name="precision">Précision (sert uniquement à départager les correspondances).</param>
    /// <returns>Le domaine trouvé, ou à défaut le nom ou le type.</returns>
    public static string GetDomainString(
        IList<DomainMapping> domains,
        string? type = null,
        string? name = null,
        string? scale = null,
        string? precision = null
    )
    {
        return FindDomain(domains, type, name, scale, precision) ?? name ?? type ?? string.Empty;
    }

    private static int GetMatchScore(string pattern, string value, int exactScore, int regexScore)
    {
        if (pattern.StartsWith('/'))
        {
            var regex = pattern.Length > 1 && pattern.EndsWith('/') ? pattern[1..^1] : pattern[1..];
            return Regex.IsMatch(value, regex) ? regexScore : 0;
        }

        return value.Equals(pattern, StringComparison.InvariantCultureIgnoreCase) ? exactScore : 0;
    }
}

