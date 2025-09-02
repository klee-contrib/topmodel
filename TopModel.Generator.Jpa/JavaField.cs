using TopModel.Utils;

namespace TopModel.Generator.Jpa;

public class JavaField(string type, string name)
{
    public string Type { get; set; } = type;

    public string Name { get; set; } = name;

    public IList<JavaAnnotation> Annotations { get; } = [];

    public IList<string> Imports { get; } = [];

    public string Comment { get; set; } = string.Empty;

    public JavaField AddAnnotation(JavaAnnotation annotation)
    {
        Imports.AddRange(annotation.Imports);
        Annotations.Add(annotation);
        return this;
    }

    public override string ToString()
    {
        var sb = new System.Text.StringBuilder();

        if (!string.IsNullOrEmpty(Comment))
        {
            sb.AppendLine($"/**");
            sb.AppendLine($" * {Comment}");
            sb.AppendLine($" */");
        }

        foreach (var annotation in Annotations)
        {
            sb.AppendLine(annotation.ToString());
        }

        sb.AppendLine($"private {Type} {Name};");

        return sb.ToString();
    }
}
