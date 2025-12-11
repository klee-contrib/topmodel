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
        foreach (var prop in classe.Properties.Where(p => !p.AssociationToMany))
        {
            yield return prop;
        }

        if (classe.ParentAssociationProperty != null)
        {
            yield return classe.ParentAssociationProperty!;
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

    public static void WriteComments(this IFileWriter writer, Class classe, SqlConfig config)
    {
        var tableName = config.CheckIdentifierLength(classe.SqlName);

        writer.WriteLine("/**");
        writer.WriteLine("  * Commentaires pour la table " + tableName);
        writer.WriteLine(" **/");

        if (config.TargetDBMS == TargetDBMS.Sqlserver)
        {
            writer.WriteLine(
                $"EXECUTE sp_addextendedproperty 'MS_Description', '{classe.Comment.Replace("'", "''")}', 'SCHEMA', 'dbo', 'TABLE', '{classe.SqlName}'"
            );
            writer.WriteLine("go");

            foreach (var p in classe.GetAllProperties(config.AvailableClasses))
            {
                writer.WriteLine(
                    $"EXECUTE sp_addextendedproperty 'MS_Description', '{p.Comment.Replace("'", "''")}', 'SCHEMA', 'dbo', 'TABLE', '{classe.SqlName}', 'COLUMN', '{p.SqlName}'"
                );
                writer.WriteLine("go");
            }
        }
        else
        {
            writer.WriteLine(
                $"COMMENT ON TABLE {tableName} IS '{classe.Comment.Replace("'", "''")}'{config.BatchSeparator}"
            );

            foreach (var p in classe.GetAllProperties(config.AvailableClasses))
            {
                writer.WriteLine(
                    $"COMMENT ON COLUMN {tableName}.{p.SqlName} IS '{p.Comment.Replace("'", "''")}'{config.BatchSeparator}"
                );
            }
        }
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
