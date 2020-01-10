namespace Kinetix.NewGenerator.Config
{
    public class RootConfig
    {
        public string? ModelRoot { get; set; }
        public string? Domains { get; set; }
        public string? StaticLists { get; set; }
        public string? ReferenceLists { get; set; }
        public ProceduralSqlConfig? ProceduralSql { get; set; }
        public JavascriptConfig? Javascript { get; set; }
        public CSharpConfig? Csharp { get; set; }
    }
}
