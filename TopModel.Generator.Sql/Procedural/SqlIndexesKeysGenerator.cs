using Microsoft.Extensions.Logging;
using TopModel.Core.Model;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Sql.Procedural;

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

    protected virtual string GetTablespaceDeclaration(string? tablespace = null)
    {
        tablespace ??= Config.IndexTablespace;

        bool ShouldGenerateTablespace()
        {
            if (!Config.AllowTablespace)
            {
                return false;
            }

            if (string.IsNullOrEmpty(tablespace))
            {
                return false;
            }

            return true;
        }

        if (ShouldGenerateTablespace())
        {
            return $"\r\nTABLESPACE  {tablespace} ";
        }

        return string.Empty;
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
    /// Génère la contrainte de clef étrangère.
    /// </summary>
    /// <param name="property">Propriété portant la clef étrangère.</param>
    /// <param name="writer">Flux d'écriture.</param>
    protected virtual void WriteForeignKeyConstraint(IProperty propertySource, IFileWriter writer, string tag)
    {
        var propertyTarget = propertySource.AssociationProperty!;
        var association = propertySource.Association!;

        if (!Config.AvailableClasses.Contains(association) || association.Enum == EnumMode.Enum)
        {
            return;
        }

        writer.WriteLine();
        writer.WriteLine("/**");
        writer.WriteLine(
            "  * Génération de la contrainte de clef étrangère pour "
                + Config.GetSqlName(propertySource.Class, tag, noQuote: true)
                + "."
                + Config.GetSqlName(propertySource, tag, noQuote: true)
        );
        writer.WriteLine(" **/");
        writer.WriteLine("alter table " + Config.GetSqlName(propertySource.Class, tag));

        writer.WriteLine(
            $"\tadd constraint {Config.GetSqlForeignKeyName(propertySource, tag)} foreign key ({Config.GetSqlName(propertySource, tag)})"
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

        writer.Write(Config.GetSqlName(propertyTarget, tag));

        writer.WriteLine($"){Config.BatchSeparator}");
    }

    /// <summary>
    /// Génère l'index portant sur la clef étrangère.
    /// </summary>
    /// <param name="property">Propriété cible de l'index.</param>
    /// <param name="writer">Flux d'écriture.</param>
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
        writer.WriteLine($"){GetTablespaceDeclaration()}{Config.BatchSeparator}");
    }

    /// <summary>
    /// Génère un index défini dans le modèle.
    /// </summary>
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
            writer.WriteLine($"){GetTablespaceDeclaration()}{Config.BatchSeparator}");
        }
    }
}
