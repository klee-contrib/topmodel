using TopModel.Core;

namespace TopModel.Generator.Csharp;

/// <summary>
/// Classe utilitaire destinée à la génération de C#.
/// </summary>
public static class CSharpUtils
{
    private static readonly HashSet<string> _reservedKeywords = new(new[]
    {
        "abstract",
        "as",
        "base",
        "bool",
        "break",
        "byte",
        "case",
        "catch",
        "char",
        "checked",
        "class",
        "const",
        "continue",
        "decimal",
        "default",
        "delegate",
        "do",
        "double",
        "else",
        "enum",
        "event",
        "explicit",
        "extern",
        "false",
        "file",
        "finally",
        "fixed",
        "float",
        "for",
        "foreach",
        "goto",
        "if",
        "implicit",
        "in",
        "int",
        "interface",
        "internal",
        "is",
        "lock",
        "long",
        "namespace",
        "new",
        "null",
        "object",
        "operator",
        "out",
        "override",
        "params",
        "private",
        "protected",
        "public",
        "readonly",
        "ref",
        "return",
        "sbyte",
        "sealed",
        "short",
        "sizeof",
        "stackalloc",
        "static",
        "string",
        "struct",
        "switch",
        "this",
        "throw",
        "true",
        "try",
        "typeof",
        "uint",
        "ulong",
        "unchecked",
        "unsafe",
        "ushort",
        "using",
        "virtual",
        "void",
        "volatile",
        "while"
    });

    private static IDictionary<string, string>? regType;

    /// <summary>
    /// Donne la valeur par défaut d'un type de base C#.
    /// Renvoie null si le type n'est pas un type par défaut.
    /// </summary>
    /// <param name="name">Nom du type à définir.</param>
    /// <returns>Vrai si le type est un type C#.</returns>
    public static string? GetCSharpDefaultValueBaseType(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        if (regType == null)
        {
            InitializeRegType();
        }

        regType!.TryGetValue(name, out var res);
        return res;
    }

    public static string GetClassFileName(this CsharpConfig config, Class classe, string tag)
    {
        return Path.Combine(
            config.OutputDirectory,
            config.GetModelPath(classe, tag),
            "generated",
            (classe.Abstract ? "I" : string.Empty) + classe.NamePascal + ".cs");
    }

    public static string GetDbContextFilePath(this CsharpConfig config, string tag)
    {
        return Path.Combine(
            config.OutputDirectory,
            config.ResolveVariables(config.DbContextPath!, tag: tag).ToFilePath(),
            "generated",
            $"{config.GetDbContextName(tag)}.cs");
    }

    public static string GetDbContextNamespace(this CsharpConfig config, string tag)
    {
        return config.ResolveVariables(config.DbContextPath!, tag: tag)
            .ToNamespace();
    }

    public static string GetMapperFilePath(this CsharpConfig config, (Class Classe, FromMapper Mapper) mapper, string tag)
    {
        var (ns, modelPath) = config.GetMapperLocation(mapper, tag);
        return Path.Combine(
            config.OutputDirectory,
            config.ResolveVariables(modelPath, tag: tag, module: ns.ModulePath).ToFilePath(),
            "generated",
            $"{config.GetMapperName(ns, modelPath)}.cs");
    }

    public static string GetMapperFilePath(this CsharpConfig config, (Class Classe, ClassMappings Mapper) mapper, string tag)
    {
        var (ns, modelPath) = config.GetMapperLocation(mapper, tag);
        return Path.Combine(
            config.OutputDirectory,
            config.ResolveVariables(modelPath, tag: tag, module: ns.ModulePath).ToFilePath(),
            "generated",
            $"{config.GetMapperName(ns, modelPath)}.cs");
    }

    public static (Namespace Namespace, string ModelPath) GetMapperLocation(this CsharpConfig config, (Class Classe, FromMapper Mapper) mapper, string tag)
    {
        var pmp = config.NoPersistence(tag) ? config.NonPersistentModelPath : config.PersistentModelPath;

        if (mapper.Classe.IsPersistent)
        {
            return (mapper.Classe.Namespace, pmp);
        }

        var persistentParam = mapper.Mapper.Params.FirstOrDefault(p => p.Class.IsPersistent);
        if (persistentParam != null)
        {
            return (persistentParam.Class.Namespace, pmp);
        }

        return (mapper.Classe.Namespace, config.NonPersistentModelPath);
    }

    public static (Namespace Namespace, string ModelPath) GetMapperLocation(this CsharpConfig config, (Class Classe, ClassMappings Mapper) mapper, string tag)
    {
        var pmp = config.NoPersistence(tag) ? config.NonPersistentModelPath : config.PersistentModelPath;

        if (mapper.Classe.IsPersistent)
        {
            return (mapper.Classe.Namespace, pmp);
        }

        if (mapper.Mapper.Class.IsPersistent)
        {
            return (mapper.Mapper.Class.Namespace, pmp);
        }

        return (mapper.Classe.Namespace, config.NonPersistentModelPath);
    }

    public static string GetMapperName(this CsharpConfig config, Namespace ns, string modelPath)
    {
        return $"{ns.ModuleFlat}{(modelPath == config.PersistentModelPath ? string.Empty : "DTO")}Mappers";
    }

    public static string GetReferenceAccessorName(this CsharpConfig config, Namespace ns, string tag)
    {
        return config.ResolveVariables(
            config.ReferenceAccessorsName,
            tag: tag,
            module: ns.ModuleFlat);
    }

    public static string GetReferenceInterfaceFilePath(this CsharpConfig config, Namespace ns, string tag)
    {
        return Path.Combine(
            config.OutputDirectory,
            config.ResolveVariables(
                config.ReferenceAccessorsInterfacePath,
                tag: tag,
                module: ns.ModulePath).ToFilePath(),
            "generated",
            $"I{config.GetReferenceAccessorName(ns, tag)}.cs");
    }

    public static string GetReferenceImplementationFilePath(this CsharpConfig config, Namespace ns, string tag)
    {
        return Path.Combine(
            config.OutputDirectory,
            config.ResolveVariables(
                config.ReferenceAccessorsImplementationPath,
                tag: tag,
                module: ns.ModulePath).ToFilePath(),
            "generated",
            $"{config.GetReferenceAccessorName(ns, tag)}.cs");
    }

    public static string GetReferenceInterfaceNamespace(this CsharpConfig config, Namespace ns, string tag)
    {
        return config.ResolveVariables(
            config.ReferenceAccessorsInterfacePath,
            tag: tag,
            module: ns.Module).ToNamespace();
    }

    public static string GetReferenceImplementationNamespace(this CsharpConfig config, Namespace ns, string tag)
    {
        return config.ResolveVariables(
            config.ReferenceAccessorsImplementationPath,
            tag: tag,
            module: ns.Module).ToNamespace();
    }

    public static string GetReturnTypeName(this CsharpConfig config, IProperty? prop)
    {
        if (prop == null)
        {
            return config.NoAsyncControllers ? "void" : "async Task";
        }

        var typeName = config.GetType(prop, nonNullable: true);
        return typeName.StartsWith("IAsyncEnumerable") || config.NoAsyncControllers
            ? typeName
            : $"async Task<{typeName}>";
    }

    /// <summary>
    /// Détermine si le type est un type de base C#.
    /// </summary>
    /// <param name="name">Nom du type à définir.</param>
    /// <returns>Vrai si le type est un type C#.</returns>
    public static bool IsCSharpBaseType(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        if (regType == null)
        {
            InitializeRegType();
        }

        return regType!.ContainsKey(name);
    }

    public static string ToFilePath(this string path)
    {
        return path.TrimEnd('.').Replace(':', Path.DirectorySeparatorChar);
    }

    public static string ToNamespace(this string path)
    {
        return path.Split(':').Last().Replace('/', '.').Replace('\\', '.').Replace("..", ".").Trim('.');
    }

    public static bool ShouldQuoteValue(this CsharpConfig config, IFieldProperty prop)
    {
        return config.GetType(prop) == "string";
    }

    /// <summary>
    /// Ajoute un verbatim (@) au cas où le nom soit réservé en C#.
    /// </summary>
    /// <param name="name">Nom.</param>
    /// <returns>Nom avec un verbatim (si besoin).</returns>
    public static string Verbatim(this string name)
    {
        if (_reservedKeywords.Contains(name))
        {
            return $"@{name}";
        }

        return name;
    }

    /// <summary>
    /// Initialisation des types.
    /// </summary>
    private static void InitializeRegType()
    {
        regType = new Dictionary<string, string>
            {
                { "int?", "0" },
                { "uint?", "0" },
                { "float?", "0.0f" },
                { "double?", "0.0" },
                { "bool?", "false" },
                { "short?", "0" },
                { "ushort?", "0" },
                { "long?", "0" },
                { "ulong?", "0" },
                { "decimal?", "0" },
                { "byte?", "0" },
                { "sbyte?", "0" },
                { "string", "\"\"" }
            };
    }
}