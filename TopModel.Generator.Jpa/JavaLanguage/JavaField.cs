using TopModel.Utils;

namespace TopModel.Generator.Jpa;

public class JavaField(string type, string name)
{
    public string Type { get; set; } = type;
    public string Visibility { get; set; } = "private";

    public string Name { get; set; } = name;
    public string DefaultValue { get; set; } = "";
    public bool Static { get; set; } = false;
    public bool Final { get; set; } = false;

    public IList<JavaAnnotation> Annotations { get; } = [];

    public IList<string> Imports { get; } = [];

    public IList<string> Comment { get; set; } = [];

    public JavaMethod DefaultGetter =>
        new(Type, Name.ToPascalCase().WithPrefix(Type == "boolean" ? "is" : "get"))
        {
            Comment = $"Getter for {Name}",
            Body =
            {
                new WriterLine() { Line = $"return this.{Name};", Indent = 0 },
            },
            ReturnComment = $"value of {{@link #{Name} {Name}}}",
            Visibility = "public",
        };

    public JavaMethod DefaultSetter =>
        new("void", $"set{Name.ToPascalCase()}")
        {
            Comment = $@"Set the value of {{@link #{Name} {Name}}}",
            Parameters = { new JavaMethodParameter(Type, Name) { Comment = $"value to set" } },
            Body =
            {
                new WriterLine() { Line = $"this.{Name} = {Name};", Indent = 0 },
            },
            Visibility = "public",
        };

    public JavaField Add(JavaAnnotation annotation)
    {
        Imports.AddRange(annotation.Imports);
        Annotations.Add(annotation);
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

    public override string ToString()
    {
        return $"{Visibility}{(Static ? " static" : string.Empty)}{(Final ? " final" : string.Empty)} {Type} {Name}{(DefaultValue != string.Empty ? $" = {DefaultValue}" : string.Empty)};";
    }
}
