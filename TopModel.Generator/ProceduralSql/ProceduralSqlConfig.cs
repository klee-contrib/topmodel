namespace TopModel.Generator.ProceduralSql;

/// <summary>
/// Paramètres pour la génération de SQL procédural.
/// </summary>
public class ProceduralSqlConfig : GeneratorConfigBase
{
    /// <summary>
    /// SGBD cible ("postgre" ou "sqlserver").
    /// </summary>
    public TargetDBMS TargetDBMS { get; set; }

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
    /// Désactive les colonnes d'identité.
    /// </summary>
    public bool DisableIdentity { get; set; }
}