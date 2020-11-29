using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace TopModel.Core.FileModel
{
    public class Relation
    {
        public Relation(Scalar scalar)
        {
            Start = scalar.Start;
            End = scalar.End;
            Value = scalar.Value;
        }

        public Mark Start { get; }

        public Mark End { get; }

        public string Value { get; }

        public Relation? Peer { get; set; }
    }
}
