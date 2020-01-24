using TopModel.Core.FileModel;

namespace TopModel.Core
{
    public struct Namespace
    {
        public string App { get; set; }

        public string Module { get; set; }

        public Kind Kind { get; set; }

        public string CSharpName => Module + (Kind == Kind.Data ? "DataContract" : "Contract");
    }
}
