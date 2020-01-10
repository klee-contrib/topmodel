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
        public static IEnumerable<(string className, IEnumerable<ReferenceValue> values)>? LoadReferenceLists(string? referenceListsFile)
        {
            if (referenceListsFile == null)
            {
                return null;
            }

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
                v =>
                {
                    object? code = null;
                    switch (classe.Stereotype)
                    {
                        case null:
                            throw new Exception($"La classe {classe.Name} n'est pas une classe de référence");
                        case Stereotype.Statique:
                            if (!v.Bean.TryGetValue(classe.PrimaryKey!.Name, out code))
                            {
                                throw new Exception($"L'initialisation de {classe.Name} pour {v.Name} n'a pas de clé primaire ({classe.PrimaryKey!.Name})");
                            }
                            break;
                        case Stereotype.Reference:
                            var uniqueKey = classe.Properties.OfType<RegularProperty>().FirstOrDefault(p => p.Unique);
                            if (uniqueKey == null)
                            {
                                throw new Exception($"La classe {classe.Name} de stéréotype 'Reference' n'a pas de propriété unique.");
                            }
                            if (!v.Bean.TryGetValue(uniqueKey.Name, out code))
                            {
                                throw new Exception($"L'initialisation de {classe.Name} pour {v.Name} n'a pas de propriété unique ({uniqueKey.Name})");
                            }
                            break;
                    }

                    var label = v.Name;
                    if (classe.LabelProperty != null)
                    {
                        if (v.Bean.TryGetValue(classe.LabelProperty.Name, out var trueLabel))
                        {
                            label = (string)trueLabel;
                        }
                    }
                    if (code!.GetType() == typeof(string))
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
