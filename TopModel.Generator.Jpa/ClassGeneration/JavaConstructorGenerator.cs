using TopModel.Core.Model;

namespace TopModel.Generator.Jpa.ClassGeneration;

/// <summary>
/// Générateur de fichiers de modèles JPA.
/// </summary>
public class JavaConstructorGenerator(JpaConfig config)
{
    protected JpaConfig Config { get; set; } = config;

    public void WriteFromMappers(JavaWriter fw, Class classe, IEnumerable<Class> availableClasses, string tag)
    {
        var fromMappers = classe.FromMappers.Where(c => c.ClassParams.All(p => availableClasses.Contains(p.Class))).Select(m => (classe, m))
            .OrderBy(m => m.classe.NamePascal)
            .ToList();

        foreach (var fromMapper in fromMappers)
        {
            var (clazz, mapper) = fromMapper;

            var constructor = new JavaConstructor(classe.NamePascal)
            {
                Visibility = "public",
                Comment = mapper.Comment ?? $"Crée une nouvelle instance de '{classe.NamePascal}'"
            };

            foreach (var param in mapper.ClassParams)
            {
                var parameter = new JavaMethodParameter(param.Class.GetImport(Config, tag), param.Class.Name, param.Name.ToCamelCase())
                {
                    Comment = param.Comment ?? $"Instance de '{param.Class.NamePascal}'"
                };
                constructor.AddParameter(parameter);
            }

            foreach (var param in mapper.PropertyParams)
            {
                var parameter = new JavaMethodParameter(Config.GetType(param.Property, availableClasses), param.Property.NameCamel)
                {
                    Comment = param.Property.Comment
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
            constructor.AddBodyLine($"{Config.GetMapperName(mapperNs, mapperModelPath)}.create{classe.NamePascal}({string.Join(", ", mapper.ClassParams.Select(p => p.Name.ToCamelCase()).Concat(mapper.PropertyParams.Select(p => p.Property.NameCamel)))}, this);");
            constructor.ReturnComment = $"Une nouvelle instance de '{classe.NamePascal}'";
            fw.WriteLine();
            fw.Write(1, constructor);
        }
    }

    public void WriteNoArgConstructor(JavaWriter fw, Class classe, string tag)
    {
        fw.WriteLine();
        var constructor = new JavaConstructor(classe.NamePascal)
        {
            Visibility = "public",
            Comment = "No arg constructor"
        };
        if (Config.GetClassExtends(classe, tag) != null)
        {
            constructor.AddBodyLine("super();");
        }

        constructor.AddBodyLine("// No arg constructor");
        fw.Write(1, constructor);
    }
}
