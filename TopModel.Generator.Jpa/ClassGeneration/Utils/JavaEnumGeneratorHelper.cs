using TopModel.Core.Model;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Jpa.ClassGeneration.Utils;

/// <summary>
/// Générateur de fichiers de modèles JPA.
/// </summary>
public class JavaEnumGeneratorHelper(JpaConfig config) : JavaConstructorGenerator(config)
{
    /// <summary>
    /// Génère l'appel du constructeur tout argument pour une valeur de référence donnée,
    /// avec les valeurs calculées pour chaque propriété de la classe.
    /// </summary>
    /// <param name="classe">Classe de l'énumération.</param>
    /// <param name="refValue">Valeur de référence pour laquelle construire l'appel.</param>
    /// <param name="allArgsConstructor">Constructeur tout argument à appeler.</param>
    /// <returns>Expression Java de la forme <c>new ClassName(val1, val2, ...)</c>.</returns>
    public string GetAllArgsConstructorCall(Class classe, ClassValue refValue, string tag)
    {
        var args = classe.Properties.Select(prop => GetPropertyValue(classe, prop, refValue)).ToArray();
        return $"new {classe.NamePascal}{GetAllArgsConstructor(classe, tag).CallWith(args)}";
    }

    /// <summary>
    /// Génération des constantes statiques.
    /// </summary>
    /// <param name="classe">La classe générée.</param>
    /// <param name="tag">Le tag de génération.</param>
    public virtual IEnumerable<JavaField> GetConstFields(Class classe, string tag)
    {
        foreach (var refValue in classe.Values.OrderBy(x => x.Name, StringComparer.Ordinal))
        {
            var field = new JavaField(classe.NamePascal, refValue.Name.ToConstantCase())
            {
                Static = true,
                Final = true,
                Visibility = "public",
                DefaultValue = GetAllArgsConstructorCall(classe, refValue, tag),
            };
            field.Imports.AddRange(GetAllArgsConstructorCallImports(classe, tag));
            yield return field;
        }
    }

    public JavaMethod GetGetValueStaticMethod(Class classe)
    {
        var key = classe.EnumKey!;
        var method = new JavaMethod(Config.GetTypeName(classe), "getValue")
        {
            Static = true,
            Visibility = "public",
            Comment = $"Retourne la valeur de l'énumération pour la clé spécifiée.",
            ReturnComment = $"La valeur de l'énumération correspondant à la clé '{key}'.",
        };
        method.AddParameter(
            new JavaMethodParameter(Config.GetType(key), key.NameCamel)
            {
                Comment = $"La clé de l'énumération pour laquelle obtenir la valeur.",
            }
        );
        method.AddBodyLine(@$"return switch ({key.NameCamel}) {{");
        foreach (var refValue in classe.Values.OrderBy(x => x.Name, StringComparer.Ordinal))
        {
            var code = Config.GetValue(key, refValue.Value[key]);
            if (key.EnumProperty != null)
            {
                code = code.Replace($"{Config.GetEnumType(key)}.", string.Empty);
            }
            method.AddBodyLine(1, $"case {code} -> {refValue.Name.ToConstantCase()};");
        }
        if (key.EnumProperty == null)
        {
            method.AddBodyLine(
                1,
                @"default -> throw new IllegalArgumentException(""Clé d'énumération inconnue : "" + "
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
    public string GetPropertyValue(Class classe, IProperty prop, ClassValue refValue)
    {
        if (prop == classe.EnumKey)
        {
            return Config.GetValue(prop, refValue.Value[prop]);
        }

        var value = refValue.Value.TryGetValue(prop, out var v) ? v : "null";

        if (Config.TranslateReferences == true && classe.DefaultProperty == prop)
        {
            return $"\"{refValue.ResourceKey}\"";
        }

        return Config.GetValue(prop, value);
    }

    public JavaField GetStaticValuesList(Class classe)
    {
        var values =
            (classe.OrderProperty ?? classe.DefaultProperty) != null
                ? classe.Values.OrderBy(v => v.Value[classe.OrderProperty ?? classe.DefaultProperty]).ToList()
                : classe.Values;
        var stringValues = string.Join(", ", values.Select(refValue => refValue.Name.ToConstantCase()));
        var field = new JavaField($"List<{classe.NamePascal}>", "VALUES")
        {
            Static = true,
            Final = true,
            Visibility = "public",
            DefaultValue = $"List.of({stringValues})",
            Comment = [$"Liste de toutes les valeurs de l'énumération {classe.NamePascal}."],
        };
        field.Imports.Add("java.util.List");
        return field;
    }

    private IList<string> GetAllArgsConstructorCallImports(Class classe, string tag)
    {
        return classe
            .Properties.SelectMany(prop =>
            {
                if (Config.UniqueValueGeneration.CanConst && prop.UniqueValuedProperty != null)
                {
                    return new[]
                    {
                        $"{Config.GetEnumPackageName(prop.UniqueValuedProperty!.Class, tag)}.{prop.UniqueValuedProperty!.Class.NamePascal}{prop.UniqueValuedProperty!.NamePascal}",
                    };
                }
                return [];
            })
            .ToList();
    }
}
