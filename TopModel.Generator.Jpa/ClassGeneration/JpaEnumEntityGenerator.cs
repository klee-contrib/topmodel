using Microsoft.Extensions.Logging;
using TopModel.Core.Model;
using TopModel.Utils;

namespace TopModel.Generator.Jpa.ClassGeneration;

/// <summary>
/// Générateur de fichiers de modèles JPA.
/// </summary>
public class JpaEnumEntityGenerator(ILogger<JpaEnumEntityGenerator> logger, IFileWriterProvider writerProvider)
    : JpaEntityGenerator(logger, writerProvider)
{
    private JavaEnumConstructorGenerator? _javaEnumConstructorGenerator;

    public override string Name => "JpaEnumEntityGen";

    protected override JavaEnumConstructorGenerator ConstructorGenerator
    {
        get
        {
            _javaEnumConstructorGenerator ??= new JavaEnumConstructorGenerator(Config);
            return _javaEnumConstructorGenerator;
        }
    }

    protected override bool FilterClass(Class classe)
    {
        return !classe.Abstract && Config.CanClassUseEnums(classe) && classe.IsPersistent;
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
            var code = refValue.Value[codeProperty];
            yield return new JavaField(classe.NamePascal, code)
            {
                Visibility = "public",
                Static = true,
                Final = true,
                DefaultValue = $"new {classe.NamePascal}({Config.GetEnumName(codeProperty, classe)}.{code})",
            }.Add(new JavaAnnotation("Transient", imports: "jakarta.persistence.Transient"));
        }
        foreach (var field in JpaModelPropertyGenerator.GetFields(classe, tag))
        {
            yield return field;
        }
    }

    protected override IEnumerable<JavaMethod> GetSetters(Class classe, string tag)
    {
        return [];
    }
}
