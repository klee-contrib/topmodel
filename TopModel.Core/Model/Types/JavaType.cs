namespace TopModel.Core.Types;

public class JavaType
{
#nullable disable
    public string Type { get; set; }
#nullable enable

    public string? Import { get; set; }

    public List<Annotation>? Annotations { get; set; }


    public class Annotation
    {
#nullable disable
        public string Name { get; set; }
#nullable enable

        public List<string>? Imports { get; set; }


    }
}