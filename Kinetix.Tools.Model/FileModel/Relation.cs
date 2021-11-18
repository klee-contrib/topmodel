using YamlDotNet.Core.Events;

namespace TopModel.Core.FileModel
{
    internal abstract class Relation
    {
        public Relation()
        {
        }

        public Relation(Scalar scalar)
        {
            Reference = new Reference(scalar);
        }

#nullable disable
        public Reference Reference { get; set; }
#nullable enable

        public string ReferenceName => Reference.Value;

        public string Position => $"[{Reference.Start.Line},{Reference.Start.Column}]";
    }
}
