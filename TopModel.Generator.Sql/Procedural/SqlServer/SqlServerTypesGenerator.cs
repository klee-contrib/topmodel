using Microsoft.Extensions.Logging;
using TopModel.Core.Model;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Sql.Procedural.SqlServer;

public class SqlServerTypesGenerator(ILogger<SqlServerTypesGenerator> logger, IFileWriterProvider writerProvider)
    : ClassGroupGeneratorBase<SqlConfig>(logger, writerProvider)
{
    /// <summary>
    /// Nom pour l'insert en bulk.
    /// </summary>
    private const string InsertKeyName = "InsertKey";

    /// <summary>
    /// Type json pour les compositions.
    /// </summary>
    private const string JsonType = "json";

    public override string Name => "SqlTypesGen";

    protected override IEnumerable<(string FileType, string FileName)> GetFileNames(Class classe, string tag)
    {
        if (classe.IsPersistent && classe.Type != ClassType.Interface)
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
            WriteTypeDeclaration(classe, writer);
        }
    }

    /// <summary>
    /// Ecrit dans le writer le script de création du type.
    /// </summary>
    /// <param name="classe">Classe.</param>
    /// <param name="writer">Writer.</param>
    private static void WriteType(Class classe, IFileWriter writer)
    {
        var typeName = classe.SqlName + "_TABLE_TYPE";
        writer.WriteLine("/**");
        writer.WriteLine("  * Création du type " + classe.SqlName + "_TABLE_TYPE");
        writer.WriteLine(" **/");
        writer.WriteLine(
            "If Exists (Select * From sys.types st Join sys.schemas ss On st.schema_id = ss.schema_id Where st.name = N'"
                + typeName
                + "')"
        );
        writer.WriteLine("Drop Type " + typeName + '\n');
        writer.WriteLine("Create type " + typeName + " as Table (");
    }

    private void WriteTypeDeclaration(Class classe, IFileWriter writer)
    {
        var isContainsInsertKey = classe.Properties.Any(p => p.Name == InsertKeyName);
        if (isContainsInsertKey)
        {
            WriteType(classe, writer);
        }

        var t = 0;

        foreach (var property in classe.AllProperties)
        {
            var persistentType = property is { Composition: null } ? Config.GetType(property) : JsonType;

            if (persistentType.ToLower().Equals("varchar") && property.Domain.Length != null)
            {
                persistentType = $"{persistentType}({property.Domain.Length})";
            }

            if (
                (persistentType.ToLower().Equals("numeric") || persistentType.ToLower().Equals("decimal"))
                && property.Domain.Length != null
            )
            {
                persistentType =
                    $"{persistentType}({property.Domain.Length}{(property.Domain.Scale != null ? $", {property.Domain.Scale}" : string.Empty)})";
            }

            if (isContainsInsertKey && !property.PrimaryKey && property.Name != InsertKeyName)
            {
                if (t > 0)
                {
                    writer.Write(",");
                    writer.WriteLine();
                }

                writer.Write("\t" + property.SqlName + " " + persistentType);
                t++;
            }
        }

        if (isContainsInsertKey)
        {
            if (t > 0)
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
}
