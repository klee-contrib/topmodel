using System.Text;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Jpa;

public static class JpaUtils
{
    public static JavaWriter OpenJavaWriter(
        this GeneratorBase<JpaConfig> generator,
        string fileName,
        string packageName
    )
    {
        return new JavaWriter(
            generator.OpenFileWriter(
                fileName,
                new() { Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false), IndentValue = "\t" }
            ),
            packageName
        );
    }

    public static string ToFilePath(this string path)
    {
        var package = path.Split(':')[^1];
        var beforePackage = string.Empty;
        if (!string.IsNullOrEmpty(package))
        {
            beforePackage = path.Replace(package, string.Empty).TrimEnd(':');
        }

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
