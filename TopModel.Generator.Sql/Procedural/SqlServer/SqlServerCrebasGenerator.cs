using Microsoft.Extensions.Logging;
using TopModel.Utils;

namespace TopModel.Generator.Sql.Procedural.SqlServer;

public class SqlServerCrebasGenerator(ILogger<SqlServerCrebasGenerator> logger, IFileWriterProvider writerProvider)
    : AbstractCrebasGenerator(logger, writerProvider)
{
    public override string Name => "SqlServerCrebasGen";

    protected override bool SupportsClusteredKey => true;

    /// <summary>
    /// Gère l'auto-incrémentation des clés primaires en ajoutant identity à la colonne.
    /// </summary>
    /// <param name="writer">Flux d'écriture création bases.</param>
    protected override void WriteIdentityColumn(IFileWriter writer)
    {
        writer.Write(" identity(1, 1)");
    }
}
