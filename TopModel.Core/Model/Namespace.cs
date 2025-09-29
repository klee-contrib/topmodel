#pragma warning disable S1133

using TopModel.Utils;

namespace TopModel.Core.Model;

public struct Namespace
{
    public string App { get; set; }

    public string Module { get; set; }

    public string ModuleFlat => Module.Replace(".", string.Empty);

    public string ModuleCamel => string.Join('.', Module.Split('.').Select(m => m.ToCamelCase()));

    public string ModulePath => Module.Replace('.', Path.DirectorySeparatorChar);

    public string ModulePathKebab =>
        string.Join(Path.DirectorySeparatorChar, Module.Split('.').Select(m => m.ToKebabCase()));
}
