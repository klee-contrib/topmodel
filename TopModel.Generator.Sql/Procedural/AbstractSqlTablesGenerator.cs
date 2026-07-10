using Microsoft.Extensions.Logging;
using TopModel.Core.Model;
using TopModel.Core.Utils;
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

        foreach (
            var classe in CoreUtils.Sort(
                classes.OrderBy(c => c.SqlName),
                c => Config.GetProperties(c).Select(p => p.Association!).Where(p => p?.Enum == EnumMode.Enum)
            )
        )
        {
            WriteTableDeclaration(classe, writer, tag);
        }
    }

    protected virtual void WriteBooleanCheckConstraints(IFileWriter writer, IEnumerable<IProperty> properties) { }

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

    private void WriteTableDeclaration(Class classe, IFileWriter writer, string tag)
    {
        var fkPropertiesList = new List<IProperty>();

        var tableName = Config.CheckIdentifierLength(classe.SqlName);

        writer.WriteLine();
        writer.WriteLine("/**");
        writer.WriteLine("  * Création de la table " + tableName);
        writer.WriteLine(" **/");

        if (classe.Enum == EnumMode.Enum)
        {
            var valeurs = string.Join(
                ", ",
                Config
                    .GetAllValues(classe)
                    .Select(v => $"{Config.FormatValue(classe.EnumKey!, v.Value[classe.EnumKey])}")
            );
            writer.Write($"create type {classe.SqlName} as enum ({valeurs}); ");
            writer.WriteLine();
        }
        else
        {
            writer.WriteLine("create table " + tableName + " (");
            foreach (var property in Config.GetProperties(classe))
            {
                Config.WriteColumn(writer, classe, property);
                writer.Write(",");
                writer.WriteLine();

                if (property is { Association.IsPersistent: true } ap)
                {
                    fkPropertiesList.Add(ap);
                }
            }

            WriteCheckConstraints(writer, Config.GetProperties(classe));
            WritePrimaryKeyConstraint(writer, classe, Config.GetProperties(classe));
            WriteEndTableDeclaration(writer);

            Config.WriteSequence(writer, classe, tag);
        }
    }
}
