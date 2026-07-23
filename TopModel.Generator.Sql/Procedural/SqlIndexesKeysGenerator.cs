using Microsoft.Extensions.Logging;
using TopModel.Core.Model;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Sql.Procedural;

/// <summary>
/// Générateur SQL procédural pour les index, les clés étrangères et uniques.
/// </summary>
public class SqlIndexesKeysGenerator(ILogger<SqlIndexesKeysGenerator> logger, IFileWriterProvider writerProvider)
    : ClassGroupGeneratorBase<SqlConfig>(logger, writerProvider)
{
    public override string Name => "SqlIndexesKeysGen";

    protected override IEnumerable<(string FileType, string FileName)> GetFileNames(Class classe, string tag)
    {
        if (classe.HasTable && classe.Enum != EnumMode.Enum)
        {
            yield return (
                "index-and-keys",
                Path.Combine(Config.OutputDirectory, Config.Procedural!.IndexesAndKeysFileName)
            );
        }
    }

    /// <summary>
    /// Retourne les propriétés portant une clé étrangère.
    /// </summary>
    /// <param name="classe">Classe.</param>
    /// <returns>Propriétés de clé étrangère.</returns>
    protected virtual IEnumerable<IProperty> GetForeignKeys(Class classe)
    {
        foreach (var property in Config.GetProperties(classe))
        {
            if (property is { Association.IsPersistent: true } ap)
            {
                yield return ap;
            }
        }
    }

    protected override void HandleFile(string fileType, string fileName, string tag, IEnumerable<Class> classes)
    {
        using var writer = this.OpenSqlWriter(fileName);

        var appName = classes.First().Namespace.App;

        writer.WriteSqlFileHeader(
            appName,
            fileName.Split('/')[^1],
            "Script de création des indexes et des clés étrangères et uniques."
        );

        foreach (var fkProperty in classes.OrderBy(c => c.SqlName).SelectMany(GetForeignKeys))
        {
            if (fkProperty.Class.PrimaryKey.Count() != 1 || fkProperty.Class.PrimaryKey.Single() != fkProperty)
            {
                WriteForeignKeyIndex(fkProperty, writer, tag);
            }

            WriteForeignKeyConstraint(fkProperty, writer, tag);
        }

        foreach (var classe in classes.Where(c => c.Indexes.Count > 0).OrderBy(c => c.SqlName))
        {
            foreach (var index in classe.Indexes)
            {
                WriteIndex(index, writer, tag);
            }
        }

        if (Config.TranslateReferences == true && Config.AvailableClasses.Any(c => c.Translation))
        {
            var resourceProperties = classes
                .Where(c => c.DefaultProperty != null && c.Values.Count > 0 && c.Enum != null)
                .OrderBy(c => c.SqlName)
                .Select(c => c.DefaultProperty!);

            foreach (var fkProperty in resourceProperties)
            {
                var index = new IndexDefinition { Class = fkProperty.Class };
                index.Properties.Add(fkProperty);
                WriteIndex(index, writer, tag);
            }
        }
    }

    /// <summary>
    /// Ecrit la contrainte de clé étrangère.
    /// </summary>
    /// <param name="sourceProperty">Propriété portant la clé étrangère.</param>
    /// <param name="writer">Writer.</param>
    /// <param name="tag">Tag.</param>
    protected virtual void WriteForeignKeyConstraint(IProperty sourceProperty, IFileWriter writer, string tag)
    {
        var targetProperty = sourceProperty.AssociationProperty!;
        var association = sourceProperty.Association!;

        if (!Config.AvailableClasses.Contains(association) || association.Enum == EnumMode.Enum)
        {
            return;
        }

        writer.WriteLine();
        writer.WriteLine("/**");
        writer.WriteLine(
            "  * Génération de la contrainte de clef étrangère pour "
                + Config.GetSqlName(sourceProperty.Class, tag, noQuote: true)
                + "."
                + Config.GetSqlName(sourceProperty, tag, noQuote: true)
        );
        writer.WriteLine(" **/");
        writer.WriteLine("alter table " + Config.GetSqlName(sourceProperty.Class, tag));

        writer.WriteLine(
            $"\tadd constraint {Config.GetSqlForeignKeyName(sourceProperty, tag)} foreign key ({Config.GetSqlName(sourceProperty, tag)})"
        );
        writer.Write(
            "\t\treferences "
                + Config.GetSqlName(
                    association.Extends?.InheritanceStrategy == InheritanceStrategy.SingleTable
                        ? association.Extends
                        : association,
                    tag
                )
                + " ("
        );

        writer.Write(Config.GetSqlName(targetProperty, tag));

        writer.WriteLine($"){Config.BatchSeparator}");
    }

    /// <summary>
    /// Ecrit l'index associé à la clé étrangère.
    /// </summary>
    /// <param name="property">Propriété portant la clé étrangère.</param>
    /// <param name="writer">Writer.</param>
    /// <param name="tag">Tag.</param>
    protected virtual void WriteForeignKeyIndex(IProperty property, IFileWriter writer, string tag)
    {
        writer.WriteLine();
        writer.WriteLine("/**");
        writer.WriteLine(
            "  * Création de l'index de clef étrangère pour "
                + Config.GetSqlName(property.Class, tag, noQuote: true)
                + "."
                + Config.GetSqlName(property, tag, noQuote: true)
        );
        writer.WriteLine(" **/");
        writer.WriteLine(
            $"create index {Config.GetSqlIndexForeignKeyName(property, tag)} on {Config.GetSqlName(property.Class, tag)} ("
        );
        writer.WriteLine("\t" + Config.GetSqlName(property, tag) + " ASC");
        writer.WriteLine($"){Config.GetTablespaceDeclaration(Config.IndexTablespace)}{Config.BatchSeparator}");
    }

    /// <summary>
    /// Ecrit un index du modèle.
    /// </summary>
    /// <param name="index">Index.</param>
    /// <param name="writer">Writer.</param>
    /// <param name="tag">Tag.</param>
    protected virtual void WriteIndex(IndexDefinition index, IFileWriter writer, string tag)
    {
        var tableName = Config.GetSqlName(index.Class, tag);

        writer.WriteLine();
        writer.WriteLine("/**");
        writer.WriteLine($"  * Création de l'index {Config.GetSqlName(index, tag)} sur {tableName}.");
        writer.WriteLine(" **/");

        if (index.Unique)
        {
            writer.WriteLine(
                $"alter table {tableName} add constraint {Config.GetSqlName(index, tag)} unique ({string.Join(", ", index.Properties.Select(p => Config.GetSqlName(p, tag)))}){Config.BatchSeparator}"
            );
        }
        else
        {
            writer.WriteLine($"create index {Config.GetSqlName(index, tag)} on {tableName} (");
            writer.WriteLine(
                $"\t{string.Join(", ", index.Properties.Select(c => $"{Config.GetSqlName(c, tag)} ASC"))}"
            );
            writer.WriteLine($"){Config.GetTablespaceDeclaration(Config.IndexTablespace)}{Config.BatchSeparator}");
        }
    }
}
