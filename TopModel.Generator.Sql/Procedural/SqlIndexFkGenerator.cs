using Microsoft.Extensions.Logging;
using TopModel.Core.Model;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Sql.Procedural;

public class SqlIndexFkGenerator(ILogger<SqlIndexFkGenerator> logger, IFileWriterProvider writerProvider)
    : ClassGroupGeneratorBase<SqlConfig>(logger, writerProvider)
{
    public override string Name => "SqlIndexFkGen";

    protected override IEnumerable<(string FileType, string FileName)> GetFileNames(Class classe, string tag)
    {
        if (classe.IsPersistent && !classe.Abstract)
        {
            yield return ("index-fk", Config.Procedural!.IndexFKFile!);
        }
    }

    protected override void HandleFile(string fileType, string fileName, string tag, IEnumerable<Class> classes)
    {
        using var writer = this.OpenSqlWriter(fileName);

        var appName = classes.First().Namespace.App;

        writer.WriteSqlFileHeader(
            appName,
            fileName.Split('/')[^1],
            "Script de création des indexes et des clef étrangères."
        );

        foreach (var fkProperty in classes.OrderBy(c => c.SqlName).SelectMany(GetForeignKeys))
        {
            if (fkProperty.Class.PrimaryKey.Count() != 1 || fkProperty.Class.PrimaryKey.Single() != fkProperty)
            {
                GenerateIndexForeignKey(fkProperty, writer);
            }
            GenerateConstraintForeignKey(fkProperty, writer);
        }

        foreach (var classe in classes.Where(c => c.Indexes.Count > 0).OrderBy(c => c.SqlName))
        {
            foreach (var index in classe.Indexes.Where(i => !i.Unique))
            {
                GenerateCustomIndex(classe, index, writer);
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
                GenerateIndexForeignKey(fkProperty, writer);
            }
        }
    }

    /// <summary>
    /// Génère la contrainte de clef étrangère.
    /// </summary>
    /// <param name="propertySource">Propriété portant la clef étrangère.</param>
    /// <param name="propertyTarget">Propriété destination de la contrainte.</param>
    /// <param name="association">Association destination de la clef étrangère.</param>
    /// <param name="writer">Flux d'écriture.</param>
    private void GenerateConstraintForeignKey(
        IProperty propertySource,
        IProperty propertyTarget,
        Class association,
        IFileWriter writer
    )
    {
        var tableName = propertySource.Class.SqlName;
        var propertyName = propertySource.SqlName;
        writer.WriteLine();
        writer.WriteLine("/**");
        writer.WriteLine("  * Génération de la contrainte de clef étrangère pour " + tableName + "." + propertyName);
        writer.WriteLine(" **/");
        writer.WriteLine("alter table " + tableName);
        var constraintName = Config.GetForeignKeyConstraintName(tableName, propertySource.Class.Trigram, propertyName);

        writer.WriteLine("\tadd constraint " + constraintName + " foreign key (" + propertyName + ")");
        writer.Write("\t\treferences " + association.SqlName + " (");

        writer.Write(propertyTarget.SqlName);

        writer.WriteLine($"){Config.BatchSeparator}");
    }

    /// <summary>
    /// Génère la contrainte de clef étrangère.
    /// </summary>
    /// <param name="property">Propriété portant la clef étrangère.</param>
    /// <param name="writer">Flux d'écriture.</param>
    private void GenerateConstraintForeignKey(IProperty property, IFileWriter writer)
    {
        GenerateConstraintForeignKey(property, property.AssociationProperty!, property.Association!, writer);
    }

    /// <summary>
    /// Génère un index personnalisé défini dans le modèle.
    /// </summary>
    private void GenerateCustomIndex(Class classe, IndexDefinition index, IFileWriter writer)
    {
        var tableName = classe.SqlName;
        var trigram = classe.Trigram ?? classe.SqlName;
        var columnNames = index.Properties.Select(c => c.SqlName).ToList();
        var columnList = string.Join('_', columnNames);
        var indexName = (index.Unique ? "UK_" : "IDX_") + trigram + '_' + columnList;

        writer.WriteLine();
        writer.WriteLine("/**");
        writer.WriteLine($"  * Création de l'index {indexName} sur {tableName}.");
        writer.WriteLine(" **/");

        if (index.Unique)
        {
            writer.WriteLine(
                $"alter table {tableName} add constraint {indexName} unique ({string.Join(", ", columnNames)}){Config.BatchSeparator}"
            );
        }
        else
        {
            writer.WriteLine($"create index {indexName} on {tableName} (");
            writer.WriteLine($"\t{string.Join(", ", columnNames.Select(c => c + " ASC"))}");
            writer.WriteLine($"){GetIndexTablespaceDeclaration()}{Config.BatchSeparator}");
        }
    }

    /// <summary>
    /// Génère l'index portant sur la clef étrangère.
    /// </summary>
    /// <param name="property">Propriété cible de l'index.</param>
    /// <param name="writer">Flux d'écriture.</param>
    private void GenerateIndexForeignKey(IProperty property, IFileWriter writer)
    {
        var tableName = property.Class.SqlName;
        var propertyName = property.SqlName;
        writer.WriteLine();
        writer.WriteLine("/**");
        writer.WriteLine("  * Création de l'index de clef étrangère pour " + tableName + "." + propertyName);
        writer.WriteLine(" **/");
        writer.WriteLine(
            "create index "
                + "IDX_"
                + (property.Class.Trigram ?? property.Class.SqlName)
                + "_"
                + propertyName
                + "_FK"
                + " on "
                + tableName
                + " ("
        );
        writer.WriteLine("\t" + propertyName + " ASC");
        writer.WriteLine($"){GetIndexTablespaceDeclaration()}{Config.BatchSeparator}");
    }

    private IEnumerable<IProperty> GetForeignKeys(Class classe)
    {
        var properties = classe.GetAllProperties(Config.Classes);

        foreach (var property in properties)
        {
            if (property is { Association.IsPersistent: true } ap)
            {
                yield return ap;
            }
        }
    }

    private string GetIndexTablespaceDeclaration() => GetTablespaceDeclaration(Config.IndexTablespace);

    private string GetTablespaceDeclaration(string? tablespace)
    {
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
}
