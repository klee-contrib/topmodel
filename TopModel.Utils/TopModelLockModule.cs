using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace TopModel.Utils;

public class TopModelLockModule : TopModelLockModuleBase, IYamlConvertible
{
    /// <inheritdoc cref="IYamlConvertible.Read" />
    public void Read(IParser parser, Type expectedType, ObjectDeserializer nestedObjectDeserializer)
    {
        if (parser.TryConsume<Scalar>(out var version))
        {
            Version = version.Value;
        }
        else if (parser.Current is MappingStart)
        {
            var lol = (TopModelLockModuleBase)nestedObjectDeserializer(typeof(TopModelLockModuleBase))!;
            Version = lol.Version;
            Hash = lol.Hash;
        }
    }

    /// <inheritdoc cref="IYamlConvertible.Write" />
    public void Write(IEmitter emitter, ObjectSerializer nestedObjectSerializer)
    {
        if (Hash != null)
        {
            nestedObjectSerializer(new TopModelLockModuleBase { Version = Version, Hash = Hash });
        }
        else
        {
            emitter.Emit(new Scalar(Version));
        }
    }
}

#pragma warning disable SA1402
public class TopModelLockModuleBase
{
    public required string Version { get; set; }

    public string? Hash { get; set; }
}