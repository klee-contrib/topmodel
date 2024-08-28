using System.Text;
using TopModel.Core;

namespace TopModel.Generator.Sql.Ssdt.Scripter;

/// <summary>
/// Scripter permettant d'écrire les scripts de création d'un type de table SQL.
/// </summary>
public class SqlTableTypeScripter : ISqlScripter<Class>
{
    private readonly SqlConfig _config;

    public SqlTableTypeScripter(SqlConfig config)
    {
        _config = config;
    }

    /// <summary>
    /// Calcule le nom du script pour l'item.
    /// </summary>
    /// <param name="item">Item à scripter.</param>
    /// <returns>Nom du fichier de script.</returns>
    public string GetScriptName(Class item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        return item.GetTableTypeName() + ".sql";
    }

    /// <summary>
    /// Ecrit dans un flux le script pour l'item.
    /// </summary>
    /// <param name="writer">Flux d'écriture.</param>
    /// <param name="item">Table à scripter.</param>
    /// <param name="availableClasses">Classes disponibles.</param>
    public void WriteItemScript(TextWriter writer, Class item, IEnumerable<Class> availableClasses)
    {
        if (writer == null)
        {
            throw new ArgumentNullException(nameof(writer));
        }

        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        // Entête du fichier.
        WriteHeader(writer, item.GetTableTypeName());

        // Ouverture du create table.
        WriteCreateTableOpening(writer, item);

        // Intérieur du create table.
        WriteInsideInstructions(writer, item);

        // Fin du create table.
        WriteCreateTableClosing(writer);
    }

    /// <summary>
    /// Ecrit le pied du script.
    /// </summary>
    /// <param name="writer">Flux.</param>
    private static void WriteCreateTableClosing(TextWriter writer)
    {
        writer.WriteLine(")");
        writer.WriteLine("go");
        writer.WriteLine();
    }

    /// <summary>
    /// Ecrit l'ouverture du create table.
    /// </summary>
    /// <param name="writer">Flux.</param>
    /// <param name="table">Table.</param>
    private static void WriteCreateTableOpening(TextWriter writer, Class table)
    {
        writer.WriteLine("Create type [" + table.GetTableTypeName() + "] as Table (");
    }

    /// <summary>
    /// Ecrit l'entête du fichier.
    /// </summary>
    /// <param name="writer">Flux.</param>
    /// <param name="tableName">Nom de la table.</param>
    private static void WriteHeader(TextWriter writer, string tableName)
    {
        writer.WriteLine("-- ===========================================================================================");
        writer.WriteLine("--   Description		:	Création du type de table " + tableName + ".");
        writer.WriteLine("-- ===========================================================================================");
        writer.WriteLine();
    }

    /// <summary>
    /// Ecrit la colonne InsertKey.
    /// </summary>
    /// <param name="sb">Flux.</param>
    /// <param name="classe">Classe.</param>
    private static void WriteInsertKeyLine(StringBuilder sb, Class classe)
    {
        sb.Append('[').Append((classe.Trigram != null ? $"{classe.Trigram}_" : string.Empty) + "INSERT_KEY] int null");
    }

    /// <summary>
    /// Ecrit le SQL pour une colonne.
    /// </summary>
    /// <param name="sb">Flux.</param>
    /// <param name="property">Propriété.</param>
    private void WriteColumn(StringBuilder sb, IProperty property)
    {
        var persistentType = _config.GetType(property);
        sb.Append('[').Append(property.SqlName).Append("] ").Append(persistentType).Append(" null");
    }

    /// <summary>
    /// Ecrit les instructions à l'intérieur du create table.
    /// </summary>
    /// <param name="writer">Flux.</param>
    /// <param name="table">Table.</param>
    private void WriteInsideInstructions(TextWriter writer, Class table)
    {
        // Construction d'une liste de toutes les instructions.
        var definitions = new List<string>();
        var sb = new StringBuilder();

        // Colonnes
        foreach (var property in table.Properties)
        {
            if ((!property.PrimaryKey || _config.ShouldQuoteValue(property)) && property.Name != ScriptUtils.InsertKeyName)
            {
                sb.Clear();
                WriteColumn(sb, property);
                definitions.Add(sb.ToString());
            }
        }

        // InsertKey.
        sb.Clear();
        WriteInsertKeyLine(sb, table);
        definitions.Add(sb.ToString());

        // Ecriture de la liste concaténée.
        var separator = "," + Environment.NewLine;
        writer.Write(string.Join(separator, definitions.Select(x => "\t" + x)));
    }
}