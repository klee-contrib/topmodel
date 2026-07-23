using System.Text;
using Microsoft.Extensions.Logging;
using TopModel.Core.Model;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Sql.Ssdt;

/// <summary>
/// Générateur permettant d'écrire les scripts de création d'un type de table SQL.
/// </summary>
public class SsdtTableTypeGenerator(ILogger<SsdtTableTypeGenerator> logger, IFileWriterProvider writerProvider)
    : ClassGeneratorBase<SqlConfig>(logger, writerProvider)
{
    public override string Name => "SsdtTableTypeGen";

    protected override bool FilterClass(Class classe)
    {
        return classe.HasTable && Config.GetProperties(classe).Any(p => p.Name == ScriptUtils.InsertKeyName);
    }

    protected override string GetFileName(Class classe, string tag)
    {
        return Path.Combine(
            Config.Ssdt!.TableTypeScriptFolder!,
            Config.GetSqlTableTypeName(classe, tag, noQuote: true) + ".sql"
        );
    }

    protected override void HandleClass(string fileName, Class classe, string tag)
    {
        using var writer = this.OpenSqlWriter(fileName);

        // Entête du fichier.
        WriteHeader(writer, Config.GetSqlTableTypeName(classe, tag));

        // Ouverture du create table.
        WriteCreateTableOpening(writer, classe, tag);

        // Intérieur du create table.
        WriteInsideInstructions(writer, classe, tag);

        // Fin du create table.
        WriteCreateTableClosing(writer);
    }

    /// <summary>
    /// Ecrit le SQL pour une colonne.
    /// </summary>
    /// <param name="sb">StringBuilder.</param>
    /// <param name="property">Propriété.</param>
    /// <param name="tag">Tag.</param>
    protected virtual void WriteColumn(StringBuilder sb, IProperty property, string tag)
    {
        var persistentType = Config.GetType(property);
        sb.Append(Config.GetSqlName(property, tag)).Append(' ').Append(persistentType).Append(" null");
    }

    /// <summary>
    /// Ecrit le pied du script.
    /// </summary>
    /// <param name="writer">Writer.</param>
    protected virtual void WriteCreateTableClosing(IFileWriter writer)
    {
        writer.WriteLine(")");
        writer.WriteLine("go");
        writer.WriteLine();
    }

    /// <summary>
    /// Ecrit l'ouverture du create table.
    /// </summary>
    /// <param name="writer">Writer.</param>
    /// <param name="classe">Classe.</param>
    /// <param name="tag">Tag.</param>
    protected virtual void WriteCreateTableOpening(IFileWriter writer, Class classe, string tag)
    {
        writer.WriteLine($"Create type {Config.GetSqlTableTypeName(classe, tag)} as Table (");
    }

    /// <summary>
    /// Ecrit l'entête du fichier.
    /// </summary>
    /// <param name="writer">Writer.</param>
    /// <param name="tableName">Nom de la table.</param>
    protected virtual void WriteHeader(IFileWriter writer, string tableName)
    {
        writer.WriteSqlFileHeader(description: $"Création du type de table {tableName}.");
        writer.WriteLine();
    }

    /// <summary>
    /// Ecrit la colonne InsertKey.
    /// </summary>
    /// <param name="sb">Flux.</param>
    /// <param name="classe">Classe.</param>
    /// <param name="tag">Tag.</param>
    protected virtual void WriteInsertKeyLine(StringBuilder sb, Class classe, string tag)
    {
        var insertKeyProp = Config.GetProperties(classe).SingleOrDefault(p => p.Name == ScriptUtils.InsertKeyName);
        if (insertKeyProp != null)
        {
            sb.Append($"{Config.GetSqlName(insertKeyProp, tag)} int null");
        }
    }

    /// <summary>
    /// Ecrit les instructions à l'intérieur du create table.
    /// </summary>
    /// <param name="writer">Writer.</param>
    /// <param name="classe">Classe.</param>
    /// <param name="tag">Tag.</param>
    protected virtual void WriteInsideInstructions(IFileWriter writer, Class classe, string tag)
    {
        // Construction d'une liste de toutes les instructions.
        var definitions = new List<string>();
        var sb = new StringBuilder();

        // Colonnes
        foreach (var property in Config.GetProperties(classe))
        {
            if (
                (!property.PrimaryKey || Config.ShouldQuoteValue(property))
                && property.Name != ScriptUtils.InsertKeyName
            )
            {
                sb.Clear();
                WriteColumn(sb, property, tag);
                definitions.Add(sb.ToString());
            }
        }

        // InsertKey.
        sb.Clear();
        WriteInsertKeyLine(sb, classe, tag);
        definitions.Add(sb.ToString());

        // Ecriture de la liste concaténée.
        var separator = "," + Environment.NewLine;
        writer.Write(string.Join(separator, definitions.Select(x => "\t" + x)));
    }
}
