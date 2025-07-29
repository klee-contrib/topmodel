using System.Reflection;
using System.Runtime.Serialization;
using TopModel.Core.FileModel;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace TopModel.Core.Loaders;

public static class LoaderUtils
{
    public static DomainReference ConsumeDomain(this IParser parser, FileChecker fileChecker, Scalar? value)
    {
        if (parser.Current is MappingStart)
        {
            Scalar? name = null;
            var parameters = new Dictionary<ParameterReference, StringWithVariables>();
            parser.ConsumeMapping(prop =>
            {
                switch (prop.Value)
                {
                    case "name":
                        name = parser.Consume<Scalar>();
                        break;
                    case "parameters":
                        parameters = fileChecker.Deserialize<Dictionary<ParameterReference, StringWithVariables>>(parser);
                        break;
                }
            });

            if (name != null)
            {
                return new DomainReference(name) { ParameterReferences = parameters };
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
        else
        {
            return new DomainReference(value!);
        }
    }

    public static void ConsumeMapping(this IParser parser, Action<Scalar> consumer)
    {
        parser.Consume<MappingStart>();

        while (parser.Current is not MappingEnd)
        {
            var prop = parser.Consume<Scalar>();
            consumer(prop);
        }

        parser.Consume<MappingEnd>();
    }

    public static void ConsumeSequence(this IParser parser, Action consumer)
    {
        parser.Consume<SequenceStart>();

        while (parser.Current is not SequenceEnd)
        {
            consumer();
        }

        parser.Consume<SequenceEnd>();
    }

    public static T? ParseEnum<T>(this string? value)
        where T : struct, Enum
    {
        foreach (var field in typeof(T).GetFields())
        {
            var attribute = field.GetCustomAttribute<EnumMemberAttribute>();
            if (attribute?.Value == value)
            {
                return (T)field.GetValue(null)!;
            }
        }

        return null;
    }
}
