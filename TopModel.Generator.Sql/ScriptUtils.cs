using TopModel.Core;

namespace TopModel.Generator.Sql;

/// <summary>
/// Classe utilitaire pour écritre du SQL.
/// </summary>
public static class ScriptUtils
{
    public const string InsertKeyName = "InsertKey";

    /// <summary>
    /// Retourne le nom du type de table SQL correspondant à la classe.
    /// </summary>
    /// <param name="classe">Classe.</param>
    /// <returns>Nom du type de table.</returns>
    public static string GetTableTypeName(this Class classe)
    {
        if (classe == null)
        {
            throw new ArgumentNullException(nameof(classe));
        }

        return classe.SqlName + "_TABLE_TYPE";
    }
}