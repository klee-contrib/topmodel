namespace TopModel.Core
{
    public class ModelConfig
    {
#nullable disable
        public string ModelRoot { get; set; }

        public string Domains { get; set; }

#nullable enable
        public string? StaticLists { get; set; }

        public string? ReferenceLists { get; set; }
    }
}
