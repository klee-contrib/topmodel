using System.Collections.Generic;

#nullable disable
namespace TopModel.Core.FileModel
{
    public class ModelFile
    {
        public string Module { get; set; }

        public IList<string> Tags { get; set; } = new List<string>();

        public IList<string> Uses { get; set; } = new List<string>();

        public string Name { get; set; }

        public string Path { get; set; }

        public IList<Class> Classes { get; set; }

        public IList<Domain> Domains { get; set; }

        public IDictionary<object, Relation> Relationships { get; set; } = new Dictionary<object, Relation>();

        public override string ToString()
        {
            return Name;
        }
    }
}
