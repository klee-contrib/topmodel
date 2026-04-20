using System.Text;
using TopModel.Utils;

namespace TopModel.Generator.Jpa;

public class JavaAnnotation
{
    public JavaAnnotation(string name, params string[] imports)
    {
        Name = name.Trim('@');
        Imports.AddRange(imports);
    }

    public JavaAnnotation(string name, string value, params string[] imports)
    {
        if (!string.IsNullOrEmpty(value))
        {
            Attributes["value"] = value;
        }

        Name = name.Trim('@');
        Imports.AddRange(imports);
    }

    public string Name { get; set; }

    public IList<string> Imports { get; set; } = [];

    private Dictionary<string, object> Attributes { get; } = [];

    public JavaAnnotation AddAttribute(string name, string value, params string[] import)
    {
        Attributes[name] = value;
        Imports.AddRange(import);
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
        Imports.AddRange(value.Imports);
        return this;
    }

    public JavaAnnotation AddAttribute(string name, IEnumerable<JavaAnnotation> value)
    {
        var list = value.ToList();
        Attributes[name] = list;
        Imports.AddRange(list.SelectMany(a => a.Imports));
        return this;
    }

    public override string ToString()
    {
        var name = Name.StartsWith('@') ? Name : $"@{Name}";
        if (!Attributes.Any())
        {
            return name;
        }
        else if (Attributes.Count == 1 && Attributes.Any(a => a.Key == "value"))
        {
            return $"{name}({Attributes.First().Value})";
        }
        else if (Attributes.Values.Any(v => v is List<JavaAnnotation>))
        {
            var sb = new StringBuilder();
            sb.Append($"{name}(");
            var attrList = Attributes.ToList();
            for (var i = 0; i < attrList.Count; i++)
            {
                var attr = attrList[i];
                var isLast = i == attrList.Count - 1;
                sb.Append("\n\t");
                if (attr.Value is List<JavaAnnotation> annotations)
                {
                    sb.Append($"{attr.Key} = {{");
                    for (var j = 0; j < annotations.Count; j++)
                    {
                        sb.Append($"\n\t\t{annotations[j]}");
                        if (j < annotations.Count - 1)
                        {
                            sb.Append(',');
                        }
                    }
                    sb.Append("\n\t}");
                }
                else
                {
                    sb.Append($"{attr.Key} = {attr.Value}");
                }

                if (!isLast)
                {
                    sb.Append(',');
                }
            }

            sb.Append("\n)");
            return sb.ToString();
        }
        else
        {
            var attributes = string.Join(", ", Attributes.Select(a => $"{a.Key} = {a.Value}"));
            return $"{name}({attributes})";
        }
    }
}
