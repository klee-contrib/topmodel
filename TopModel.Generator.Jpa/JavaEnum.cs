namespace TopModel.Generator.Jpa;

public class JavaEnum : JavaClass
{
    public JavaEnum(string name)
        : base(name)
    {
        ClassType = "enum";
    }

    public IList<JavaEnumValue> Values { get; } = [];
}
