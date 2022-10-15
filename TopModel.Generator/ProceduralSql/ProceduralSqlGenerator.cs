using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Core.FileModel;

namespace TopModel.Generator.ProceduralSql;

public class ProceduralSqlGenerator : GeneratorBase
{
    private readonly ProceduralSqlConfig _config;
    private readonly ILogger<ProceduralSqlGenerator> _logger;

    private readonly AbstractSchemaGenerator? _schemaGenerator;

    public ProceduralSqlGenerator(ILogger<ProceduralSqlGenerator> logger, ProceduralSqlConfig config)
        : base(logger, config)
    {
        _config = config;
        _logger = logger;
        _schemaGenerator = _config.TargetDBMS == TargetDBMS.Postgre
            ? new PostgreSchemaGenerator(_config, _logger)
            : new SqlServerSchemaGenerator(_config, _logger);
    }

    public override string Name => "ProceduralSqlGen";

    public override IEnumerable<string> GeneratedFiles => new List<string>()
    {
        _config.CrebasFile!,
        _config.IndexFKFile!,
        _config.InitListFile!,
        _config.UniqueKeysFile!,
    }.Where(t => !string.IsNullOrEmpty(t));

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
                SqlName = $"{ap.Class.SqlName}_{ap.Association.SqlName}{(ap.Role != null ? $"_{ap.Role.ToUpper()}" : string.Empty)}"
            };
            traClass.Properties.Add(new AssociationProperty()
            {
                Association = ap.Class,
                Class = traClass,
                Comment = ap.Comment,
                Type = AssociationType.ManyToOne,
                Required = true,
                Role = ap.Role,
                DefaultValue = ap.DefaultValue,
                Label = ap.Label,
                Trigram = ap.Class.PrimaryKey!.Trigram
            });
            traClass.Properties.Add(new AssociationProperty()
            {
                Association = ap.Association,
                Class = traClass,
                Comment = ap.Comment,
                Type = AssociationType.ManyToOne,
                Required = true,
                Role = ap.Role,
                DefaultValue = ap.DefaultValue,
                Label = ap.Label,
                Trigram = ap.Trigram ?? ap.Association.PrimaryKey?.Trigram ?? ap.Association.Trigram
            });
            classes.Add(traClass);
        }

        _schemaGenerator?.GenerateSchemaScript(classes.OrderBy(c => c.SqlName));

        GenerateListInitScript();
    }

    private void GenerateListInitScript()
    {
        var classes = Classes.Where(c => c.ReferenceValues.Any());

        if (classes.Any())
        {
            _schemaGenerator?.GenerateListInitScript(classes);
        }
    }
}