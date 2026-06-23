namespace TopModel.Generator.Sql.Procedural;

/// <summary>
/// Paramètres pour la génération de SQL procédural.
/// </summary>
public class ProceduralSqlConfig
{
    /// <summary>
    /// Nom du fichier contenant les scripts de création de tables. Par défaut : '01_tables.sql'.
    /// </summary>
    public string TablesFileName { get; set; } = "01_tables.sql";

    /// <summary>
    /// Nom du fichier contenant le script de création des indexes et des clés étrangères et uniques. Par défaut : '02_indexes_and_keys.sql'.
    /// </summary>
    public string IndexesAndKeysFileName { get; set; } = "02_indexes_and_keys.sql";

    /// <summary>
    /// Nom du fichier contenant les scripts d'insertion des valeurs initiales. Par défaut : '03_values.sql'.
    /// </summary>
    public string ValuesFileName { get; set; } = "03_values.sql";

    /// <summary>
    /// Nom du fichier contenant le script d'insertion des ressources (libellés traduits). Par défaut : '04_resources.sql'.
    /// </summary>
    public string ResourcesFileName { get; set; } = "04_resources.sql";

    /// <summary>
    /// Nom du fichier contenant les scripts de création de types (pour SQL Server). Par défaut : '05_types.sql'.
    /// </summary>
    public string TypesFileName { get; set; } = "05_types.sql";

    /// <summary>
    /// Nom du fichier contenant les scripts de création de commentaires sur les tables et les colonnes.
    /// </summary>
    public string? CommentsFileName { get; set; }
}
