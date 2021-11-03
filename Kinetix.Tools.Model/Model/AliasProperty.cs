namespace TopModel.Core
{
    public class AliasProperty : IFieldProperty
    {
        private string? _comment;
        private string? _label;
        private bool? _required;

#nullable disable
        public IFieldProperty Property { get; set; }

        public Class Class { get; set; }

        public Endpoint Endpoint { get; set; }

#nullable enable
        public string? Prefix { get; set; }

        public string? Suffix { get; set; }

        public string Name => (Prefix ?? string.Empty) + Property?.Name + (Suffix ?? string.Empty);

        public string? Label
        {
            get => _label ?? Property.Label;
            set => _label = value;
        }

        public bool PrimaryKey => (Property?.PrimaryKey ?? false) && Prefix == null && Suffix == null;

        public bool Required
        {
            get => _required ?? Property.Required;
            set => _required = value;
        }

        public Domain Domain => Property.Domain;

        public string Comment
        {
            get => _comment ?? Property.Comment;
            set => _comment = value;
        }

        public string? DefaultValue => null;

        public Domain? ListDomain { get; set; }
    }
}
