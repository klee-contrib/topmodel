using System.Security.Cryptography;
using System.Text;
using TopModel.Generator.Core;

namespace TopModel.Generator;

public static class ModuleUtils
{
    public static string? GetFolderHash(string path)
    {
        if (!Directory.Exists(path))
        {
            return null;
        }

        return GetHash(Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories), path);
    }

    public static string? GetHash(IEnumerable<string> f, string path)
    {
        var md5 = MD5.Create();
        var files = f.Order().ToList();
        foreach (var file in files)
        {
            var relativePath = Path.GetRelativePath(path, file).Replace('\\', '/');
            var pathBytes = Encoding.UTF8.GetBytes(relativePath.ToLower());
            md5.TransformBlock(pathBytes, 0, pathBytes.Length, pathBytes, 0);

            var contentBytes = File.ReadAllBytes(file);
            if (files.IndexOf(file) == files.Count - 1)
            {
                md5.TransformFinalBlock(contentBytes, 0, contentBytes.Length);
            }
            else
            {
                md5.TransformBlock(contentBytes, 0, contentBytes.Length, contentBytes, 0);
            }
        }

        return md5.Hash != null ? BitConverter.ToString(md5.Hash).Replace("-", string.Empty).ToLower() : null;
    }

    public static Type? GetIGenRegInterface(Type t)
    {
        return t.GetInterfaces()
            .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IGeneratorRegistration<>));
    }

    public static (Type Type, string Name) GetIGenRegInterfaceAndName(Type generator)
    {
        var configType = GetIGenRegInterface(generator)!.GetGenericArguments()[0];
        var configName = configType.Name.Replace("Config", string.Empty).ToLower();
        return (configType, configName);
    }
}
