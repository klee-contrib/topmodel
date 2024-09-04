using Microsoft.Extensions.Logging;
using TopModel.Core;

namespace TopModel.Generator.Sql.Procedural;

/// <summary>
/// Générateur de Oracle.
/// </summary>
public class OracleSchemaGenerator : AbstractSchemaGenerator
{
    public OracleSchemaGenerator(SqlConfig config, ILogger<ProceduralSqlGenerator> logger, TranslationStore translationStore)
        : base(config, logger, translationStore)
    {
    }

    protected override string BatchSeparator => "\r\n/";

    protected override bool AllowTablespace => true;

    protected override bool SupportsClusteredKey => false;

    protected override bool UseQuotes => false;

    protected override bool ExplicitSequenceNextVal => true;

    protected override string GetNextValCall(string sequenceName)
    {
        return $"{sequenceName}.nextval";
    }

    protected override string GetSequenceName(Class classe)
    {
        return $"{classe.Trigram}_SEQ";
    }

    protected override void WriteBooleanCheckConstraints(SqlFileWriter writerCrebas, IList<IProperty> properties)
    {
        /* En Oracle, en 2024, il n'y a pas de type booléen. On utilise un numeric(1) et on rajoute une check constraint pour forcer les valeurs 0 et 1. */
        bool IsNumericBoolean(IProperty property)
        {
            var sqlType = MainConfig.GetType(property);
            return sqlType == "number(1)" && MainConfig.IsBoolean(property);
        }

        foreach (var property in properties)
        {
            if (IsNumericBoolean(property))
            {
                writerCrebas.WriteLine("\tconstraint " + CheckIdentifierLength($"CHK_{property.SqlName}") + " check (" + property.SqlName + " in (0,1)),");
            }
        }
    }

    protected override void WriteComments(SqlFileWriter writerComment, Class classe, string tableName, List<IProperty> properties)
    {
        writerComment.WriteLine();
        writerComment.WriteLine("/**");
        writerComment.WriteLine("  * Commentaires pour la table " + tableName);
        writerComment.WriteLine(" **/");
        writerComment.WriteLine($"COMMENT ON TABLE {tableName} IS '{classe.Comment.Replace("'", "''")}'{BatchSeparator}");
        foreach (var p in properties)
        {
            writerComment.WriteLine($"COMMENT ON COLUMN {tableName}.{p.SqlName} IS '{p.Comment.Replace("'", "''")}'{BatchSeparator}");
        }
    }

    /// <summary>
    /// Gère l'auto-incrémentation des clés primaires en ajoutant identity à la colonne.
    /// </summary>
    /// <param name="writerCrebas">Flux d'écriture création bases.</param>
    protected override void WriteIdentityColumn(SqlFileWriter writerCrebas)
    {
        throw new NotImplementedException("Non implémenté");
    }

    protected override void WriteSequenceDeclaration(Class classe, SqlFileWriter writerCrebas, string tableName)
    {
        writerCrebas.Write($"create sequence {GetSequenceName(classe)}");

        if (Config.Identity.Start != null)
        {
            writerCrebas.Write($"{$" start with {Config.Identity.Start}"}");
        }

        if (Config.Identity.Increment != null)
        {
            writerCrebas.Write($"{$" increment by {Config.Identity.Increment} nocycle"}");
        }
    }
}