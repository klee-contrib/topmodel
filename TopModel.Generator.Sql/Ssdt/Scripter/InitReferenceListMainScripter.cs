using TopModel.Core;

namespace TopModel.Generator.Sql.Ssdt.Scripter;

/// <summary>
/// Scripter écrivant un script qui ordonnance l'appel aux scripts d'insertions de valeurs de listes de références.
/// </summary>
public class InitReferenceListMainScripter : ISqlScripter<IEnumerable<Class>>
{
    private readonly SqlConfig _config;

    public InitReferenceListMainScripter(SqlConfig config)
    {
        _config = config;
    }

    /// <summary>
    /// Calcule le nom du script pour l'item.
    /// </summary>
    /// <param name="item">Item à scripter.</param>
    /// <returns>Nom du fichier de script.</returns>
    public string GetScriptName(IEnumerable<Class> item)
    {
        return _config.Ssdt!.InitListMainScriptName!;
    }

    /// <summary>
    /// Ecrit dans un flux le script pour l'item.
    /// </summary>
    /// <param name="writer">Flux d'écriture.</param>
    /// <param name="item">Table à scripter.</param>
    /// <param name="availableClasses">Classes disponibles.</param>
    public void WriteItemScript(TextWriter writer, IEnumerable<Class> item, IEnumerable<Class> availableClasses)
    {
        if (writer == null)
        {
            throw new ArgumentNullException(nameof(writer));
        }

        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        // Entête du fichier.
        WriteHeader(writer);

        // Appel des scripts d'insertion.
        WriteScriptCalls(writer, item);
    }

    /// <summary>
    /// Ecrit l'entête du fichier.
    /// </summary>
    /// <param name="writer">Flux.</param>
    private static void WriteHeader(TextWriter writer)
    {
        writer.WriteLine("-- ===========================================================================================");
        writer.WriteLine("--   Description		:	Insertion des valeurs de listes statiques.");
        writer.WriteLine("-- ===========================================================================================");
        writer.WriteLine();
    }

    /// <summary>
    /// Ecrit les appels de scripts.
    /// </summary>
    /// <param name="writer">Flux.</param>
    /// <param name="classSet">Ensemble des listes de référence.</param>
    private static void WriteScriptCalls(TextWriter writer, IEnumerable<Class> classSet)
    {
        foreach (var classe in classSet)
        {
            var subscriptName = classe.SqlName + ".insert.sql";
            writer.WriteLine("/* Insertion dans la table " + classe.SqlName + ". */");
            writer.WriteLine(":r .\\" + subscriptName);
            writer.WriteLine();
        }
    }
}