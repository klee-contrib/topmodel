using TopModel.Generator.Core;

namespace TopModel.Generator.Sql.Procedural;

/// <summary>
/// Paramètres pour la génération de SQL procédural.
/// </summary>
public class ProceduralSqlConfig
{
    /// <summary>
    /// Retourne ou définit l'emplacement du fichier de création de base (SQL).
    /// </summary>
    public string? CrebasFile { get; set; }

    /// <summary>
    /// Retourne ou définit l'emplacement du fichier de création des index uniques (SQL).
    /// </summary>
    public string? UniqueKeysFile { get; set; }

    /// <summary>
    /// Retourne ou définit l'emplacement du fichier de création des clés étrangères (SQL).
    /// </summary>
    public string? IndexFKFile { get; set; }

    /// <summary>
    /// Retourne ou définit l'emplacement du fichier de création des types (SQL).
    /// </summary>
    public string? TypeFile { get; set; }

    /// <summary>
    /// Retourne ou définit l'emplacement du script d'insertion des données des listes de référence (SQL).
    /// </summary>
    public string? InitListFile { get; set; }

    /// <summary>
    /// Mode de génération des séquences.
    /// </summary>
    public IdentityConfig Identity { get; set; } = new() { Mode = IdentityMode.IDENTITY };
}