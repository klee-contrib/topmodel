using TopModel.Utils;

namespace TopModel.Generator.Jpa;

public class JavaMethod
{
    private readonly IList<string> _imports = [];

    public JavaMethod(string returnType, string name)
    {
        Name = name;
        ReturnType = returnType;
    }

    public JavaMethod(string import, string returnType, string name)
    {
        Name = name;
        ReturnType = returnType;
        _imports.Add(import);
    }

    public IList<JavaAnnotation> Annotations { get; } = [];

    public IList<WriterLine> Body { get; } = [];

    public IEnumerable<string> Imports =>
        _imports
            .Concat(Annotations.SelectMany(a => a.Imports))
            .Concat(Parameters.SelectMany(p => p.Imports))
            .Distinct();

    public string Visibility { get; set; } = string.Empty;

    public bool Static { get; set; }

    public string Comment { get; set; } = string.Empty;

    public string ReturnComment { get; set; } = string.Empty;

    public IList<JavaMethodParameter> Parameters { get; } = [];

    public string ReturnType { get; }

    public string Name { get; }

    public IList<string> GenericTypes { get; } = [];

    public JavaMethod AddAnnotation(JavaAnnotation annotation)
    {
        Annotations.Add(annotation);
        return this;
    }

    public JavaMethod AddAnnotations(IEnumerable<JavaAnnotation> annotation)
    {
        foreach (var a in annotation)
        {
            AddAnnotation(a);
        }
        return this;
    }

    public JavaMethod AddBodyLine()
    {
        Body.Add(new WriterLine() { Line = string.Empty, Indent = 0 });
        return this;
    }

    public JavaMethod AddBodyLine(string line)
    {
        Body.Add(new WriterLine() { Line = line, Indent = 0 });
        return this;
    }

    public JavaMethod AddBodyLine(int indentationLevel, string line)
    {
        Body.Add(new WriterLine() { Line = line, Indent = indentationLevel });
        return this;
    }

    public JavaMethod AddGenericType(string type)
    {
        GenericTypes.Add(type);
        return this;
    }

    public JavaMethod AddImports(params IEnumerable<string> imports)
    {
        _imports.AddRange(imports);
        return this;
    }

    public virtual JavaMethod AddParameter(JavaMethodParameter parameter)
    {
        Parameters.Add(parameter);
        return this;
    }

    public JavaMethod AddParameters(IEnumerable<JavaMethodParameter> parameters)
    {
        foreach (var parameter in parameters)
        {
            AddParameter(parameter);
        }

        return this;
    }

    public string CallWith(params string[] parameters)
    {
        return $"{Name}({string.Join(", ", parameters)})";
    }
}
