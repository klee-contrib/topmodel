using Microsoft.Extensions.Logging;
using TopModel.Core.Model;
using TopModel.Utils;

namespace TopModel.Generator.Jpa.ClassGeneration;

/// <summary>
/// Générateur de fichiers de modèles JPA.
/// </summary>
public class JavaEnumEnumGenerator(ILogger<JavaEnumEnumGenerator> logger, IFileWriterProvider writerProvider)
    : JavaClassGeneratorBase(logger, writerProvider)
{
    public override string Name => "JavaEnumEnumGen";

    public override IEnumerable<string> GeneratedFiles =>
        Config
            .Files.Values.SelectMany(f => f.Classes.Where(FilterClass))
            .SelectMany(c =>
                Config.Tags.Intersect(c.Tags).SelectMany(tag => GetEnumProperties(c).Select(p => GetFileName(c, tag)))
            )
            .Distinct();

    protected override bool FilterClass(Class classe)
    {
        return !classe.Abstract && classe.Enum == EnumMode.Enum;
    }

    protected override string GetFileName(Class classe, string tag)
    {
        return Config.GetEnumFileName(classe, tag);
    }

    protected override void HandleClass(string fileName, Class classe, string tag)
    {
        var packageName = Config.GetEnumPackageName(classe, tag);
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
                enumAsString.Add($"{refValue.Value[classe.EnumKey!]}");
            }
            else
            {
                enumAsString.Add($"{refValue.Value[classe.EnumKey!]}(");
                foreach (var prop in notPkProperties)
                {
                    var value = refValue.Value.TryGetValue(prop, out var v) ? v : "null";
                    value = Config.GetValue(prop, value);
                    if (
                        prop is { Association: Class association, AssociationProperty: IProperty ap }
                        && association.Values.Any(r => r.Value.ContainsKey(ap) && r.Value[ap] == value)
                    )
                    {
                        fw.AddImport(
                            $"{Config.GetEnumPackageName(association.EnumKey!.Class, tag)}.{association.NamePascal}"
                        );
                        if (value != "null")
                        {
                            value = association.NamePascal + "." + value;
                        }
                    }
                    else if (prop.EnumProperty != null && value != "null")
                    {
                        value = Config.GetType(prop) + "." + value;
                    }

                    if (
                        Config.TranslateReferences == true
                        && classe.DefaultProperty == prop
                        && prop.EnumProperty == null
                    )
                    {
                        value = @$"""{refValue.ResourceKey}""";
                    }

                    enumAsString.Add($@"{value}{(prop == notPkProperties.Last() ? string.Empty : ", ")}");
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
            fw.WriteDocStart(1, $@"{prop.NamePascal}");
            fw.WriteDocEnd(1);
            var fieldName = prop.NameCamel;
            if (prop is { Association: Class association })
            {
                fieldName = $"{prop.NameCamel}";
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
            var fieldName = prop.NameCamel;
            var fieldType = Config.GetType(prop);
            if (prop is { Association: Class { Enum: EnumMode.Enum } association })
            {
                fieldName = $"{prop.NameCamel}";
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

    private static IEnumerable<IProperty> GetEnumProperties(Class classe)
    {
        return classe.Properties.Where(e => e.EnumProperty == e);
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
                var fieldName = prop.NameCamel;
                var fieldType = Config.GetType(prop);
                if (prop is { Association: Class association })
                {
                    fieldName = $"{prop.NameCamel}";
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
