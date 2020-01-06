using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Kinetix.NewGenerator.Model;
using Newtonsoft.Json;

namespace Kinetix.NewGenerator.Loaders
{
    public static class ReferenceListsLoader
    {
        public static IEnumerable<(string className, IEnumerable<ReferenceValue> values)> LoadReferenceLists(string referenceListsFile)
        {
            var file = File.ReadAllText(referenceListsFile);
            return JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, Dictionary<string, object>>>>(file)
                .Select(reference => (
                    reference.Key, 
                    reference.Value
                        .Select(item => new ReferenceValue { Name = item.Key, Bean = item.Value })
                        .OrderBy(item => item.Name)
                        .AsEnumerable()))
                .OrderBy(r => r.Key);
        }

        public static void AddReferenceValues(Class classe, IEnumerable<ReferenceValue> values)
        {
            classe.ReferenceValues = values.ToDictionary(
                v => v.Name,
                v => {
                    if (!v.Bean.TryGetValue(classe.PrimaryKey!.Name, out var code))
                    {
                        throw new Exception($"L'initialisation de {classe.Name} pour {v.Name} n'a pas de clé primaire ({classe.PrimaryKey!.Name})");
                    }
                    var label = v.Name;
                    if (classe.LabelProperty != null)
                    {
                        if (v.Bean.TryGetValue(classe.LabelProperty.Name, out var trueLabel))
                        {
                            label = (string)trueLabel;
                        }
                    }
                    if (code.GetType() == typeof(string))
                    {
                        code = "\"" + code + "\"";
                    }
                    return (
                        code.ToString()!,
                        label);
                    });
        }
    }
}
