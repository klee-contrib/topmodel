using Microsoft.Extensions.Logging;
using TopModel.Core.Model;
using TopModel.Core.Utils;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Sql.Procedural;

/// <summary>
/// Générateur SQL procédural pour les déclarations de tables.
/// </summary>
public class SqlTablesGenerator(ILogger<SqlTablesGenerator> logger, IFileWriterProvider writerProvider)
    : ClassGroupGeneratorBase<SqlConfig>(logger, writerProvider)
{
    public override string Name => "SqlTablesGen";

    protected override IEnumerable<(string FileType, string FileName)> GetFileNames(Class classe, string tag)
    {
        if (classe.HasTable)
        {
            yield return ("tables", Config.Procedural!.TablesFileName);
        }
    }

    protected override void HandleFile(string fileType, string fileName, string tag, IEnumerable<Class> classes)
    {
        using var writer = this.OpenSqlWriter(fileName);

        var appName = classes.First().Namespace.App;

        writer.WriteSqlFileHeader(appName, fileName.Split('/')[^1], "Script de création des tables.");

        foreach (
            var classe in CoreUtils.Sort(
                classes.OrderBy(c => c.SqlName),
                c => Config.GetProperties(c).Select(p => p.Association!).Where(p => p?.Enum == EnumMode.Enum)
            )
        )
        {
            WriteTableDeclaration(classe, writer, tag);
        }
    }

    /// <summary>
    /// Ecrit les contraintes de check (pour Oracle, pour les booléens).
    /// </summary>
    /// <param name="writer">Writer.</param>
    /// <param name="properties">Liste des propriétés persistantes.</param>
    /// <param name="tag">Tag.</param>
    protected virtual void WriteCheckConstraints(IFileWriter writer, IEnumerable<IProperty> properties, string tag)
    {
        if (Config.TargetDBMS != TargetDBMS.Oracle)
        {
            return;
        }

        /* En Oracle, en 2024, il n'y a pas de type booléen. On utilise un numeric(1) et on rajoute une check constraint pour forcer les valeurs 0 et 1. */
        bool IsNumericBoolean(IProperty property)
        {
            var sqlType = Config.GetType(property);
            return sqlType == "number(1)" && SqlConfig.IsBoolean(property);
        }

        foreach (var property in properties)
        {
            if (IsNumericBoolean(property))
            {
                writer.WriteLine(
                    $"\tconstraint CHK_{property.SqlName} check ({Config.GetSqlName(property, tag)} in (0,1)),"
                );
            }
        }
    }

    /// <summary>
    /// Ajoute la fin de la déclaration de la table.
    /// </summary>
    /// <param name="writer">Writer.</param>
    protected virtual void WriteEndTableDeclaration(IFileWriter writer)
    {
        writer.WriteLine($"){Config.GetTablespaceDeclaration(Config.TableTablespace)}{Config.BatchSeparator}");
    }

    /// <summary>
    /// Ajoute les contraintes de clés primaires.
    /// </summary>
    /// <param name="writer">Writer.</param>
    /// <param name="classe">Classe pour laquelle écrire la contrainte.</param>
    /// <param name="properties">Propriétés persistantes de la classe.</param>
    /// <param name="tag">Tag.</param>
    protected virtual void WritePrimaryKeyConstraint(
        IFileWriter writer,
        Class classe,
        IEnumerable<IProperty> properties,
        string tag
    )
    {
        if (!properties.Any(p => p.PrimaryKey))
        {
            return;
        }

        writer.Write($"\tconstraint {Config.GetSqlPrimaryKeyName(classe, tag)} primary key ");
        if (Config.TargetDBMS == TargetDBMS.Sqlserver)
        {
            writer.Write("clustered ");
        }

        writer.WriteLine(
            $"({string.Join(',', properties.Where(p => p.PrimaryKey).Select(pk => Config.GetSqlName(pk, tag)))})"
        );
    }

    /// <summary>
    /// Ecrit la déclaration SQL d'une table.
    /// </summary>
    /// <param name="classe">Classe.</param>
    /// <param name="writer">Writer.</param>
    /// <param name="tag">Tag.</param>
    protected virtual void WriteTableDeclaration(Class classe, IFileWriter writer, string tag)
    {
        var fkPropertiesList = new List<IProperty>();

        writer.WriteLine();
        writer.WriteLine("/**");
        writer.WriteLine($"  * Création de la table {Config.GetSqlName(classe, tag, noQuote: true)}");
        writer.WriteLine(" **/");

        if (classe.Enum == EnumMode.Enum)
        {
            Config.WriteEnumType(writer, classe, tag);
        }
        else
        {
            writer.WriteLine($"create table {Config.GetSqlName(classe, tag)} (");
            foreach (var property in Config.GetProperties(classe))
            {
                Config.WriteColumn(writer, classe, property, tag);
                writer.Write(",");
                writer.WriteLine();

                if (property is { Association.IsPersistent: true } ap)
                {
                    fkPropertiesList.Add(ap);
                }
            }

            WriteCheckConstraints(writer, Config.GetProperties(classe), tag);
            WritePrimaryKeyConstraint(writer, classe, Config.GetProperties(classe), tag);
            WriteEndTableDeclaration(writer);

            Config.WriteSequence(writer, classe, tag);
        }
    }
}
