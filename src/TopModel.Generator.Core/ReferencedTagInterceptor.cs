using Castle.DynamicProxy;
using TopModel.Core;

namespace TopModel.Generator.Core;

public class ReferencedTagInterceptor : IInterceptor
{
    /// <inheritdoc cref="IInterceptor.Intercept" />
    public void Intercept(IInvocation invocation)
    {
        if (invocation.InvocationTarget is WatcherConfigBase config)
        {
            var parameters = invocation.Method.GetParameters().ToList();
            var tagParameter = parameters.SingleOrDefault(p => p.Name == "tag" && p.ParameterType == typeof(string));

            if (tagParameter != null)
            {
                var tagValue = invocation.Arguments?[parameters.IndexOf(tagParameter)];
                if (
                    tagValue is string tag
                    && !config.Tags.Contains(tagValue)
                    && config.ReferencedTagConfigs.TryGetValue(tag, out var referencedConfig)
                )
                {
                    invocation.ReturnValue = invocation.Method.Invoke(referencedConfig, invocation.Arguments);
                    return;
                }
            }
        }

        invocation.Proceed();
    }
}
