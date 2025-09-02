using Microsoft.Extensions.Logging;
using TopModel.Core.FileModel;
using TopModel.Core.Model;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Sql.Procedural;

public class SqlIndexFkGenerator(ILogger<SqlIndexFkGenerator> logger, IFileWriterProvider writerProvider)
    : ClassGroupGeneratorBase<SqlConfig>(logger, writerProvider)
{
    public override string Name => "SqlIndexFkGen";

    protected override bool PersistentOnly => true;

    protected override IEnumerable<Class> GetExtraClasses(ModelFile file)
    {
        return file.GetExtraClasses();
    }

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
            GenerateIndexForeignKey(fkProperty, writer);
            GenerateConstraintForeignKey(fkProperty, writer);
        }

        if (
            (Config.TranslateReferences == true || Config.TranslateProperties == true)
            && Config.ResourcesTableName != null
        )
        {
            var resourceProperties = classes
                .Where(c => c.DefaultProperty != null && c.Values.Count > 0 && c.Enum)
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
        AssociationProperty propertySource,
        IProperty propertyTarget,
        Class association,
        IFileWriter writer
    )
    {
        var tableName = propertySource.Class.SqlName;
        var propertyName = ((IProperty)propertySource).SqlName;
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
    private void GenerateConstraintForeignKey(AssociationProperty property, IFileWriter writer)
    {
        GenerateConstraintForeignKey(property, property.Property, property.Association, writer);
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

    private IEnumerable<AssociationProperty> GetForeignKeys(Class classe)
    {
        var properties = classe.GetAllProperties(Classes);

        foreach (var property in properties)
        {
            if (property is AssociationProperty { Association.IsPersistent: true } ap)
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
