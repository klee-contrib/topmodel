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

    public static string GetDbContextFilePath(this CSharpConfig config, string appName)
    {
        var dbContextName = config.GetDbContextName(appName);
        var destDirectory = Path.Combine(config.OutputDirectory, config.DbContextPath!).Replace("{app}", appName);
        Directory.CreateDirectory(destDirectory);

        return Path.Combine(destDirectory, "generated", $"{dbContextName}.cs");
    }

    public static string GetDbContextNamespace(this CSharpConfig config, string appName)
    {
        var ns = config.DbContextPath!.Replace("\\", "/").Replace("/", ".");
        return ns[Math.Max(0, ns.IndexOf("{app}"))..]
             .Replace("{app}", appName);
    }

    public static string? GetMapperFilePath(this CSharpConfig config, Class? sampleClass)
    {
        if (sampleClass == null)
        {
            return null;
        }

        var directory = Path.Combine(config.OutputDirectory, config.GetModelPath(sampleClass), "generated");
        Directory.CreateDirectory(directory);

        return Path.Combine(directory, $"{sampleClass.Namespace.Module}{(sampleClass.IsPersistent ? string.Empty : "DTO")}Mappers.cs");
    }

    public static string GetReferenceAccessorName(this CSharpConfig config, Class classe)
    {
        return config.ReferenceAccessorsName
            .Replace("{module}", classe.Namespace.Module.Replace(".", string.Empty))
            .Replace("{app}", classe.Namespace.App);
    }

    public static string GetReferenceInterfaceFilePath(this CSharpConfig config, IEnumerable<Class> classList)
    {
        var firstClass = classList.FirstOrDefault();

        if (firstClass == null)
        {
            return null!;
        }

        var projectDir = Path.Combine(
            config.OutputDirectory,
            config.ReferenceAccessorsInterfacePath
                .Replace("{module}", firstClass.Namespace.Module.Replace('.', Path.DirectorySeparatorChar))
                .Replace("{app}", firstClass.Namespace.App));
        var className = config.GetReferenceAccessorName(firstClass);
        return Path.Combine(projectDir, "generated", $"I{className}.cs");
    }

    public static string GetReferenceImplementationFilePath(this CSharpConfig config, IEnumerable<Class> classList)
    {
        var firstClass = classList.FirstOrDefault();

        if (firstClass == null)
        {
            return null!;
        }

        var projectDir = Path.Combine(
            config.OutputDirectory,
            config.ReferenceAccessorsImplementationPath
                .Replace("{module}", firstClass.Namespace.Module.Replace('.', Path.DirectorySeparatorChar))
                .Replace("{app}", firstClass.Namespace.App));
        var className = config.GetReferenceAccessorName(firstClass);
        return Path.Combine(projectDir, "generated", $"{className}.cs");
    }

    public static string GetReferenceInterfaceNamespace(this CSharpConfig config, Class classe)
    {
        var ns = config.ReferenceAccessorsInterfacePath.Replace("\\", "/").Replace("/", ".");
        return ns[Math.Max(0, ns.IndexOf("{app}"))..]
            .Replace("{app}", classe.Namespace.App)
            .Replace("{module}", classe.Namespace.Module);
    }

    public static string GetReferenceImplementationNamespace(this CSharpConfig config, Class classe)
    {
        var ns = config.ReferenceAccessorsImplementationPath.Replace("\\", "/").Replace("/", ".");
        return ns[Math.Max(0, ns.IndexOf("{app}"))..]
            .Replace("{app}", classe.Namespace.App)
            .Replace("{module}", classe.Namespace.Module);
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
                string _ when cp.DomainKind!.CSharp!.Type.Contains("{composition.name}") => cp.DomainKind.CSharp.Type.ParseTemplate(cp),
                string _ => $"{cp.DomainKind.CSharp.Type}<{{composition.name}}>".ParseTemplate(cp)
            },
            AssociationProperty { Association: var assoc } when config.CanClassUseEnums(assoc) => $"{assoc}.{assoc.PrimaryKey!.Name}s?",
            AliasProperty { Property: AssociationProperty { Association: var assoc }, AsList: var asList } when config.CanClassUseEnums(assoc) => $"{assoc}.{assoc.PrimaryKey!.Name}s{(asList ? "[]" : "?")}",
            RegularProperty { PrimaryKey: true } when config.CanClassUseEnums(prop.Class) => $"{prop.Name}s?",
            AliasProperty { Property: RegularProperty { PrimaryKey: true, Class: var alClass }, AsList: var asList } when config.CanClassUseEnums(alClass) => $"{alClass}.{alClass.PrimaryKey!.Name}s{(asList ? "[]" : "?")}",
            IFieldProperty fp => fp.Domain.CSharp?.Type.ParseTemplate(fp) ?? string.Empty,
            _ => string.Empty
        };

        type = nonNullable && type.EndsWith("?") ? type[0..^1] : type;
        return type;
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