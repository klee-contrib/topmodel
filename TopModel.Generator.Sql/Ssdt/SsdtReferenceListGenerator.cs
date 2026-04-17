using Microsoft.Extensions.Logging;
using TopModel.Core.Model;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Sql.Ssdt;

/// <summary>
/// Scripter permettant d'écrire les scripts d'initialisation des valeurs de listes de référence.
/// </summary>
public class SsdtReferenceListGenerator(ILogger<SsdtReferenceListGenerator> logger, IFileWriterProvider writerProvider)
    : ClassGeneratorBase<SqlConfig>(logger, writerProvider)
{
    public override string Name => "SsdtRefListGen";

    protected override bool FilterClass(Class classe)
    {
        return classe.IsPersistent && !classe.Abstract && classe.Values.Count > 0;
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
        WriteInsertLines(writer, classe);

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
    private void WriteInsertLines(IFileWriter writer, Class item)
    {
        foreach (var initItem in item.Values)
        {
            writer.WriteLine(Config.GetInsertLine(item, initItem));
        }
    }
}
