using TopModel.Utils;

namespace TopModel.Core;

public struct Namespace
{
    public string App { get; set; }

    public string AppPath => App.Replace('.', Path.DirectorySeparatorChar);

    public string Module { get; set; }

    public string ModuleFlat => Module.Replace(".", string.Empty);

    public string ModuleCamel => string.Join('.', Module.Split('.').Select(m => m.ToCamelCase()));

    public string ModulePath => Module.Replace('.', Path.DirectorySeparatorChar);

    public string ModulePathKebab => string.Join(Path.DirectorySeparatorChar, Module.Split('.').Select(m => m.ToKebabCase()));

    public string RootModule => Module.Split('.').First();
}