using Microsoft.Extensions.Logging;
using TopModel.Core.Model;
using TopModel.Core.Utils;
using TopModel.Utils;

namespace TopModel.Generator.Jpa.ClassGeneration;

/// <summary>
/// Générateur de fichiers de modèles JPA.
/// </summary>
public class JpaEnumValuesGenerator(ILogger<JpaEnumValuesGenerator> logger, IFileWriterProvider writerProvider)
    : JavaClassGeneratorBase(logger, writerProvider)
{
    public override string Name => "JpaEnumValuesGen";

    public override IEnumerable<string> GeneratedFiles =>
        Config
            .Files.Values.SelectMany(f => f.Classes.Where(FilterClass))
            .SelectMany(c =>
                Config.Tags.Intersect(c.Tags).SelectMany(tag => GetEnumProperties(c).Select(p => GetFileName(c, tag)))
            )
            .Distinct();

    protected override bool FilterClass(Class classe)
    {
        return !classe.Abstract
            && (Config.CanClassUseEnums(classe) || Config.EnumsAsEnums && classe.Enum)
            && classe.Enum;
    }

    protected override string GetFileName(Class classe, string tag)
    {
        return Config.GetEnumValueFileName(classe, tag);
    }

    protected override void HandleClass(string fileName, Class classe, string tag)
    {
        var packageName = Config.GetEnumValuePackageName(classe, tag);
        using var fw = this.OpenJavaWriter(fileName, packageName, codePage: null);
        fw.WriteLine();
        fw.WriteDocStart(0, $"Enumération des valeurs possibles de la classe {classe.NamePascal}");
        fw.WriteDocEnd(0);

        WriteAnnotations(fw, classe, tag);
        if (classe.Extends is not null)
        {
            fw.AddImport($"{Config.GetPackageName(classe.Extends, tag)}.{classe.Extends.NamePascal}");
        }

        var implements = Config.GetClassImplements(classe, tag).ToList();

        fw.WriteClassDeclaration(classe.NamePascal, modifier: null, inheritedClass: null, implements, "enum");
        var i = 0;

        var refs = Config.GetAllValues(classe).ToList();

        var notPkProperties = classe.Properties.Where(p => p != classe.EnumKey);
        foreach (var refValue in refs)
        {
            if (i > 0)
            {
                fw.WriteLine();
            }

            i++;
            if (classe.DefaultProperty != null)
            {
                fw.WriteDocStart(1, $"{refValue.Value[classe.DefaultProperty]}");
                fw.WriteDocEnd(1);
            }

            List<string> enumAsString = [];
            if (!notPkProperties.Any())
            {
                enumAsString.Add($"{refValue.Value[classe.EnumKey!].ToConstantCase()}");
            }
            else
            {
                enumAsString.Add($"{refValue.Value[classe.EnumKey!].ToConstantCase()}(");
                foreach (var prop in notPkProperties)
                {
                    var isString = Config.GetType(prop) == "String";
                    var value = refValue.Value.TryGetValue(prop, out var v) ? v : "null";

                    if (
                        prop is { Association: Class association, AssociationProperty: IProperty ap }
                        && association.Values.Any(r => r.Value.ContainsKey(ap) && r.Value[ap] == value)
                    )
                    {
                        fw.AddImport(
                            $"{Config.GetEnumValuePackageName(association.EnumKey!.Class, tag)}.{association.NamePascal}"
                        );
                        value = association.NamePascal + "." + value;
                        isString = false;
                    }
                    else if (Config.CanClassUseEnums(classe, prop))
                    {
                        value = Config.GetType(prop) + "." + value;
                    }

                    if (
                        Config.TranslateReferences == true
                        && classe.DefaultProperty == prop
                        && !Config.CanClassUseEnums(classe, prop)
                    )
                    {
                        value = refValue.ResourceKey;
                    }

                    var quote = isString ? "\"" : string.Empty;
                    var val = quote + value + quote;
                    enumAsString.Add($@"{val}{(prop == notPkProperties.Last() ? string.Empty : ", ")}");
                }

                enumAsString.Add($")");
            }

            enumAsString.Add(",");

            fw.WriteLine(1, enumAsString.Aggregate(string.Empty, (acc, curr) => acc + curr));
        }

        fw.WriteLine();
        fw.WriteLine(1, ";");

        foreach (var prop in notPkProperties)
        {
            fw.WriteLine();
            fw.WriteDocStart(1, $@"{prop.NameByClassPascal}");
            fw.WriteDocEnd(1);
            var fieldName = prop.NameByClassCamel;
            if (prop is { Association: Class association })
            {
                fieldName = $"{prop.NameByClassCamel}";
                fw.WriteLine(1, $@"private final {association.NamePascal} {fieldName};");
            }
            else
            {
                fw.WriteLine(1, $@"private final {Config.GetType(prop)} {fieldName};");
            }
        }

        if (notPkProperties.Any())
        {
            WriteConstructor(classe, fw);
        }

        foreach (var prop in notPkProperties)
        {
            var fieldName = prop.NameByClassCamel;
            var fieldType = Config.GetType(prop);
            if (prop is { Association: Class association } && Config.CanClassUseEnums(association))
            {
                fieldName = $"{prop.NameByClassCamel}";
                fieldType = $"{association.NamePascal}";
            }

            var method = new JavaMethod(fieldType, $"get{fieldName.ToFirstUpper()}")
            {
                Visibility = "public",
                Comment = $"Getter for {fieldName}",
            };
            method.AddBodyLine($@"return this.{fieldName};");
            fw.Write(1, method);
        }

        fw.WriteLine("}");
    }

    private List<IProperty> GetEnumProperties(Class classe)
    {
        List<IProperty> result = [];
        if (
            classe.EnumKey != null
            && Config.CanClassUseEnums(classe, prop: classe.EnumKey)
            && !(classe.Extends != null && Config.CanClassUseEnums(classe.Extends, prop: classe.EnumKey))
        )
        {
            result.Add(classe.EnumKey);
        }

        var uks = classe
            .UniqueKeys.Where(uk =>
                uk.Count == 1
                && Config.CanClassUseEnums(classe, uk.Single())
                && !(classe.Extends != null && Config.CanClassUseEnums(classe.Extends, prop: classe.EnumKey))
            )
            .Select(uk => uk.Single());
        result.AddRange(uks);
        return result;
    }

    private void WriteAnnotations(JavaWriter fw, Class classe, string tag)
    {
        fw.AddImports(Config.GetDecoratorImports(classe, tag).ToList());
        fw.Write(0, GetAnnotations(classe, tag));
    }

    private void WriteConstructor(Class classe, JavaWriter fw)
    {
        // Constructeur
        var properties = classe.Properties.Where(p => p != classe.EnumKey);
        var constructor = new JavaConstructor(classe.NamePascal) { Comment = "Enum values constructor" };
        var methodParams = properties.Select(
            (prop, index) =>
            {
                var fieldName = prop.NameByClassCamel;
                var fieldType = Config.GetType(prop);
                if (prop is { Association: Class association })
                {
                    fieldName = $"{prop.NameByClassCamel}";
                    fieldType = $"{association.NamePascal}";
                }

                return new JavaMethodParameter(fieldType, fieldName) { Final = true };
            }
        );
        constructor.AddParameters(methodParams);
        foreach (var param in methodParams)
        {
            constructor.AddBodyLine($@"this.{param.Name} = {param.Name};");
        }

        fw.Write(1, constructor);
    }
}
