using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Core.FileModel;
using TopModel.Utils;
namespace TopModel.Generator.ProceduralSql;

public class ProceduralSqlGenerator : GeneratorBase
{
    private readonly ProceduralSqlConfig _config;
    private readonly ILogger<ProceduralSqlGenerator> _logger;
    private readonly IDictionary<string, ModelFile> _files = new Dictionary<string, ModelFile>();

    private readonly AbstractSchemaGenerator? _schemaGenerator;

    public ProceduralSqlGenerator(ILogger<ProceduralSqlGenerator> logger, ProceduralSqlConfig config)
        : base(logger, config)
    {
        _config = config;
        _logger = logger;
        _schemaGenerator = _config.TargetDBMS == TargetDBMS.Postgre
            ? new PostgreSchemaGenerator(_config, _logger)
            : (AbstractSchemaGenerator)new SqlServerSchemaGenerator(_config, _logger);
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
        foreach (var file in files)
        {
            _files[file.Name] = file;
        }

        var classes = _files.Values.SelectMany(f => f.Classes).Distinct();
        classes.ToList().ForEach(c =>
        {
            var oneToManyProperties = classes.SelectMany(cl => cl.Properties).Where(p => p is AssociationProperty ap && ap.Type == AssociationType.OneToMany && ap.Association == c).Select(p => (AssociationProperty)p);
            foreach (var ap in oneToManyProperties)
            {
                var asp = new PgAssociationProperty()
                {
                    Association = ap.Class,
                    Class = ap.Association,
                    Comment = ap.Comment,
                    Type = AssociationType.ManyToOne,
                    Required = ap.Required,
                    Role = ap.Role,
                    DefaultValue = ap.DefaultValue,
                    Label = ap.Label
                };
                c.Properties.Add(asp);
            }
        });

        var manyToManyProperties = classes.SelectMany(cl => cl.Properties).Where(p => p is AssociationProperty ap && ap.Type == AssociationType.ManyToMany).Select(p => (AssociationProperty)p);
        foreach (var ap in manyToManyProperties)
        {
            var traClass = new Class()
            {
                Comment = ap.Comment,
                Label = ap.Label,
                SqlName = $"{ap.Class.SqlName}_{ap.Association.SqlName}{(ap.Role != null ? $"_{ap.Role.ToUpper()}" : string.Empty)}"
            };
            traClass.Properties.Add(new PgAssociationProperty()
            {
                Association = ap.Class,
                Class = traClass,
                Comment = ap.Comment,
                Type = AssociationType.ManyToOne,
                Required = true,
                Role = ap.Role,
                DefaultValue = ap.DefaultValue,
                Label = ap.Label
            });
            traClass.Properties.Add(new PgAssociationProperty()
            {
                Association = ap.Association,
                Class = traClass,
                Comment = ap.Comment,
                Type = AssociationType.ManyToOne,
                Required = true,
                Role = ap.Role,
                DefaultValue = ap.DefaultValue,
                Label = ap.Label
            });
            classes = classes.Append(traClass);
        }

        _schemaGenerator?.GenerateSchemaScript(classes.OrderBy(c => c.SqlName));
        foreach (var p in classes.SelectMany(c => c.Properties).Where(p => p is PgAssociationProperty).ToList())
        {
            p.Class.Properties.Remove(p);
        }

        GenerateListInitScript();
    }

    private void GenerateListInitScript()
    {
        var classes = _files.Values
            .SelectMany(f => f.Classes)
            .Distinct()
            .Where(c => c.ReferenceValues.Any());

        if (classes.Any())
        {
            _schemaGenerator?.GenerateListInitScript(classes);
        }
    }
}