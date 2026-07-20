using Microsoft.Extensions.Logging;
using TopModel.Core.Model;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Sql.Ssdt;

/// <summary>
/// Scripter permettant d'écrire les scripts de création d'une table SQL avec :
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
        return Path.Combine(Config.Ssdt!.TableScriptFolder!, classe.SqlName + ".sql");
    }

    protected override void HandleClass(string fileName, Class classe, string tag)
    {
        using var writer = this.OpenSqlWriter(fileName);

        // Entête du fichier.
        WriteHeader(writer, classe.SqlName);

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

            return;
        }

        // Ouverture du create table.
        WriteCreateTableOpening(writer, classe);

        // Intérieur du create table.
        WriteInsideInstructions(writer, classe);

        // Fin du create table.
        WriteCreateTableClosing(writer, classe);

        Config.WriteSequence(writer, classe, tag);

        // Indexes sur les clés étrangères.
        WriteIndexes(writer, classe, Config.GetProperties(classe));

        // Définition
        if (Config.Ssdt!.GenerateComments)
        {
            WriteComments(writer, classe);
        }
    }

    /// <summary>
    /// Ecrit la création de la propriété de description de la table.
    /// </summary>
    /// <param name="writer">Writer.</param>
    /// <param name="classe">Classe de la table.</param>
    protected virtual void WriteComments(IFileWriter writer, Class classe)
    {
        writer.WriteLine();
        writer.WriteComments(classe, Config);
    }

    /// <summary>
    /// Ecrit le pied du script.
    /// </summary>
    /// <param name="writer">Flux.</param>
    /// <param name="classe">Classe de la table.</param>
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
    /// <param name="writer">Flux.</param>
    /// <param name="table">Table.</param>
    protected virtual void WriteCreateTableOpening(IFileWriter writer, Class table)
    {
        if (Config.TargetDBMS == TargetDBMS.Sqlserver)
        {
            writer.WriteLine($"create table [dbo].[{table.SqlName}] (");
        }
        else
        {
            writer.WriteLine($"create table {table.SqlName} (");
        }
    }

    /// <summary>
    /// Génère la contrainte de clef étrangère.
    /// </summary>
    /// <param name="sb">Flux d'écriture.</param>
    /// <param name="property">Propriété portant la clef étrangère.</param>
    protected virtual void WriteForeignKeyConstraint(IFileWriter writer, IProperty property)
    {
        var referenceClass = property.Association!;
        var referencedSqlName = (
            referenceClass.Extends?.InheritanceStrategy == InheritanceStrategy.SingleTable
                ? referenceClass.Extends
                : referenceClass
        ).SqlName;

        if (Config.TargetDBMS == TargetDBMS.Sqlserver)
        {
            writer.Write(
                $"constraint [{property.ForeignKeyName}] foreign key ([{property.SqlName}]) references [dbo].[{referencedSqlName}] ([{property.AssociationProperty!.SqlName}])"
            );
        }
        else
        {
            writer.Write(
                $"constraint {property.ForeignKeyName} foreign key ({property.SqlName}) references {referencedSqlName} ({property.AssociationProperty!.SqlName})"
            );
        }
    }

    /// <summary>
    /// Ecrit l'entête du fichier.
    /// </summary>
    /// <param name="writer">Flux.</param>
    /// <param name="tableName">Nom de la table.</param>
    protected virtual void WriteHeader(IFileWriter writer, string tableName)
    {
        writer.WriteSqlFileHeader(description: $"Création de la table {tableName}.");
        writer.WriteLine();
    }

    protected virtual void WriteIndex(
        IFileWriter writer,
        string indexName,
        string tableName,
        params IEnumerable<string> propertyNames
    )
    {
        if (Config.TargetDBMS == TargetDBMS.Sqlserver)
        {
            writer.WriteLine($"create nonclustered index [{indexName}]");
            writer.Write("\ton [dbo].[" + tableName + "] (");
            writer.Write(string.Join(", ", propertyNames.Select(n => $"[{n}] ASC")));
            writer.Write(")");
            writer.WriteLine();
            writer.WriteLine("go");
        }
        else
        {
            writer.WriteLine($"create index {indexName}");
            writer.Write($"\ton {tableName} (");
            writer.Write(string.Join(", ", propertyNames.Select(n => $"{n} asc")));
            writer.WriteLine(");");
        }
    }

    protected virtual void WriteIndex(IFileWriter writer, IndexDefinition index)
    {
        WriteIndex(writer, index.SqlName, index.Class.SqlName, index.Properties.Select(p => p.SqlName));
    }

    /// <summary>
    /// Génère les indexes portant sur les FK.
    /// </summary>
    /// <param name="writer">Flux d'écriture.</param>
    /// <param name="tableName">Nom de la table.</param>
    /// <param name="properties">Champs.</param>
    protected virtual void WriteIndexes(IFileWriter writer, Class classe, IEnumerable<IProperty> properties)
    {
        foreach (
            var property in properties.Where(p =>
                p.Association != null && (p.Class.PrimaryKey.Count() != 1 || p.Class.PrimaryKey.Single() != p)
            )
        )
        {
            writer.WriteLine();
            writer.WriteLine($"/* Index on foreign key column for {classe.SqlName}.{property.SqlName} */");
            WriteIndex(writer, property.IndexForeignKeyName!, classe.SqlName, property.SqlName);
        }

        foreach (var index in classe.Indexes.Where(idx => !idx.Unique))
        {
            writer.WriteLine();
            writer.WriteLine($"/* Index {index.SqlName} on {classe.SqlName} */");
            WriteIndex(writer, index);
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
            WriteIndex(writer, index);
        }
    }

    /// <summary>
    /// Ecrit les instructions à l'intérieur du create table.
    /// </summary>
    /// <param name="writer">Flux.</param>
    /// <param name="table">Table.</param>
    protected virtual void WriteInsideInstructions(IFileWriter writer, Class table)
    {
        var properties = Config.GetProperties(table).ToList();

        foreach (var property in properties)
        {
            Config.WriteColumn(writer, table, property);
            if (properties[^1] != property)
            {
                writer.WriteLine(",");
            }
        }

        // Primary Key
        WritePkLine(writer, table, properties);

        // Foreign key constraints
        foreach (
            var property in properties.Where(ap =>
                (ap.Association?.IsPersistent ?? false) && Config.AvailableClasses.Contains(ap.Association)
            )
        )
        {
            writer.WriteLine(",");
            writer.Write("\t");
            WriteForeignKeyConstraint(writer, property);
        }

        // Unique constraints (clés unique déclarées via "unique:" et index uniques via "indexes:")
        foreach (var index in table.Indexes.Where(idx => idx.Unique))
        {
            writer.WriteLine(",");
            writer.Write("\t");
            WriteUniqueKeyConstraint(writer, index);
        }
    }

    /// <summary>
    /// Ecrit la ligne de création de la PK.
    /// </summary>
    /// <param name="sb">Flux.</param>
    /// <param name="classe">Classe.</param>
    protected virtual void WritePkLine(IFileWriter writer, Class classe, IEnumerable<IProperty> properties)
    {
        var pkCount = 0;

        if (!properties.Any(p => p.PrimaryKey))
        {
            return;
        }

        writer.WriteLine(",");
        writer.Write("\t");

        if (Config.TargetDBMS == TargetDBMS.Sqlserver)
        {
            writer.Write($"constraint [PK_{classe.SqlName}] primary key clustered (");
        }
        else
        {
            writer.Write($"constraint PK_{classe.SqlName} primary key (");
        }

        foreach (var pk in properties.Where(p => p.PrimaryKey))
        {
            ++pkCount;
            if (Config.TargetDBMS == TargetDBMS.Sqlserver)
            {
                writer.Write($"[{pk.SqlName}] ASC");
            }
            else
            {
                writer.Write(pk.SqlName);
            }

            if (pkCount < properties.Count(p => p.PrimaryKey))
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
    /// <param name="idw">Index unique de la table.</param>
    protected virtual void WriteUniqueKeyConstraint(IFileWriter writer, IndexDefinition idx)
    {
        writer.Write(
            Config.TargetDBMS == TargetDBMS.Sqlserver
                ? $"constraint [{idx.SqlName}] unique nonclustered ({string.Join(", ", idx.Properties.Select(p => $"[{p.SqlName}] ASC"))})"
                : $"constraint {idx.SqlName} unique ({string.Join(", ", idx.Properties.Select(p => $"{p.SqlName}"))})"
        );
    }
}
