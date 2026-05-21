using TopModel.Utils;

namespace TopModel.Generator.Jpa;

public class JavaEnum : JavaClass
{
    public JavaEnum(string name)
        : base(name)
    {
        ClassType = "enum";
    }

    public IList<JavaEnumValue> Values { get; } = [];

    public JavaEnum Add(JavaEnumValue javaEnumValue)
    {
        Values.Add(javaEnumValue);
        Imports.AddRange(javaEnumValue.Imports);
        return this;
    }

    public JavaEnum AddRange(IEnumerable<JavaEnumValue> javaEnumValues)
    {
        foreach (var javaEnumValue in javaEnumValues)
        {
            Add(javaEnumValue);
        }

        return this;
    }
}
