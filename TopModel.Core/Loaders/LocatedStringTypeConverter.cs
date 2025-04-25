using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace TopModel.Core.Loaders;

internal class LocatedStringTypeConverter : IYamlTypeConverter
{
    /// <inheritdoc cref="IYamlTypeConverter.Accepts" />
    public bool Accepts(Type type)
    {
        return type == typeof(LocatedString);
    }

    /// <inheritdoc cref="IYamlTypeConverter.ReadYaml" />
    public object? ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
    {
        return new LocatedString(parser.Consume<Scalar>());
    }

    /// <inheritdoc cref="IYamlTypeConverter.WriteYaml" />
    public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serializer)
    {
        throw new NotImplementedException();
    }
}
