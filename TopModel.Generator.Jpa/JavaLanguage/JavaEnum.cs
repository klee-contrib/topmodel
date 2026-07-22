namespace TopModel.Generator.Jpa;

public class JavaEnum : JavaClass
{
    public JavaEnum(string name)
        : base(name)
    {
        ClassType = "enum";
    }

    public IList<JavaEnumValue> Values { get; } = [];

    public override IEnumerable<string> Imports => base.Imports.Concat(Values.SelectMany(v => v.Imports)).Distinct();

    public JavaEnum Add(JavaEnumValue javaEnumValue)
    {
        Values.Add(javaEnumValue);
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
