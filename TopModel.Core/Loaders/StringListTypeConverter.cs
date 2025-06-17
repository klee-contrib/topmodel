using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace TopModel.Core.Loaders;

public class StringListTypeConverter : IYamlTypeConverter
{
    /// <inheritdoc cref="IYamlTypeConverter.Accepts" />
    public bool Accepts(Type type)
    {
        return false;
    }

    /// <inheritdoc cref="IYamlTypeConverter.ReadYaml" />
    public object? ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
    {
        var result = new List<string>();
        if (parser.TryConsume<Scalar>(out var language))
        {
            result.Add(language.Value);
        }
        else if (parser.Current is SequenceStart)
        {
            parser.ConsumeSequence(() =>
            {
                result.Add(parser.Consume<Scalar>().Value);
            });
        }

        return result;
    }

    /// <inheritdoc cref="IYamlTypeConverter.WriteYaml" />
    public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serializer)
    {
        throw new NotImplementedException();
    }
}
