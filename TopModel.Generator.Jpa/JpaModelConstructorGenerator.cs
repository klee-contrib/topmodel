using TopModel.Core;
using TopModel.Generator.Core;

namespace TopModel.Generator.Jpa;

/// <summary>
/// Générateur de fichiers de modèles JPA.
/// </summary>
public class JpaModelConstructorGenerator
{
    private readonly JpaConfig _config;

    public JpaModelConstructorGenerator(JpaConfig config)
    {
        _config = config;
    }

    public void WriteEnumConstructor(JavaWriter fw, Class classe, List<Class> availableClasses, string tag, ModelConfig modelConfig)
    {
        var codeProperty = classe.EnumKey!;
        fw.WriteLine();
        foreach (var refValue in classe.Values.OrderBy(x => x.Name, StringComparer.Ordinal))
        {
            var code = refValue.Value[codeProperty];
            fw.WriteLine(1, $@"public static final {classe.NamePascal} {code} = new {classe.NamePascal}({_config.GetEnumName(codeProperty, classe)}.{code});");
        }

        fw.WriteLine();
        fw.WriteDocStart(1, "Enum constructor");
        fw.WriteParam(classe.EnumKey!.NameCamel, "Code dont on veut obtenir l'instance");
        fw.WriteDocEnd(1);
        fw.WriteLine(1, $"public {classe.NamePascal}({_config.GetType(classe.EnumKey!)} {classe.EnumKey!.NameCamel}) {{");
        if (classe.Extends != null || classe.Decorators.Any(d => _config.GetImplementation(d.Decorator)?.Extends is not null))
        {
            fw.WriteLine(2, $"super();");
        }

        fw.WriteLine(2, $@"this.{classe.EnumKey!.NameCamel} = {classe.EnumKey!.NameCamel};");
        if (classe.GetProperties(availableClasses).Count > 1)
        {
            fw.WriteLine(2, $@"switch({classe.EnumKey!.NameCamel}) {{");
            foreach (var refValue in classe.Values.OrderBy(x => x.Name, StringComparer.Ordinal))
            {
                var code = refValue.Value[codeProperty];
                fw.WriteLine(2, $@"case {code} :");
                foreach (var prop in classe.GetProperties(availableClasses).OfType<IFieldProperty>()
                    .Where(p => p != codeProperty))
                {
                    var isString = _config.GetType(prop) == "String";
                    var value = refValue.Value.ContainsKey(prop) ? refValue.Value[prop] : "null";
                    if (prop is AssociationProperty ap && codeProperty.PrimaryKey && ap.Association.Values.Any(r => r.Value.ContainsKey(ap.Property) && r.Value[ap.Property] == value))
                    {
                        value = ap.Association.NamePascal + "." + value;
                        isString = false;
                        fw.AddImport(ap.Association.GetImport(_config, tag));
                    }
                    else if (_config.CanClassUseEnums(classe, prop: prop))
                    {
                        value = _config.GetType(prop) + "." + value;
                    }

                    if (modelConfig.I18n.TranslateReferences && classe.DefaultProperty == prop && !_config.CanClassUseEnums(classe, prop: prop))
                    {
                        value = refValue.ResourceKey;
                    }

                    var quote = isString ? "\"" : string.Empty;
                    var val = quote + value + quote;
                    fw.WriteLine(3, $@"this.{prop.NameByClassCamel} = {val};");
                }

                fw.WriteLine(3, $@"break;");
            }

            fw.WriteLine(2, $@"}}");
        }

        fw.WriteLine(1, $"}}");
    }

    public void WriteFromMappers(JavaWriter fw, Class classe, List<Class> availableClasses, string tag)
    {
        var fromMappers = classe.FromMappers.Where(c => c.ClassParams.All(p => availableClasses.Contains(p.Class))).Select(m => (classe, m))
            .OrderBy(m => m.classe.NamePascal)
            .ToList();

        foreach (var fromMapper in fromMappers)
        {
            var (clazz, mapper) = fromMapper;
            fw.WriteLine();
            fw.WriteDocStart(1, $"Crée une nouvelle instance de '{classe.NamePascal}'");
            if (mapper.Comment != null)
            {
                fw.WriteLine(1, $" * {mapper.Comment}");
            }

            foreach (var param in mapper.ClassParams)
            {
                if (param.Comment != null)
                {
                    fw.WriteLine(1, $" * {param.Comment}");
                }

                fw.WriteParam(param.Name.ToCamelCase(), $"Instance de '{param.Class.NamePascal}'");
            }

            fw.WriteReturns(1, $"Une nouvelle instance de '{classe.NamePascal}'");
            fw.WriteDocEnd(1);
            fw.WriteLine(1, $"public {classe.NamePascal}({string.Join(", ", mapper.ClassParams.Select(p => $"{p.Class.NamePascal} {p.Name.ToCamelCase()}"))}) {{");
            if (classe.Extends != null)
            {
                fw.WriteLine(2, $"super();");
            }

            var (mapperNs, mapperModelPath) = _config.GetMapperLocation(fromMapper);
            fw.WriteLine(2, $"{_config.GetMapperName(mapperNs, mapperModelPath)}.create{classe.NamePascal}({string.Join(", ", mapper.ClassParams.Select(p => p.Name.ToCamelCase()))}, this);");
            fw.AddImport(_config.GetMapperImport(mapperNs, mapperModelPath, tag)!);
            fw.WriteLine(1, "}");
        }
    }

    public void WriteNoArgConstructor(JavaWriter fw, Class classe)
    {
        fw.WriteLine();
        fw.WriteDocStart(1, "No arg constructor");
        fw.WriteDocEnd(1);
        fw.WriteLine(1, $"public {classe.NamePascal}() {{");
        if (classe.Extends != null || classe.Decorators.Any(d => _config.GetImplementation(d.Decorator)?.Extends is not null))
        {
            fw.WriteLine(2, $"super();");
        }

        fw.WriteLine(1, $"}}");
    }
}
