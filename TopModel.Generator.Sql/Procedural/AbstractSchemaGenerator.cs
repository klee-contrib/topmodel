using System.Text;
using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Generator.Core;

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

    public AbstractSchemaGenerator(SqlConfig config, ILogger<ProceduralSqlGenerator> logger)
    {
        _config = config;
        _logger = logger;
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
    public void GenerateListInitScript(IEnumerable<Class> classes)
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
            .Where(a => a.Values.Any()));

        foreach (var classe in orderList)
        {
            WriteInsert(writerInsert, classe);
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

        if (Config.TypeFile != null)
        {
            writerType = new SqlFileWriter(Config.TypeFile, _logger);
        }

        if (Config.UniqueKeysFile != null)
        {
            writerUk = new SqlFileWriter(Config.UniqueKeysFile, _logger);
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
        writerUk?.WriteLine("--   Description		:	Script de création des index uniques.");
        writerUk?.WriteLine("-- =========================================================================================== ");

        writerType?.WriteLine("-- =========================================================================================== ");
        writerType?.WriteLine($"--   Application Name	:	{appName} ");
        writerType?.WriteLine("--   Script Name		:	" + Config.TypeFile?.Split('/').Last());
        writerType?.WriteLine("--   Description		:	Script de création des types. ");
        writerType?.WriteLine("-- =========================================================================================== ");

        var foreignKeys = classes
            .OrderBy(c => c.SqlName)
            .Where(c => c.IsPersistent && !c.Abstract && classes.Contains(c))
            .SelectMany(classe => WriteTableDeclaration(classe, writerCrebas, writerUk, writerType, classes.ToList()))
            .ToList();

        writerType?.Dispose();
        writerUk?.Dispose();

        using var writer = new SqlFileWriter(Config.IndexFKFile, _logger);

        writer.WriteLine("-- =========================================================================================== ");
        writer.WriteLine($"--   Application Name	:	{appName} ");
        writer.WriteLine("--   Script Name		:	" + Config.IndexFKFile.Split('/').Last());
        writer.WriteLine("--   Description		:	Script de création des indexes et des clef étrangères. ");
        writer.WriteLine("-- =========================================================================================== ");

        foreach (var fkProperty in foreignKeys)
        {
            GenerateIndexForeignKey(writer, fkProperty);
            GenerateConstraintForeignKey(fkProperty, writer);
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
    /// <returns>Dictionnaire contenant { nom de la propriété => valeur }.</returns>
    protected Dictionary<string, string?> CreatePropertyValueDictionary(Class modelClass, ClassValue initItem)
    {
        var nameValueDict = new Dictionary<string, string?>();
        var definition = initItem.Value;
        foreach (var property in modelClass.Properties.OfType<IFieldProperty>())
        {
            if (!property.PrimaryKey || !property.Domain.AutoGeneratedValue)
            {
                definition.TryGetValue(property, out var value);
                nameValueDict[property.SqlName] = value switch
                {
                    null or "null" => "null",
                    string s when _config.GetType(property).Contains("varchar") || _config.GetType(property).Contains("timestamp") => $"'{ScriptUtils.PrepareDataToSqlDisplay(s)}'",
                    object v => v.ToString()
                };
            }
        }

        return nameValueDict;
    }

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
        var tableName = property.Class.SqlName;
        var propertyName = ((IFieldProperty)property).SqlName;
        writer.WriteLine("/**");
        writer.WriteLine("  * Génération de la contrainte de clef étrangère pour " + tableName + "." + propertyName);
        writer.WriteLine(" **/");
        writer.WriteLine("alter table " + Quote(tableName));
        var constraintName = Quote($"FK_{property.Class.SqlName}_{propertyName}");

        writer.WriteLine("\tadd constraint " + constraintName + " foreign key (" + Quote(propertyName) + ")");
        writer.Write("\t\treferences " + Quote(property.Association.SqlName) + " (");

        writer.Write(Quote(property.Property.SqlName));

        writer.WriteLine($"){BatchSeparator}");
        writer.WriteLine();
    }

    /// <summary>
    /// Génère l'index portant sur la clef étrangère.
    /// </summary>
    /// <param name="writer">Flux d'écriture.</param>
    /// <param name="property">Propriété cible de l'index.</param>
    private void GenerateIndexForeignKey(SqlFileWriter writer, AssociationProperty property)
    {
        var tableName = Quote(property.Class.SqlName);
        var propertyName = ((IFieldProperty)property).SqlName;
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
    /// <returns>Requête.</returns>
    private string GetInsertLine(Class modelClass, ClassValue initItem)
    {
        var propertyValueDict = CreatePropertyValueDictionary(modelClass, initItem);
        return GetInsertLine(modelClass.SqlName, propertyValueDict);
    }

    private string Quote(string name)
    {
        return !UseQuotes ? name : $@"""{name}""";
    }

    /// <summary>
    /// Ecrit dans le writer le script d'insertion dans la table staticTable ayant pour model modelClass.
    /// </summary>
    /// <param name="writer">Writer.</param>
    /// <param name="modelClass">Modele de la classe.</param>
    private void WriteInsert(SqlFileWriter writer, Class modelClass)
    {
        writer.WriteLine("/**\t\tInitialisation de la table " + modelClass.SqlName + "\t\t**/");
        foreach (var initItem in modelClass.Values)
        {
            writer.WriteLine(GetInsertLine(modelClass, initItem));
        }

        writer.WriteLine();
    }

    /// <summary>
    /// Ajoute les contraintes de clés primaires.
    /// </summary>
    /// <param name="writerCrebas">Writer.</param>
    /// <param name="classe">Classe.</param>
    private void WritePrimaryKeyConstraint(SqlFileWriter writerCrebas, Class classe)
    {
        if (!classe.Properties.Any(p => p.PrimaryKey))
        {
            writerCrebas.WriteLine(")");
            return;
        }

        writerCrebas.Write("\tconstraint " + Quote("PK_" + classe.SqlName) + " primary key ");
        if (SupportsClusteredKey)
        {
            writerCrebas.Write("clustered ");
        }

        writerCrebas.WriteLine($"({string.Join(",", classe.PrimaryKey.Select(pk => Quote(pk.SqlName)))})");
        writerCrebas.WriteLine($"){BatchSeparator}");
    }

    /// <summary>
    /// Déclaration de la table.
    /// </summary>
    /// <param name="classe">La table à ecrire.</param>
    /// <param name="writerCrebas">Flux d'écriture crebas.</param>
    /// <param name="writerUk">Flux d'écriture Unique Key.</param>
    /// <param name="writerType">Flux d'écritures des types.</param>
    /// <returns>Liste des propriétés étrangères persistentes.</returns>
    private IEnumerable<AssociationProperty> WriteTableDeclaration(Class classe, SqlFileWriter writerCrebas, SqlFileWriter? writerUk, SqlFileWriter? writerType, IList<Class> availableClasses)
    {
        var fkPropertiesList = new List<AssociationProperty>();

        var tableName = Quote(CheckIdentifierLength(classe.SqlName));
        if (Config.Identity.Mode == IdentityMode.SEQUENCE && classe.PrimaryKey.Count() == 1 && classe.PrimaryKey.Single().Domain.AutoGeneratedValue && !_config.GetType(classe.PrimaryKey.Single()).ToLower().Contains("varchar"))
        {
            writerCrebas.WriteLine("/**");
            writerCrebas.WriteLine($"  * Création de la séquence pour la clé primaire de la table {tableName}");
            writerCrebas.WriteLine(" **/");
            writerCrebas.Write($"create sequence SEQ_{tableName}");

            if (Config.Identity.Start != null)
            {
                writerCrebas.Write($"{$" start {Config.Identity.Start}"}");
            }

            if (Config.Identity.Increment != null)
            {
                writerCrebas.Write($"{$" increment {Config.Identity.Increment}"}");
            }

            writerCrebas.WriteLine(BatchSeparator);
        }

        writerCrebas.WriteLine("/**");
        writerCrebas.WriteLine("  * Création de la table " + tableName);
        writerCrebas.WriteLine(" **/");
        writerCrebas.WriteLine("create table " + tableName + " (");

        var isContainsInsertKey = writerType != null && classe.Properties.Any(p => p.Name == InsertKeyName);
        if (isContainsInsertKey)
        {
            WriteType(classe, writerType);
        }

        var properties = classe.Properties.OfType<IFieldProperty>().Where(p => p is not AssociationProperty ap || ap.Type == AssociationType.ManyToOne || ap.Type == AssociationType.OneToOne).ToList();
        var t = 0;
        var classes = availableClasses.Distinct();

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
            var persistentType = _config.GetType(property);

            if (persistentType.ToLower().Equals("varchar") && property.Domain.Length != null)
            {
                persistentType = $"{persistentType}({property.Domain.Length})";
            }

            if ((persistentType.ToLower().Equals("numeric") || persistentType.ToLower().Equals("decimal")) && property.Domain.Length != null)
            {
                persistentType = $"{persistentType}({property.Domain.Length}{(property.Domain.Scale != null ? $", {property.Domain.Scale}" : string.Empty)})";
            }

            writerCrebas.Write("\t" + Quote(CheckIdentifierLength(property.SqlName)) + " " + persistentType);
            if (property.PrimaryKey && property.Domain.AutoGeneratedValue && persistentType.Contains("int") && Config.Identity.Mode == IdentityMode.IDENTITY)
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

            writerCrebas.Write(",");
            writerCrebas.WriteLine();

            if (property is AssociationProperty { Association.IsPersistent: true } ap)
            {
                fkPropertiesList.Add(ap);
            }
        }

        WriteUniqueKeys(classe, writerUk);
        WritePrimaryKeyConstraint(writerCrebas, classe);
        WriteComments(writerCrebas, classe, tableName, properties);

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

        return fkPropertiesList;
    }

    protected abstract void WriteComments(SqlFileWriter writerCrebas, Class classe, string tableName, List<IFieldProperty> properties);

    /// <summary>
    /// Ajoute les contraintes d'unicité.
    /// </summary>
    /// <param name="classe">Classe.</param>
    /// <param name="writerUk">Writer.</param>
    private void WriteUniqueKeys(Class classe, SqlFileWriter? writerUk)
    {
        foreach (var uk in classe.UniqueKeys
            .Concat(classe.Properties.OfType<AssociationProperty>().Where(ap => ap.Type == AssociationType.OneToOne).Select(ap => new List<IFieldProperty> { ap })))
        {
            writerUk?.WriteLine($"alter table {classe.SqlName} add constraint {Quote($"UK_{classe.SqlName}_{string.Join("_", uk.Select(p => p.SqlName))}")} unique ({string.Join(", ", uk.Select(p => Quote(p.SqlName)))}){BatchSeparator}");
            writerUk?.WriteLine();
        }
    }
}