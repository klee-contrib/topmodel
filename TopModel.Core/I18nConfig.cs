namespace TopModel.Core;

/// <summary>
/// Configuration de la localisation de l'application.
/// </summary>
public class I18nConfig
{
    /// <summary>
    /// Langue par défaut de l'application. Laisser vide si votre application n'a qu'une seule langue (non nommée). Une valeur nulle désactive la langue par défaut et l'utilisation des libellés définis dans le modèle dans une langue par défaut.
    /// </summary>
    public string? DefaultLang { get; set; } = string.Empty;

    /// <summary>
    /// Liste des langues de l'application (autres que la langue par défaut), indiquées par leur code.
    /// </summary>
    public IList<string> Langs { get; set; } = [];

    /// <summary>
    /// Template du chemin des dossiers de traductions entrants. Doit contenir le template <c>{lang}</c>.
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

    /// <summary>
    /// Si le libellé d'une propriété n'est pas renseigné, autorise l'usage de son nom comme libellé.
    /// </summary>
    public bool AllowPropertyLabelFallback { get; set; } = true;
}
