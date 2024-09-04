using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Core.FileModel;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Sql.Procedural;

public class ProceduralSqlGenerator : GeneratorBase<SqlConfig>
{
    private readonly ILogger<ProceduralSqlGenerator> _logger;

    private readonly TranslationStore _translationStore;

    private AbstractSchemaGenerator? _schemaGenerator;

    public ProceduralSqlGenerator(ILogger<ProceduralSqlGenerator> logger, TranslationStore translationStore)
        : base(logger)
    {
        _logger = logger;
        _translationStore = translationStore;
    }

    public override string Name => "ProceduralSqlGen";

    public override IEnumerable<string> GeneratedFiles => new List<string>()
    {
        Config.Procedural!.CrebasFile!,
        Config.Procedural!.IndexFKFile!,
        Config.Procedural!.InitListFile!,
        Config.Procedural!.UniqueKeysFile!,
        Config.Procedural!.CommentFile!,
        Config.Procedural!.ResourceFile!,
    }.Where(t => !string.IsNullOrEmpty(t));

    protected override bool PersistentOnly => true;

    private AbstractSchemaGenerator SchemaGenerator
    {
        get
        {
            _schemaGenerator ??= Config.TargetDBMS switch
            {
                TargetDBMS.Postgre => new PostgreSchemaGenerator(Config, _logger, _translationStore),
                TargetDBMS.Sqlserver => new SqlServerSchemaGenerator(Config, _logger, _translationStore),
                TargetDBMS.Oracle => new OracleSchemaGenerator(Config, _logger, _translationStore),
                _ => throw new NotImplementedException($"Procedural SQL generation is not implemented with {Config.TargetDBMS}")
            };

            return _schemaGenerator;
        }
    }

    protected override void HandleFiles(IEnumerable<ModelFile> files)
    {
        var classes = Classes.Where(c => c.IsPersistent).ToList();

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
                Trigram = ap.Trigram ?? ap.Property.Trigram ?? ap.Association.Trigram
            });

            classes.Add(traClass);
        }

        SchemaGenerator.GenerateSchemaScript(classes.OrderBy(c => c.SqlName));

        GenerateListInitScript();
    }

    private void GenerateListInitScript()
    {
        var classes = Classes.Where(c => c.IsPersistent).Where(c => c.Values.Any());

        if (classes.Any())
        {
            SchemaGenerator.GenerateListInitScript(classes, Classes);
        }
    }
}