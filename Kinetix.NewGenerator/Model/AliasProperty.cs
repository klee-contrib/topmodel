namespace Kinetix.NewGenerator.Model
{
    public class AliasProperty : IProperty
    {
        public IProperty Property { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }

        public string Name => (Prefix ?? string.Empty) + Property.Name + (Suffix ?? string.Empty);

        public bool PrimaryKey => false;
    }
}
