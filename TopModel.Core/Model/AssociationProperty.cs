using System.Linq;

namespace TopModel.Core
{
    public class AssociationProperty : IFieldProperty
    {
#nullable disable
        public Class Association { get; set; }

        public string Label { get; set; }

        public string Comment { get; set; }

        public Class Class { get; set; }
#nullable enable

        public string? Role { get; set; }

        public bool Required { get; set; }

        public bool Unique { get; set; }

        public string? DefaultValue { get; set; }

        public string Name => (Association?.Extends == null && !AsAlias ? Association?.Name : string.Empty) + Association?.Properties.Single(p => p.PrimaryKey).Name + (Role?.Replace(" ", string.Empty) ?? string.Empty);

        public Domain Domain => Association.Properties.OfType<IFieldProperty>().Single(p => p.PrimaryKey).Domain;

        public bool PrimaryKey => false;

        public bool AsAlias { get; set; }
    }
}
