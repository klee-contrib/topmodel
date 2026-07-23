using Microsoft.Extensions.Logging;
using TopModel.Core.Model;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Sql.Ssdt;

/// <summary>
/// Générateur permettant d'écrire les scripts de création d'une table SQL avec :
/// - sa structure
/// - sa contrainte PK
/// - ses contraintes FK
/// - ses indexes FK
/// - ses contraintes d'unicité sur colonne unique.
/// </summary>
public class SsdtTableGenerator(ILogger<SsdtTableGenerator> logger, IFileWriterProvider writerProvider)
    : ClassGeneratorBase<SqlConfig>(logger, writerProvider)
{
    public override string Name => "SsdtTableGen";

    protected override bool FilterClass(Class classe)
    {
        return classe.HasTable;
    }

    protected override string GetFileName(Class classe, string tag)
    {
        return Path.Combine(Config.Ssdt!.TableScriptFolder!, Config.GetSqlName(classe, tag, noQuote: true) + ".sql");
    }

    protected override void HandleClass(string fileName, Class classe, string tag)
    {
        using var writer = this.OpenSqlWriter(fileName);

        // Entête du fichier.
        WriteHeader(writer, Config.GetSqlName(classe, tag, noQuote: true));

        if (classe.Enum == EnumMode.Enum)
        {
            Config.WriteEnumType(writer, classe, tag);

            return;
        }

        // Ouverture du create table.
        WriteCreateTableOpening(writer, classe, tag);

        // Intérieur du create table.
        WriteInsideInstructions(writer, classe, tag);

        // Fin du create table.
        WriteCreateTableClosing(writer, classe);

        Config.WriteSequence(writer, classe, tag);

        // Indexes sur les clés étrangères.
        WriteIndexes(writer, classe, Config.GetProperties(classe), tag);

        // Définition
        if (Config.Ssdt!.GenerateComments)
        {
            WriteComments(writer, classe, tag);
        }
    }

    /// <summary>
    /// Ecrit la création de la propriété de description de la table.
    /// </summary>
    /// <param name="writer">Writer.</param>
    /// <param name="classe">Classe.</param>
    /// <param name="tag">Tag.</param>
    protected virtual void WriteComments(IFileWriter writer, Class classe, string tag)
    {
        writer.WriteLine();
        writer.WriteComments(classe, Config, tag);
    }

    /// <summary>
    /// Ecrit le pied du script.
    /// </summary>
    /// <param name="writer">Writer.</param>
    /// <param name="classe">Classe.</param>
    protected virtual void WriteCreateTableClosing(IFileWriter writer, Class classe)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(classe);

        writer.WriteLine(")");
        writer.WriteLine(Config.TargetDBMS == TargetDBMS.Sqlserver ? "go" : ";");
    }

    /// <summary>
    /// Ecrit l'ouverture du create table.
    /// </summary>
    /// <param name="writer">Writer.</param>
    /// <param name="classe">Classe.</param>
    /// <param name="tag">Tag.</param>
    protected virtual void WriteCreateTableOpening(IFileWriter writer, Class classe, string tag)
    {
        writer.WriteLine($"create table {Config.GetSqlName(classe, tag)} (");
    }

    /// <summary>
    /// Génère la contrainte de clé étrangère.
    /// </summary>
    /// <param name="writer">Writer.</param>
    /// <param name="property">Propriété portant la clé étrangère.</param>
    /// <param name="tag">Tag.</param>
    protected virtual void WriteForeignKeyConstraint(IFileWriter writer, IProperty property, string tag)
    {
        var referenceClass = property.Association!;
        var referencedSqlName = Config.GetSqlName(
            referenceClass.Extends?.InheritanceStrategy == InheritanceStrategy.SingleTable
                ? referenceClass.Extends
                : referenceClass,
            tag
        );

        writer.Write(
            $"constraint {Config.GetSqlForeignKeyName(property, tag)} foreign key ({Config.GetSqlName(property, tag)}) references {referencedSqlName} ({Config.GetSqlName(property.AssociationProperty!, tag)})"
        );
    }

    /// <summary>
    /// Ecrit l'entête du fichier.
    /// </summary>
    /// <param name="writer">Writer.</param>
    /// <param name="tableName">Nom de la table.</param>
    protected virtual void WriteHeader(IFileWriter writer, string tableName)
    {
        writer.WriteSqlFileHeader(description: $"Création de la table {tableName}.");
        writer.WriteLine();
    }

    /// <summary>
    /// Ecrit un index.
    /// </summary>
    /// <param name="writer">Writer.</param>
    /// <param name="indexName">Nom de l'index.</param>
    /// <param name="tableName">Nom de la table.</param>
    /// <param name="propertyNames">Noms des propriétés.</param>
    protected virtual void WriteIndex(
        IFileWriter writer,
        string indexName,
        string tableName,
        params IEnumerable<string> propertyNames
    )
    {
        writer.WriteLine(
            $"create{(Config.TargetDBMS == TargetDBMS.Sqlserver ? " nonclustered" : "")} index {indexName}"
        );
        writer.Write("\ton " + tableName + " (");
        writer.Write(string.Join(", ", propertyNames.Select(n => $"{n} asc")));
        writer.WriteLine($"){Config.BatchSeparator}");
    }

    /// <summary>
    /// Ecrit un index du modèle.
    /// </summary>
    /// <param name="index">Index.</param>
    /// <param name="writer">Writer.</param>
    /// <param name="tag">Tag.</param>
    protected virtual void WriteIndex(IFileWriter writer, IndexDefinition index, string tag)
    {
        WriteIndex(
            writer,
            Config.GetSqlName(index, tag),
            Config.GetSqlName(index.Class, tag),
            index.Properties.Select(p => Config.GetSqlName(p, tag))
        );
    }

    /// <summary>
    /// Génère les indexes portant sur les FK.
    /// </summary>
    /// <param name="writer">Writer.</param>
    /// <param name="classe">Classe.</param>
    /// <param name="properties">Champs.</param>
    /// <param name="tag">Tag.</param>
    protected virtual void WriteIndexes(IFileWriter writer, Class classe, IEnumerable<IProperty> properties, string tag)
    {
        foreach (
            var property in properties.Where(p =>
                p.Association != null && (p.Class.PrimaryKey.Count() != 1 || p.Class.PrimaryKey.Single() != p)
            )
        )
        {
            writer.WriteLine();
            writer.WriteLine(
                $"/* Index on foreign key column for {Config.GetSqlName(classe, tag)}.{Config.GetSqlName(property, tag)} */"
            );
            WriteIndex(
                writer,
                Config.GetSqlIndexForeignKeyName(property, tag)!,
                Config.GetSqlName(classe, tag),
                Config.GetSqlName(property, tag)
            );
        }

        foreach (var index in classe.Indexes.Where(idx => !idx.Unique))
        {
            writer.WriteLine();
            writer.WriteLine($"/* Index {Config.GetSqlName(index, tag)} on {Config.GetSqlName(classe, tag)} */");
            WriteIndex(writer, index, tag);
        }

        if (
            Config.TranslateReferences == true
            && Config.AvailableClasses.Any(c => c.Translation)
            && classe.DefaultProperty != null
            && classe.Values.Count > 0
            && classe.Enum != null
        )
        {
            var index = new IndexDefinition { Class = classe };
            index.Properties.Add(classe.DefaultProperty);
            writer.WriteLine();
            WriteIndex(writer, index, tag);
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
        var properties = Config.GetProperties(classe).ToList();

        foreach (var property in properties)
        {
            Config.WriteColumn(writer, classe, property, tag);
            if (properties[^1] != property)
            {
                writer.WriteLine(",");
            }
        }

        // Primary Key
        WritePkLine(writer, classe, properties, tag);

        // Foreign key constraints
        foreach (
            var property in properties.Where(ap =>
                (ap.Association?.IsPersistent ?? false) && Config.AvailableClasses.Contains(ap.Association)
            )
        )
        {
            writer.WriteLine(",");
            writer.Write("\t");
            WriteForeignKeyConstraint(writer, property, tag);
        }

        // Unique constraints (clés unique déclarées via "unique:" et index uniques via "indexes:")
        foreach (var index in classe.Indexes.Where(index => index.Unique))
        {
            writer.WriteLine(",");
            writer.Write("\t");
            WriteUniqueKeyConstraint(writer, index, tag);
        }
    }

    /// <summary>
    /// Ecrit la ligne de création de la PK.
    /// </summary>
    /// <param name="writer">Writer.</param>
    /// <param name="classe">Classe.</param>
    /// <param name="properties">Propriétés persistantes de la classe.</param>
    /// <param name="tag">Tag.</param>
    protected virtual void WritePkLine(IFileWriter writer, Class classe, IEnumerable<IProperty> properties, string tag)
    {
        var pkCount = 0;

        if (!properties.Any(p => p.PrimaryKey))
        {
            return;
        }

        writer.WriteLine(",");
        writer.Write("\t");
        writer.Write(
            $"constraint {Config.GetSqlPrimaryKeyName(classe, tag)} primary key{(Config.TargetDBMS == TargetDBMS.Sqlserver ? " clustered" : "")} ("
        );

        foreach (var primaryKey in properties.Where(property => property.PrimaryKey))
        {
            ++pkCount;
            writer.Write(
                $"{Config.GetSqlName(primaryKey, tag)}{(Config.TargetDBMS == TargetDBMS.Sqlserver ? " asc" : "")}"
            );

            if (pkCount < properties.Count(property => property.PrimaryKey))
            {
                writer.Write(", ");
            }
        }

        writer.Write(")");
    }

    /// <summary>
    /// Ecrit un index unique.
    /// </summary>
    /// <param name="writer">Writer.</param>
    /// <param name="index">Index unique de la table.</param>
    /// <param name="tag">Tag.</param>
    protected virtual void WriteUniqueKeyConstraint(IFileWriter writer, IndexDefinition index, string tag)
    {
        writer.Write(
            $"constraint {Config.GetSqlName(index, tag)} unique{(Config.TargetDBMS == TargetDBMS.Sqlserver ? " nonclustered" : "")} ({string.Join(", ", index.Properties.Select(p => $"{Config.GetSqlName(p, tag)}{(Config.TargetDBMS == TargetDBMS.Sqlserver ? " asc" : "")}"))})"
        );
    }
}
