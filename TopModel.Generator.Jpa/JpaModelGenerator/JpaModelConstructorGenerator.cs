using TopModel.Core;
using TopModel.Generator.Core;
using TopModel.Utils;

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

    public void WriteAllArgConstructor(JavaWriter fw, Class classe, List<Class> availableClasses, string tag)
    {
        fw.WriteLine();
        fw.WriteDocStart(1, "All arg constructor");
        var properties = GetAllArgsProperties(classe, availableClasses, tag);

        if (properties.Count == 0)
        {
            return;
        }

        var propertiesSignature = string.Join(", ", properties.Select(p => $"{_config.GetType(p, useClassForAssociation: classe.IsPersistent)} {p.GetJavaName()}"));

        foreach (var property in properties)
        {
            fw.WriteLine(1, $" * @param {property.GetJavaName()} {property.Comment}");
        }

        fw.WriteDocEnd(1);
        fw.WriteLine(1, $"public {classe.NamePascal}({propertiesSignature}) {{");
        if (classe.Extends != null)
        {
            var parentAllArgConstructorArguments = string.Join(", ", GetAllArgsProperties(classe.Extends, availableClasses, tag).Select(p => $"{p.GetJavaName()}"));
            fw.WriteLine(2, $"super({parentAllArgConstructorArguments});");
        }
        else if (classe.Decorators.Any(d => _config.GetImplementation(d.Decorator)?.Extends is not null))
        {
            fw.WriteLine(2, $"super();");
        }

        foreach (var property in classe.GetProperties(availableClasses, tag))
        {
            fw.WriteLine(2, $"this.{property.GetJavaName()} = {property.GetJavaName()};");
        }

        fw.WriteLine(1, $"}}");
    }

    public void WriteAllArgConstructorEnumShortcut(JavaWriter fw, Class classe, List<Class> availableClasses, string tag)
    {
        var properties = GetAllArgsProperties(classe, availableClasses, tag);
        if (!properties.OfType<AssociationProperty>().Any(p => _config.CanClassUseEnums(p.Association) && (p.Type == AssociationType.OneToOne || p.Type == AssociationType.ManyToOne)))
        {
            return;
        }

        fw.WriteLine();
        fw.WriteDocStart(1, "All arg constructor when Enum shortcut mode is set");

        if (properties.Count == 0)
        {
            return;
        }

        var propertiesSignature = string.Join(", ", properties.Select(p => $"{_config.GetType(p, useClassForAssociation: p is not AssociationProperty ap || !_config.CanClassUseEnums(ap.Association))} {(p is AssociationProperty asp && _config.CanClassUseEnums(asp.Association) ? p.NameCamel : p.GetJavaName())}"));

        foreach (var property in properties)
        {
            fw.WriteLine(1, $" * @param {property.GetJavaName()} {property.Comment}");
        }

        fw.WriteDocEnd(1);
        fw.WriteLine(1, $"public {classe.NamePascal}({propertiesSignature}) {{");
        if (classe.Extends != null)
        {
            var parentAllArgConstructorArguments = string.Join(", ", GetAllArgsProperties(classe.Extends, availableClasses, tag).Select(p => $"{p.GetJavaName()}"));
            fw.WriteLine(2, $"super({parentAllArgConstructorArguments});");
        }
        else if (classe.Decorators.Any(d => _config.GetImplementation(d.Decorator)?.Extends is not null))
        {
            fw.WriteLine(2, $"super();");
        }

        foreach (var property in classe.GetProperties(availableClasses, tag))
        {
            if (!(property is AssociationProperty aspr2 && _config.CanClassUseEnums(aspr2.Association)))
            {
                fw.WriteLine(2, $"this.{property.GetJavaName()} = {property.GetJavaName()};");
            }
            else
            {
                var isMultiple = aspr2.Type == AssociationType.OneToMany || aspr2.Type == AssociationType.ManyToMany;
                fw.WriteLine(2, $"this.set{aspr2.NamePascal}{(isMultiple ? aspr2.Property.NamePascal : string.Empty)}({property.NameCamel});");
            }
        }

        fw.WriteLine(1, $"}}");
    }

    public void WriteCopyConstructor(JavaWriter fw, Class classe, List<Class> availableClasses, string tag)
    {
        fw.WriteLine();
        fw.WriteDocStart(1, "Copy constructor");
        fw.WriteLine(1, $" * @param {classe.NameCamel} to copy");
        var properties = classe.GetProperties(availableClasses, tag);
        fw.WriteDocEnd(1);
        fw.WriteLine(1, $"public {classe.NamePascal}({classe.NamePascal} {classe.NameCamel}) {{");
        if (classe.Extends != null)
        {
            var parentAllArgConstructorArguments = string.Join(", ", GetAllArgsProperties(classe.Extends, availableClasses, tag).Select(p => $"{p.GetJavaName()}"));
            fw.WriteLine(2, $"super({classe.NameCamel});");
        }
        else if (classe.Decorators.Any(d => _config.GetImplementation(d.Decorator)?.Extends is not null))
        {
            fw.WriteLine(2, $"super();");
        }

        fw.WriteLine(2, $"if({classe.NameCamel} == null) {{");
        fw.WriteLine(3, $"return;");
        fw.WriteLine(2, "}");
        fw.WriteLine();

        foreach (var property in classe.GetProperties(availableClasses, tag).Where(p => !_config.EnumShortcutMode || !(p is AssociationProperty apo && _config.CanClassUseEnums(apo.Association))))
        {
            if (!(property is AssociationProperty ap && (ap.Type == AssociationType.OneToMany || ap.Type == AssociationType.ManyToMany) || property is CompositionProperty cp && cp.Kind == "list"))
            {
                var getterPrefix = _config.GetType(property) == "boolean" ? "is" : "get";
                fw.WriteLine(2, $"this.{property.GetJavaName()} = {classe.NameCamel}.{getterPrefix}{property.GetJavaName(true)}();");
            }
        }

        var propertyListToCopy = classe.GetProperties(availableClasses, tag)
            .Where(p => !_config.EnumShortcutMode || !(p is AssociationProperty apo && _config.CanClassUseEnums(apo.Association)))
            .Where(property => property is AssociationProperty ap && (ap.Type == AssociationType.OneToMany || ap.Type == AssociationType.ManyToMany) || property is CompositionProperty cp && cp.Kind == "list");

        if (propertyListToCopy.Any())
        {
            fw.WriteLine();
        }

        foreach (var property in propertyListToCopy)
        {
            if (property is AssociationProperty ap || property is CompositionProperty cp && cp.Kind == "list")
            {
                var getterPrefix = _config.GetType(property) == "boolean" ? "is" : "get";
                fw.WriteLine(2, $"this.{property.GetJavaName()} = {classe.NameCamel}.{getterPrefix}{property.GetJavaName(true)}().stream().collect(Collectors.toList());");
                fw.AddImport("java.util.stream.Collectors");
            }
        }

        if (_config.EnumShortcutMode)
        {
            fw.WriteLine();
            foreach (var ap in classe.GetProperties(availableClasses, tag).OfType<AssociationProperty>().Where(ap => _config.CanClassUseEnums(ap.Association)))
            {
                var propertyName = ap.NameCamel;
                var getterPrefix = _config.GetType(ap) == "boolean" ? "is" : "get";
                fw.WriteLine(2, $"this.set{ap.NameCamel.ToFirstUpper()}({classe.NameCamel}.{getterPrefix}{ap.NameCamel.ToFirstUpper()}());");
            }
        }

        fw.WriteLine(1, $"}}");
    }

    public void WriteFromMappers(JavaWriter fw, Class classe, List<Class> availableClasses, string tag)
    {
        var fromMappers = classe.FromMappers.Where(c => c.Params.All(p => availableClasses.Contains(p.Class))).Select(m => (classe, m))
        .OrderBy(m => m.classe.NamePascal)
        .ToList();

        foreach (var fromMapper in fromMappers)
        {
            var (clazz, mapper) = fromMapper;
            fw.WriteLine();
            fw.WriteDocStart(1, $"Crée une nouvelle instance de '{classe}'");
            if (mapper.Comment != null)
            {
                fw.WriteLine(1, $" * {mapper.Comment}");
            }

            foreach (var param in mapper.Params)
            {
                if (param.Comment != null)
                {
                    fw.WriteLine(1, $" * {param.Comment}");
                }

                fw.WriteParam(param.Name.ToCamelCase(), $"Instance de '{param.Class}'");
            }

            fw.WriteReturns(1, $"Une nouvelle instance de '{classe}'");
            fw.WriteDocEnd(1);
            fw.WriteLine(1, $"public {classe}({string.Join(", ", mapper.Params.Select(p => $"{p.Class} {p.Name.ToCamelCase()}"))}) {{");
            if (classe.Extends != null)
            {
                fw.WriteLine(2, $"super();");
            }

            var (mapperNs, mapperModelPath) = _config.GetMapperLocation(fromMapper);
            fw.WriteLine(2, $"{_config.GetMapperName(mapperNs, mapperModelPath)}.create{classe}({string.Join(", ", mapper.Params.Select(p => p.Name.ToCamelCase()))}, this);");
            fw.AddImport(_config.GetMapperImport(mapperNs, mapperModelPath, tag)!);
            fw.WriteLine(1, "}");
        }
    }

    private IList<IProperty> GetAllArgsProperties(Class classe, List<Class> availableClasses, string tag)
    {
        if (classe.Extends is null)
        {
            return classe.GetProperties(availableClasses, tag);
        }
        else
        {
            return GetAllArgsProperties(classe.Extends, availableClasses, tag).Concat(classe.GetProperties(availableClasses, tag)).ToList();
        }
    }
}