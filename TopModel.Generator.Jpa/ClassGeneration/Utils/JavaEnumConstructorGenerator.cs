using TopModel.Core.Model;

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

        foreach (
            var uvp in classe
                .Properties.Where(p => p.UniqueValuedProperty != null && p.EnumProperty == null)
                .Select(p => p.UniqueValuedProperty!)
        )
        {
            parameter.Imports.Add(
                $"{Config.GetEnumPackageName(uvp.Class, tag)}.{uvp.Class.NamePascal}{uvp.NamePascal}"
            );
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
                constructor.AddBodyLine(1, $@"case {Config.GetValue(codeProperty, code)}:");

                foreach (var prop in classe.Properties.Where(p => p != codeProperty))
                {
                    var isString = Config.GetType(prop) == "String";
                    var value = refValue.Value.TryGetValue(prop, out var v) ? v : "null";
                    if (value == "null")
                    {
                        isString = false;
                    }
                    else if (
                        prop is { Association: Class association, AssociationProperty: IProperty ap }
                        && ap.EnumProperty != null
                        && association.Values.Any(r => r.Value.ContainsKey(ap) && r.Value[ap] == value)
                    )
                    {
                        value = association.NamePascal + "." + value;
                        isString = false;
                        constructor.Imports.Add(association.GetImport(Config, tag));
                    }
                    else if (Config.TranslateReferences == true && classe.DefaultProperty == prop)
                    {
                        value = refValue.ResourceKey;
                    }
                    else
                    {
                        value = Config.GetValue(prop, value);
                    }

                    var quote = isString ? "\"" : string.Empty;
                    var val = quote + value + quote;
                    constructor.AddBodyLine(2, $@"this.{prop.NameCamel} = {val};");
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
