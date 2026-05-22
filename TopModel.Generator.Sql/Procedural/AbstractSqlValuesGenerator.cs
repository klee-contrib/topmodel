using Microsoft.Extensions.Logging;
using TopModel.Core.Model;
using TopModel.Core.Utils;
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
        if (classe.IsPersistent && classe.Type != ClassType.Interface && classe.Values.Count > 0)
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

        // Construit la liste des Reference Class ordonnée.
        var orderList = CoreUtils.Sort(
            classes.OrderBy(c => c.SqlName),
            c =>
                c.Properties.Select(a => a.Association!)
                    .Where(a => a != null && a != c && a.Values.Count > 0 && a.IsPersistent)
        );

        foreach (var classe in orderList)
        {
            WriteInsert(writerInsert, classe);
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
    private void WriteInsert(IFileWriter writer, Class modelClass)
    {
        writer.WriteLine();
        writer.WriteLine("/**\t\tInitialisation de la table " + modelClass.SqlName + "\t\t**/");
        foreach (var initItem in modelClass.Values)
        {
            writer.WriteLine(Config.GetInsertLine(modelClass, initItem));
        }
    }
}
