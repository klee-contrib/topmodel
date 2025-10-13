using TopModel.Core.Model;

namespace TopModel.Generator.Jpa.ClassGeneration;

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
                constructor.AddBodyLine(1, $@"case {code} :");
                foreach (var prop in classe.Properties.Where(p => p != codeProperty))
                {
                    var isString = Config.GetType(prop) == "String";
                    var value = refValue.Value.TryGetValue(prop, out var v) ? v : "null";
                    if (value == "null")
                    {
                        isString = false;
                    }
                    else if (
                        prop is AssociationProperty ap
                        && Config.CanClassUseEnums(ap.Association, prop: ap.Property)
                        && ap.Association.Values.Any(r =>
                            r.Value.ContainsKey(ap.Property) && r.Value[ap.Property] == value
                        )
                    )
                    {
                        value = ap.Association.NamePascal + "." + value;
                        isString = false;
                        constructor.Imports.Add(ap.Association.GetImport(Config, tag));
                    }
                    else if (
                        prop is AliasProperty alp
                        && Config.CanClassUseEnums(alp.Property.Class, prop: alp.Property)
                    )
                    {
                        value = Config.GetType(alp.Property) + "." + value;
                    }
                    else if (
                        Config.TranslateReferences == true
                        && classe.DefaultProperty == prop
                        && !Config.CanClassUseEnums(classe, prop: prop)
                    )
                    {
                        value = refValue.ResourceKey;
                    }

                    var quote = isString ? "\"" : string.Empty;
                    var val = quote + value + quote;
                    constructor.AddBodyLine(2, $@"this.{prop.NameByClassCamel} = {val};");
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
