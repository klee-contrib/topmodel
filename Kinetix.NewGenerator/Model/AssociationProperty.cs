using System.Linq;

namespace Kinetix.NewGenerator.Model
{
    public class AssociationProperty : IProperty
    {
        public Class Association { get; set; }
        public string Role { get; set; }
        public bool Required { get; set; }
        public string Comment { get; set; }

        public string Name => Association.Name + Association.Properties.Single(p => p.PrimaryKey).Name;

        public bool PrimaryKey => false;
    }
}
