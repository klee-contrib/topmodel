namespace TopModel.Core.Config
{
    public class RootConfig
    {
#nullable disable
        public string ModelRoot { get; set; }
        public string Domains { get; set; }
#nullable enable
        public string? StaticLists { get; set; }
        public string? ReferenceLists { get; set; }
    }
}
