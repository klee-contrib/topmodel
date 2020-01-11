using System.Collections.Generic;

#nullable disable
namespace TopModel.Core
{
    public class ReferenceValue
    {
        public string Name { get; set; }
        public IDictionary<string, object> Bean { get; set; }
    }
}
