using Microsoft.Extensions.Logging;
using TopModel.Core.Model;
using TopModel.Generator.Jpa.ClassGeneration.Utils;
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
        return !classe.Abstract && classe.IsPersistent && classe.Enum == EnumMode.Class && classe.Readonly;
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
                var field = new JavaField(classe.NamePascal, refValue.Name.ToConstantCase())
                {
                    Visibility = "public",
                    Static = true,
                    Final = true,
                    DefaultValue = $"new {classe.NamePascal}({Config.GetValue(classe.EnumKey, code)})",
                }.Add(new JavaAnnotation("Transient", imports: "jakarta.persistence.Transient"));

                yield return field;
            }
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
