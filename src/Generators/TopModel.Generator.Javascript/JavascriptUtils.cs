using TopModel.Core.Model;
using TopModel.Utils;

namespace TopModel.Generator.Javascript;

/// <summary>
/// Utilitaires pour la génération JavaScript.
/// </summary>
public static class JavascriptUtils
{
    /// <summary>
    /// Regroupe et trie les imports par chemin.
    /// </summary>
    /// <param name="imports">Imports à regrouper.</param>
    /// <returns>Imports regroupés et triés.</returns>
    public static IList<(string Import, string Path)> GroupAndSort(
        this IEnumerable<(string Import, string Path)> imports
    )
    {
        return imports
            .GroupBy(i => i.Path)
            .Select(i => (Import: string.Join(", ", i.Select(l => l.Import).Distinct().Order()), Path: i.Key))
            .OrderBy(i => i.Path.StartsWith('.') ? i.Path : $"...{i.Path}")
            .ToList();
    }

    /// <summary>
    /// Ecrit la définition d'une référence.
    /// </summary>
    /// <param name="fw">Writer.</param>
    /// <param name="classe">Classe de référence.</param>
    /// <param name="config">Config.</param>
    public static void WriteReferenceDefinition(this IFileWriter fw, Class classe, JavascriptConfig config)
    {
        fw.Write($"export const {classe.NameCamel} = {{");

        if (classe.Readonly)
        {
            fw.Write($"list: {classe.NameCamel}List,");
        }
        else
        {
            fw.Write($"type: {{}} as {classe.NamePascal},");
        }

        fw.WriteLine(
            $" valueKey: \"{classe.ReferenceKey!.NameCamel}\", labelKey: \"{classe.DefaultProperty?.NameCamel}\"}} as const;"
        );
    }
}
