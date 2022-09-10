using System.Text;
using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Generator.Ssdt;

namespace TopModel.Generator.ProceduralSql;

/// <summary>
/// Classe abstraite de génération des sripts de création.
/// </summary>
public abstract class AbstractSchemaGenerator
{
    /// <summary>
    /// Nom pour l'insert en bulk.
    /// </summary>
    protected const string InsertKeyName = "InsertKey";

    private readonly ProceduralSqlConfig _config;
    private readonly ILogger<ProceduralSqlGenerator> _logger;

    public AbstractSchemaGenerator(ProceduralSqlConfig config, ILogger<ProceduralSqlGenerator> logger)
    {
        _config = config;
        _logger = logger;
    }

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
        if (_config.InitListFile == null || !classes.Any())
        {
            return;
        }

        using var writerInsert = new SqlFileWriter(_config.InitListFile, _logger);

        writerInsert.WriteLine("-- =========================================================================================== ");
        writerInsert.WriteLine($"--   Application Name	:	{classes.First().Namespace.App} ");
        writerInsert.WriteLine("--   Script Name		:	" + _config.InitListFile.Split("\\").Last());
        writerInsert.WriteLine("--   Description		:	Script d'insertion des données de références");
        writerInsert.WriteLine("-- ===========================================================================================");

        // Construit la liste des Reference Class ordonnée.
        var orderList = SortUtils.Sort(classes.OrderBy(c => c.Name), c => c.Properties
            .OfType<AssociationProperty>()
            .Select(a => a.Association)
            .Where(a => a.ReferenceValues.Any()));

        foreach (var classe in orderList)
        {
            WriteInsert(writerInsert, classe);
        }
    }

    /// <summary>
    /// Génère le script SQL.
    /// </summary>
    /// <param name="classes">Classes.</param>
    public void GenerateSchemaScript(IEnumerable<Class> classes)
    {
        var outputFileNameCrebas = _config.CrebasFile;
        var outputFileNameIndex = _config.IndexFKFile;
        var outputFileNameType = _config.TypeFile;
        var outputFileNameUK = _config.UniqueKeysFile;

        if (outputFileNameCrebas == null || outputFileNameIndex == null || !classes.Any())
        {
            return;
        }

        using var writerCrebas = new SqlFileWriter(outputFileNameCrebas, _logger);

        SqlFileWriter? writerType = null;
        SqlFileWriter? writerUk = null;

        if (outputFileNameType != null)
        {
            writerType = new SqlFileWriter(outputFileNameType, _logger);
        }

        if (outputFileNameUK != null)
        {
            writerUk = new SqlFileWriter(outputFileNameUK, _logger);
        }

        var appName = classes.First().Namespace.App;

        writerCrebas.WriteLine("-- =========================================================================================== ");
        writerCrebas.WriteLine($"--   Application Name	:	{appName} ");
        writerCrebas.WriteLine("--   Script Name		:	" + outputFileNameCrebas.Split("\\").Last());
        writerCrebas.WriteLine("--   Description		:	Script de création des tables.");
        writerCrebas.WriteLine("-- =========================================================================================== ");

        writerUk?.WriteLine("-- =========================================================================================== ");
        writerUk?.WriteLine($"--   Application Name	:	{appName} ");
        writerUk?.WriteLine("--   Script Name		:	" + outputFileNameUK?.Split("\\").Last());
        writerUk?.WriteLine("--   Description		:	Script de création des index uniques.");
        writerUk?.WriteLine("-- =========================================================================================== ");

        writerType?.WriteLine("-- =========================================================================================== ");
        writerType?.WriteLine($"--   Application Name	:	{appName} ");
        writerType?.WriteLine("--   Script Name		:	" + outputFileNameType?.Split("\\").Last());
        writerType?.WriteLine("--   Description		:	Script de création des types. ");
        writerType?.WriteLine("-- =========================================================================================== ");

        var foreignKeys = classes
            .OrderBy(c => c.Name)
            .Where(c => c.IsPersistent)
            .SelectMany(classe => WriteTableDeclaration(classe, writerCrebas, writerUk, writerType, classes.ToList()))
            .ToList();

        if (writerType != null)
        {
            writerType.Dispose();
        }

        if (writerUk != null)
        {
            writerUk.Dispose();
        }

        using var writer = new SqlFileWriter(outputFileNameIndex, _logger);

        writer.WriteLine("-- =========================================================================================== ");
        writer.WriteLine($"--   Application Name	:	{appName} ");
        writer.WriteLine("--   Script Name		:	" + outputFileNameIndex.Split("\\").Last());
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
    protected Dictionary<string, string?> CreatePropertyValueDictionary(Class modelClass, ReferenceValue initItem)
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
                    null => "null",
                    string s when property.Domain.SqlType!.Contains("varchar") || property.Domain.SqlType!.Contains("timestamp") => $"'{ScriptUtils.PrepareDataToSqlDisplay(s)}'",
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
        var constraintName = Quote("FK_" + (property.Class.Trigram ?? property.Class.SqlName) + "_" + propertyName);

        writer.WriteLine("\tadd constraint " + constraintName + " foreign key (" + Quote(propertyName) + ")");
        writer.Write("\t\treferences " + Quote(property.Association.SqlName) + " (");

        writer.Write(Quote(property.Property.SqlName));

        writer.WriteLine(")");
        writer.WriteLine(BatchSeparator);
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
        writer.WriteLine(")");
        writer.WriteLine(BatchSeparator);
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
    private string GetInsertLine(Class modelClass, ReferenceValue initItem)
    {
        var propertyValueDict = CreatePropertyValueDictionary(modelClass, initItem);
        return GetInsertLine(modelClass.SqlName, propertyValueDict);
    }

    /// <summary>
    /// Ecrit dans le writer le script d'insertion dans la table staticTable ayant pour model modelClass.
    /// </summary>
    /// <param name="writer">Writer.</param>
    /// <param name="modelClass">Modele de la classe.</param>
    private void WriteInsert(SqlFileWriter writer, Class modelClass)
    {
        writer.WriteLine("/**\t\tInitialisation de la table " + modelClass.Name + "\t\t**/");
        foreach (var initItem in modelClass.ReferenceValues)
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
        var pkCount = 0;

        if (!classe.Properties.Any(p => p.PrimaryKey) && !classe.Properties.All(p => p is AssociationProperty))
        {
            writerCrebas.WriteLine(")");
            return;
        }

        writerCrebas.Write("\tconstraint " + Quote("PK_" + classe.SqlName) + " primary key ");
        if (SupportsClusteredKey)
        {
            writerCrebas.Write("clustered ");
        }

        writerCrebas.Write("(");

        if (classe.Properties.All(p => p is AssociationProperty))
        {
            foreach (var fkProperty in classe.Properties.OfType<IFieldProperty>())
            {
                ++pkCount;
                writerCrebas.Write(Quote(fkProperty.SqlName));
                if (pkCount < classe.Properties.Count)
                {
                    writerCrebas.Write(",");
                }
                else
                {
                    writerCrebas.WriteLine(")");
                }
            }
        }
        else
        {
            writerCrebas.Write(string.Join(", ", classe.Properties.OfType<IFieldProperty>().Where(p => p.PrimaryKey).Select(p => Quote(p.SqlName))));
            writerCrebas.WriteLine(")");
        }

        writerCrebas.WriteLine(")");
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

        if (classe.PrimaryKey != null && classe.PrimaryKey.Domain.AutoGeneratedValue && !classe.PrimaryKey.Domain.SqlType!.ToLower().Contains("varchar"))
        {
            writerCrebas.WriteLine("/**");
            writerCrebas.WriteLine($"  * Création de la séquence pour la clé primaire de la table {tableName}");
            writerCrebas.WriteLine(" **/");
            writerCrebas.WriteLine($"create sequence SEQ_{tableName} start 1000 increment 50");
            writerCrebas.WriteLine(BatchSeparator);
        }

        writerCrebas.WriteLine("/**");
        writerCrebas.WriteLine("  * Création de la table " + tableName);
        writerCrebas.WriteLine(" **/");
        writerCrebas.WriteLine("create table " + tableName + " (");

        var isContainsInsertKey = writerType != null && classe.Properties.Count(p => p.Name == InsertKeyName) > 0;
        if (isContainsInsertKey)
        {
            WriteType(classe, writerType);
        }

        var properties = classe.Properties.OfType<IFieldProperty>().Where(p => !(p is AssociationProperty ap) || ap.Type == AssociationType.ManyToOne || ap.Type == AssociationType.OneToOne).ToList();
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
            var persistentType = property.Domain.SqlType!;
            if (persistentType is null)
            {
                throw new ModelException(property.Domain, $"Le domaine {property.Domain.Name} n'a pas de définition du sqlType");
            }

            if (persistentType.ToLower().Equals("varchar") && property.Domain.Length != null)
            {
                persistentType = $"{persistentType}({property.Domain.Length})";
            }

            if ((persistentType.ToLower().Equals("numeric") || persistentType.ToLower().Equals("decimal")) && property.Domain.Length != null)
            {
                persistentType = $"{persistentType}({property.Domain.Length}{(property.Domain.Scale != null ? $", {property.Domain.Scale}" : string.Empty)})";
            }

            writerCrebas.Write("\t" + Quote(CheckIdentifierLength(property.SqlName)) + " " + persistentType);
            if (property.PrimaryKey && property.Domain.AutoGeneratedValue && persistentType.Contains("int") && !_config.DisableIdentity)
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

            if (property is { Domain: var domain, DefaultValue: var dv } && !string.IsNullOrWhiteSpace(dv))
            {
                if (domain.ShouldQuoteSqlValue)
                {
                    writerCrebas.Write($" default '{dv}'");
                }
                else
                {
                    writerCrebas.Write($" default {dv}");
                }
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
        writerCrebas.WriteLine(BatchSeparator);
        writerCrebas.WriteLine();

        if (isContainsInsertKey)
        {
            if (writerType == null)
            {
                throw new ArgumentNullException(nameof(_config.TypeFile));
            }

            if (t > 0)
            {
                writerType.Write(',');
                writerType.WriteLine();
            }

            writerType.WriteLine('\t' + classe.Trigram + "_INSERT_KEY int");
            writerType.WriteLine();
            writerType.WriteLine(")");
            writerType.WriteLine(BatchSeparator);
            writerType.WriteLine();
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
            .Concat(classe.Properties.OfType<AssociationProperty>().Where(ap => ap.Type == AssociationType.OneToOne).Select(ap => new List<IFieldProperty> { ap })))
        {
            writerUk?.Write($"alter table {classe.SqlName} add constraint {Quote($"UK_{classe.SqlName}_{string.Join("_", uk.Select(p => p.SqlName))}")} unique ({string.Join(", ", uk.Select(p => Quote(p.SqlName)))})");
            writerUk?.WriteLine(BatchSeparator);
            writerUk?.WriteLine();
        }
    }

    private string Quote(string name)
    {
        return !UseQuotes ? name : $@"""{name}""";
    }
}