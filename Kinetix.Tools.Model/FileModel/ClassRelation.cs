using YamlDotNet.Core.Events;

namespace TopModel.Core.FileModel
{
    internal class ClassRelation : Relation
    {
        public ClassRelation()
        {
        }

        public ClassRelation(Scalar scalar)
            : base(scalar)
        {
        }
    }
}
