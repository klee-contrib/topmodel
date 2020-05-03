#nullable disable
using System.Collections.Generic;

namespace TopModel.Core.FileModel
{
    public class DependencyDescriptor
    {
        public string Module { get; set; }

        public string Kind { get; set; }

        public IList<string> Files { get; set; }
    }
}
