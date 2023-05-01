namespace TopModel.Generator.Php;

public static class PhpUtils
{
    public static string ToFilePath(this string path)
    {
        return path.Replace(':', '.').Replace('.', Path.DirectorySeparatorChar);
    }

    public static string ToPackageName(this string path)
    {
        return @"App\" + path.Split(':').Last().Replace('/', '\\').Replace('.', '\\');
    }
}
