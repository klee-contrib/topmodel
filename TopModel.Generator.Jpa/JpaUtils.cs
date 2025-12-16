using System.Text;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Jpa;

public static class JpaUtils
{
    public static JavaWriter OpenJavaWriter(
        this GeneratorBase<JpaConfig> generator,
        string fileName,
        string packageName,
        int? codePage = 1252
    )
    {
        return new JavaWriter(
            generator.OpenFileWriter(
                fileName,
                codePage != null
                    ? CodePagesEncodingProvider.Instance.GetEncoding(codePage.Value)!
                    : new UTF8Encoding(encoderShouldEmitUTF8Identifier: false)
            ),
            packageName
        );
    }

    public static string ToFilePath(this string path)
    {
        var package = path.Split(':')[^1];
        var beforePackage = path.Replace(package, string.Empty).TrimEnd(':');
        return Path.Combine(beforePackage, package.ToPackageName().Replace('.', Path.DirectorySeparatorChar));
    }

    public static string ToPackageName(this string path)
    {
        return path.Split(':')[^1].ToLower().Replace('/', '.').Replace('\\', '.').Replace('-', '_').ToLower();
    }

    public static string WithPrefix(this string name, string prefix)
    {
        return $"{prefix}{name.ToFirstUpper()}";
    }
}
