using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Generator.Core;

namespace TopModel.Generator.Jpa;

/// <summary>
/// Générateur de fichiers de modèles JPA.
/// </summary>
public class JpaEnumGenerator : ClassGeneratorBase<JpaConfig>
{
    private readonly ILogger<JpaEnumGenerator> _logger;

    private readonly Dictionary<string, string> _newableTypes = new()
    {
        ["List"] = "ArrayList",
        ["Set"] = "HashSet"
    };

    public JpaEnumGenerator(ILogger<JpaEnumGenerator> logger, ModelConfig modelConfig)
        : base(logger)
    {
        _logger = logger;
    }

    public override string Name => "JpaEnumGen";

    protected override bool FilterClass(Class classe)
    {
        return !classe.Abstract && Config.CanClassUseEnums(classe, Classes.ToList());
    }

    protected override string GetFileName(Class classe, string tag)
    {
        return Config.GetEnumFileName(classe, tag);
    }

    protected override void HandleClass(string fileName, Class classe, string tag)
    {
        var packageName = Config.GetEnumPackageName(classe, tag);
        using var fw = new JavaWriter(Config.GetEnumFileName(classe, tag), _logger, packageName, null);
        fw.WriteLine();
        var codeProperty = classe.EnumKey!;
        fw.WriteDocStart(0, $"Enumération des valeurs possibles de la propriété {codeProperty.NamePascal} de la classe {classe.NamePascal}");
        fw.WriteDocEnd(0);
        fw.WriteLine($@"public enum {classe.NamePascal}{codeProperty.NamePascal} {{");
        var i = 0;
        foreach (var value in classe.Values.OrderBy(x => x.Name, StringComparer.Ordinal))
        {
            i++;
            var isLast = i == classe.Values.Count();
            if (classe.DefaultProperty != null)
            {
                fw.WriteDocStart(1, $"{value.Value[classe.DefaultProperty]}");
                fw.WriteDocEnd(1);
            }

            fw.WriteLine(1, $"{value.Value[codeProperty]}{(isLast ? ";" : ",")}");
        }

        fw.WriteLine("}");
    }
}