using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Core.FileModel;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Sql.Procedural;

public class ProceduralSqlGenerator : GeneratorBase<SqlConfig>
{
    private readonly ILogger<ProceduralSqlGenerator> _logger;

    private AbstractSchemaGenerator? _schemaGenerator;

    public ProceduralSqlGenerator(ILogger<ProceduralSqlGenerator> logger)
        : base(logger)
    {
        _logger = logger;
    }

    public override string Name => "ProceduralSqlGen";

    public override IEnumerable<string> GeneratedFiles => new List<string>()
    {
        Config.Procedural!.CrebasFile!,
        Config.Procedural!.IndexFKFile!,
        Config.Procedural!.InitListFile!,
        Config.Procedural!.UniqueKeysFile!,
    }.Where(t => !string.IsNullOrEmpty(t));

    protected override bool PersistentOnly => true;

    private AbstractSchemaGenerator SchemaGenerator
    {
        get
        {
            _schemaGenerator ??= Config.TargetDBMS == TargetDBMS.Postgre
                ? new PostgreSchemaGenerator(Config, _logger)
                : new SqlServerSchemaGenerator(Config, _logger);

            return _schemaGenerator;
        }
    }

    protected override void HandleFiles(IEnumerable<ModelFile> files)
    {
        var classes = Classes.ToList();

        var manyToManyProperties = Classes.SelectMany(cl => cl.Properties).Where(p => p is AssociationProperty ap && ap.Type == AssociationType.ManyToMany).Select(p => (AssociationProperty)p);
        foreach (var ap in manyToManyProperties)
        {
            var traClass = new Class()
            {
                Comment = ap.Comment,
                Label = ap.Label,
                SqlName = $"{ap.Class.SqlName}_{ap.Association.SqlName}{(ap.Role != null ? $"_{ap.Role.ToConstantCase()}" : string.Empty)}"
            };

            traClass.Properties.Add(new AssociationProperty
            {
                Association = ap.Class,
                Class = traClass,
                Comment = ap.Comment,
                Type = AssociationType.ManyToOne,
                PrimaryKey = true,
                Required = true,
                Role = ap.Role,
                DefaultValue = ap.DefaultValue,
                Label = ap.Label,
                Trigram = ap.Class.PrimaryKey.Single().Trigram
            });

            traClass.Properties.Add(new AssociationProperty
            {
                Association = ap.Association,
                Class = traClass,
                Comment = ap.Comment,
                Type = AssociationType.ManyToOne,
                PrimaryKey = true,
                Required = true,
                Role = ap.Role,
                DefaultValue = ap.DefaultValue,
                Label = ap.Label,
                Trigram = ap.Trigram ?? ap.Association.PrimaryKey.Single().Trigram ?? ap.Association.Trigram
            });

            classes.Add(traClass);
        }

        SchemaGenerator.GenerateSchemaScript(classes.OrderBy(c => c.SqlName));

        GenerateListInitScript();
    }

    private void GenerateListInitScript()
    {
        var classes = Classes.Where(c => c.Values.Any());

        if (classes.Any())
        {
            SchemaGenerator.GenerateListInitScript(classes);
        }
    }
}