using TopModel.Core;

namespace TopModel.Generator.Php;

public static class ImportsPhpExtensions
{
    public static string GetImport(this Class classe, PhpConfig config, string tag)
    {
        return @$"{config.GetPackageName(classe, tag)}\{classe.NamePascal}";
    }
}
