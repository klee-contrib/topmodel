using System.Collections.Generic;

namespace Kinetix.NewGenerator.Model
{
    public class Class
    {
        public string Trigram { get; set; }
        public string Name { get; set; }
        public Class Extends { get; set; }
        public string Label { get; set; }
        public string Stereotype { get; set; }
        public string OrderProperty { get; set; }
        public string DefaultProperty { get; set; }
        public string Comment { get; set; }
        public IList<IProperty> Properties { get; set; }
    }
}
