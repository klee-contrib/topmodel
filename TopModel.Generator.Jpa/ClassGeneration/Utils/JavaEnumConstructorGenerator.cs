using TopModel.Core.Model;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Jpa.ClassGeneration.Utils;

/// <summary>
/// Générateur de fichiers de modèles JPA.
/// </summary>
public class JavaEnumConstructorGenerator(JpaConfig config) : JavaConstructorGenerator(config)
{
    public JavaConstructor GetEnumConstructor(Class classe, string tag)
    {
        var codeProperty = classe.EnumKey!;
        var constructor = new JavaConstructor(classe.NamePascal)
        {
            Visibility = "public",
            Comment = "Enum constructor",
        };

        var parameter = new JavaMethodParameter(Config.GetType(classe.EnumKey!), classe.EnumKey!.NameCamel)
        {
            Comment = "Code dont on veut obtenir l'instance.",
        };

        if (Config.UniqueValueGeneration.CanConst)
        {
            foreach (
                var uvp in classe
                    .Properties.Where(p =>
                        p.UniqueValuedProperty != null
                        && (
                            p.EnumProperty == null
                            || Config.UniqueValueGeneration == UniqueValueGenerationMode.ConstOnly
                        )
                    )
                    .Select(p => p.UniqueValuedProperty!)
            )
            {
                parameter.Imports.Add(
                    $"{Config.GetEnumPackageName(uvp.Class, tag)}.{uvp.Class.NamePascal}{uvp.NamePascal}"
                );
            }
        }

        constructor.AddParameter(parameter);

        if (Config.GetClassExtends(classe, tag) != null)
        {
            constructor.AddBodyLine("super();");
        }

        constructor.AddBodyLine($@"this.{classe.EnumKey!.NameCamel} = {classe.EnumKey!.NameCamel};");
        if (classe.Properties.Count > 1)
        {
            constructor.AddBodyLine($@"switch({classe.EnumKey!.NameCamel}) {{");
            foreach (var refValue in classe.Values.OrderBy(x => x.Name, StringComparer.Ordinal))
            {
                var code = refValue.Value[codeProperty];
                var codeValue = Config.GetValue(codeProperty, code);
                if (codeProperty.EnumProperty != null && Config.UniqueValueGeneration.CanEnum)
                {
                    codeValue = codeValue.Replace($"{Config.GetEnumType(codeProperty)}.", string.Empty);
                }

                constructor.AddBodyLine(1, $@"case {codeValue}:");

                foreach (var prop in classe.Properties.Where(p => p != codeProperty))
                {
                    var value = refValue.Value.TryGetValue(prop, out var v) ? v : "null";

                    if (Config.TranslateReferences == true && classe.DefaultProperty == prop)
                    {
                        value = $"\"{refValue.ResourceKey}\"";
                    }
                    else if (
                        prop.UseClassForAssociation
                        && prop.Association?.Enum == EnumMode.Class
                        && prop.Association?.Readonly == true
                    )
                    {
                        var associationRefValue = prop.Association!.Values.SingleOrDefault(e =>
                            e.Value[prop.AssociationProperty] == value
                        );
                        value = $"{prop.Association!.NamePascal}.{associationRefValue?.Name.ToConstantCase()}";
                    }
                    else
                    {
                        value = Config.GetValue(prop, value);
                    }

                    constructor.AddBodyLine(2, $@"this.{prop.NameCamel} = {value};");
                }

                constructor.AddBodyLine(2, $@"break;");
            }

            constructor.AddBodyLine($@"}}");
        }

        return constructor;
    }

    public void WriteEnumConstructor(JavaWriter fw, Class classe, string tag)
    {
        var constructor = GetEnumConstructor(classe, tag);
        fw.Write(1, constructor);
    }
}
