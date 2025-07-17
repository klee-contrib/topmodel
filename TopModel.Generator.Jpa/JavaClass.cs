using System.Text;

namespace TopModel.Generator.Jpa;

public class JavaClass(string name)
{
    public string Name { get; set; } = name;

    public string? Package { get; set; }

    public string Visibility { get; set; } = "public";

    public string? Extends { get; set; }

    public string? Modifier { get; set; }

    public List<JavaAnnotation> Annotations { get; } = [];

    public List<string> Imports { get; } = [];

    public List<string> Implements { get; } = [];

    public bool Interface { get; set; }

    public List<JavaField> Fields { get; } = [];

    public List<JavaConstructor> Constructors { get; } = [];

    public List<JavaMethod> Methods { get; } = [];

    public string Comment { get; set; } = string.Empty;

    public JavaClass Add(JavaAnnotation annotation)
    {
        Imports.AddRange(annotation.Imports);
        Annotations.Add(annotation);
        return this;
    }

    public JavaClass AddRange(IEnumerable<JavaAnnotation> annotations)
    {
        foreach (var annotation in annotations)
        {
            Add(annotation);
        }

        return this;
    }

    public JavaClass Add(JavaConstructor constructor)
    {
        Imports.AddRange(constructor.Imports);
        Constructors.Add(constructor);
        return this;
    }

    public JavaClass Add(JavaField field)
    {
        Imports.AddRange(field.Imports);
        Fields.Add(field);
        return this;
    }

    public JavaClass Add(JavaMethod method)
    {
        Imports.AddRange(method.Imports);
        Methods.Add(method);
        return this;
    }

    public JavaClass AddRange(IEnumerable<JavaMethod> methods)
    {
        foreach (var method in methods)
        {
            Add(method);
        }

        return this;
    }

    public string GetDeclaration()
    {
        var sb = new StringBuilder();
        if (Visibility != null)
        {
            sb.Append($"{Visibility} ");
        }

        var classType = Interface ? "interface" : "class";
        sb.Append($"{classType}");
        if (!string.IsNullOrEmpty(Modifier))
        {
            sb.Append($" {Modifier}");
        }

        sb.Append($" {Name}");
        if (!string.IsNullOrEmpty(Extends))
        {
            sb.Append($" extends {Extends}");
        }

        if (Implements.Count > 0)
        {
            sb.Append($" implements {string.Join(", ", Implements)}");
        }

        return sb.ToString();
    }
}