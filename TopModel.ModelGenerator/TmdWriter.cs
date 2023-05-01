using Microsoft.Extensions.Logging;
using TopModel.Utils;

namespace TopModel.ModelGenerator;

/// <summary>
/// FileWriter avec des méthodes spécialisées pour écrire du TopModel.
/// </summary>
public class TmdWriter : IDisposable
{
    private readonly TmdFile _file;
    private readonly string _modelRoot;
    private readonly FileWriter _writer;

    public TmdWriter(string path, TmdFile file, ILogger logger, string modelRoot)
    {
        _file = file;
        _writer = new FileWriter(path + '/' + file.Module + '/' + file.Name + ".tmd", logger) { StartCommentToken = "####" };
        _modelRoot = Path.GetRelativePath(modelRoot, path);
        if (_modelRoot == ".")
        {
            _modelRoot = string.Empty;
        }
        else
        {
            _modelRoot += '/';
        }
    }

    /// <inheritdoc cref="IDisposable.Dispose" />
    public void Dispose()
    {
        _writer.Dispose();
    }

    public void Write()
    {
        _writer.WriteLine($"---");
        _writer.WriteLine($"module: {_file.Module!.Split('_')[1]}");
        _writer.WriteLine($"tags:");
        foreach (var tag in _file.Tags)
        {
            _writer.WriteLine($"  - {tag}");
        }

        if (_file.Uses.Any())
        {
            _writer.WriteLine($"uses: ");
            foreach (var u in _file.Uses.OrderBy(u => u.Name).Where(u => u.Name != _file.Name))
            {
                _writer.WriteLine($"  - {u.Module}/{u.Name}");
            }
        }

        foreach (var classe in _file.Classes.OrderBy(c => c.Name))
        {
            _writer.WriteLine($"---");
            _writer.WriteLine($"class:");
            _writer.WriteLine($"  name: {classe.Name}");
            _writer.WriteLine($"  comment: {classe.Name}");
            if (!string.IsNullOrEmpty(classe.Trigram))
            {
                _writer.WriteLine($"  trigram: {classe.Trigram.ToUpper()}");
            }

            _writer.WriteLine($"  properties:");
            foreach (var property in classe.Properties)
            {
                _writer.WriteLine();
                if (property is TmdAssociationProperty ap)
                {
                    _writer.WriteLine($"    - association: {ap.ForeignClass.Name}");
                    _writer.WriteLine($"      comment: {property.Name}");
                    if (!string.IsNullOrEmpty(ap.Role))
                    {
                        _writer.WriteLine($"      role: {ap.Role}");
                    }
                }
                else
                {
                    _writer.WriteLine($"    - name: {property.Name}");
                    _writer.WriteLine($"      comment: {property.Name}");
                    _writer.WriteLine($"      domain: {property.Domain}");
                }

                if (property.Required)
                {
                    _writer.WriteLine($"      required: true");
                }

                if (property.PrimaryKey)
                {
                    _writer.WriteLine($"      primaryKey: true");
                }
            }

            if (classe.Unique.Any())
            {
                _writer.WriteLine();
                _writer.WriteLine($"  unique:");
                foreach (var uniq in classe.Unique)
                {
                    _writer.WriteLine($"    - [{string.Join(", ", uniq)}]");
                }
            }

            if (classe.Values.Any())
            {
                _writer.WriteLine($"  values:");
                var i = 0;
                foreach (var row in classe.Values)
                {
                    _writer.WriteLine(@$"    Value{i++}: {{{string.Join(", ", row.Where(v => v.Value != null).Select(v => $@"{v.Key}: ""{v.Value}"""))}}}");
                }
            }
        }
    }
}