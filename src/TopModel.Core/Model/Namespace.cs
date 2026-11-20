using TopModel.Utils;

namespace TopModel.Core.Model;

public readonly struct Namespace
{
    public string App { get; init; }

    public string Module { get; init; }

    public string ModuleFlat => Module.Replace(".", string.Empty);

    public string ModuleCamel => string.Join('.', Module.Split('.').Select(m => m.ToCamelCase()));

    public string ModulePath => Module.Replace('.', Path.DirectorySeparatorChar);

    public string ModulePathKebab =>
        string.Join(Path.DirectorySeparatorChar, Module.Split('.').Select(m => m.ToKebabCase()));
}
