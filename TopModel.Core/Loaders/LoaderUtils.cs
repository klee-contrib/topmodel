using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace TopModel.Core.Loaders;
public static class LoaderUtils
{
    public static void ConsumeMapping(this Parser parser, Action consumer)
    {
        parser.Consume<MappingStart>();

        while (parser.Current is not MappingEnd)
        {
            consumer();
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
