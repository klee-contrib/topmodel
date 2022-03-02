using TopModel.Core;

namespace TopModel.Generator.CSharp;

/// <summary>
/// Classe utilitaire destinée à la génération de C#.
/// </summary>
public static class CSharpUtils
{
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
            throw new ArgumentNullException("name");
        }

        if (regType == null)
        {
            InitializeRegType();
        }

        regType!.TryGetValue(name, out var res);
        return res;
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
            throw new ArgumentNullException("name");
        }

        if (regType == null)
        {
            InitializeRegType();
        }

        return regType!.ContainsKey(name);
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