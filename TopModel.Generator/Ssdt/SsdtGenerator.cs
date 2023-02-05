using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Core.FileModel;
using TopModel.Generator.Ssdt.Scripter;
using TopModel.Utils;

namespace TopModel.Generator.Ssdt;

public class SsdtGenerator : GeneratorBase
{
    private readonly SsdtConfig _config;
    private readonly ILogger<SsdtGenerator> _logger;

    private readonly ISqlScripter<Class> _tableScripter;
    private readonly ISqlScripter<Class> _tableTypeScripter = new SqlTableTypeScripter();
    private readonly ISqlScripter<Class> _initReferenceListScript;
    private readonly ISqlScripter<IEnumerable<Class>> _initReferenceListMainScripter;

    public SsdtGenerator(ILogger<SsdtGenerator> logger, SsdtConfig config)
        : base(logger, config)
    {
        _config = config;
        _logger = logger;
        _tableScripter = new SqlTableScripter(config);

        _initReferenceListScript = new InitReferenceListScripter(_config);
        _initReferenceListMainScripter = new InitReferenceListMainScripter(_config);
    }

    public override string Name => "SsdtGen";

    public override IEnumerable<string> GeneratedFiles =>
        Classes.Where(c => c.IsPersistent).SelectMany(c =>
        {
            var files = new List<string>();
            if (_config.TableScriptFolder != null)
            {
                files.Add(Path.Combine(_config.TableScriptFolder, _tableScripter.GetScriptName(c)));

                foreach (var ap in Classes.SelectMany(cl => cl.Properties).OfType<AssociationProperty>().Where(ap => ap.Type == AssociationType.ManyToMany))
                {
                    files.Add(Path.Combine(_config.TableScriptFolder, _tableScripter.GetScriptName(new Class
                    {
                        SqlName = $"{ap.Class.SqlName}_{ap.Association.SqlName}{(ap.Role != null ? $"_{ap.Role.ToConstantCase()}" : string.Empty)}"
                    })));
                }
            }

            if (c.Properties.Any(p => p.Name == ScriptUtils.InsertKeyName) && _config.TableTypeScriptFolder != null)
            {
                files.Add(Path.Combine(_config.TableTypeScriptFolder, _tableTypeScripter.GetScriptName(c)));
            }

            return files;
        })
        .Concat(Classes.Where(c => c.IsPersistent && c.ReferenceValues.Any()).Select(c =>
        {
            if (_config.InitListScriptFolder != null)
            {
                return Path.Combine(_config.InitListScriptFolder, _initReferenceListScript.GetScriptName(c));
            }

            return null;
        }))
        .Concat(Classes.Where(c => c.IsPersistent && c.ReferenceValues.Any()).Any() && _config.InitListScriptFolder != null && _config.InitListMainScriptName != null ? new[] { Path.Combine(_config.InitListScriptFolder, _config.InitListMainScriptName) } : Array.Empty<string>())
        .Where(f => f != null)!;

    protected override void HandleFiles(IEnumerable<ModelFile> files)
    {
        foreach (var file in files)
        {
            GenerateClasses(file);
        }

        GenerateListInitScript();
    }

    private void GenerateClasses(ModelFile file)
    {
        if (_config.TableScriptFolder != null)
        {
            var tableCount = 0;
            var tableTypeCount = 0;

            var classes = file.Classes.Where(c => c.IsPersistent).ToList();

            var manyToManyProperties = file.Classes.SelectMany(cl => cl.Properties).OfType<AssociationProperty>().Where(ap => ap.Type == AssociationType.ManyToMany);
            foreach (var ap in manyToManyProperties)
            {
                var traClass = new Class
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

            foreach (var classe in classes)
            {
                tableCount++;
                _tableScripter.Write(classe, _config.TableScriptFolder, _logger, Classes);

                if (classe.Properties.Any(p => p.Name == ScriptUtils.InsertKeyName) && _config.TableTypeScriptFolder != null)
                {
                    tableTypeCount++;
                    _tableTypeScripter.Write(classe, _config.TableTypeScriptFolder, _logger, Classes);
                }
            }
        }
    }

    private void GenerateListInitScript()
    {
        var classes = Classes.Where(c => c.ReferenceValues.Any());

        if (!classes.Any() || _config.InitListScriptFolder == null)
        {
            return;
        }

        Directory.CreateDirectory(_config.InitListScriptFolder);

        // Construit la liste des Reference Class ordonnée.
        var orderList = SortUtils.Sort(classes.OrderBy(c => c.Name), c => c.Properties
            .OfType<AssociationProperty>()
            .Select(a => a.Association)
            .Where(a => a.ReferenceValues.Any()));

        // Script un fichier par classe.
        foreach (var referenceClass in orderList)
        {
            _initReferenceListScript.Write(referenceClass, _config.InitListScriptFolder, _logger, Classes);
        }

        // Script le fichier appelant les fichiers dans le bon ordre.
        if (_config.InitListMainScriptName != null)
        {
            _initReferenceListMainScripter.Write(orderList, _config.InitListScriptFolder, _logger, Classes);
        }
    }
}