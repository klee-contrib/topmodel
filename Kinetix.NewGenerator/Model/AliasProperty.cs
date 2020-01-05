namespace Kinetix.NewGenerator.Model
{
    public class AliasProperty : IFieldProperty
    {
        public IFieldProperty Property { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }

        public string Name => (Prefix ?? string.Empty) + Property.Name + (Suffix ?? string.Empty);
        public string Label => Property.Label;
        public bool PrimaryKey => Property.PrimaryKey && Prefix == null && Suffix == null;
        public bool Required => Property.Required;
        public Domain Domain => Property.Domain;
        public string Comment => Property.Comment;
    }
}
