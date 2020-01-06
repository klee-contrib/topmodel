namespace Kinetix.NewGenerator.Model
{
    public class AliasProperty : IFieldProperty
    {
#nullable disable
        public IFieldProperty Property { get; set; }
        public Class Class { get; set; }
#nullable enable
        public string? Prefix { get; set; }
        public string? Suffix { get; set; }

        public string Name => (Prefix ?? string.Empty) + Property.Name + (Suffix ?? string.Empty);
        public string Label => Property.Label;
        public bool PrimaryKey => Property.PrimaryKey && Prefix == null && Suffix == null;
        public bool Required => Property.Required;
        public Domain Domain => Property.Domain;
        public string Comment => Property.Comment;
    }
}
