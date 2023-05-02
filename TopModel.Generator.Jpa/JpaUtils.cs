using TopModel.Core;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Jpa;

public static class JpaUtils
{
    public static IList<IProperty> GetAllArgsProperties(this Class classe, List<Class> availableClasses, string tag)
    {
        if (classe.Extends is null)
        {
            return classe.GetProperties(availableClasses);
        }
        else
        {
            return GetAllArgsProperties(classe.Extends, availableClasses, tag).Concat(classe.GetProperties(availableClasses)).ToList();
        }
    }

    public static string ToFilePath(this string path)
    {
        return path.ToLower().Replace(':', '.').Replace('.', Path.DirectorySeparatorChar);
    }

    public static string ToPackageName(this string path)
    {
        return path.Split(':').Last().ToLower().Replace('/', '.').Replace('\\', '.');
    }

    public static string WithPrefix(this string name, string prefix)
    {
        return $"{prefix}{name.ToFirstUpper()}";
    }
}
