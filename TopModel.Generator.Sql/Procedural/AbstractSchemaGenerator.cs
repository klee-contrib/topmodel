using System.Text;
using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Sql.Procedural;

/// <summary>
/// Classe abstraite de génération des sripts de création.
/// </summary>
public abstract class AbstractSchemaGenerator
{
    /// <summary>
    /// Nom pour l'insert en bulk.
    /// </summary>
    protected const string InsertKeyName = "InsertKey";

    private readonly SqlConfig _config;
    private readonly ILogger<ProceduralSqlGenerator> _logger;
    private readonly TranslationStore _translationStore;

    public AbstractSchemaGenerator(SqlConfig config, ILogger<ProceduralSqlGenerator> logger, TranslationStore translationStore)
    {
        _config = config;
        _logger = logger;
        _translationStore = translationStore;
    }

    protected ProceduralSqlConfig Config => _config.Procedural!;

    /// <summary>
    /// Séparateur de lots de commandes SQL.
    /// </summary>
    protected abstract string BatchSeparator
    {
        get;
    }

    /// <summary>
    /// Type json pour les compositions.
    /// </summary>
    protected virtual string JsonType => "json";

    /// <summary>
    /// Utilise des guillemets autour des noms d'objets dans les scripts.
    /// </summary>
    protected virtual bool UseQuotes => false;

    /// <summary>
    /// Indique si le moteur de BDD visé supporte "primary key clustered ()".
    /// </summary>
    protected abstract bool SupportsClusteredKey
    {
        get;
    }

    /// <summary>
    /// Indique la limite de longueur d'un identifiant.
    /// </summary>
    private static int IdentifierLengthLimit => 128;

    /// <summary>
    /// Génère le script SQL d'initialisation des listes reference.
    /// </summary>
    /// <param name="classes">Classes avec des initialisations de listes de référence.</param>
    /// <param name="availableClasses">Classes disponibles.</param>
    public void GenerateListInitScript(IEnumerable<Class> classes, IEnumerable<Class> availableClasses)
    {
        if (Config.InitListFile == null || !classes.Any())
        {
            return;
        }

        using var writerInsert = new SqlFileWriter(Config.InitListFile, _logger);

        writerInsert.WriteLine("-- =========================================================================================== ");
        writerInsert.WriteLine($"--   Application Name	:	{classes.First().Namespace.App} ");
        writerInsert.WriteLine("--   Script Name		:	" + Config.InitListFile.Split('/').Last());
        writerInsert.WriteLine("--   Description		:	Script d'insertion des données de références");
        writerInsert.WriteLine("-- ===========================================================================================");

        WriteInsertStart(writerInsert);

        // Construit la liste des Reference Class ordonnée.
        var orderList = CoreUtils.Sort(classes.OrderBy(c => c.SqlName), c => c.Properties
            .OfType<AssociationProperty>()
            .Select(a => a.Association)
            .Where(a => a != c && a.Values.Any() && a.IsPersistent));

        foreach (var classe in orderList)
        {
            WriteInsert(writerInsert, classe, availableClasses);
        }

        WriteInsertEnd(writerInsert);
    }

    /// <summary>
    /// Génère le script SQL.
    /// </summary>
    /// <param name="classes">Classes.</param>
    public void GenerateSchemaScript(IEnumerable<Class> classes)
    {
        if (Config.CrebasFile == null || Config.IndexFKFile == null || !classes.Any())
        {
            return;
        }

        using var writerCrebas = new SqlFileWriter(Config.CrebasFile, _logger);

        SqlFileWriter? writerType = null;
        SqlFileWriter? writerUk = null;
        SqlFileWriter? writerComment = null;
        SqlFileWriter? writerResource = null;

        if (Config.TypeFile != null)
        {
            writerType = new SqlFileWriter(Config.TypeFile, _logger);
        }

        if (Config.UniqueKeysFile != null)
        {
            writerUk = new SqlFileWriter(Config.UniqueKeysFile, _logger);
        }

        if (Config.CommentFile != null)
        {
            writerComment = new SqlFileWriter(Config.CommentFile, _logger);
        }

        if (Config.ResourceFile != null)
        {
            writerResource = new SqlFileWriter(Config.ResourceFile, _logger);
        }

        var appName = classes.First().Namespace.App;

        writerCrebas.WriteLine("-- =========================================================================================== ");
        writerCrebas.WriteLine($"--   Application Name	:	{appName} ");
        writerCrebas.WriteLine("--   Script Name		:	" + Config.CrebasFile.Split('/').Last());
        writerCrebas.WriteLine("--   Description		:	Script de création des tables.");
        writerCrebas.WriteLine("-- =========================================================================================== ");

        writerUk?.WriteLine("-- =========================================================================================== ");
        writerUk?.WriteLine($"--   Application Name	:	{appName} ");
        writerUk?.WriteLine("--   Script Name		:	" + Config.UniqueKeysFile?.Split('/').Last());
        writerUk?.WriteLine("--   Description		:	Script de création des contraintes d'unicité.");
        writerUk?.WriteLine("-- =========================================================================================== ");

        writerType?.WriteLine("-- =========================================================================================== ");
        writerType?.WriteLine($"--   Application Name	:	{appName} ");
        writerType?.WriteLine("--   Script Name		:	" + Config.TypeFile?.Split('/').Last());
        writerType?.WriteLine("--   Description		:	Script de création des types. ");
        writerType?.WriteLine("-- =========================================================================================== ");

        writerComment?.WriteLine("-- =========================================================================================== ");
        writerComment?.WriteLine($"--   Application Name	:	{appName} ");
        writerComment?.WriteLine("--   Script Name		:	" + Config.CommentFile?.Split('/').Last());
        writerComment?.WriteLine("--   Description		:	Script de création des commentaires. ");
        writerComment?.WriteLine("-- =========================================================================================== ");

        writerResource?.WriteLine("-- =========================================================================================== ");
        writerResource?.WriteLine($"--   Application Name	:	{appName} ");
        writerResource?.WriteLine("--   Script Name		:	" + Config.ResourceFile?.Split('/').Last());
        writerResource?.WriteLine("--   Description		:	Script de création des resources (libellés traduits). ");
        writerResource?.WriteLine("-- =========================================================================================== ");

        var foreignKeys = classes
            .OrderBy(c => c.SqlName)
            .Where(c => c.IsPersistent && !c.Abstract && classes.Contains(c))
            .SelectMany(classe => WriteTableDeclaration(classe, writerCrebas, writerUk, writerType, writerComment, writerResource, classes.ToList()))
            .ToList();

        if (_config.TranslateProperties == true || _config.TranslateReferences == true)
        {
            WriteResourceTableDeclaration(writerCrebas);
        }

        writerType?.Dispose();
        writerUk?.Dispose();
        writerComment?.Dispose();
        writerResource?.Dispose();

        using var writer = new SqlFileWriter(Config.IndexFKFile, _logger);

        writer.WriteLine("-- =========================================================================================== ");
        writer.WriteLine($"--   Application Name	:	{appName} ");
        writer.WriteLine("--   Script Name		:	" + Config.IndexFKFile.Split('/').Last());
        writer.WriteLine("--   Description		:	Script de création des indexes et des clef étrangères. ");
        writer.WriteLine("-- =========================================================================================== ");

        foreach (var fkProperty in foreignKeys)
        {
            GenerateIndexForeignKey(fkProperty, writer);
            GenerateConstraintForeignKey(fkProperty, writer);
        }

        if ((_config.TranslateReferences == true || _config.TranslateProperties == true) && _config.ResourcesTableName != null)
        {
            var resourceProperties = classes.Where(c => c.DefaultProperty != null && c.Values.Count() > 0 && c.Enum).Select(c => c.DefaultProperty!);
            foreach (var fkProperty in resourceProperties)
            {
                GenerateIndexForeignKey(fkProperty, writer);
            }
        }
    }

    /// <summary>
    /// Lève une ArgumentException si l'identifiant est trop long.
    /// </summary>
    /// <param name="identifier">Identifiant à vérifier.</param>
    /// <returns>Identifiant passé en paramètre.</returns>
    protected static string CheckIdentifierLength(string identifier)
    {
        return identifier.Length > IdentifierLengthLimit
            ? throw new ArgumentException($"Le nom {identifier} est trop long ({identifier.Length} caractères). Limite: {IdentifierLengthLimit} caractères.")
            : identifier;
    }

    /// <summary>
    /// Crée un dictionnaire { nom de la propriété => valeur } pour un item à insérer.
    /// </summary>
    /// <param name="modelClass">Modele de la classe.</param>
    /// <param name="initItem">Item a insérer.</param>
    /// <param name="availableClasses">Classes disponibles.</param>
    /// <returns>Dictionnaire contenant { nom de la propriété => valeur }.</returns>
    protected Dictionary<string, string?> CreatePropertyValueDictionary(Class modelClass, ClassValue initItem, IEnumerable<Class> availableClasses)
    {
        var nameValueDict = new Dictionary<string, string?>();
        var definition = initItem.Value;
        foreach (var property in modelClass.Properties)
        {
            if (!property.PrimaryKey || !property.Domain.AutoGeneratedValue)
            {
                definition.TryGetValue(property, out var value);
                nameValueDict[property.SqlName] = _config.GetValue(property, availableClasses, value);

                if (_config.TranslateReferences == true && modelClass.DefaultProperty == property && !_config.CanClassUseEnums(modelClass, prop: property))
                {
                    nameValueDict[property.SqlName] = $@"'{initItem.ResourceKey}'";
                }
            }
        }

        return nameValueDict;
    }

    protected abstract void WriteComments(SqlFileWriter writerCrebas, Class classe, string tableName, List<IProperty> properties);

    /// <summary>
    /// Gère l'auto-incrémentation des clés primaires.
    /// </summary>
    /// <param name="writerCrebas">Flux d'écriture création bases.</param>
    protected abstract void WriteIdentityColumn(SqlFileWriter writerCrebas);

    protected virtual void WriteInsertEnd(SqlFileWriter writerInsert)
    {
    }

    protected virtual void WriteInsertStart(SqlFileWriter writerInsert)
    {
    }

    /// <summary>
    /// Ecrit dans le writer le script de création du type.
    /// </summary>
    /// <param name="classe">Classe.</param>
    /// <param name="writerType">Writer.</param>
    protected virtual void WriteType(Class classe, SqlFileWriter? writerType)
    {
    }

    /// <summary>
    /// Génère la contrainte de clef étrangère.
    /// </summary>
    /// <param name="property">Propriété portant la clef étrangère.</param>
    /// <param name="writer">Flux d'écriture.</param>
    private void GenerateConstraintForeignKey(AssociationProperty property, SqlFileWriter writer)
    {
        GenerateConstraintForeignKey(property, property.Property, property.Association, writer);
    }

    /// <summary>
    /// Génère la contrainte de clef étrangère.
    /// </summary>
    /// <param name="propertySource">Propriété portant la clef étrangère.</param>
    /// <param name="propertyTarget">Propriété destination de la contrainte.</param>
    /// <param name="association">Association destination de la clef étrangère.</param>
    /// <param name="writer">Flux d'écriture.</param>
    private void GenerateConstraintForeignKey(IProperty propertySource, IProperty propertyTarget, Class association, SqlFileWriter writer)
    {
        var tableName = propertySource.Class.SqlName;
        var propertyName = propertySource.SqlName;
        writer.WriteLine("/**");
        writer.WriteLine("  * Génération de la contrainte de clef étrangère pour " + tableName + "." + propertyName);
        writer.WriteLine(" **/");
        writer.WriteLine("alter table " + Quote(tableName));
        var constraintName = Quote($"FK_{propertySource.Class.SqlName}_{propertyName}");

        writer.WriteLine("\tadd constraint " + constraintName + " foreign key (" + Quote(propertyName) + ")");
        writer.Write("\t\treferences " + Quote(association.SqlName) + " (");

        writer.Write(Quote(propertyTarget.SqlName));

        writer.WriteLine($"){BatchSeparator}");
        writer.WriteLine();
    }

    /// <summary>
    /// Génère l'index portant sur la clef étrangère.
    /// </summary>
    /// <param name="property">Propriété cible de l'index.</param>
    /// <param name="writer">Flux d'écriture.</param>
    private void GenerateIndexForeignKey(IProperty property, SqlFileWriter writer)
    {
        var tableName = Quote(property.Class.SqlName);
        var propertyName = property.SqlName;
        writer.WriteLine("/**");
        writer.WriteLine("  * Création de l'index de clef étrangère pour " + tableName + "." + propertyName);
        writer.WriteLine(" **/");
        writer.WriteLine("create index " + Quote("IDX_" + (property.Class.Trigram ?? property.Class.SqlName) + "_" + propertyName + "_FK") + " on " + tableName + " (");
        writer.WriteLine("\t" + Quote(propertyName) + " ASC");
        writer.WriteLine($"){BatchSeparator}");
        writer.WriteLine();
    }

    /// <summary>
    /// Retourne la ligne d'insert.
    /// </summary>
    /// <param name="tableName">Nom de la table dans laquelle ajouter la ligne.</param>
    /// <param name="propertyValuePairs">Dictionnaire au format {nom de la propriété => valeur}.</param>
    /// <returns>La requête "INSERT INTO ..." générée.</returns>
    private string GetInsertLine(string tableName, Dictionary<string, string?> propertyValuePairs)
    {
        var sb = new StringBuilder();
        sb.Append("INSERT INTO " + Quote(tableName) + "(");
        var isFirst = true;
        foreach (var columnName in propertyValuePairs.Keys)
        {
            if (!isFirst)
            {
                sb.Append(", ");
            }

            isFirst = false;
            sb.Append(Quote(columnName));
        }

        sb.Append(") VALUES(");

        isFirst = true;
        foreach (var value in propertyValuePairs.Values)
        {
            if (!isFirst)
            {
                sb.Append(", ");
            }

            isFirst = false;
            sb.Append(string.IsNullOrEmpty(value) ? "null" : value);
        }

        sb.Append(");");
        return sb.ToString();
    }

    /// <summary>
    /// Retourne la ligne d'insert.
    /// </summary>
    /// <param name="modelClass">Modele de la classe.</param>
    /// <param name="initItem">Item a insérer.</param>
    /// <param name="availableClasses">Classes disponibles.</param>
    /// <returns>Requête.</returns>
    private string GetInsertLine(Class modelClass, ClassValue initItem, IEnumerable<Class> availableClasses)
    {
        var propertyValueDict = CreatePropertyValueDictionary(modelClass, initItem, availableClasses);
        return GetInsertLine(modelClass.SqlName, propertyValueDict);
    }

    private string Quote(string name)
    {
        return !UseQuotes ? name : $@"""{name}""";
    }

    private string SingleQuote(string name)
    {
        return $@"'{name.Replace("'", "''")}'";
    }

    /// <summary>
    /// Ecrit dans le writer le script d'insertion dans la table staticTable ayant pour model modelClass.
    /// </summary>
    /// <param name="writer">Writer.</param>
    /// <param name="modelClass">Modele de la classe.</param>
    /// <param name="availableClasses">Classes disponibles.</param>
    private void WriteInsert(SqlFileWriter writer, Class modelClass, IEnumerable<Class> availableClasses)
    {
        writer.WriteLine("/**\t\tInitialisation de la table " + modelClass.SqlName + "\t\t**/");
        foreach (var initItem in modelClass.Values)
        {
            writer.WriteLine(GetInsertLine(modelClass, initItem, availableClasses));
        }

        writer.WriteLine();
    }

    /// <summary>
    /// Ajoute les contraintes de clés primaires.
    /// </summary>
    /// <param name="writerCrebas">Writer.</param>
    /// <param name="classe">Classe.</param>
    private void WritePrimaryKeyConstraint(SqlFileWriter writerCrebas, Class classe, List<IProperty> properties)
    {
        if (!properties.Any(p => p.PrimaryKey))
        {
            writerCrebas.WriteLine(")");
            return;
        }

        writerCrebas.Write("\tconstraint " + Quote("PK_" + classe.SqlName) + " primary key ");
        if (SupportsClusteredKey)
        {
            writerCrebas.Write("clustered ");
        }

        writerCrebas.WriteLine($"({string.Join(",", properties.Where(p => p.PrimaryKey).Select(pk => Quote(pk.SqlName)))})");
        writerCrebas.WriteLine($"){BatchSeparator}");
    }

    private void WriteResourceTableDeclaration(SqlFileWriter writer)
    {
        if (_config.ResourcesTableName != null)
        {
            var tableName = Quote(_config.ResourcesTableName);
            writer.WriteLine("/**");
            writer.WriteLine("  * Création de ta table " + tableName + " contenant les traductions");
            writer.WriteLine(" **/");
            writer.WriteLine($"create table {_config.ResourcesTableName} (");
            writer.WriteLine(1, "RESOURCE_KEY varchar(255),");
            var hasLocale = _translationStore.Translations.Keys.Count > 1 || _translationStore.Translations.Keys.Any(a => a != string.Empty);
            if (hasLocale)
            {
                writer.WriteLine(1, "LOCALE varchar(10),");
            }

            writer.WriteLine(1, "LABEL varchar(4000),");
            writer.WriteLine(1, $"constraint PK_{_config.ResourcesTableName.ToConstantCase()} primary key (RESOURCE_KEY, LOCALE)");
            writer.WriteLine($"){BatchSeparator}");

            writer.WriteLine("/**");
            writer.WriteLine("  * Création de l'index pour " + tableName + " (RESOURCE_KEY, LOCALE)");
            writer.WriteLine(" **/");
            writer.WriteLine("create index " + Quote($"IDX_{_config.ResourcesTableName}_RESOURCE_KEY{(hasLocale ? "_LOCALE" : string.Empty)}") + " on " + tableName + " (");
            writer.WriteLine("\t" + $"RESOURCE_KEY{(hasLocale ? ", LOCALE" : string.Empty)}" + " ASC");
            writer.WriteLine($"){BatchSeparator}");
            writer.WriteLine();
        }
    }

    private void WriteResources(SqlFileWriter writer, Class modelClass)
    {
        var hasLocale = _translationStore.Translations.Keys.Count > 1 || _translationStore.Translations.Keys.Any(a => a != string.Empty);
        if (_config.TranslateProperties == true && modelClass.Properties.Where(p => p.Label != null).Count() > 0 && modelClass.ModelFile != null)
        {
            writer.WriteLine();
            writer.WriteLine("/**\t\tInitialisation des traductions des propriétés de la table " + modelClass.SqlName + "\t\t**/");

            foreach (var lang in _translationStore.Translations.Keys)
            {
                foreach (var property in modelClass.Properties.Where(p => p.Label != null))
                {
                    writer.WriteLine($@"INSERT INTO {_config.ResourcesTableName}(RESOURCE_KEY{(hasLocale ? ", LOCALE" : string.Empty)}, LABEL) VALUES({SingleQuote(property.ResourceKey)}{(string.IsNullOrEmpty(lang) ? string.Empty : @$", {SingleQuote(lang)}")}, {SingleQuote(_translationStore.GetTranslation(property, lang))});");
                }
            }
        }

        if (modelClass.DefaultProperty != null && modelClass.Values.Count() > 0 && _config.TranslateReferences == true)
        {
            writer.WriteLine();
            writer.WriteLine("/**\t\tInitialisation des traductions des valeurs de la table " + modelClass.SqlName + "\t\t**/");
            foreach (var lang in _translationStore.Translations.Keys)
            {
                foreach (var val in modelClass.Values)
                {
                    writer.WriteLine(@$"INSERT INTO {_config.ResourcesTableName}(RESOURCE_KEY{(hasLocale ? ", LOCALE" : string.Empty)}, LABEL) VALUES({SingleQuote(val.ResourceKey)}{(string.IsNullOrEmpty(lang) ? string.Empty : @$", {SingleQuote(lang)}")}, {SingleQuote(_translationStore.GetTranslation(val, lang))});");
                }
            }
        }
    }

    private void WriteSequence(Class classe, SqlFileWriter writerCrebas, string tableName)
    {
        writerCrebas.WriteLine("/**");
        writerCrebas.WriteLine($"  * Création de la séquence pour la clé primaire de la table {tableName}");
        writerCrebas.WriteLine(" **/");
        writerCrebas.Write($"create sequence SEQ_{tableName} as {_config.GetType(classe.PrimaryKey.Single()).ToUpper()}");

        if (Config.Identity.Start != null)
        {
            writerCrebas.Write($"{$" start {Config.Identity.Start}"}");
        }

        if (Config.Identity.Increment != null)
        {
            writerCrebas.Write($"{$" increment {Config.Identity.Increment}"}");
        }

        writerCrebas.Write($" owned by {tableName}.{classe.PrimaryKey.Single().SqlName}");

        writerCrebas.WriteLine(BatchSeparator);
        writerCrebas.WriteLine();
    }

    /// <summary>
    /// Déclaration de la table.
    /// </summary>
    /// <param name="classe">La table à ecrire.</param>
    /// <param name="writerCrebas">Flux d'écriture crebas.</param>
    /// <param name="writerUk">Flux d'écriture Unique Key.</param>
    /// <param name="writerType">Flux d'écritures des types.</param>
    /// <param name="writerComment">Flux d'écritures des commentaires.</param>
    /// <param name="writerResources">Flux d'écritures des commentaires.</param>
    /// <returns>Liste des propriétés étrangères persistentes.</returns>
    private IEnumerable<AssociationProperty> WriteTableDeclaration(Class classe, SqlFileWriter writerCrebas, SqlFileWriter? writerUk, SqlFileWriter? writerType, SqlFileWriter? writerComment, SqlFileWriter? writerResources, IList<Class> availableClasses)
    {
        var fkPropertiesList = new List<AssociationProperty>();

        var tableName = Quote(CheckIdentifierLength(classe.SqlName));

        writerCrebas.WriteLine("/**");
        writerCrebas.WriteLine("  * Création de la table " + tableName);
        writerCrebas.WriteLine(" **/");
        writerCrebas.WriteLine("create table " + tableName + " (");

        var isContainsInsertKey = writerType != null && classe.Properties.Any(p => p.Name == InsertKeyName);
        if (isContainsInsertKey)
        {
            WriteType(classe, writerType);
        }

        var properties = classe.Properties.Where(p => p is not AssociationProperty ap || ap.Type == AssociationType.ManyToOne || ap.Type == AssociationType.OneToOne).ToList();
        var t = 0;
        var classes = availableClasses.Distinct();
        if (classe.Extends != null)
        {
            properties.Add(new AssociationProperty
            {
                Association = classe.Extends,
                Class = classe,
                Comment = "Association vers la clé primaire de la classe parente",
                Required = true,
                PrimaryKey = !classe.PrimaryKey.Any()
            });
        }

        var oneToManyProperties = classes.SelectMany(cl => cl.Properties).Where(p => p is AssociationProperty ap && ap.Type == AssociationType.OneToMany && ap.Association == classe).Select(p => (AssociationProperty)p);
        foreach (var ap in oneToManyProperties)
        {
            var asp = new AssociationProperty()
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
            properties.Add(asp);
        }

        foreach (var property in properties)
        {
            var persistentType = property is not CompositionProperty ? _config.GetType(property, availableClasses) : JsonType;

            if (persistentType.ToLower().Equals("varchar") && property.Domain.Length != null)
            {
                persistentType = $"{persistentType}({property.Domain.Length})";
            }

            if ((persistentType.ToLower().Equals("numeric") || persistentType.ToLower().Equals("decimal")) && property.Domain.Length != null)
            {
                persistentType = $"{persistentType}({property.Domain.Length}{(property.Domain.Scale != null ? $", {property.Domain.Scale}" : string.Empty)})";
            }

            writerCrebas.Write("\t" + Quote(CheckIdentifierLength(property.SqlName)) + " " + persistentType);
            if (property is not AssociationProperty && property.PrimaryKey && property.Domain.AutoGeneratedValue && persistentType.Contains("int") && Config.Identity.Mode == IdentityMode.IDENTITY)
            {
                WriteIdentityColumn(writerCrebas);
            }

            if (isContainsInsertKey && !property.PrimaryKey && property.Name != InsertKeyName)
            {
                if (t > 0)
                {
                    writerType?.Write(",");
                    writerType?.WriteLine();
                }

                writerType?.Write("\t" + property.SqlName + " " + persistentType);
                t++;
            }

            if (property.Required)
            {
                writerCrebas.Write(" not null");
            }

            var defaultValue = _config.GetValue(property, availableClasses);
            if (defaultValue != "null")
            {
                writerCrebas.Write($" default {defaultValue}");
            }

            writerCrebas.Write(",");
            writerCrebas.WriteLine();

            if (property is AssociationProperty { Association.IsPersistent: true } ap)
            {
                fkPropertiesList.Add(ap);
            }
        }

        WriteUniqueKeys(classe, writerUk);
        WritePrimaryKeyConstraint(writerCrebas, classe, properties);
        if (writerComment is not null)
        {
            WriteComments(writerComment, classe, tableName, properties);
        }

        if (writerResources is not null)
        {
            WriteResources(writerResources, classe);
        }

        writerCrebas.WriteLine();

        if (isContainsInsertKey)
        {
            if (writerType == null)
            {
                throw new ArgumentNullException(nameof(Config.TypeFile));
            }

            if (t > 0)
            {
                writerType.Write(',');
                writerType.WriteLine();
            }

            writerType.WriteLine('\t' + classe.Trigram + "_INSERT_KEY int");
            writerType.WriteLine();
            writerType.WriteLine($"){BatchSeparator}");
            writerType.WriteLine();
        }

        var shouldWriteSequence = Config.Identity.Mode == IdentityMode.SEQUENCE && classe.PrimaryKey.Count() == 1 && classe.PrimaryKey.Single().Domain.AutoGeneratedValue && !_config.GetType(classe.PrimaryKey.Single()).ToLower().Contains("varchar");
        if (shouldWriteSequence)
        {
            WriteSequence(classe, writerCrebas, tableName);
        }

        return fkPropertiesList;
    }

    /// <summary>
    /// Ajoute les contraintes d'unicité.
    /// </summary>
    /// <param name="classe">Classe.</param>
    /// <param name="writerUk">Writer.</param>
    private void WriteUniqueKeys(Class classe, SqlFileWriter? writerUk)
    {
        foreach (var uk in classe.UniqueKeys
            .Concat(classe.Properties.OfType<AssociationProperty>().Where(ap => ap.Type == AssociationType.OneToOne).Select(ap => new List<IProperty> { ap })))
        {
            writerUk?.WriteLine($"alter table {classe.SqlName} add constraint {Quote($"UK_{classe.SqlName}_{string.Join("_", uk.Select(p => p.SqlName))}")} unique ({string.Join(", ", uk.Select(p => Quote(p.SqlName)))}){BatchSeparator}");
            writerUk?.WriteLine();
        }
    }
}