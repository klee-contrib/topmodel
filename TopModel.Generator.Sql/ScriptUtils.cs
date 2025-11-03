using TopModel.Core.Model;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Sql;

/// <summary>
/// Classe utilitaire pour écritre du SQL.
/// </summary>
public static class ScriptUtils
{
    public const string InsertKeyName = "InsertKey";

    public static IEnumerable<IProperty> GetAllProperties(this Class classe, IEnumerable<Class> availableClasses)
    {
        foreach (
            var prop in classe.Properties.Where(p =>
                p
                    is not AssociationProperty { Type: AssociationType.OneToMany or AssociationType.ManyToMany }
                        and not AliasProperty
                        {
                            Property: AssociationProperty
                            {
                                Type: AssociationType.OneToMany or AssociationType.ManyToMany
                            }
                        }
            )
        )
        {
            yield return prop;
        }

        if (classe.Extends != null)
        {
            yield return new AssociationProperty
            {
                Association = classe.Extends,
                Class = classe,
                Comment = "Association vers la clé primaire de la classe parente",
                Required = true,
                PrimaryKey = !classe.PrimaryKey.Any(),
            };
        }
    }

    /// <summary>
    /// Retourne le nom du type de table SQL correspondant à la classe.
    /// </summary>
    /// <param name="classe">Classe.</param>
    /// <returns>Nom du type de table.</returns>
    public static string GetTableTypeName(this Class classe)
    {
        return classe == null ? throw new ArgumentNullException(nameof(classe)) : classe.SqlName + "_TABLE_TYPE";
    }

    public static IFileWriter OpenSqlWriter(this GeneratorBase<SqlConfig> generator, string fileName)
    {
        var fw = generator.OpenFileWriter(fileName);
        fw.StartCommentToken = "----";
        return fw;
    }

    public static void WriteSqlFileHeader(
        this IFileWriter writer,
        string? appName = null,
        string? scriptName = null,
        string? description = null
    )
    {
        writer.WriteLine(
            "-- ==========================================================================================="
        );
        if (appName != null)
        {
            writer.WriteLine($"--   Application Name\t:\t{appName} ");
        }

        if (scriptName != null)
        {
            writer.WriteLine($"--   Script Name\t\t:\t{scriptName}");
        }

        if (description != null)
        {
            writer.WriteLine($"--   Description\t\t:\t{description}");
        }

        writer.WriteLine(
            "-- ==========================================================================================="
        );
    }
}
