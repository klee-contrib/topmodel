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
        if (classe.IsPersistent && classe.Type != ClassType.Interface)
        {
            yield return (
                "index-and-keys",
                Path.Combine(Config.OutputDirectory, Config.Procedural!.IndexesAndKeysFileName)
            );
        }
    }

    protected virtual IEnumerable<IProperty> GetForeignKeys(Class classe)
    {
        foreach (var property in classe.AllProperties)
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
                WriteForeignKeyIndex(fkProperty, writer);
            }

            WriteForeignKeyConstraint(fkProperty, writer);
        }

        foreach (var classe in classes.Where(c => c.Indexes.Count > 0).OrderBy(c => c.SqlName))
        {
            foreach (var index in classe.Indexes)
            {
                WriteIndex(index, writer);
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
                WriteIndex(index, writer);
            }
        }
    }

    /// <summary>
    /// Génère la contrainte de clef étrangère.
    /// </summary>
    /// <param name="property">Propriété portant la clef étrangère.</param>
    /// <param name="writer">Flux d'écriture.</param>
    protected virtual void WriteForeignKeyConstraint(IProperty propertySource, IFileWriter writer)
    {
        var propertyTarget = propertySource.AssociationProperty!;
        var association = propertySource.Association!;

        if (!Config.AvailableClasses.Contains(association))
        {
            return;
        }

        writer.WriteLine();
        writer.WriteLine("/**");
        writer.WriteLine(
            "  * Génération de la contrainte de clef étrangère pour "
                + propertySource.Class.SqlName
                + "."
                + propertySource.SqlName
        );
        writer.WriteLine(" **/");
        writer.WriteLine("alter table " + propertySource.Class.SqlName);

        writer.WriteLine($"\tadd constraint {propertySource.ForeignKeyName} foreign key ({propertySource.SqlName})");
        writer.Write("\t\treferences " + association.SqlName + " (");

        writer.Write(propertyTarget.SqlName);

        writer.WriteLine($"){Config.BatchSeparator}");
    }

    /// <summary>
    /// Génère l'index portant sur la clef étrangère.
    /// </summary>
    /// <param name="property">Propriété cible de l'index.</param>
    /// <param name="writer">Flux d'écriture.</param>
    protected virtual void WriteForeignKeyIndex(IProperty property, IFileWriter writer)
    {
        var tableName = property.Class.SqlName;
        var propertyName = property.SqlName;
        writer.WriteLine();
        writer.WriteLine("/**");
        writer.WriteLine("  * Création de l'index de clef étrangère pour " + tableName + "." + propertyName);
        writer.WriteLine(" **/");
        writer.WriteLine($"create index {property.IndexForeignKeyName} on {tableName} (");
        writer.WriteLine("\t" + propertyName + " ASC");
        writer.WriteLine($"){GetTablespaceDeclaration()}{Config.BatchSeparator}");
    }

    /// <summary>
    /// Génère un index défini dans le modèle.
    /// </summary>
    protected virtual void WriteIndex(IndexDefinition index, IFileWriter writer)
    {
        var tableName = index.Class.SqlName;

        writer.WriteLine();
        writer.WriteLine("/**");
        writer.WriteLine($"  * Création de l'index {index.SqlName} sur {tableName}.");
        writer.WriteLine(" **/");

        if (index.Unique)
        {
            writer.WriteLine(
                $"alter table {tableName} add constraint {index.SqlName} unique ({string.Join(", ", index.Properties.Select(c => c.SqlName))}){Config.BatchSeparator}"
            );
        }
        else
        {
            writer.WriteLine($"create index {index.SqlName} on {tableName} (");
            writer.WriteLine($"\t{string.Join(", ", index.Properties.Select(c => $"{c.SqlName} ASC"))}");
            writer.WriteLine($"){GetTablespaceDeclaration()}{Config.BatchSeparator}");
        }
    }
}
