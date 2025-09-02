using System.Diagnostics.CodeAnalysis;
using TopModel.Core.Model;

namespace TopModel.Core.Utils;

internal static class VariableUtils
{
    internal static readonly Dictionary<string, string> ClassProperties = new()
    {
        ["trigram"] = "Trigramme de la classe",
        ["name"] = "Nom de la classe",
        ["sqlName"] = "Nom de la table SQL de la classe",
        ["comment"] = "Commentaire de la classe",
        ["label"] = "Libellé de la classe",
        ["pluralName"] = "Nom au pluriel de la classe",
        ["module"] = "Module du fichier contenant la classe",
    };

    internal static readonly Dictionary<string, string> DomainProperties = new()
    {
        ["mediaType"] = "Media type d'une proprieté du domaine",
        ["length"] = "Longueur d'une proprieté du domaine",
        ["scale"] = "Nombre de chiffres après la virgule pour une propriété du domaine",
        ["name"] = "Nom du domaine",
        ["type"] = "Type d'implémentation du domaine",
    };

    internal static readonly Dictionary<string, string> EndpointProperties = new()
    {
        ["name"] = "Nom de l'endpoint",
        ["method"] = "Méthode HTTP de l'endpoint",
        ["route"] = "Route de l'endpoint",
        ["description"] = "Description de l'endpoint",
        ["module"] = "Module du fichier contenant l'endpoint",
    };

    internal static readonly Dictionary<string, string> PropertyProperties = new()
    {
        ["T"] = "Type sur lequel s'applique le type générique",
        ["value"] = "Valeur de la propriété",
        ["name"] = "Nom de la propriété",
        ["sqlName"] = "Nom de la colonne SQL de la propriété",
        ["paramName"] = "Nom de la propriété en tant que paramètre d'endpoint",
        ["trigram"] = "Trigramme de la propriété",
        ["label"] = "Libellé de la propriété",
        ["comment"] = "Commentaire de la propriété",
        ["required"] = "Si la propriété est obligatoire",
        ["resourceKey"] = "Clé de traduction de la propriété",
        ["commentResourceKey"] = "Clé de traduction pour le commentaire de la propriété",
        ["defaultValue"] = "Valeur par défaut de la propriété",
    };

    internal static readonly string[] Transforms =
    [
        "camel",
        "constant",
        "kebab",
        "lower",
        "pascal",
        "snake",
        "upper",
        "flat",
        "path",
        "head",
        "last",
        "tail",
    ];

    public static bool IsValidTransform(this string input)
    {
        return Transforms.Contains(input);
    }

    public static bool TryGetClassVariable(
        this string input,
        ModelConfig config,
        IList<TemplateParameter> templateParameters,
        [MaybeNullWhen(false)] out Variable variable
    )
    {
        if (input.StartsWith("primaryKey."))
        {
            return input["primaryKey.".Length..].TryGetPropertyVariable(config, templateParameters, out variable);
        }

        if (input.StartsWith("extends."))
        {
            return input["extends.".Length..].TryGetClassVariable(config, templateParameters, out variable);
        }

        if (input.StartsWith($"customProperties."))
        {
            variable = new Variable { Description = "Propriété personnalisée" };
            return true;
        }

        if (input.StartsWith("properties["))
        {
            return input[(input.IndexOf(']') + 2)..].TryGetPropertyVariable(config, templateParameters, out variable);
        }

        if (templateParameters.Any(tp => tp.Name == input))
        {
            variable = new Variable { TemplateParameter = templateParameters.First(tp => tp.Name == input) };
            return true;
        }

        if (ClassProperties.TryGetValue(input, out var description))
        {
            variable = new Variable { Description = description };
            return true;
        }

        if (config.GlobalVariables.TryGetValue(input, out var gVariable))
        {
            variable = gVariable;
            return true;
        }

        if (config.TagVariables.TryGetValue(input, out var tVariable))
        {
            variable = tVariable;
            return true;
        }

        variable = null;
        return false;
    }

    public static bool TryGetConverterVariable(this string input, [MaybeNullWhen(false)] out Variable variable)
    {
        if (input.StartsWith("from."))
        {
            return input["from.".Length..].TryGetDomainVariable(out variable);
        }
        else if (input.StartsWith("to."))
        {
            return input["to.".Length..].TryGetDomainVariable(out variable);
        }

        if (input == "value")
        {
            variable = new Variable { Description = "Valeur de la propriété à convertir" };
            return true;
        }

        variable = null;
        return false;
    }

    public static bool TryGetDomainVariable(this string input, [MaybeNullWhen(false)] out Variable variable)
    {
        if (DomainProperties.TryGetValue(input, out var description))
        {
            variable = new Variable { Description = description };
            return true;
        }

        variable = null;
        return false;
    }

    public static bool TryGetEndpointVariable(
        this string input,
        ModelConfig config,
        IList<TemplateParameter> templateParameters,
        [MaybeNullWhen(false)] out Variable variable
    )
    {
        if (input.StartsWith("returns."))
        {
            return input["returns.".Length..].TryGetClassVariable(config, templateParameters, out variable);
        }

        if (input.StartsWith("customProperties."))
        {
            variable = new Variable { Description = "Propriété personnalisée" };
            return true;
        }

        if (input.StartsWith("params["))
        {
            return input[(input.IndexOf(']') + 2)..].TryGetPropertyVariable(config, templateParameters, out variable);
        }

        if (templateParameters.Any(tp => tp.Name == input))
        {
            variable = new Variable { TemplateParameter = templateParameters.First(tp => tp.Name == input) };
            return true;
        }

        if (EndpointProperties.TryGetValue(input, out var description))
        {
            variable = new Variable { Description = description };
            return true;
        }

        if (config.GlobalVariables.TryGetValue(input, out var gVariable))
        {
            variable = gVariable;
            return true;
        }

        if (config.TagVariables.TryGetValue(input, out var tVariable))
        {
            variable = tVariable;
            return true;
        }

        variable = null;
        return false;
    }

    public static bool TryGetPropertyVariable(
        this string input,
        ModelConfig config,
        IList<TemplateParameter> templateParameters,
        [MaybeNullWhen(false)] out Variable variable
    )
    {
        if (input.StartsWith("parent."))
        {
            return input["parent.".Length..].TryGetClassVariable(config, templateParameters, out variable)
                || input["parent.".Length..].TryGetEndpointVariable(config, templateParameters, out variable);
        }

        if (input.StartsWith("class."))
        {
            return input["class.".Length..].TryGetClassVariable(config, templateParameters, out variable);
        }

        if (input.StartsWith("endpoint."))
        {
            return input["endpoint.".Length..].TryGetEndpointVariable(config, templateParameters, out variable);
        }

        if (input.StartsWith("domain."))
        {
            return input["domain.".Length..].TryGetDomainVariable(out variable);
        }

        if (input.StartsWith($"customProperties."))
        {
            variable = new Variable { Description = "Propriété personnalisée" };
            return true;
        }

        if (input.StartsWith("association."))
        {
            return input["association.".Length..].TryGetClassVariable(config, templateParameters, out variable);
        }

        if (input.StartsWith("composition."))
        {
            return input["composition.".Length..].TryGetClassVariable(config, templateParameters, out variable);
        }

        if (templateParameters.Any(tp => tp.Name == input))
        {
            variable = new Variable { TemplateParameter = templateParameters.First(tp => tp.Name == input) };
            return true;
        }

        if (PropertyProperties.TryGetValue(input, out var description))
        {
            variable = new Variable { Description = description };
            return true;
        }

        if (config.GlobalVariables.TryGetValue(input, out var gVariable))
        {
            variable = gVariable;
            return true;
        }

        if (config.TagVariables.TryGetValue(input, out var tVariable))
        {
            variable = tVariable;
            return true;
        }

        variable = null;
        return false;
    }
}
