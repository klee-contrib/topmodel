#nullable disable
namespace TopModel.Core
{
    public class CompositionProperty : IProperty
    {
        public Class Composition { get; set; }

        public string Name { get; set; }

        public Composition Kind { get; set; }

        public string Comment { get; set; }

        public Class Class { get; set; }

        public Endpoint Endpoint { get; set; }

        public string Label => Name;

        public bool PrimaryKey => false;
    }
}
