using Microsoft.Extensions.Logging;
using TopModel.Core.Model;
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
        return !classe.Abstract && Config.CanClassUseEnums(classe) && !classe.IsPersistent;
    }

    protected override IEnumerable<JavaMethod> GetConstuctors(Class classe, string tag)
    {
        yield return ConstructorGenerator.GetNoArgConstructor(classe, tag);
        yield return ConstructorGenerator.GetEnumConstructor(classe, tag);
    }

    protected override IEnumerable<JavaField> GetFields(Class classe, string tag)
    {
        var codeProperty = classe.EnumKey!;
        foreach (var refValue in classe.Values.OrderBy(x => x.Name, StringComparer.Ordinal))
        {
            var code = refValue.Value[codeProperty].ToConstantCase();
            yield return new JavaField(classe.NamePascal, code)
            {
                Static = true,
                Final = true,
                Visibility = "public",
                DefaultValue = $"new {classe.NamePascal}({Config.GetEnumName(codeProperty, classe)}.{code})",
            };
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
