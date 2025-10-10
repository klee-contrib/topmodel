using System.Text;
using TopModel.Utils;

namespace TopModel.Generator.Jpa;

public class JavaClass(string name)
{
    public string Name { get; set; } = name;

    public string? Package { get; set; }

    public string Visibility { get; set; } = "public";

    public string? Extends { get; set; }

    public string? Modifier { get; set; }

    public IList<JavaAnnotation> Annotations { get; } = [];

    public IList<string> Imports { get; } = [];

    public IList<string> Implements { get; } = [];

    public string ClassType { get; set; } = "class";

    public IList<JavaField> Fields { get; } = [];

    public IList<JavaConstructor> Constructors { get; } = [];

    public IList<JavaMethod> Methods { get; } = [];

    public string Comment { get; set; } = string.Empty;

    public IList<JavaClass> InnerClasses { get; } = [];

    public JavaClass Add(JavaAnnotation annotation)
    {
        Imports.AddRange(annotation.Imports);
        Annotations.Add(annotation);
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

    public JavaClass AddRange(IEnumerable<JavaAnnotation> annotations)
    {
        foreach (var annotation in annotations)
        {
            Add(annotation);
        }

        return this;
    }

    public JavaClass Add(JavaClass innerClass)
    {
        InnerClasses.Add(innerClass);
        Imports.AddRange(innerClass.Imports);

        return this;
    }

    public JavaClass AddRange(IEnumerable<JavaClass> innerClasses)
    {
        foreach (var innerClass in innerClasses)
        {
            Add(innerClass);
        }
        return this;
    }

    public JavaClass AddRange(IEnumerable<JavaField> javaFields)
    {
        foreach (var field in javaFields)
        {
            Add(field);
        }

        return this;
    }

    public JavaClass AddRange(IEnumerable<JavaConstructor> constructors)
    {
        foreach (var constructor in constructors)
        {
            Add(constructor);
        }

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

    public JavaConstructor GetAllArgsConstructor()
    {
        var constructor = new JavaConstructor(Name);
        foreach (var field in Fields)
        {
            constructor.Parameters.Add(new JavaMethodParameter(field.Type, field.Name));
            constructor.Body.Add(new WriterLine() { Line = $"this.{field.Name} = {field.Name};", Indent = 0 });
        }
        return constructor;
    }

    public JavaConstructor GetNoArgsConstructor()
    {
        return new JavaConstructor(Name);
    }

    public string GetDeclaration()
    {
        var sb = new StringBuilder();
        if (Visibility != null)
        {
            sb.Append($"{Visibility} ");
        }

        if (!string.IsNullOrEmpty(Modifier))
        {
            sb.Append($"{Modifier} ");
        }

        sb.Append($"{ClassType} {Name}");
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
