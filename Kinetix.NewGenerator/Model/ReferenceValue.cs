using System.Collections.Generic;

#nullable disable
namespace Kinetix.NewGenerator.Model
{
    public class ReferenceValue
    {
        public string Name { get; set; }
        public IDictionary<string, object> Bean { get; set; }
    }
}
