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

    public IList<string> Imports { get; } = [];

    public IList<JavaAnnotation> Annotations { get; } = [];

    public bool Final { get; set; } = false;

    public string Name { get; }

    public string Comment { get; set; } = string.Empty;

    public string Type { get; set; }

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
