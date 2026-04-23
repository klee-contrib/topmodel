using TopModel.Core.Model;
using TopModel.Utils;

namespace TopModel.Generator.Jpa.ClassGeneration.Utils;

/// <summary>
/// Générateur de fichiers de modèles JPA.
/// </summary>
public class JavaConstructorGenerator(JpaConfig config)
{
    protected JpaConfig Config { get; set; } = config;

    public JavaConstructor GetAllArgsConstructor(Class classe, string tag)
    {
        var properties = classe.Properties;
        return GetConstructor(classe, properties, tag);
    }

    public JavaConstructor GetConstructor(Class classe, IEnumerable<IProperty> properties, string tag)
    {
        var constructor = new JavaConstructor(classe.NamePascal)
        {
            Visibility = "public",
            Comment = $"All args constructor for '{classe.NamePascal}'.",
        };

        if (Config.GetClassExtends(classe, tag) != null)
        {
            constructor.AddBodyLine("super();");
        }

        foreach (var property in properties)
        {
            var propName = !Config.UseJdbc ? property.NameCamel : property.PropertyNameCamel;
            var parameter = new JavaMethodParameter(
                Config.GetType(property, forceAssociationPropertyType: Config.UseJdbc),
                propName
            )
            {
                Comment = property.Comment,
            };
            parameter.Imports.AddRange(property.GetTypeImports(Config, tag));
            constructor.AddParameter(parameter);
            constructor.AddBodyLine($"this.{propName} = {propName};");
        }

        return constructor;
    }

    public IEnumerable<JavaMethod> GetFromMappers(Class classe, string tag)
    {
        var fromMappers = classe
            .FromMappers.Where(c => c.ClassParams.All(p => Config.AvailableClasses.Contains(p.Class)))
            .Select(m => (classe, m))
            .OrderBy(m => m.classe.NamePascal)
            .ToList();

        foreach (var fromMapper in fromMappers)
        {
            var (_, mapper) = fromMapper;

            var constructor = new JavaConstructor(classe.NamePascal)
            {
                Visibility = "public",
                Comment = mapper.Comment ?? $"Crée une nouvelle instance de '{classe.NamePascal}'",
            };

            foreach (var param in mapper.ClassParams)
            {
                var parameter = new JavaMethodParameter(
                    param.Class.GetImport(Config, tag),
                    param.Class.Name,
                    param.Name.ToCamelCase()
                )
                {
                    Comment = param.Comment ?? $"Instance de '{param.Class.NamePascal}'",
                };
                constructor.AddParameter(parameter);
            }

            foreach (var param in mapper.PropertyParams)
            {
                var parameter = new JavaMethodParameter(Config.GetType(param.Property), param.Property.NameCamel)
                {
                    Comment = param.Property.Comment,
                };
                parameter.Imports.AddRange(param.Property.GetTypeImports(Config, tag));
                constructor.AddParameter(parameter);
            }

            if (Config.GetClassExtends(classe, tag) != null)
            {
                constructor.AddBodyLine("super();");
            }

            var (mapperNs, mapperModelPath) = Config.GetMapperLocation(fromMapper);
            constructor.Imports.Add(Config.GetMapperImport(mapperNs, mapperModelPath, tag)!);
            constructor.AddBodyLine(
                $"{Config.GetMapperName(mapperNs, mapperModelPath)}.map{classe.NamePascal}({string.Join(", ", mapper.ClassParams.Select(p => p.Name.ToCamelCase()).Concat(mapper.PropertyParams.Select(p => p.Property.NameCamel)))}, this);"
            );
            constructor.ReturnComment = $"Une nouvelle instance de '{classe.NamePascal}'";
            yield return constructor;
        }
    }

    public JavaMethod GetNoArgConstructor(Class classe, string tag)
    {
        var constructor = new JavaConstructor(classe.NamePascal)
        {
            Visibility = "public",
            Comment = "No arg constructor",
        };
        if (Config.GetClassExtends(classe, tag) != null)
        {
            constructor.AddBodyLine("super();");
        }
        else
        {
            constructor.AddBodyLine("// No arg constructor");
        }

        return constructor;
    }
}
