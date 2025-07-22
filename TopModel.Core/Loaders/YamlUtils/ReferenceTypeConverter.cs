using TopModel.Core.FileModel;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace TopModel.Core.Loaders.YamlUtils;

internal class ReferenceTypeConverter : IYamlTypeConverter
{
    /// <inheritdoc cref="IYamlTypeConverter.Accepts" />
    public bool Accepts(Type type)
    {
        return type == typeof(ParameterReference);
    }

    /// <inheritdoc cref="IYamlTypeConverter.ReadYaml" />
    public object? ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
    {
        return new ParameterReference(parser.Consume<Scalar>());
    }

    /// <inheritdoc cref="IYamlTypeConverter.WriteYaml" />
    public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serializer)
    {
        throw new NotImplementedException();
    }
}
