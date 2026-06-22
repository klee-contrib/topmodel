using Microsoft.Extensions.Logging;
using TopModel.Core.Model;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Sql.Ssdt;

/// <summary>
/// Scripter permettant d'écrire les scripts d'initialisation des valeurs.
/// </summary>
public class SsdtValuesGenerator(ILogger<SsdtValuesGenerator> logger, IFileWriterProvider writerProvider)
    : ClassGeneratorBase<SqlConfig>(logger, writerProvider)
{
    public override string Name => "SsdtValuesGen";

    protected override bool FilterClass(Class classe)
    {
        return classe.HasTable && classe.Values.Count > 0
            || (
                classe.InheritanceStrategy == InheritanceStrategy.SingleTable
                && Config.Classes.Any(c => c.Extends == classe && c.Values.Count > 0)
            );
    }

    protected override string GetFileName(Class classe, string tag)
    {
        return Path.Combine(Config.Ssdt!.InitListScriptFolder!, classe.SqlName + ".insert.sql");
    }

    protected override void HandleClass(string fileName, Class classe, string tag)
    {
        using var writer = this.OpenSqlWriter(fileName);

        var tableName = classe.SqlName;

        // Entête du fichier.
        WriteHeader(writer, tableName);

        // Ecrit les inserts.
        WriteInsertLines(writer, classe, tag);

        WriteFooter(writer);
    }

    /// <summary>
    /// Ecrit l'entête du fichier.
    /// </summary>
    /// <param name="writer">Flux.</param>
    /// <param name="tableName">Nom de la table.</param>
    private static void WriteHeader(IFileWriter writer, string tableName)
    {
        writer.WriteSqlFileHeader(description: $"Insertion des valeurs de la table {tableName}.");
        writer.WriteLine();
    }

    /// <summary>
    /// Ecrit le pied du fichier.
    /// </summary>
    /// <param name="writer">Flux.</param>
    private void WriteFooter(IFileWriter writer)
    {
        if (Config.TargetDBMS == TargetDBMS.Sqlserver)
        {
            writer.WriteLine("GO");
        }
    }

    /// <summary>
    /// Ecrit les lignes d'insertion pour la liste des valeurs.
    /// </summary>
    /// <param name="writer">Flux.</param>
    /// <param name="item">Liste de références.</param>
    private void WriteInsertLines(IFileWriter writer, Class item, string tag)
    {
        foreach (var initItem in Config.GetAllValues(item))
        {
            writer.WriteLine(Config.GetInsertLine(item, initItem, tag));
        }
    }
}
