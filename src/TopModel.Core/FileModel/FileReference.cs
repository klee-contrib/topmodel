using YamlDotNet.Core.Events;

namespace TopModel.Core.FileModel;

public class FileReference : Reference
{
    internal FileReference(Scalar value)
        : base(value) { }
}
