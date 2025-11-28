using Microsoft.Extensions.Logging;
using TopModel.Core.Model;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Sql.Procedural;

public class SqlCommentGenerator(ILogger<SqlCommentGenerator> logger, IFileWriterProvider writerProvider)
    : ClassGroupGeneratorBase<SqlConfig>(logger, writerProvider)
{
    public override string Name => "SqlCommentGen";

    protected override IEnumerable<(string FileType, string FileName)> GetFileNames(Class classe, string tag)
    {
        if (classe.IsPersistent && !classe.Abstract)
        {
            yield return ("comment", Config.Procedural!.CommentFile!);
        }
    }

    protected override void HandleFile(string fileType, string fileName, string tag, IEnumerable<Class> classes)
    {
        using var writer = this.OpenSqlWriter(fileName);

        var appName = classes.First().Namespace.App;

        writer.WriteSqlFileHeader(appName, fileName.Split('/')[^1], "Script de création des commentaires.");

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
