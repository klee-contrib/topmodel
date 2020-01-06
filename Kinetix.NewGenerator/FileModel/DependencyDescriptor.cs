using System.Collections.Generic;

#nullable disable
namespace Kinetix.NewGenerator.FileModel
{
    public class DependencyDescriptor
    {
        public string Module { get; set; }
        public Kind Kind { get; set; }
        public IList<DependencyFileDescriptor> Files { get; set; }
    }
}
