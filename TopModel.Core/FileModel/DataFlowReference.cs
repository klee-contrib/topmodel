using YamlDotNet.Core.Events;

namespace TopModel.Core.FileModel;

public class DataFlowReference : Reference
{
    internal DataFlowReference(Scalar scalar)
        : base(scalar)
    {
    }
}