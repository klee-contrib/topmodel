using System.Text;

namespace TopModel.Generator.Jpa;

public class JavaRecord : JavaClass
{
    public JavaRecord(string name)
        : base(name)
    {
        ClassType = "record";
    }
}

