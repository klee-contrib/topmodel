namespace Kinetix.NewGenerator.Model
{
    public class Domain
    {
#nullable disable
        public string Name { get; set; }
        public string Label { get; set; }
        public string CsharpType { get; set; }
#nullable enable
        public string? SqlType { get; set; }
        public string? CustomAnnotation { get; set; }
        public string? CustomUsings { get; set; }
    }
}
