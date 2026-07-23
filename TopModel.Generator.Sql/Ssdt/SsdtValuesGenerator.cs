using Microsoft.Extensions.Logging;
using TopModel.Core.Model;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Sql.Ssdt;

/// <summary>
/// Générateur permettant d'écrire les scripts d'initialisation des valeurs.
/// </summary>
public class SsdtValuesGenerator(ILogger<SsdtValuesGenerator> logger, IFileWriterProvider writerProvider)
    : ClassGeneratorBase<SqlConfig>(logger, writerProvider)
{
    public override string Name => "SsdtValuesGen";

    protected override bool FilterClass(Class classe)
    {
        return Config.HasValues(classe);
    }

    protected override string GetFileName(Class classe, string tag)
    {
        return Path.Combine(
            Config.Ssdt!.InitListScriptFolder!,
            Config.GetSqlName(classe, tag, noQuote: true) + ".insert.sql"
        );
    }

    protected override void HandleClass(string fileName, Class classe, string tag)
    {
        using var writer = this.OpenSqlWriter(fileName);

        // Entête du fichier.
        WriteHeader(writer, Config.GetSqlName(classe, tag, noQuote: true));

        // Ecrit les inserts.
        WriteInsertLines(writer, classe, tag);

        WriteFooter(writer);
    }

    /// <summary>
    /// Ecrit le pied du fichier.
    /// </summary>
    /// <param name="writer">Writer.</param>
    protected virtual void WriteFooter(IFileWriter writer)
    {
        if (Config.TargetDBMS == TargetDBMS.Sqlserver)
        {
            writer.WriteLine("go");
        }
    }

    /// <summary>
    /// Ecrit l'entête du fichier.
    /// </summary>
    /// <param name="writer">Writer.</param>
    /// <param name="tableName">Nom de la table.</param>
    protected virtual void WriteHeader(IFileWriter writer, string tableName)
    {
        writer.WriteSqlFileHeader(description: $"Insertion des valeurs de la table {tableName}.");
        writer.WriteLine();
    }

    /// <summary>
    /// Ecrit les lignes d'insertion pour la liste des valeurs.
    /// </summary>
    /// <param name="writer">Writer.</param>
    /// <param name="classe">Classe contenant les valeurs à insérer.</param>
    /// <param name="tag">Tag.</param>
    protected virtual void WriteInsertLines(IFileWriter writer, Class classe, string tag)
    {
        Config.WriteInsertLines(writer, classe, tag);
    }
}
