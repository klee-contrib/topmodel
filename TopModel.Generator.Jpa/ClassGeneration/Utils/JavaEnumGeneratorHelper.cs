using TopModel.Core.Model;
using TopModel.Utils;

namespace TopModel.Generator.Jpa.ClassGeneration.Utils;

/// <summary>
/// Générateur de fichiers de modèles JPA.
/// </summary>
public class JavaEnumGeneratorHelper(JpaConfig config) : JavaConstructorGenerator(config)
{
    /// <summary>
    /// Retourne les valeurs de la classe triées par OrderProperty, sinon par DefaultProperty,
    /// sinon dans l'ordre de déclaration (même politique que la liste <c>Values</c> du générateur C#).
    /// </summary>
    /// <param name="classe">La classe.</param>
    /// <returns>Les valeurs triées.</returns>
    public static IList<ClassValue> GetOrderedValues(Class classe)
    {
        var orderProperty = classe.OrderProperty ?? classe.DefaultProperty;
        return orderProperty != null
            ? classe
                .Values.OrderBy(
                    v => v.Value.TryGetValue(orderProperty, out var value) ? value : null,
                    StringComparer.InvariantCulture
                )
                .ToList()
            : classe.Values.ToList();
    }

    /// <summary>
    /// Retourne les valeurs triées par nom (même politique que le générateur C# pour les constantes et les enums).
    /// </summary>
    /// <param name="classe">La classe.</param>
    /// <param name="values">Valeurs à trier (par défaut, les valeurs de la classe).</param>
    /// <returns>Les valeurs triées par nom.</returns>
    public static IList<ClassValue> GetValuesByName(Class classe, IEnumerable<ClassValue>? values = null)
    {
        return (values ?? classe.Values)
            .OrderBy(v => v.Name.ToPascalCase(strictIfUppercase: true), StringComparer.Ordinal)
            .ToList();
    }

    /// <summary>
    /// Génère l'appel du constructeur tout argument pour une valeur de référence donnée,
    /// avec les valeurs calculées pour chaque propriété de la classe.
    /// </summary>
    /// <param name="classe">Classe de l'énumération.</param>
    /// <param name="refValue">Valeur de référence pour laquelle construire l'appel.</param>
    /// <param name="tag">Le tag de génération.</param>
    /// <returns>Expression Java de la forme <c>new ClassName(val1, val2, ...)</c>.</returns>
    public string GetAllArgsConstructorCall(Class classe, ClassValue refValue, string tag)
    {
        var args = Config.GetProperties(classe).Select(prop => GetPropertyValue(classe, prop, refValue, tag)).ToArray();
        return $"new {classe.NamePascal}{GetAllArgsConstructor(classe, tag).CallWith(args)}";
    }

    /// <summary>
    /// Génération des constantes statiques.
    /// </summary>
    /// <param name="classe">La classe générée.</param>
    /// <param name="tag">Le tag de génération.</param>
    public virtual IEnumerable<JavaField> GetConstFields(Class classe, string tag)
    {
        foreach (var refValue in GetValuesByName(classe))
        {
            var field = new JavaField(classe.NamePascal, refValue.Name.ToConstantCase())
            {
                Static = true,
                Final = true,
                Visibility = "public",
                DefaultValue = GetAllArgsConstructorCall(classe, refValue, tag),
            };
            field.AddImports(GetAllArgsConstructorCallImports(classe, refValue, tag));
            yield return field;
        }
    }

    public JavaMethod GetGetValueStaticMethod(Class classe, string tag)
    {
        var key = classe.EnumKey!;
        var method = new JavaMethod(Config.GetTypeName(classe), "getValue")
        {
            Static = true,
            Visibility = "public",
            Comment = Config.GetMessage(JpaMessage.EnumReturn),
            ReturnComment = Config.GetMessage(JpaMessage.EnumValueForKey, key),
        };
        method.AddParameter(
            new JavaMethodParameter(Config.GetType(key, tag), key.NameCamel)
            {
                Comment = Config.GetMessage(JpaMessage.EnumValueKey),
            }
        );
        method.AddBodyLine(@$"return switch ({key.NameCamel}) {{");
        foreach (var refValue in classe.Values.OrderBy(v => v.Value[key], StringComparer.Ordinal))
        {
            var code = Config.GetValue(key, tag, refValue.Value[key]);
            if (key.EnumProperty != null && Config.UniqueValueGeneration != Core.UniqueValueGenerationMode.ConstOnly)
            {
                code = code.Replace($"{Config.GetEnumType(key)}.", string.Empty);
            }
            method.AddBodyLine(1, $"case {code} -> {refValue.Name.ToConstantCase()};");
        }

        if (key.EnumProperty == null || Config.UniqueValueGeneration == Core.UniqueValueGenerationMode.ConstOnly)
        {
            method.AddBodyLine(
                1,
                $"default -> throw new IllegalArgumentException(\"{Config.GetMessage(JpaMessage.EnumUnknownKey)}\" + "
                    + key.NameCamel
                    + ");"
            );
        }
        method.AddBodyLine("};");

        return method;
    }

    /// <summary>
    /// Calcule la valeur Java à utiliser pour une propriété d'une valeur de référence
    /// (utilisée aussi bien dans le corps du constructeur enum que pour l'appel du constructeur tout argument).
    /// </summary>
    public string GetPropertyValue(Class classe, IProperty prop, ClassValue refValue, string tag)
    {
        if (prop == classe.EnumKey)
        {
            return Config.GetValue(prop, tag, refValue.Value[prop]);
        }

        var value = refValue.Value.TryGetValue(prop, out var v) ? v : "null";

        if (Config.TranslateReferences == true && classe.DefaultProperty == prop)
        {
            return $"\"{refValue.ResourceKey}\"";
        }

        return Config.GetValue(prop, tag, value);
    }

    public JavaField GetStaticValuesList(Class classe)
    {
        var stringValues = string.Join(
            ", ",
            GetOrderedValues(classe).Select(refValue => refValue.Name.ToConstantCase())
        );
        var field = new JavaField($"List<{classe.NamePascal}>", "VALUES")
        {
            Static = true,
            Final = true,
            Visibility = "public",
            DefaultValue = $"List.of({stringValues})",
        }
            .AddCommentLine(Config.GetMessage(JpaMessage.EnumAllValues, classe.NamePascal))
            .AddImports("java.util.List");
        return field;
    }

    private List<string> GetAllArgsConstructorCallImports(Class classe, ClassValue refValue, string tag)
    {
        return Config
            .GetProperties(classe)
            .SelectMany(prop =>
            {
                var rawValue =
                    prop == classe.EnumKey ? refValue.Value[prop]
                    : refValue.Value.TryGetValue(prop, out var v) ? v
                    : null;

                return rawValue != null ? Config.GetValueImportsJpa(prop, tag, rawValue) : [];
            })
            .ToList();
    }
}
