using TopModel.Utils;

namespace TopModel.Generator.Core;

public static class GeneratorUtils
{
    public static void HandleConfigs<T>(string dn, IEnumerable<T>? configs, Action<T, int> handler)
        where T : GeneratorConfigBase
    {
        if (configs != null)
        {
            for (var i = 0; i < configs.Count(); i++)
            {
                var config = configs.ElementAt(i);
                var number = i + 1;

                config.InitVariables(number);

                ModelUtils.CombinePath(dn, config, c => c.OutputDirectory);

                handler(config, number);
            }
        }
    }
}
