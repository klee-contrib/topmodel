using System.Linq;

namespace Kinetix.NewGenerator.Model
{
    public class AssociationProperty : IFieldProperty
    {
        public Class Association { get; set; }
        public string Role { get; set; }
        public string Label { get; set; }
        public bool Required { get; set; }
        public string Comment { get; set; }

        public string Name => Association.Name + Association.Properties.Single(p => p.PrimaryKey).Name + (Role ?? string.Empty);
        public Domain Domain => Association.Properties.OfType<IFieldProperty>().Single(p => p.PrimaryKey).Domain;
        public bool PrimaryKey => false;
    }
}
