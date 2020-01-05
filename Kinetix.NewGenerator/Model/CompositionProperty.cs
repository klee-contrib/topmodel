namespace Kinetix.NewGenerator.Model
{
    public class CompositionProperty : IProperty
    {
        public Class Composition { get; set; }
        public string Name { get; set; }
        public string Kind { get; set; }
        public string Comment { get; set; }

        public string Label => Name;
        public bool PrimaryKey => false;
    }
}
