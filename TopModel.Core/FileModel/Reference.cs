using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace TopModel.Core.FileModel
{
    internal class Reference
    {
        public Reference(Scalar scalar)
        {
            Start = scalar.Start;
            End = scalar.End;
            Value = scalar.Value;
        }

        public Mark Start { get; }

        public Mark End { get; }

        public string Value { get; }
    }
}
