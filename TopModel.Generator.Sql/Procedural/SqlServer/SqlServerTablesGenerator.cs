using Microsoft.Extensions.Logging;
using TopModel.Core.Model;
using TopModel.Utils;

namespace TopModel.Generator.Sql.Procedural.SqlServer;

public class SqlServerTablesGenerator(ILogger<SqlServerTablesGenerator> logger, IFileWriterProvider writerProvider)
    : AbstractSqlTablesGenerator(logger, writerProvider)
{
    protected override bool SupportsClusteredKey => true;

    /// <summary>
    /// Gère l'auto-incrémentation des clés primaires en ajoutant identity à la colonne.
    /// </summary>
    /// <param name="writer">Flux d'écriture création bases.</param>
    protected override void WriteIdentityColumn(IFileWriter writer, GeneratedValueDefinition generatedValue)
    {
        writer.Write($" identity({generatedValue.Start}, {generatedValue.Increment})");
    }
}
