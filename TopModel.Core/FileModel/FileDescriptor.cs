using System.Collections.Generic;

namespace TopModel.Core.FileModel
{
    public class FileDescriptor
    {
#nullable disable
        public string App { get; set; }
        public string Module { get; set; }
        public Kind Kind { get; set; }
        public string File { get; set; }
#nullable enable
        public IList<DependencyDescriptor>? Uses { get; set; }
        public bool Loaded { get; set; }
    }
}
