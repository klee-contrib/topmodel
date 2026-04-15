using Microsoft.Extensions.Logging;
using TopModel.Core.Model;
using TopModel.Generator.Jpa.ClassGeneration.Utils;
using TopModel.Utils;

namespace TopModel.Generator.Jpa.ClassGeneration;

/// <summary>
/// Générateur de fichiers de modèles JPA.
/// </summary>
public class JavaEnumDtoGenerator(ILogger<JavaEnumDtoGenerator> logger, IFileWriterProvider writerProvider)
    : JavaDtoGenerator(logger, writerProvider)
{
    private JavaEnumConstructorGenerator? _jpaModelConstructorGenerator;

    public override string Name => "JavaEnumDtoGen";

    protected override JavaEnumConstructorGenerator ConstructorGenerator
    {
        get
        {
            _jpaModelConstructorGenerator ??= new JavaEnumConstructorGenerator(Config);
            return _jpaModelConstructorGenerator;
        }
    }

    protected override bool FilterClass(Class classe)
    {
        return !classe.Abstract && !classe.IsPersistent && classe.Enum == EnumMode.Class;
    }

    protected override IEnumerable<JavaMethod> GetConstuctors(Class classe, string tag)
    {
        yield return ConstructorGenerator.GetNoArgConstructor(classe, tag);
        if (classe.EnumKey != null)
        {
            yield return ConstructorGenerator.GetEnumConstructor(classe, tag);
        }
    }

    protected override IEnumerable<JavaField> GetFields(Class classe, string tag)
    {
        if (classe.EnumKey != null)
        {
            foreach (var refValue in classe.Values.OrderBy(x => x.Name, StringComparer.Ordinal))
            {
                var code = refValue.Value[classe.EnumKey];
                yield return new JavaField(classe.NamePascal, code)
                {
                    Static = true,
                    Final = true,
                    Visibility = "public",
                    DefaultValue = $"new {classe.NamePascal}({Config.GetEnumType(classe.EnumKey)}.{code})",
                };
            }
        }

        foreach (var property in base.GetFields(classe, tag))
        {
            yield return property;
        }
    }

    protected override IEnumerable<JavaMethod> GetSetters(Class classe, string tag)
    {
        return [];
    }
}
