using Microsoft.Extensions.Logging;
using TopModel.Core.Model;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Sql.Procedural;

public class SqlCommentsGenerator(ILogger<SqlCommentsGenerator> logger, IFileWriterProvider writerProvider)
    : ClassGroupGeneratorBase<SqlConfig>(logger, writerProvider)
{
    public override string Name => "SqlCommentsGen";

    protected override IEnumerable<(string FileType, string FileName)> GetFileNames(Class classe, string tag)
    {
        if (classe.HasTable && classe.Enum != EnumMode.Enum)
        {
            yield return ("comments", Path.Combine(Config.OutputDirectory, Config.Procedural!.CommentsFileName!));
        }
    }

    protected override void HandleFile(string fileType, string fileName, string tag, IEnumerable<Class> classes)
    {
        using var writer = this.OpenSqlWriter(fileName);

        var appName = classes.First().Namespace.App;

        writer.WriteSqlFileHeader(
            appName,
            Path.GetFileName(fileName).Split(Path.PathSeparator)[^1],
            "Script de création de commentaires sur les tables et les colonnes."
        );

        foreach (var classe in classes.OrderBy(c => c.SqlName))
        {
            WriteTableDeclaration(classe, writer);
        }
    }

    /// <summary>
    /// Déclaration de la table.
    /// </summary>
    /// <param name="classe">La table à ecrire.</param>
    /// <param name="writerComment">Flux d'écritures des commentaires.</param>
    private void WriteTableDeclaration(Class classe, IFileWriter writerComment)
    {
        writerComment.WriteLine();
        writerComment.WriteComments(classe, Config);
    }
}
