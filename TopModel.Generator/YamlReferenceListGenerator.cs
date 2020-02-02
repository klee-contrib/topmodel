using System.Collections.Generic;
using System.Linq;
using TopModel.Core.FileModel;
using Microsoft.Extensions.Logging;

namespace TopModel.Generator
{
    public class YamlReferenceListGenerator : IModelWatcher
    {
        private readonly ILogger<YamlReferenceListGenerator> _logger;

        public YamlReferenceListGenerator(ILogger<YamlReferenceListGenerator> logger)
        {
            _logger = logger;
        }

        public string Name => "YAML références";

        public void OnFilesChanged(IEnumerable<ModelFile> files)
        {
            using var file = new FileWriter("yolo.yml", _logger);

            foreach (var refClass in files.SelectMany(f => f.Classes).Where(f => f.ReferenceValues != null).OrderBy(c => c.ModelFile.ToString()).ThenBy(c => c.Name))
            {
                file.WriteLine("---");
                file.WriteLine($"name: {refClass.Name}");
                file.WriteLine($"file: {refClass.ModelFile}");
                file.WriteLine("values:");

                var keyLength = refClass.ReferenceValues!.Select(v => v.Name).Max(n => n.Length);

                var maxLengths = refClass.ReferenceValues!.SelectMany(rv => rv.Value).GroupBy(p => p.Key)
                    .ToDictionary(p => p.Key, p => p.Max(v => v.Value?.ToString()?.Length ?? 0));

                foreach (var value in refClass.ReferenceValues!)
                {
                    file.Write("  ");
                    file.Write($"{value.Name}:{string.Join(string.Empty, Enumerable.Range(0, keyLength - value.Name.Length).Select(_ => " "))} {{");

                    var props = value.Value.ToList();
                    foreach (var prop in props)
                    {
                        var propName = prop.Key switch
                        {
                            RegularProperty rp => rp.Name,
                            AssociationProperty ap => $"{ap.Association.Name}{ap.Role ?? string.Empty}",
                        };
                        file.Write($" {propName}: {prop.Value}");

                        if (props.IndexOf(prop) < props.Count - 1)
                        {
                            file.Write(",");
                            file.Write(string.Join(string.Empty, Enumerable.Range(0, maxLengths[prop.Key] - (prop.Value?.ToString()?.Length ?? 0)).Select(_ => " ")));
                        }
                    }

                    file.WriteLine(" }");
                }
            }
        }
    }
}
