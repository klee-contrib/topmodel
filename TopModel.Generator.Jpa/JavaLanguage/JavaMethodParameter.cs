using TopModel.Utils;

namespace TopModel.Generator.Jpa;

public class JavaMethodParameter
{
    public JavaMethodParameter(string type, string name)
    {
        Name = name;
        Type = type;
    }

    public JavaMethodParameter(string import, string type, string name)
    {
        Name = name;
        Type = type;
        Imports.Add(import);
    }

    public string Declaration =>
        $@"{(Final ? "final " : string.Empty)}{string.Join(' ', Annotations.DistinctBy(e => e.Name.Split('(')[0]).OrderBy(a => a.Name))}{(Annotations.Count > 0 ? ' ' : string.Empty)}{Type} {Name}";

    public IList<string> Imports { get; } = [];

    public IList<JavaAnnotation> Annotations { get; } = [];

    public bool Final { get; set; } = false;

    public string Name { get; }

    public string Comment { get; set; } = string.Empty;

    private string Type { get; set; }

    public JavaMethodParameter Add(JavaAnnotation annotation)
    {
        Imports.AddRange(annotation.Imports);
        Annotations.Add(annotation);
        return this;
    }

    public JavaMethodParameter AddRange(IEnumerable<JavaAnnotation> annotations)
    {
        foreach (var a in annotations)
        {
            Add(a);
        }

        return this;
    }
}
