using YamlDotNet.Core;

internal interface ILoader<T>
{
    T Load(Parser parser);
}
