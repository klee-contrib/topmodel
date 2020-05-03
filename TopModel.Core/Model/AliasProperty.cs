namespace TopModel.Core
{
    public class AliasProperty : IFieldProperty
    {
        private string? _label;
        private bool? _required;

#nullable disable
        public IFieldProperty Property { get; set; }

        public Class Class { get; set; }

#nullable enable
        public string? Prefix { get; set; }

        public string? Suffix { get; set; }

        public string Name => (Prefix ?? string.Empty) + Property?.Name + (Suffix ?? string.Empty);

        public string Label
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

        public string Comment => Property.Comment;

        public string? DefaultValue => null;
    }
}
