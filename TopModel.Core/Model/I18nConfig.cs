#nullable disable

namespace TopModel.Core;

/// <summary>
/// Configuration i18n.
/// </summary>
public class I18nConfig
{
    /// <summary>
    /// Langue par défaut de l'application.
    /// </summary>
    public string DefaultLang { get; set; } = string.Empty;

    /// <summary>
    /// Liste des langues de l'application (autre que la langue par défaut).
    /// </summary>
    public List<string> Langs { get; set; } = new();

    /// <summary>
    /// Template du chemin des dossiers de traductions entrants. Doit contenir le template {lang}.
    /// </summary>
    public string RootPath { get; set; } = "{lang}";

    /// <summary>
    /// Si les libellés des listes de références doivent être traduits.
    /// </summary>
    public bool TranslateReferences { get; set; } = true;

    /// <summary>
    /// Si les libellés des propriétés doivent être traduits.
    /// </summary>
    public bool TranslateProperties { get; set; } = true;
}
