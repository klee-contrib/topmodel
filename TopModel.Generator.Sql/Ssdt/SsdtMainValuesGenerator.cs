using Microsoft.Extensions.Logging;
using TopModel.Core.Model;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Sql.Ssdt;

/// <summary>
/// Générateur écrivant un script qui ordonnance l'appel aux scripts d'insertions de valeurs.
/// </summary>
public class SsdtMainValuesGenerator(ILogger<SsdtMainValuesGenerator> logger, IFileWriterProvider writerProvider)
    : ClassGroupGeneratorBase<SqlConfig>(logger, writerProvider)
{
    public override string Name => "SsdtMainValuesGen";

    protected override IEnumerable<(string FileType, string FileName)> GetFileNames(Class classe, string tag)
    {
        if (Config.HasValues(classe))
        {
            yield return (
                "main",
                Path.Combine(Config.Ssdt!.InitListScriptFolder!, Config.Ssdt!.InitListMainScriptName!)
                    .Replace('\\', '/')
            );
        }
    }

    protected override void HandleFile(string fileType, string fileName, string tag, IEnumerable<Class> classes)
    {
        using var writer = this.OpenSqlWriter(fileName);

        // Entête du fichier.
        WriteHeader(writer);

        // Appel des scripts d'insertion.
        WriteScriptCalls(writer, classes.SortInserts(Config), tag);
    }

    /// <summary>
    /// Ecrit l'entête du fichier.
    /// </summary>
    /// <param name="writer">Writer.</param>
    protected virtual void WriteHeader(IFileWriter writer)
    {
        writer.WriteSqlFileHeader(description: "Insertion des valeurs de listes statiques.");
        writer.WriteLine();
    }

    /// <summary>
    /// Ecrit les appels de scripts.
    /// </summary>
    /// <param name="writer">Writer.</param>
    /// <param name="classes">Ensemble des listes de référence.</param>
    /// <param name="tag">Tag.</param>
    protected virtual void WriteScriptCalls(IFileWriter writer, IEnumerable<Class> classes, string tag)
    {
        foreach (var classe in classes)
        {
            var subscriptName = Config.GetSqlName(classe, tag, noQuote: true) + ".insert.sql";
            writer.WriteLine("/* Insertion dans la table " + Config.GetSqlName(classe, tag) + ". */");
            writer.WriteLine(":r .\\" + subscriptName);
            writer.WriteLine();
        }
    }
}
