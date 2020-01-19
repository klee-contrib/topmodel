using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace TopModel.Core.FileModel
{
    public class ModelFile
    {
        public string Path { get; set; }

        public FileDescriptor Descriptor { get; set; }

        public IList<Class> Classes { get; set; }

        public IList<(object Source, Relation Target)> Relationships { get; set; }

        public IEnumerable<FileName> Dependencies => Descriptor.Uses?.SelectMany(u =>
            u.Files.Select(f => new FileName { Module = u.Module, Kind = u.Kind, File = f }))
            ?? new FileName[0];

        public FileName Name => new FileName { Module = Descriptor.Module, Kind = Descriptor.Kind, File = Descriptor.File };

        public override string ToString()
        {
            return $"{Descriptor.Module}/{Descriptor.Kind}/{Descriptor.File}";
        }
    }
}
