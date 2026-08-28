using TopModel.Utils;

namespace TopModel.Generator.Jpa;

public class JavaField(string type, string name)
{
    private readonly IList<string> _comments = [];
    private readonly IList<string> _imports = [];

    public string Type { get; set; } = type;

    public string Visibility { get; set; } = "private";

    public string Name { get; set; } = name;

    public string DefaultValue { get; set; } = "";

    public bool Static { get; set; } = false;

    public bool Final { get; set; } = false;

    public bool Volatile { get; set; } = false;

    public IList<JavaAnnotation> Annotations { get; } = [];

    public IEnumerable<string> Imports => _imports.Concat(Annotations.SelectMany(a => a.Imports)).Distinct();

    public IList<string> Comments => _comments;

    public JavaField Add(JavaAnnotation annotation)
    {
        Annotations.Add(annotation);
        return this;
    }

    public JavaField AddCommentLine(string line)
    {
        if (!string.IsNullOrWhiteSpace(line))
        {
            if (line.Contains(Environment.NewLine) || line.Contains('\n') || line.Contains('\r'))
            {
                foreach (var subLine in line.Split([Environment.NewLine[0], '\n', '\r'], StringSplitOptions.None))
                {
                    AddCommentLine(subLine);
                }
                return this;
            }
            _comments.Add(line);
        }

        return this;
    }

    public JavaField AddImports(params IEnumerable<string> imports)
    {
        _imports.AddRange(imports);
        return this;
    }

    public JavaField AddRange(IEnumerable<JavaAnnotation> annotations)
    {
        foreach (var annotation in annotations)
        {
            Add(annotation);
        }
        return this;
    }
}
