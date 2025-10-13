using Microsoft.Extensions.Logging;
using TopModel.Core.FileModel;
using TopModel.Core.Model;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Jpa.ClassGeneration;

/// <summary>
/// Générateur de fichiers de modèles JPA.
/// </summary>
public class JpaEnumGenerator(ILogger<JpaEnumGenerator> logger, IFileWriterProvider writerProvider)
    : GeneratorBase<JpaConfig>(logger, writerProvider)
{
    public override string Name => "JpaEnumGen";

    public override IEnumerable<string> GeneratedFiles =>
        Config
            .Files.Values.SelectMany(f => f.Classes.Where(FilterClass))
            .SelectMany(c =>
                Config
                    .Tags.Intersect(c.Tags)
                    .SelectMany(tag => GetEnumProperties(c).Select(p => GetFileName(p, c, tag)))
            )
            .Distinct();

    protected bool FilterClass(Class classe)
    {
        return !classe.Abstract && Config.CanClassUseEnums(classe);
    }

    protected IEnumerable<IProperty> GetEnumProperties(Class classe)
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

    protected string GetFileName(IProperty property, Class classe, string tag)
    {
        return Config.GetEnumFileName(property, classe, tag);
    }

    protected void HandleClass(Class classe, string tag)
    {
        foreach (var p in GetEnumProperties(classe))
        {
            WriteEnum(p, classe, tag);
        }
    }

    protected override void HandleFiles(IEnumerable<ModelFile> files)
    {
        foreach (var file in files)
        {
            foreach (var classe in file.Classes.Where(FilterClass))
            {
                foreach (var tag in Config.Tags.Intersect(classe.Tags))
                {
                    HandleClass(classe, tag);
                }
            }
        }
    }

    protected virtual void WriteEnum(IProperty property, Class classe, string tag)
    {
        var packageName = Config.GetEnumPackageName(classe, tag);
        using var fw = this.OpenJavaWriter(Config.GetEnumFileName(property, classe, tag), packageName, codePage: null);
        var javaEnum = GetJavaEnum(property, classe);
        fw.Write(0, javaEnum);
    }

    private JavaEnum GetJavaEnum(IProperty property, Class classe)
    {
        var javaEnum = new JavaEnum(Config.GetEnumName(property, classe))
        {
            ClassType = "enum",
            Comment =
                $"Enumération des valeurs possibles de la propriété {property.NamePascal} de la classe {classe.NamePascal}",
        };
        var i = 0;
        var refs = Config.GetAllValues(classe).OrderBy(x => x.Name, StringComparer.Ordinal).ToList();

        foreach (var value in refs)
        {
            i++;
            var enumValue = new JavaEnumValue(value.Value[property]);
            if (classe.DefaultProperty != null)
            {
                enumValue.Comment = value.Value[classe.DefaultProperty];
            }

            javaEnum.Add(enumValue);
        }

        return javaEnum;
    }
}
