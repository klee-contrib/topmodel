using TopModel.Core.FileModel;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace TopModel.Core.Loaders;

public static class LoaderUtils
{
    public static DomainReference ConsumeDomain(this Parser parser, Scalar? value)
    {
        if (parser.Current is MappingStart)
        {
            Scalar? name = null;
            var paramaters = new List<Reference>();
            parser.ConsumeMapping(prop =>
            {
                switch (prop.Value)
                {
                    case "name":
                        name = parser.Consume<Scalar>();
                        break;
                    case "parameters":
                        parser.ConsumeSequence(() => paramaters.Add(new Reference(parser.Consume<Scalar>())));
                        break;
                }
            });

            if (name != null)
            {
                var domain = new DomainReference(name);
                domain.ParameterReferences.AddRange(paramaters);
                return domain;
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

    public static void ConsumeMapping(this Parser parser, Action<Scalar> consumer)
    {
        parser.Consume<MappingStart>();

        while (parser.Current is not MappingEnd)
        {
            var prop = parser.Consume<Scalar>();
            consumer(prop);
        }

        parser.Consume<MappingEnd>();
    }

    public static void ConsumeSequence(this Parser parser, Action consumer)
    {
        parser.Consume<SequenceStart>();

        while (parser.Current is not SequenceEnd)
        {
            consumer();
        }

        parser.Consume<SequenceEnd>();
    }
}
