using System.Collections.Generic;

namespace Kinetix.NewGenerator.FileModel
{
    public class FileDescriptor
    {
        public string App { get; set; }
        public string Module { get; set; }
        public string Kind { get; set; }
        public string File { get; set; }
        public IList<DependencyDescriptor> Uses { get; set; }
        public bool Loaded { get; set; }
    }
}
