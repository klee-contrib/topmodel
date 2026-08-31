#pragma warning disable KTA1200

using System.Text;
using TopModel.Core.Model;
using TopModel.Core.Utils;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Sql;

/// <summary>
/// Classe utilitaire pour écrire du SQL.
/// </summary>
public static class ScriptUtils
{
    /// <summary>
    /// Nom de la propriété "InsertKey".
    /// </summary>
    public const string InsertKeyName = "InsertKey";

    extension(Class classe)
    {
        /// <summary>
        /// Indique si on doit créer une table pour cette classe.
        /// </summary>
        public bool HasTable =>
            classe.IsPersistent
            && classe.Type != ClassType.Interface
            && (classe.Type != ClassType.Abstract || classe.InheritanceStrategy != InheritanceStrategy.DistinctTables)
            && (classe.Extends == null || classe.Extends.InheritanceStrategy != InheritanceStrategy.SingleTable);
    }

    /// <summary>
    /// Retourne le type SQL d'une colonne.
    /// </summary>
    /// <param name="config">Config.</param>
    /// <param name="property">Propriété correspondant à la colonne.</param>
    /// <returns>Type SQL de la colonne.</returns>
    public static string GetColumnType(this SqlConfig config, IProperty property)
    {
        return property switch
        {
            { Composition: null, Domain: not null } => config.GetType(property),
            { Composition: null, Domain: null } => $"varchar({config.SqlIdentifierLengthLimit})",
            _ => config.JsonType,
        };
    }

    /// <summary>
    /// Récupère la déclaration SQL du tablespace.
    /// </summary>
    /// <param name="config">Config.</param>
    /// <param name="tablespace">Nom du tablespace.</param>
    /// <returns>Déclaration SQL du tablespace.</returns>
    public static string GetTablespaceDeclaration(this SqlConfig config, string? tablespace)
    {
        if (!config.AllowTablespace || string.IsNullOrEmpty(tablespace))
        {
            return string.Empty;
        }

        return $" tablespace {tablespace}";
    }

    /// <summary>
    /// Indique si une classe doit générer des scripts d'insertion de valeurs.
    /// </summary>
    /// <param name="config">Config.</param>
    /// <param name="classe">Classe à tester.</param>
    /// <returns><see langword="true" /> si la classe porte des valeurs initiales à insérer.</returns>
    public static bool HasValues(this SqlConfig config, Class classe)
    {
        return classe.HasTable && classe.Values.Count > 0 && classe.Enum != EnumMode.Enum
            || (
                classe.InheritanceStrategy != InheritanceStrategy.DistinctTables
                && config.Classes.Any(c => c.Extends == classe && c.Values.Count > 0)
            );
    }

    /// <summary>
    /// Ouvre un fichier SQL pour écrire.
    /// </summary>
    /// <param name="generator">Générateur source.</param>
    /// <param name="fileName">Nom du fichier SQL.</param>
    /// <returns>Writer.</returns>
    public static IFileWriter OpenSqlWriter(this GeneratorBase<SqlConfig> generator, string fileName)
    {
        var writer = generator.OpenFileWriter(fileName);
        writer.StartCommentToken = "----";
        return writer;
    }

    /// <summary>
    /// Trie les classes dans l'ordre d'insertion attendu par leurs dépendances.
    /// </summary>
    /// <param name="classes">Classes à trier.</param>
    /// <param name="config">Config.</param>
    /// <returns>Classes triées pour l'insertion.</returns>
    public static IEnumerable<Class> SortInserts(this IEnumerable<Class> classes, SqlConfig config)
    {
        return CoreUtils.Sort(
            classes.OrderBy(c => c.SqlName),
            c =>
                config
                    .GetProperties(c)
                    .SelectMany(a => new[] { a.Association!, a.Association?.Extends! })
                    .Where(a => a != null && a != c && classes.Contains(a))
                    .Concat(c.Extends != null && c.Extends.Values.Count > 0 ? [c.Extends] : [])
        );
    }

    /// <summary>
    /// Ecrit la déclaration SQL d'une colonne.
    /// </summary>
    /// <param name="config">Config.</param>
    /// <param name="writer">Writer.</param>
    /// <param name="classe">Classe contenant la colonne à écrire.</param>
    /// <param name="property">Propriété correspondant à la colonne à écrire.</param>
    /// <param name="tag">Tag.</param>
    public static void WriteColumn(
        this SqlConfig config,
        IFileWriter writer,
        Class classe,
        IProperty property,
        string tag
    )
    {
        var type = config.GetColumnType(property);

        writer.Write("\t");
        writer.Write(config.GetSqlName(property, tag));
        writer.Write($" {type}");

        if (property.Required && !property.PrimaryKey && (property.Class == classe || property.Class.Extends != classe))
        {
            writer.Write(" not null");
        }

        if (
            property.Association == null
            && property.PrimaryKey
            && property.GeneratedValue != null
            && property.GeneratedValue.SequenceName == null
            && config.GetType(property).Contains("int")
            && (config.Ssdt == null || !config.Ssdt.DisableIdentity)
            && (classe.Extends == null || classe.Extends.InheritanceStrategy != InheritanceStrategy.DistinctTables)
        )
        {
            if (config.TargetDBMS == TargetDBMS.Sqlserver)
            {
                writer.Write($" identity");

                if (property.GeneratedValue.Start != 1 || property.GeneratedValue.Increment != 1)
                {
                    writer.Write($"({property.GeneratedValue.Start}, {property.GeneratedValue.Increment})");
                }
            }
            else
            {
                writer.Write(" generated by default as identity");
                if (property.GeneratedValue.Start != 1 || property.GeneratedValue.Increment != 1)
                {
                    writer.Write("(");
                    if (property.GeneratedValue.Start != 1)
                    {
                        writer.Write($"start with {property.GeneratedValue.Start}");
                    }

                    if (property.GeneratedValue.Increment != 1)
                    {
                        writer.Write(
                            $"{(property.GeneratedValue.Start != 1 ? " " : string.Empty)}increment by {property.GeneratedValue.Increment}"
                        );
                    }

                    writer.Write(")");
                }
            }
        }

        var defaultValue = config.GetValue(property);
        if (defaultValue != "null")
        {
            writer.Write($" default {defaultValue}");
        }
    }

    /// <summary>
    /// Ecrit les commentaires SQL de la table et de ses colonnes.
    /// </summary>
    /// <param name="writer">Writer.</param>
    /// <param name="classe">Classe dont écrire les commentaires SQL.</param>
    /// <param name="config">Config.</param>
    /// <param name="tag">Tag.</param>
    public static void WriteComments(this IFileWriter writer, Class classe, SqlConfig config, string tag)
    {
        writer.WriteLine("/**");
        writer.WriteLine($"  * Commentaires pour la table {config.GetSqlName(classe, tag, noQuote: true)}");
        writer.WriteLine(" **/");

        if (config.TargetDBMS == TargetDBMS.Sqlserver)
        {
            writer.WriteLine(
                $"execute sp_addextendedproperty 'MS_Description', '{classe.Comment.Replace("'", "''")}', 'SCHEMA', 'dbo', 'TABLE', '{config.GetSqlName(classe, tag, noQuote: true)}'"
            );
            writer.WriteLine("go");

            foreach (var property in config.GetProperties(classe))
            {
                writer.WriteLine(
                    $"execute sp_addextendedproperty 'MS_Description', '{property.Comment.Replace("'", "''")}', 'SCHEMA', 'dbo', 'TABLE', '{config.GetSqlName(classe, tag, noQuote: true)}', 'COLUMN', '{config.GetSqlName(property, tag, noQuote: true)}'"
                );
                writer.WriteLine("go");
            }
        }
        else
        {
            writer.WriteLine(
                $"comment on table {config.GetSqlName(classe, tag)} is '{classe.Comment.Replace("'", "''")}'{config.BatchSeparator}"
            );

            foreach (var property in config.GetProperties(classe))
            {
                writer.WriteLine(
                    $"comment on column {config.GetSqlName(classe, tag)}.{config.GetSqlName(property, tag)} is '{property.Comment.Replace("'", "''")}'{config.BatchSeparator}"
                );
            }
        }
    }

    /// <summary>
    /// Ecrit la déclaration SQL d'un type enum.
    /// </summary>
    /// <param name="config">Config.</param>
    /// <param name="writer">Writer.</param>
    /// <param name="classe">Classe enum à écrire.</param>
    /// <param name="tag">Tag.</param>
    public static void WriteEnumType(this SqlConfig config, IFileWriter writer, Class classe, string tag)
    {
        var values = string.Join(
            ", ",
            config
                .GetAllValues(classe)
                .Select(value => $"{config.FormatValue(classe.EnumKey!, value.Value[classe.EnumKey!])}")
        );

        writer.Write($"create type {config.GetSqlName(classe, tag)} as enum ({values}); ");
        writer.WriteLine();
    }

    /// <summary>
    /// Ecrit les lignes d'insertion des valeurs initiales d'une classe.
    /// </summary>
    /// <param name="config">Config.</param>
    /// <param name="writer">Writer.</param>
    /// <param name="classe">Classe contenant les valeurs à insérer.</param>
    /// <param name="tag">Tag.</param>
    public static void WriteInsertLines(this SqlConfig config, IFileWriter writer, Class classe, string tag)
    {
        foreach (var refValue in config.GetAllValues(classe))
        {
            var properties = new Dictionary<string, string?>();
            foreach (var property in config.GetProperties(classe))
            {
                var insertValue = GetInsertValue(config, classe, property, refValue, tag);
                if (insertValue != null)
                {
                    properties[config.GetSqlName(property, tag)] = insertValue;
                }
            }

            // Création de la requête.
            var sb = new StringBuilder();
            sb.Append("insert into ").Append(config.GetSqlName(classe, tag)).Append('(');
            var isFirst = true;
            foreach (var columnName in properties.Keys)
            {
                if (!isFirst)
                {
                    sb.Append(", ");
                }

                isFirst = false;
                sb.Append(columnName);
            }

            sb.Append(") values(");

            isFirst = true;
            foreach (var value in properties.Values)
            {
                if (!isFirst)
                {
                    sb.Append(", ");
                }

                isFirst = false;
                sb.Append(value);
            }

            sb.Append(");");

            writer.WriteLine(sb.ToString());
        }
    }

    /// <summary>
    /// Ecrit la création de la séquence associée à la clé primaire d'une table, si nécessaire.
    /// </summary>
    /// <param name="config">Config.</param>
    /// <param name="writer">Writer.</param>
    /// <param name="classe">Classe pour laquelle créer la séquence.</param>
    /// <param name="tag">Tag.</param>
    public static void WriteSequence(this SqlConfig config, IFileWriter writer, Class classe, string tag)
    {
        var classeForSequence =
            classe.Extends?.InheritanceStrategy != InheritanceStrategy.DistinctTables ? classe
            : classe.Extends.Type == ClassType.Abstract
            && config.Classes.Where(c => classe.Extends == c.Extends).OrderBy(c => c.SqlName).First() == classe
                ? classe.Extends
            : null;

        if (classeForSequence != null)
        {
            var primaryKey = classeForSequence.PrimaryKey.FirstOrDefault();
            if (classeForSequence.PrimaryKey.Count() > 1 || primaryKey?.Domain.GeneratedValue?.SequenceName == null)
            {
                return;
            }

            writer.WriteLine();
            writer.WriteLine("/**");
            writer.WriteLine(
                $"  * Création de la séquence pour la clé primaire de la table {config.GetSqlName(classeForSequence, tag, noQuote: true)}"
            );
            writer.WriteLine(" **/");

            writer.Write($"create sequence {config.GetSequenceName(primaryKey, tag)}");

            if (config.TargetDBMS != TargetDBMS.Oracle)
            {
                writer.Write($" as {config.GetType(primaryKey)} ");
            }

            writer.Write(
                $"start with {primaryKey.Domain.GeneratedValue!.Start} increment by {primaryKey.Domain.GeneratedValue!.Increment}"
            );

            if (config.TargetDBMS == TargetDBMS.Oracle)
            {
                writer.Write("nocycle");
            }

            if (classeForSequence.HasTable && config.TargetDBMS == TargetDBMS.Postgre)
            {
                writer.Write(
                    $" owned by {config.GetSqlName(classeForSequence, tag)}.{config.GetSqlName(primaryKey, tag)}"
                );
            }

            writer.WriteLine(config.BatchSeparator);
        }
    }

    /// <summary>
    /// Ecrit l'en-tête d'un fichier SQL.
    /// </summary>
    /// <param name="writer">Writer.</param>
    /// <param name="appName">Nom de l'application.</param>
    /// <param name="scriptName">Nom du script.</param>
    /// <param name="description">Description du script.</param>
    public static void WriteSqlFileHeader(
        this IFileWriter writer,
        string? appName = null,
        string? scriptName = null,
        string? description = null
    )
    {
        writer.WriteLine(
            "-- ==========================================================================================="
        );
        if (appName != null)
        {
            writer.WriteLine($"--   Application Name\t:\t{appName} ");
        }

        if (scriptName != null)
        {
            writer.WriteLine($"--   Script Name\t\t:\t{scriptName}");
        }

        if (description != null)
        {
            writer.WriteLine($"--   Description\t\t:\t{description}");
        }

        writer.WriteLine(
            "-- ==========================================================================================="
        );
    }

    /// <summary>
    /// Retourne la valeur SQL à utiliser dans une ligne d'insertion.
    /// </summary>
    /// <param name="config">Config.</param>
    /// <param name="classe">Classe contenant la valeur.</param>
    /// <param name="property">Propriété à insérer.</param>
    /// <param name="refValue">Valeur de classe à insérer.</param>
    /// <param name="tag">Tag.</param>
    /// <returns>Valeur SQL, ou <see langword="null" /> si la propriété ne doit pas être insérée.</returns>
    private static string? GetInsertValue(
        SqlConfig config,
        Class classe,
        IProperty property,
        ClassValue refValue,
        string tag
    )
    {
        if (
            refValue.Value.TryGetValue(
                classe.Extends != null && property.Association == classe.Extends
                    ? classe.Extends.PrimaryKey.Single()
                    : property,
                out var value
            )
        )
        {
            if (config.TranslateReferences == true && classe.DefaultProperty == property)
            {
                return $@"{(config.TargetDBMS == TargetDBMS.Sqlserver ? "N" : string.Empty)}'{refValue.ResourceKey}'";
            }

            return config.GetValue(property, value);
        }

        if (property.GeneratedValue != null && property.PrimaryKey)
        {
            return property.GeneratedValue.SequenceName != null ? config.GetNextValCall(property, tag) : null;
        }

        if (property == classe.DiscriminatorProperty && !classe.Properties.Contains(property))
        {
            return config.GetValue(property, refValue.Class.DiscriminatorValue ?? refValue.Class.SqlName);
        }

        return property.GeneratedValue != null ? null : "null";
    }
}
