using Microsoft.Extensions.Logging;
using TopModel.Core.Model;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Sql.Procedural;

/// <summary>
/// Générateur SQL procédural pour les valeurs de classes.
/// </summary>
public class SqlValuesGenerator(ILogger<ClassGroupGeneratorBase<SqlConfig>> logger, IFileWriterProvider writerProvider)
    : ClassGroupGeneratorBase<SqlConfig>(logger, writerProvider)
{
    public override string Name => "SqlValuesGen";

    protected override IEnumerable<(string FileType, string FileName)> GetFileNames(Class classe, string tag)
    {
        if (Config.HasValues(classe))
        {
            yield return ("values", Config.Procedural!.ValuesFileName);
        }
    }

    protected override void HandleFile(string fileType, string fileName, string tag, IEnumerable<Class> classes)
    {
        using var writer = this.OpenSqlWriter(fileName);

        writer.WriteSqlFileHeader(
            classes.First().Namespace.App,
            fileName.Split('/')[^1],
            "Script d'insertion des valeurs initiales."
        );

        WriteInsertStart(writer);

        foreach (var classe in classes.SortInserts(Config))
        {
            WriteInsert(writer, classe, tag);
        }

        WriteInsertEnd(writer);
    }

    /// <summary>
    /// Ecrit les lignes d'insertion pour les valeurs d'une classe.
    /// </summary>
    /// <param name="writer">Writer.</param>
    /// <param name="classe">Classe.</param>
    /// <param name="tag">Tag.</param>
    protected virtual void WriteInsert(IFileWriter writer, Class classe, string tag)
    {
        writer.WriteLine();
        writer.WriteLine($"/**\t\tInitialisation de la table {Config.GetSqlName(classe, tag, noQuote: true)}\t\t**/");
        Config.WriteInsertLines(writer, classe, tag);
    }

    /// <summary>
    /// Ecrit la fin du script d'insertion.
    /// </summary>
    /// <param name="writer">Writer.</param>
    protected virtual void WriteInsertEnd(IFileWriter writer)
    {
        if (Config.TargetDBMS == TargetDBMS.Sqlserver)
        {
            writer.WriteLine("set nocount off;");
            writer.WriteLine();
        }
    }

    /// <summary>
    /// Ecrit le début du script d'insertion.
    /// </summary>
    /// <param name="writer">Writer.</param>
    protected virtual void WriteInsertStart(IFileWriter writer)
    {
        if (Config.TargetDBMS == TargetDBMS.Sqlserver)
        {
            writer.WriteLine("set nocount off;");
            writer.WriteLine();
        }
    }
}
