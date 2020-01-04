using System.Collections.Generic;

namespace Kinetix.NewGenerator.FileModel
{
    public class DependencyFileDescriptor
    {
        public string File { get; set; }
        public IList<string> Classes { get; set; }
    }
}