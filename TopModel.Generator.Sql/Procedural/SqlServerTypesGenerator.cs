using Microsoft.Extensions.Logging;
using TopModel.Core.Model;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Sql.Procedural;

/// <summary>
/// Générateur SQL Server procédural pour les types de table.
/// </summary>
public class SqlServerTypesGenerator(ILogger<SqlServerTypesGenerator> logger, IFileWriterProvider writerProvider)
    : ClassGroupGeneratorBase<SqlConfig>(logger, writerProvider)
{
    public override string Name => "SqlTypesGen";

    protected override IEnumerable<(string FileType, string FileName)> GetFileNames(Class classe, string tag)
    {
        if (classe.HasTable && Config.GetProperties(classe).Any(p => p.Name == ScriptUtils.InsertKeyName))
        {
            yield return ("type", Path.Combine(Config.OutputDirectory, Config.Procedural!.TypesFileName!));
        }
    }

    protected override void HandleFile(string fileType, string fileName, string tag, IEnumerable<Class> classes)
    {
        using var writer = this.OpenSqlWriter(fileName);

        var appName = classes.First().Namespace.App;

        writer.WriteSqlFileHeader(appName, fileName.Split('/')[^1], "Script de création des types.");

        foreach (var classe in classes.OrderBy(c => c.SqlName))
        {
            WriteTypeDeclaration(classe, writer, tag);
        }
    }

    /// <summary>
    /// Ecrit la déclaration SQL pour le type de table.
    /// </summary>
    /// <param name="classe">Classe.</param>
    /// <param name="writer">Writer.</param>
    /// <param name="tag">Tag.</param>
    protected virtual void WriteTypeDeclaration(Class classe, IFileWriter writer, string tag)
    {
        var typeName = Config.GetSqlTableTypeName(classe, tag);
        writer.WriteLine("/**");
        writer.WriteLine("  * Création du type " + typeName);
        writer.WriteLine(" **/");
        writer.WriteLine(
            "If Exists (Select * From sys.types st Join sys.schemas ss On st.schema_id = ss.schema_id Where st.name = N'"
                + typeName
                + "')"
        );
        writer.WriteLine("Drop Type " + typeName + Environment.NewLine);
        writer.WriteLine("Create type " + typeName + " as Table (");

        var columnCount = 0;

        foreach (var property in Config.GetProperties(classe))
        {
            var type = Config.GetColumnType(property);

            if (!property.PrimaryKey && property.Name != ScriptUtils.InsertKeyName)
            {
                if (columnCount > 0)
                {
                    writer.Write(",");
                    writer.WriteLine();
                }

                writer.Write("\t" + Config.GetSqlName(property, tag) + " " + type);
                columnCount++;
            }
        }

        if (columnCount > 0)
        {
            writer.Write(",");
            writer.WriteLine();
        }

        writer.WriteLine('\t' + classe.Trigram + "_INSERT_KEY int");
        writer.WriteLine();
        writer.WriteLine($"){Config.BatchSeparator}");
        writer.WriteLine();
    }
}
