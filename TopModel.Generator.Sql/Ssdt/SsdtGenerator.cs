using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Core.FileModel;
using TopModel.Generator.Core;
using TopModel.Generator.Sql.Ssdt.Scripter;
using TopModel.Utils;

namespace TopModel.Generator.Sql.Ssdt;

public class SsdtGenerator : GeneratorBase<SqlConfig>
{
    private readonly ILogger<SsdtGenerator> _logger;

    private ISqlScripter<Class>? _tableScripter;
    private ISqlScripter<Class>? _initReferenceListScript;
    private ISqlScripter<IEnumerable<Class>>? _initReferenceListMainScripter;

    public SsdtGenerator(ILogger<SsdtGenerator> logger)
        : base(logger)
    {
        _logger = logger;
    }

    public override string Name => "SsdtGen";

    public override IEnumerable<string> GeneratedFiles =>
        Classes.Where(c => c.IsPersistent && !c.Abstract).SelectMany(c =>
        {
            var files = new List<string>();
            if (Config.Ssdt!.TableScriptFolder != null)
            {
                files.Add(Path.Combine(Config.Ssdt!.TableScriptFolder, TableScripter.GetScriptName(c)));

                foreach (var ap in Classes.SelectMany(cl => cl.Properties).OfType<AssociationProperty>().Where(ap => ap.Type == AssociationType.ManyToMany))
                {
                    files.Add(Path.Combine(Config.Ssdt!.TableScriptFolder, TableScripter.GetScriptName(new Class
                    {
                        SqlName = $"{ap.Class.SqlName}_{ap.Association.SqlName}{(ap.Role != null ? $"_{ap.Role.ToConstantCase()}" : string.Empty)}"
                    })));
                }
            }

            if (c.Properties.Any(p => p.Name == ScriptUtils.InsertKeyName) && Config.Ssdt!.TableTypeScriptFolder != null)
            {
                files.Add(Path.Combine(Config.Ssdt!.TableTypeScriptFolder, TableTypeScripter.GetScriptName(c)));
            }

            return files;
        })
        .Concat(Classes.Where(c => c.IsPersistent && !c.Abstract && c.Values.Any()).Select(c =>
        {
            if (Config.Ssdt!.InitListScriptFolder != null)
            {
                return Path.Combine(Config.Ssdt!.InitListScriptFolder, InitReferenceListScript.GetScriptName(c));
            }

            return null;
        }))
        .Concat(Classes.Where(c => c.IsPersistent && !c.Abstract && c.Values.Any()).Any() && Config.Ssdt!.InitListScriptFolder != null && Config.Ssdt!.InitListMainScriptName != null ? new[] { Path.Combine(Config.Ssdt!.InitListScriptFolder, Config.Ssdt!.InitListMainScriptName) } : Array.Empty<string>())
        .Where(f => f != null)!;

    private ISqlScripter<Class> TableScripter
    {
        get
        {
            _tableScripter ??= new SqlTableScripter(Config);
            return _tableScripter;
        }
    }

    private ISqlScripter<Class> TableTypeScripter { get; } = new SqlTableTypeScripter();

    private ISqlScripter<Class> InitReferenceListScript
    {
        get
        {
            _initReferenceListScript ??= new InitReferenceListScripter(Config);
            return _initReferenceListScript;
        }
    }

    private ISqlScripter<IEnumerable<Class>> InitReferenceListMainScripter
    {
        get
        {
            _initReferenceListMainScripter ??= new InitReferenceListMainScripter(Config);
            return _initReferenceListMainScripter;
        }
    }

    protected override object? GetDomainType(Domain domain)
    {
        return domain.SqlType;
    }

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
        if (Config.Ssdt!.TableScriptFolder != null)
        {
            var tableCount = 0;
            var tableTypeCount = 0;

            var classes = file.Classes.Where(c => c.IsPersistent && !c.Abstract).ToList();

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
                TableScripter.Write(classe, Config.Ssdt!.TableScriptFolder, _logger, Classes);

                if (classe.Properties.Any(p => p.Name == ScriptUtils.InsertKeyName) && Config.Ssdt!.TableTypeScriptFolder != null)
                {
                    tableTypeCount++;
                    TableTypeScripter.Write(classe, Config.Ssdt!.TableTypeScriptFolder, _logger, Classes);
                }
            }
        }
    }

    private void GenerateListInitScript()
    {
        var classes = Classes.Where(c => c.IsPersistent && !c.Abstract && c.Values.Any());

        if (!classes.Any() || Config.Ssdt!.InitListScriptFolder == null)
        {
            return;
        }

        Directory.CreateDirectory(Config.Ssdt!.InitListScriptFolder);

        // Construit la liste des Reference Class ordonnée.
        var orderList = SortUtils.Sort(classes.OrderBy(c => c.SqlName), c => c.Properties
            .OfType<AssociationProperty>()
            .Select(a => a.Association)
            .Where(a => a.Values.Any()));

        // Script un fichier par classe.
        foreach (var referenceClass in orderList)
        {
            InitReferenceListScript.Write(referenceClass, Config.Ssdt!.InitListScriptFolder, _logger, Classes);
        }

        // Script le fichier appelant les fichiers dans le bon ordre.
        if (Config.Ssdt!.InitListMainScriptName != null)
        {
            InitReferenceListMainScripter.Write(orderList, Config.Ssdt!.InitListScriptFolder, _logger, Classes);
        }
    }
}