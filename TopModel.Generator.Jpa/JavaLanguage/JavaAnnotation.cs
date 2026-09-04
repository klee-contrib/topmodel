using OneOf;
using TopModel.Utils;

namespace TopModel.Generator.Jpa;

public class JavaAnnotation
{
    private readonly IList<string> _imports = [];

    public JavaAnnotation(string name, params string[] imports)
    {
        Name = name.Trim('@');
        _imports.AddRange(imports);
    }

    public JavaAnnotation(string name, string value, params string[] imports)
    {
        if (!string.IsNullOrEmpty(value))
        {
            Attributes["value"] = value;
        }

        Name = name.Trim('@');
        _imports.AddRange(imports);
    }

    public string Name { get; set; }

    public IEnumerable<string> Imports =>
        _imports
            .Concat(Attributes.Values.Where(v => v.IsT1).SelectMany(a => a.AsT1.Imports))
            .Concat(Attributes.Values.Where(v => v.IsT2).SelectMany(l => l.AsT2.SelectMany(a => a.Imports)))
            .Distinct();

    public IDictionary<string, OneOf<string, JavaAnnotation, IList<JavaAnnotation>>> Attributes { get; } =
        new Dictionary<string, OneOf<string, JavaAnnotation, IList<JavaAnnotation>>>();

    public JavaAnnotation AddAttribute(string name, string value, params string[] import)
    {
        Attributes[name] = value;
        _imports.AddRange(import);
        return this;
    }

    public JavaAnnotation AddAttribute(string name, string value)
    {
        Attributes[name] = value;
        return this;
    }

    public JavaAnnotation AddAttribute(string name, string[] value)
    {
        Attributes[name] = $@"{{{string.Join(", ", value)}}}";
        return this;
    }

    public JavaAnnotation AddAttribute(string value)
    {
        Attributes["value"] = value;
        return this;
    }

    public JavaAnnotation AddAttribute(string name, JavaAnnotation value)
    {
        Attributes[name] = value;
        return this;
    }

    public JavaAnnotation AddAttribute(string name, IEnumerable<JavaAnnotation> value)
    {
        Attributes[name] = value.ToList();
        return this;
    }

    public JavaAnnotation AddImport(string import)
    {
        _imports.Add(import);
        return this;
    }

    public JavaAnnotation AddImports(IEnumerable<string> imports)
    {
        _imports.AddRange(imports);
        return this;
    }
}
