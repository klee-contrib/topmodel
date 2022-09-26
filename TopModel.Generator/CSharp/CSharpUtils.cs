using TopModel.Core;

namespace TopModel.Generator.CSharp;

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

    public static string GetClassFileName(this CSharpConfig config, Class classe)
    {
        var directory = Path.Combine(config.OutputDirectory, config.GetModelPath(classe), "generated");
        Directory.CreateDirectory(directory);

        return Path.Combine(directory, classe.Name + ".cs");
    }

    public static string? GetDbContextFilePath(this CSharpConfig config, string appName)
    {
        if (config.DbContextPath == null)
        {
            return null;
        }

        var dbContextName = config.GetDbContextName(appName);
        var destDirectory = Path.Combine(config.OutputDirectory, config.DbContextPath);
        Directory.CreateDirectory(destDirectory);

        return Path.Combine(destDirectory, "generated", $"{dbContextName}.cs");
    }

    public static string? GetMapperFilePath(this CSharpConfig config, IEnumerable<Class> classList)
    {
        var firstClass = classList.FirstOrDefault();

        if (firstClass == null)
        {
            return null;
        }

        var directory = Path.Combine(config.OutputDirectory, config.GetModelPath(firstClass), "Mappers/generated");
        Directory.CreateDirectory(directory);

        return Path.Combine(directory, $"{firstClass.Namespace.Module}Mappers.cs");
    }

    public static string? GetReferenceInterfaceFilePath(this CSharpConfig config, IEnumerable<Class> classList)
    {
        var firstClass = classList.FirstOrDefault();

        if (firstClass == null)
        {
            return null;
        }

        string projectDir;
        string interfaceName;
        if (config.DbContextPath != null)
        {
            projectDir = $"{config.OutputDirectory}\\{config.DbContextPath}";
            interfaceName = $"I{firstClass.Namespace.Module}AccessorsDal";
        }
        else
        {
            projectDir = $"{config.OutputDirectory}\\{config.GetModelPath(firstClass).Replace("DataContract", "Contract")}";
            interfaceName = $"IService{firstClass.Namespace.Module}Accessors";
        }

        return Path.Combine(projectDir, config.DbContextPath == null ? "generated" : "generated\\Reference", $"{interfaceName}.cs");
    }

    public static string? GetReferenceImplementationFilePath(this CSharpConfig config, IEnumerable<Class> classList)
    {
        var firstClass = classList.FirstOrDefault();

        if (firstClass == null)
        {
            return null;
        }

        string projectDir;
        string implementationName;

        if (config.DbContextPath != null)
        {
            projectDir = $"{config.OutputDirectory}\\{config.DbContextPath}";
            implementationName = $"{firstClass.Namespace.Module}AccessorsDal";
        }
        else
        {
            var projectName = $"{firstClass.Namespace.App}.{firstClass.Namespace.Module}Implementation";
            projectDir = $"{config.OutputDirectory}\\{firstClass.Namespace.App}.Implementation\\{projectName}\\Service.Implementation";
            implementationName = $"Service{firstClass.Namespace.Module}Accessors";
        }

        return Path.Combine(projectDir, config.DbContextPath == null ? "generated" : "generated\\Reference", $"{implementationName}.cs");
    }

    public static string GetPropertyTypeName(this CSharpConfig config, IProperty prop, bool nonNullable = false, bool useIEnumerable = true)
    {
        var type = prop switch
        {
            CompositionProperty cp => cp.Kind switch
            {
                "object" => cp.Composition.Name,
                "list" => $"{(useIEnumerable ? "IEnumerable" : "ICollection")}<{cp.Composition.Name}>",
                "async-list" => $"IAsyncEnumerable<{cp.Composition.Name}>",
                string _ when cp.DomainKind!.CSharp!.Type.Contains("{class}") => cp.DomainKind.CSharp.Type.Replace("{class}", cp.Composition.Name),
                string _ => $"{cp.DomainKind.CSharp.Type}<{cp.Composition.Name}>"
            },
            AssociationProperty { Association: var assoc } when config.CanClassUseEnums(assoc) => $"{assoc}.{assoc.PrimaryKey!.Name}s?",
            AliasProperty { Property: AssociationProperty { Association: var assoc } } when config.CanClassUseEnums(assoc) => $"{assoc}.{assoc.PrimaryKey!.Name}s?",
            RegularProperty { PrimaryKey: true } when config.CanClassUseEnums(prop.Class) => $"{prop.Name}s?",
            AliasProperty { Property: RegularProperty { PrimaryKey: true, Class: var alClass } } when config.CanClassUseEnums(alClass) => $"{alClass}.{alClass.PrimaryKey!.Name}s?",
            IFieldProperty fp => fp.Domain.CSharp?.Type ?? string.Empty,
            _ => string.Empty
        };

        var isListDomain = prop is AliasProperty { ListDomain: not null };

        type = (nonNullable || isListDomain) && type.EndsWith("?") ? type[0..^1] : type;

        return isListDomain ? $"{type}[]" : type;
    }

    public static string GetReturnTypeName(this CSharpConfig config, IProperty? prop)
    {
        if (prop == null)
        {
            return config.NoAsyncControllers ? "void" : "async Task";
        }

        var typeName = GetPropertyTypeName(config, prop, true);
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