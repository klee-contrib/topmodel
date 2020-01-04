using System.Collections.Generic;

namespace Kinetix.NewGenerator.FileModel
{
    public class DependencyDescriptor
    {
        public string Module { get; set; }
        public string Kind { get; set; }
        public IList<DependencyFileDescriptor> Files { get; set; }
    }
}
