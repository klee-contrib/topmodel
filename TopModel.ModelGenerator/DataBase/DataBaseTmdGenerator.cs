using System.Text.RegularExpressions;
using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using TopModel.Utils;

namespace TopModel.ModelGenerator.Database;

class DatabaseTmdGenerator : IDisposable
{
    private DatabaseConfig _config;

    private Dictionary<string, TmdClass> _classes = new();

    private ILogger _logger;

    private string _modelRoot;


    private int _moduleIndice = 0;

    private string moduleIndice
    {
        get
        {
            _moduleIndice++;
            return _moduleIndice.ToString().PadLeft(2, '0');
        }
    }

    private int _fileIndice = 10;

    private IEnumerable<TmdFile> _files => _classes.Select(c => c.Value.File!).Distinct().OrderBy(c => c.Name);

    private NpgsqlConnection _connection;

    public DatabaseTmdGenerator(DatabaseConfig config, ILogger logger, string modelRoot)
    {
        _config = config;
        _logger = logger;
        _modelRoot = modelRoot;
        // Connexion à la base de données {_config.Source.DbName}
        _connection = new NpgsqlConnection(config.ConnectionString);
        _classes = new();
    }

    public void Dispose()
    {
        _connection.Dispose();
    }

    public void Generate()
    {
        var columns = GetColumns();
        var classGroups = columns.GroupBy(c => c.TableName);
        // Initialisation de {classGroups.Count()} classes
        InitClasses(classGroups);

        // Initialisation des propriétés
        InitProperties(classGroups);

        InitUniqConstraints(classGroups);

        ReadValues();

        CreateFiles();

        // Création des modules
        CreateModules();

        // Regroupement des fichiers qui ont les mêmes imports
        GroupFiles();

        // Renumérotation et réétiquettage des fichiers 
        RenameFiles();

        // Ecriture des fichiers
        WriteFiles();

    }

    private void ReadValues()
    {
        // Extraction des valeurs pour les tables paramétrées
        foreach (var classe in _config.ExtractValues.Select(classe => _classes.FirstOrDefault(c => classe == c.Value.Name)).Where(c => c.Value != null))
        {
            var values = _connection
            .Query(@$"
            select  *
            from {classe.Key}
                    ");

            var val = values.Select(r =>
            {
                var d = new Dictionary<string, string?>();
                foreach (var kv in r)
                {
                    d.Add(classe.Value.Properties.First(p => p.SqlName == kv.Key).Name, kv.Value?.ToString());
                }
                return d;
            });
            classe.Value.Values.AddRange(val);
        }
    }

    private IEnumerable<DbColumn> GetColumns()
    {
        // Récupération des colonnes
        return _connection
            .Query<DbColumn>(@$"
            select  table_name                                                              as TableName, 
                    column_name                                                             as ColumnName, 
                    data_type                                                               as DataType,
                    is_nullable = 'YES'                                                     as Nullable,
                    coalesce(numeric_precision, datetime_precision, interval_precision)     as Precision,
                    coalesce(character_maximum_length, numeric_scale, interval_precision)   as Scale
            from information_schema.columns 
            where table_schema  = '{_config.Source.Schema}'
            order by ordinal_position 
                    ")
            .Where(c => !_config.Exclude.Contains(c.TableName));
    }

    private void InitProperties(IEnumerable<IGrouping<string, DbColumn>> classGroups)
    {
        var primaryKeys = GetPrimaryKeys();
        var foreignKeys = GetForeignKeys();
        var foreignKeysGroups = foreignKeys.GroupBy(c => c.TableName);
        var primaryKeysGroups = primaryKeys.GroupBy(c => c.TableName);
        foreach (var group in classGroups)
        {
            FeedProperties(group, primaryKeysGroups.FirstOrDefault(f => f.Key == group.Key), foreignKeysGroups.FirstOrDefault(f => f.Key == group.Key));
        }

        // Résolution des contraintes de clés étrangères
        foreach (var group in classGroups)
        {
            ResolveForeignProperties(group, foreignKeysGroups.FirstOrDefault(f => f.Key == group.Key));
        }
    }

    private IEnumerable<ConstraintKey> GetForeignKeys()
    {
        // Récupération des contraintes de clés étrangères
        return GetContraintKey("FOREIGN KEY");
    }

    private IEnumerable<ConstraintKey> GetPrimaryKeys()
    {
        // Récupération des contraintes de clés primaires
        return GetContraintKey("PRIMARY KEY");
    }

    private IEnumerable<ConstraintKey> GetUniqueKeys()
    {
        // Récupération des contraintes d'unicité
        return _connection
            .Query<ConstraintKey>(@$"
SELECT
    tc.constraint_name  AS Name,
    tc.table_name       AS TableName,
    kcu.column_name     AS ColumnName
FROM 
    information_schema.table_constraints        AS tc 
    JOIN information_schema.key_column_usage    AS kcu
        ON  tc.constraint_name      = kcu.constraint_name
        AND tc.table_schema         = kcu.table_schema
WHERE       tc.constraint_type      = 'UNIQUE'
    AND     tc.table_schema         = '{_config.Source.Schema}'
                    ")
            .Where(c => !_config.Exclude.Contains(c.TableName));
    }

    private IEnumerable<ConstraintKey> GetContraintKey(string constraint)
    {
        return _connection
            .Query<ConstraintKey>(@$"
SELECT
    tc.table_name   AS TableName, 
    kcu.column_name AS ColumnName, 
    ccu.table_name  AS ForeignTableName,
    ccu.column_name AS ForeignColumnName 
FROM 
            information_schema.table_constraints   AS tc 
    JOIN    information_schema.key_column_usage    AS kcu
        ON  tc.constraint_name   = kcu.constraint_name
        AND tc.table_schema      = kcu.table_schema
    JOIN information_schema.constraint_column_usage AS ccu
        ON  ccu.constraint_name  = tc.constraint_name
        AND ccu.table_schema     = tc.table_schema
WHERE       tc.constraint_type   = '{constraint}'
    AND     tc.table_schema      = '{_config.Source.Schema}'
    AND     ccu.table_schema     = '{_config.Source.Schema}'
                    ")
            .Where(c => !_config.Exclude.Contains(c.TableName));
    }

    private void ResolveForeignProperties(IGrouping<string, DbColumn> group, IGrouping<string, ConstraintKey>? foreignKeys)
    {
        var classe = _classes[group.Key];
        if (foreignKeys != null)
        {
            foreach (var fk in classe.Properties.OfType<TmdAssociationProperty>())
            {
                var foreignColumnName = foreignKeys.First(p => p.ColumnName == fk.SqlName).ForeignColumnName;
                fk.ForeignProperty = fk.ForeignClass!.Properties.First(p => p.SqlName == foreignColumnName);
            }
        }
    }

    private void InitUniqConstraints(IEnumerable<IGrouping<string, DbColumn>> classGroups)
    {
        var uniqueKeys = GetUniqueKeys();
        // Résolution des contraintes d'unicité
        var uniqueKeysGroups = uniqueKeys.GroupBy(c => c.TableName);
        foreach (var group in classGroups)
        {
            var classe = _classes[group.Key];
            var uniqConstraints = uniqueKeysGroups.Where(f => f.Key == group.Key);
            if (uniqConstraints != null)
            {
                AddUniqConstraints(classe, uniqConstraints);
            }
        }
    }

    private void AddUniqConstraints(TmdClass classe, IEnumerable<IGrouping<string, ConstraintKey>> groupings)
    {
        foreach (var group in groupings)
        {
            var uniqConstraints = group.GroupBy(g => g.Name);
            classe.Unique
                .AddRange(uniqConstraints.Select(u => u.Select(c =>
                {
                    var property = classe.Properties.First(p => p.SqlName == c.ColumnName);
                    string name = string.Empty;
                    if (property is TmdAssociationProperty ap)
                    {
                        name = ap.ForeignClass.Name + ap.ForeignProperty!.Name + ap.Role;
                    }
                    else
                    {
                        name = property.Name;
                    }

                    return name;
                }).ToList()));
        }
    }

    private void CreateFiles()
    {
        // Création des fichiers de classes sans dépendances cirulaires
        CreateSimpleFiles();
        // Création des fichiers des {_classes.Where(c => c.Value.File == null).Count()} classes contenant des dépendances cirulaires
        CreateCircularFiles();
    }

    private void CreateSimpleFiles()
    {
        while (_classes.Where(c => c.Value.File == null && c.Value.Dependencies.All(d => d.File != null)).Any())
        {
            foreach (var classe in _classes.Where(c => c.Value.File == null && c.Value.Dependencies.All(d => d.File != null)).OrderBy(c => c.Value.Dependencies.Count))
            {
                var file = new TmdFile()
                {
                    Name = $"{_fileIndice++}_Model",
                    Tags = _config.Tags
                };
                classe.Value.File = file;
                file.Classes.Add(classe.Value);
            }
        }
    }

    private void CreateCircularFiles()
    {
        while (_classes.Any(c => c.Value.File == null))
        {
            var file = new TmdFile()
            {
                Name = $"{_fileIndice++}_Model",
                Tags = _config.Tags
            };
            var rootClass = _classes.Where(c => c.Value.File == null).First();
            var stack = new Queue<TmdClass>();
            stack.Enqueue(rootClass.Value);
            while (stack.TryDequeue(out var s))
            {
                s.File = file;
                file.Classes.Add(s);
                foreach (var d in s.Dependencies.Where(d => d.File is null))
                {
                    stack.Enqueue(d);
                }
            }
        }
    }


    private void CreateModules()
    {
        // Regroupement des classes dans les modules définis dans la configuration
        CreateUserModules();
        // Recherche automatique des modules restants (classes ayant plus de deux dépendances)
        CreateAutomaticModules();
        // Regroupement des classes qui ont une ou deux dépendances
        CreateJoinModule();
        // Regroupement des classes qui n'ont aucune dépendance
        CreateOtherModule();
    }


    private TmdClass FeedProperties(IGrouping<string, DbColumn> group, IEnumerable<ConstraintKey>? primaryKeyConstraints, IEnumerable<ConstraintKey>? foreignConstraints)
    {
        var classe = _classes[group.Key];
        if (group.Any())
        {
            var regularProperties = group.Where(p => !foreignConstraints?.Any(f => f.ColumnName == p.ColumnName) ?? true);
            var trigram = regularProperties.FirstOrDefault()?.ColumnName.Split('_').First();
            if (trigram == null || !regularProperties.All(p => p.ColumnName.StartsWith(trigram)))
            {
                trigram = string.Empty;
            }

            classe.Trigram = trigram;
            classe.Properties = group.Select(c => ColumnToProperty(classe, c, trigram, primaryKeyConstraints?.FirstOrDefault(f => f.ColumnName == c.ColumnName), foreignConstraints?.FirstOrDefault(f => f.ColumnName == c.ColumnName))).ToList();
        }

        return classe;
    }

    private void InitClasses(IEnumerable<IGrouping<string, DbColumn>> classGroups)
    {
        foreach (var group in classGroups)
        {
            _classes.Add(group.Key, new TmdClass() { Name = group.Key.ToPascalCase() });
        }
    }


    private TmdProperty ColumnToProperty(TmdClass classe, DbColumn column, string trigram, ConstraintKey? primaryKeyConstraint, ConstraintKey? foreignConstraint)
    {
        var domain = TmdGenUtils.GetDomainString(_config.Domains, column.ColumnName, column.Scale, column.Precision);
        if (domain == column.ColumnName)
        {
            domain = TmdGenUtils.GetDomainString(_config.Domains, column.DataType, column.Scale, column.Precision);
        }

        var columnName = column.ColumnName;

        TmdProperty tmdProperty;
        if (foreignConstraint != null && _classes.ContainsKey(foreignConstraint.ForeignTableName))
        {
            tmdProperty = new TmdAssociationProperty();
            var foreignClass = _classes[foreignConstraint.ForeignTableName];
            ((TmdAssociationProperty)tmdProperty).ForeignClass = foreignClass;
        }
        else
        {
            tmdProperty = new TmdProperty();
            if (!string.IsNullOrEmpty(trigram))
            {
                var regex = new Regex(Regex.Escape(trigram));
                columnName = regex.Replace(columnName, string.Empty, 1);
            }
        }

        tmdProperty.Name = columnName.ToPascalCase();
        tmdProperty.Required = !column.Nullable || primaryKeyConstraint != null;
        tmdProperty.PrimaryKey = primaryKeyConstraint != null;
        tmdProperty.Domain = domain;
        tmdProperty.SqlName = column.ColumnName;
        tmdProperty.Class = classe;

        return tmdProperty;
    }


    private void CreateUserModules()
    {
        // Recherche des modules forcés par la configuration
        foreach (var module in _config.Modules)
        {
            var moduleName = $"{moduleIndice}_{module.Name}";
            foreach (var mainClass in module.Classes.Select(c => _classes.Where(cl => cl.Value.Name == c).FirstOrDefault()))
            {
                if (mainClass.Value != null)
                {
                    mainClass.Value.File!.Module = moduleName;
                }
            }
        }

        foreach (var module in _config.Modules)
        {
            foreach (var mainClass in module.Classes.Select(c => _classes.Where(cl => cl.Value.Name == c).FirstOrDefault()))
            {
                if (mainClass.Value != null)
                {
                    AffectModule(mainClass.Value, mainClass.Value.File!.Module);
                }
            }
        }
    }

    private void CreateAutomaticModules()
    {
        while (_files.Any(f => f.Module is null && f.Classes.SelectMany(c => c.Dependencies).Count() > 2))
        {
            var rootFile = _files.Where(f => f.Module is null && f.Classes.SelectMany(c => c.Dependencies).Count() > 2)
                .OrderByDescending(f => f.Classes.Select(c => c.Dependencies.Count() + _classes.Select(cl => cl.Value.Dependencies.Contains(c)).Count()).Sum())
                .First();
            var mainClass = rootFile.Classes.OrderByDescending(cl => cl.Dependencies.Count() + _classes.Select(c => c.Value.Dependencies.Contains(cl)).Count()).First();

            AffectModule(mainClass);
        }
    }

    private void CreateJoinModule()
    {
        var joinModule = $"{moduleIndice}_Join";
        foreach (var file in _files.Where(f => f.Module is null && f.Classes.SelectMany(c => c.Dependencies).Count() >= 1))
        {
            var dep = file.Classes.SelectMany(c => c.Dependencies).Select(c => c.File!.Module).Distinct();
            if (dep.Count() == 1 && dep.First() != null)
            {
                file.Module = dep.First();
            }
            else
            {
                file.Module = joinModule;
            }
        }
    }

    private void CreateOtherModule()
    {
        var trashModule = $"{moduleIndice}_Autres";
        foreach (var file in _files.Where(f => f.Module is null))
        {
            file.Module = trashModule;
        }
    }

    private void AffectModule(TmdClass mainClass, string? module = null)
    {
        var rootFile = mainClass.File!;
        if (module == null)
        {
            module = $"{moduleIndice}_{mainClass.Name}";
        }

        var stack = new Queue<TmdFile>();
        stack.Enqueue(rootFile);
        while (stack.TryDequeue(out var f))
        {
            f.Module = module;

            // Ajout des classes dont je dépends
            foreach (var d in f.Classes.SelectMany(c => c.Dependencies.Select(d => d.File!)).Distinct().Where(d => d.Module is null))
            {
                stack.Enqueue(d);
            }
        }
    }

    private void GroupFiles()
    {

        // Regroupement des fichiers qui ont les mêmes imports 
        var hasChanged = true;
        while (hasChanged)
        {
            hasChanged = false;
            foreach (var file in _files)
            {
                foreach (var f in _files.Where(f => f.Classes.Count() > 0 && _files.ToList().IndexOf(f) > _files.ToList().IndexOf(file)))
                {
                    if (string.Join(string.Empty, f.Uses.Select(u => u.Name)) == string.Join(string.Empty, file.Uses.Select(u => u.Name)) && f.Module == file.Module)
                    {
                        hasChanged = true;
                        file.Classes.AddRange(f.Classes);
                        foreach (var c in f.Classes)
                        {
                            c.File = file;
                        }

                        f.Classes = new();
                        break;
                    }
                }

                if (hasChanged) break;
            }
        }
    }
    private void RenameFiles()
    {
        // Renommage des fichiers
        foreach (var group in _files.GroupBy(f => f.Module))
        {
            var indice = 1;
            foreach (var file in group.OrderBy(f => f.Name))
            {
                var mainClass = file.Classes.OrderByDescending(cl => cl.Dependencies.Count() + _classes.SelectMany(c => c.Value.Dependencies).Where(c => c == cl).Count()).First();
                file.Name = (indice < 10 ? "0" : string.Empty) + (indice++) + "_" + mainClass.Name;
            }
        }
    }

    private void WriteFiles()
    {
        foreach (var file in _files)
        {
            using var tmdFileWriter = new TmdWriter(Path.Combine(_modelRoot, _config.OutputDirectory), file!, _logger, _modelRoot);
            tmdFileWriter.Write();
        }
    }
}