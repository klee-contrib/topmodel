using Microsoft.Extensions.Logging;
using TopModel.Core.Model;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Sql.Procedural;

public abstract class AbstractSqlTablesGenerator(
    ILogger<AbstractSqlTablesGenerator> logger,
    IFileWriterProvider writerProvider
) : ClassGroupGeneratorBase<SqlConfig>(logger, writerProvider)
{
    public override string Name => "SqlTablesGen";

    /// <summary>
    /// Type json pour les compositions.
    /// </summary>
    protected virtual string JsonType => "json";

    /// <summary>
    /// Indique si le moteur de BDD visé supporte "primary key clustered ()".
    /// </summary>
    protected abstract bool SupportsClusteredKey { get; }

    protected override IEnumerable<(string FileType, string FileName)> GetFileNames(Class classe, string tag)
    {
        if (classe.HasTable)
        {
            yield return ("tables", Config.Procedural!.TablesFileName);
        }
    }

    protected override void HandleFile(string fileType, string fileName, string tag, IEnumerable<Class> classes)
    {
        using var writer = this.OpenSqlWriter(fileName);

        var appName = classes.First().Namespace.App;

        writer.WriteSqlFileHeader(appName, fileName.Split('/')[^1], "Script de création des tables.");

        foreach (var classe in classes.OrderBy(c => c.SqlName))
        {
            WriteTableDeclaration(classe, writer);
        }
    }

    protected virtual void WriteBooleanCheckConstraints(IFileWriter writer, IEnumerable<IProperty> properties) { }

    /// <summary>
    /// Gère l'auto-incrémentation des clés primaires.
    /// </summary>
    /// <param name="writer">Flux d'écriture création bases.</param>
    protected abstract void WriteIdentityColumn(IFileWriter writer);

    protected virtual void WriteSequenceDeclaration(Class classe, IFileWriter writer)
    {
        writer.Write(
            $"create sequence {Config.GetSequenceName(classe)} as {Config.GetType(classe.PrimaryKey.Single()).ToUpper()} start with {Config.Procedural?.Identity.Start ?? 1} increment by {Config.Procedural?.Identity.Increment ?? 1}"
        );
    }

    private string GetTableTablespaceDeclaration() => GetTablespaceDeclaration(Config.TableTablespace);

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

    /// <summary>
    /// Ecrit les contraintes de check.
    /// </summary>
    /// <param name="writer">Flux crebas.</param>
    /// <param name="properties">Liste des propriétés persistantes.</param>
    private void WriteCheckConstraints(IFileWriter writer, IEnumerable<IProperty> properties)
    {
        WriteBooleanCheckConstraints(writer, properties);
    }

    /// <summary>
    /// Ajoute la fin de la déclaration de la table.
    /// </summary>
    /// <param name="writer">Flux d'écriture crebas.</param>
    private void WriteEndTableDeclaration(IFileWriter writer)
    {
        writer.WriteLine($"){GetTableTablespaceDeclaration()}{Config.BatchSeparator}");
    }

    /// <summary>
    /// Ajoute les contraintes de clés primaires.
    /// </summary>
    /// <param name="writer">Writer.</param>
    /// <param name="classe">Classe.</param>
    private void WritePrimaryKeyConstraint(IFileWriter writer, Class classe, IEnumerable<IProperty> properties)
    {
        if (!properties.Any(p => p.PrimaryKey))
        {
            return;
        }

        writer.Write("\tconstraint " + "PK_" + classe.SqlName + " primary key ");
        if (SupportsClusteredKey)
        {
            writer.Write("clustered ");
        }

        writer.WriteLine($"({string.Join(',', properties.Where(p => p.PrimaryKey).Select(pk => pk.SqlName))})");
    }

    private void WriteSequence(Class classe, IFileWriter writer)
    {
        writer.WriteLine();
        writer.WriteLine("/**");
        writer.WriteLine($"  * Création de la séquence pour la clé primaire de la table {classe.SqlName}");
        writer.WriteLine(" **/");

        WriteSequenceDeclaration(classe, writer);

        writer.WriteLine(Config.BatchSeparator);
    }

    private void WriteTableDeclaration(Class classe, IFileWriter writer)
    {
        var fkPropertiesList = new List<IProperty>();

        var tableName = Config.CheckIdentifierLength(classe.SqlName);

        writer.WriteLine();
        writer.WriteLine("/**");
        writer.WriteLine("  * Création de la table " + tableName);
        writer.WriteLine(" **/");
        writer.WriteLine("create table " + tableName + " (");

        foreach (var property in Config.GetAllProperties(classe))
        {
            var persistentType =
                property is { Composition: null, Domain: not null } ? Config.GetType(property)
                : property is { Composition: null, Domain: null } ? $"varchar({Config.IdentifierLengthLimit})"
                : JsonType;

            if (persistentType.ToLower().Equals("varchar") && property.Domain?.Length != null)
            {
                persistentType = $"{persistentType}({property.Domain.Length})";
            }

            if (
                (persistentType.ToLower().Equals("numeric") || persistentType.ToLower().Equals("decimal"))
                && property.Domain?.Length != null
            )
            {
                persistentType =
                    $"{persistentType}({property.Domain.Length}{(property.Domain.Scale != null ? $", {property.Domain.Scale}" : string.Empty)})";
            }

            writer.Write("\t" + Config.CheckIdentifierLength(property.SqlName) + " " + persistentType);
            if (
                property.Association == null
                && property.PrimaryKey
                && property.GeneratedValue != null
                && persistentType.Contains("int")
                && Config.Procedural!.Identity.Mode == IdentityMode.IDENTITY
                && (classe.Extends == null || classe.Extends.InheritanceStrategy != InheritanceStrategy.DistinctTables)
            )
            {
                WriteIdentityColumn(writer);
            }

            if (property.Required && (property.Class == classe || property.Class.Extends != classe))
            {
                writer.Write(" not null");
            }

            var defaultValue = Config.GetValue(property);
            if (defaultValue != "null")
            {
                writer.Write($" default {defaultValue}");
            }

            writer.Write(",");
            writer.WriteLine();

            if (property is { Association.IsPersistent: true } ap)
            {
                fkPropertiesList.Add(ap);
            }
        }

        WriteCheckConstraints(writer, Config.GetAllProperties(classe));
        WritePrimaryKeyConstraint(writer, classe, Config.GetAllProperties(classe));
        WriteEndTableDeclaration(writer);

        var classeForSequence =
            classe.Extends?.InheritanceStrategy != InheritanceStrategy.DistinctTables ? classe
            : classe.Extends.Type == ClassType.Abstract
            && Config.Classes.Where(c => c.Extends == classe.Extends).OrderBy(c => c.SqlName).First() == classe
                ? classe.Extends
            : null;

        if (classeForSequence != null && Config.UsesSequence(classeForSequence))
        {
            WriteSequence(classeForSequence, writer);
        }
    }
}
