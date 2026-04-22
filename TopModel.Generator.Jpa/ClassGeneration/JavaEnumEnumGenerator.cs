using Microsoft.Extensions.Logging;
using TopModel.Core.Model;
using TopModel.Generator.Core;
using TopModel.Generator.Jpa.ClassGeneration.Utils;
using TopModel.Utils;

namespace TopModel.Generator.Jpa.ClassGeneration;

/// <summary>
/// Générateur de fichiers de modèles JPA.
/// </summary>
public class JavaEnumEnumGenerator(ILogger<JavaEnumEnumGenerator> logger, IFileWriterProvider writerProvider)
    : ClassGeneratorBase<JpaConfig>(logger, writerProvider)
{
    private JavaEnumGeneratorHelper? _javaEnumGeneratorHelper;
    public override string Name => "JavaDtoGen";
    public override IEnumerable<string> GeneratedFiles =>
        Config
            .Files.Values.SelectMany(f => f.Classes.Where(FilterClass))
            .SelectMany(c =>
                Config.Tags.Intersect(c.Tags).SelectMany(tag => GetEnumProperties(c).Select(p => GetFileName(c, tag)))
            )
            .Distinct();

    protected virtual JavaEnumGeneratorHelper JavaEnumGeneratorHelper
    {
        get
        {
            _javaEnumGeneratorHelper ??= new JavaEnumGeneratorHelper(Config);
            return _javaEnumGeneratorHelper;
        }
    }

    public virtual JavaMethod GetGetter(string tag, IProperty property)
    {
        var field = GetField(property, tag);
        var method = new JavaMethod(field.Type, Config.GetGetterName(property))
        {
            Comment = $"Getter for {field.Name}",
            Body =
            {
                new WriterLine() { Line = $"return this.{field.Name};", Indent = 0 },
            },
            ReturnComment = $"value of {{@link #{field.Name} {field.Name}}}",
            Visibility = "public",
        };
        method.Imports.AddRange(property.GetTypeImports(Config, tag));
        return method;
    }

    protected override bool FilterClass(Class classe)
    {
        return !classe.Abstract && classe.Enum == EnumMode.Enum;
    }

    protected virtual IEnumerable<JavaMethod> GetConstuctors(Class classe, string tag)
    {
        var constructor = JavaEnumGeneratorHelper.GetConstructor(
            classe,
            classe.Properties.Where(p => p.EnumProperty != p),
            tag
        );
        constructor.Visibility = "private";
        return [constructor];
    }

    protected virtual IEnumerable<JavaField> GetFields(Class classe, string tag)
    {
        foreach (var property in classe.Properties.Where(p => p.EnumProperty != p))
        {
            JavaField field = GetField(property, tag);
            yield return field;
        }
    }

    protected override string GetFileName(Class classe, string tag)
    {
        return Path.Combine(
            Config.OutputDirectory,
            Config.ResolveVariables(Config.EnumsPath, tag, module: classe.Namespace.Module).ToFilePath(),
            $"{classe.NamePascal}.java"
        );
    }

    protected virtual IEnumerable<JavaMethod> GetGetters(Class classe, string tag)
    {
        if (!(Config.HasAnnotation(classe, "Getter") || Config.HasAnnotation(classe, "Data")))
        {
            foreach (var property in Config.GetAvailableProperties(classe).Where(p => p.EnumProperty != p))
            {
                if (!Config.HasAnnotation(property, "Getter") || Config.HasAnnotation(classe, "Data"))
                {
                    yield return GetGetter(tag, property);
                }
            }
        }
    }

    protected override void HandleClass(string fileName, Class classe, string tag)
    {
        var packageName = Config.GetEnumPackageName(classe, tag);

        var javaClass = InitClass(classe, tag);

        using var fw = this.OpenJavaWriter(fileName, packageName, codePage: null);
        fw.Write(0, javaClass);
    }

    protected virtual JavaClass InitClass(Class classe, string tag)
    {
        var javaEnum = new JavaEnum(classe.NamePascal) { Comment = classe.Comment };

        if (Config.GeneratedHint)
        {
            javaEnum.Add(Config.GeneratedAnnotation);
        }

        var implements = Config.GetClassImplements(classe, tag).ToList();
        javaEnum.Values.AddRange(GetEnumValues(classe));
        javaEnum.Implements.AddRange(implements);
        javaEnum.Imports.AddRange(Config.GetDecoratorImports(classe, tag));
        javaEnum.AddRange(GetConstuctors(classe, tag));
        javaEnum.AddRange(GetFields(classe, tag));
        javaEnum.AddRange(GetGetters(classe, tag));
        return javaEnum;
    }

    private static IEnumerable<IProperty> GetEnumProperties(Class classe)
    {
        return classe.Properties.Where(e => e.EnumProperty == e);
    }

    private IEnumerable<JavaEnumValue> GetEnumValues(Class classe)
    {
        foreach (var refValue in classe.Values)
        {
            var args = classe
                .Properties.Where(p => p.EnumProperty != p)
                .Select(prop => JavaEnumGeneratorHelper.GetPropertyValue(classe, prop, refValue))
                .ToArray();
            var value = new JavaEnumValue(refValue.Value[classe.EnumKey]);
            if (
                classe.DefaultProperty != null
                && refValue.Value.TryGetValue(classe.DefaultProperty, out var defaultValue)
            )
            {
                value.Comment = defaultValue;
            }
            value.Parameters.AddRange(args);
            yield return value;
        }
    }

    private JavaField GetField(IProperty property, string tag)
    {
        var field = new JavaField(
            Config.GetType(property, forceAssociationPropertyType: Config.UseJdbc),
            property.NameCamel
        )
        {
            Comment = { property.Comment },
        };
        field.Imports.AddRange(property.GetTypeImports(Config, tag));
        return field;
    }
}
