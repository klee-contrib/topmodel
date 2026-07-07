using Microsoft.Extensions.Logging;
using TopModel.Core.Model;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Sql.Procedural;

public abstract class AbstractSqlValuesGenerator(
    ILogger<ClassGroupGeneratorBase<SqlConfig>> logger,
    IFileWriterProvider writerProvider
) : ClassGroupGeneratorBase<SqlConfig>(logger, writerProvider)
{
    public override string Name => "SqlValuesGen";

    protected override IEnumerable<(string FileType, string FileName)> GetFileNames(Class classe, string tag)
    {
        if (
            classe.HasTable && classe.Values.Count > 0 && classe.Enum != EnumMode.Enum
            || (
                classe.InheritanceStrategy != InheritanceStrategy.DistinctTables
                && Config.Classes.Any(c => c.Extends == classe && c.Values.Count > 0)
            )
        )
        {
            yield return ("values", Config.Procedural!.ValuesFileName);
        }
    }

    protected override void HandleFile(string fileType, string fileName, string tag, IEnumerable<Class> classes)
    {
        using var writerInsert = this.OpenSqlWriter(fileName);

        writerInsert.WriteSqlFileHeader(
            classes.First().Namespace.App,
            fileName.Split('/')[^1],
            "Script d'insertion des valeurs initiales."
        );

        WriteInsertStart(writerInsert);

        foreach (var classe in classes.SortInserts(Config))
        {
            WriteInsert(writerInsert, classe, tag);
        }

        WriteInsertEnd(writerInsert);
    }

    protected virtual void WriteInsertEnd(IFileWriter writerInsert) { }

    protected virtual void WriteInsertStart(IFileWriter writerInsert) { }

    /// <summary>
    /// Ecrit dans le writer le script d'insertion dans la table staticTable ayant pour model modelClass.
    /// </summary>
    /// <param name="writer">Writer.</param>
    /// <param name="modelClass">Modele de la classe.</param>
    private void WriteInsert(IFileWriter writer, Class modelClass, string tag)
    {
        writer.WriteLine();
        writer.WriteLine("/**\t\tInitialisation de la table " + modelClass.SqlName + "\t\t**/");
        foreach (var initItem in Config.GetAllValues(modelClass))
        {
            writer.WriteLine(Config.GetInsertLine(modelClass, initItem, tag));
        }
    }
}
