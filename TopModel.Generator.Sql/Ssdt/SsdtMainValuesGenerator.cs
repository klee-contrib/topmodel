using Microsoft.Extensions.Logging;
using TopModel.Core.Model;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Sql.Ssdt;

/// <summary>
/// Scripter écrivant un script qui ordonnance l'appel aux scripts d'insertions de valeurs.
/// </summary>
public class SsdtMainValuesGenerator(ILogger<SsdtMainValuesGenerator> logger, IFileWriterProvider writerProvider)
    : ClassGroupGeneratorBase<SqlConfig>(logger, writerProvider)
{
    public override string Name => "SsdtMainValuesGen";

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
        WriteScriptCalls(writer, classes.SortInserts(Config));
    }

    /// <summary>
    /// Ecrit l'entête du fichier.
    /// </summary>
    /// <param name="writer">Flux.</param>
    private static void WriteHeader(IFileWriter writer)
    {
        writer.WriteSqlFileHeader(description: "Insertion des valeurs de listes statiques.");
        writer.WriteLine();
    }

    /// <summary>
    /// Ecrit les appels de scripts.
    /// </summary>
    /// <param name="writer">Flux.</param>
    /// <param name="classSet">Ensemble des listes de référence.</param>
    private void WriteScriptCalls(IFileWriter writer, IEnumerable<Class> classSet)
    {
        foreach (var classe in classSet)
        {
            var subscriptName = Config.GetSqlName(classe, noQuote: true) + ".insert.sql";
            writer.WriteLine("/* Insertion dans la table " + Config.GetSqlName(classe) + ". */");
            writer.WriteLine(":r .\\" + subscriptName);
            writer.WriteLine();
        }
    }
}
