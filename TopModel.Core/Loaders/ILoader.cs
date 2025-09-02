using YamlDotNet.Core;

namespace TopModel.Core.Loaders;

internal interface ILoader<T>
{
    T Load(Parser parser);
}
