using TopModel.Core;
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
        fw.WriteLine(1, $"public {classe.Name}() {{");
        if (classe.Extends != null || classe.Decorators.Any(d => d.Decorator.Java?.Extends is not null))
        {
            fw.WriteLine(2, $"super();");
        }

        fw.WriteLine(1, $"}}");
    }

    public void WriteAllArgConstructor(JavaWriter fw, Class classe, List<Class> availableClasses)
    {
        fw.WriteLine();
        fw.WriteDocStart(1, "All arg constructor");
        var properties = GetAllArgsProperties(classe, availableClasses);

        if (properties.Count == 0)
        {
            return;
        }

        var propertiesSignature = string.Join(", ", properties.Select(p => $"{p.GetJavaType()} {p.GetJavaName()}"));

        foreach (var property in properties)
        {
            fw.WriteLine(1, $" * @param {property.GetJavaName()} {property.Comment}");
        }

        fw.WriteDocEnd(1);
        fw.WriteLine(1, $"public {classe.Name}({propertiesSignature}) {{");
        if (classe.Extends != null)
        {
            var parentAllArgConstructorArguments = string.Join(", ", GetAllArgsProperties(classe.Extends, availableClasses).Select(p => $"{p.GetJavaName()}"));
            fw.WriteLine(2, $"super({parentAllArgConstructorArguments});");
        }
        else if (classe.Decorators.Any(d => d.Decorator.Java?.Extends is not null))
        {
            fw.WriteLine(2, $"super();");
        }

        foreach (var property in classe.GetProperties(_config, availableClasses))
        {
            fw.WriteLine(2, $"this.{property.GetJavaName()} = {property.GetJavaName()};");
        }

        fw.WriteLine(1, $"}}");
    }

    public void WriteAllArgConstructorEnumShortcut(JavaWriter fw, Class classe, List<Class> availableClasses)
    {
        var properties = GetAllArgsProperties(classe, availableClasses);
        if (!properties.OfType<AssociationProperty>().Any(p => p.IsEnum() && (p.Type == AssociationType.OneToOne || p.Type == AssociationType.ManyToOne)))
        {
            return;
        }

        fw.WriteLine();
        fw.WriteDocStart(1, "All arg constructor when Enum shortcut mode is set");

        if (properties.Count == 0)
        {
            return;
        }

        var propertiesSignature = string.Join(", ", properties.Select(p => $"{(p is AssociationProperty ap && ap.IsEnum() ? (ap.Type == AssociationType.OneToMany || ap.Type == AssociationType.ManyToMany) ? $"List<{ap.Association.Name}.Values>" : $"{p.GetJavaType()}.Values" : p.GetJavaType())} {(p is AssociationProperty asp && asp.IsEnum() ? p.Name.ToFirstLower() : p.GetJavaName())}"));

        foreach (var property in properties)
        {
            fw.WriteLine(1, $" * @param {property.GetJavaName()} {property.Comment}");
        }

        fw.WriteDocEnd(1);
        fw.WriteLine(1, $"public {classe.Name}({propertiesSignature}) {{");
        if (classe.Extends != null)
        {
            var parentAllArgConstructorArguments = string.Join(", ", GetAllArgsProperties(classe.Extends, availableClasses).Select(p => $"{p.GetJavaName()}"));
            fw.WriteLine(2, $"super({parentAllArgConstructorArguments});");
        }
        else if (classe.Decorators.Any(d => d.Decorator.Java?.Extends is not null))
        {
            fw.WriteLine(2, $"super();");
        }

        foreach (var property in classe.GetProperties(_config, availableClasses))
        {
            if (!(property is AssociationProperty aspr2 && aspr2.IsEnum()))
            {
                fw.WriteLine(2, $"this.{property.GetJavaName()} = {property.GetJavaName()};");
            }
            else
            {
                var isMultiple = aspr2.Type == AssociationType.OneToMany || aspr2.Type == AssociationType.ManyToMany;
                fw.WriteLine(2, $"this.set{aspr2.Name.ToFirstUpper()}{(isMultiple ? aspr2.Association.PrimaryKey!.Name : string.Empty)}({property.Name.ToFirstLower()});");
            }
        }

        fw.WriteLine(1, $"}}");
    }

    public void WriteCopyConstructor(JavaWriter fw, Class classe, List<Class> availableClasses)
    {
        fw.WriteLine();
        fw.WriteDocStart(1, "Copy constructor");
        fw.WriteLine(1, $" * @param {classe.Name.ToFirstLower()} to copy");
        var properties = classe.GetProperties(_config, availableClasses);
        fw.WriteDocEnd(1);
        fw.WriteLine(1, $"public {classe.Name}({classe.Name} {classe.Name.ToFirstLower()}) {{");
        if (classe.Extends != null)
        {
            var parentAllArgConstructorArguments = string.Join(", ", GetAllArgsProperties(classe.Extends, availableClasses).Select(p => $"{p.GetJavaName()}"));
            fw.WriteLine(2, $"super({classe.Name.ToFirstLower()});");
        }
        else if (classe.Decorators.Any(d => d.Decorator.Java?.Extends is not null))
        {
            fw.WriteLine(2, $"super();");
        }

        fw.WriteLine(2, $"if({classe.Name.ToFirstLower()} == null) {{");
        fw.WriteLine(3, $"return;");
        fw.WriteLine(2, "}");
        fw.WriteLine();

        foreach (var property in classe.GetProperties(_config, availableClasses).Where(p => !_config.EnumShortcutMode || !(p is AssociationProperty apo && apo.Association.IsStatic())))
        {
            if (!(property is AssociationProperty ap && (ap.Type == AssociationType.OneToMany || ap.Type == AssociationType.ManyToMany) || property is CompositionProperty cp && cp.Kind == "list"))
            {
                var getterPrefix = property.GetJavaType().ToUpper() == "BOOLEAN" ? "is" : "get";
                fw.WriteLine(2, $"this.{property.GetJavaName().ToFirstLower()} = {classe.Name.ToFirstLower()}.{getterPrefix}{property.GetJavaName().ToFirstUpper()}();");
            }
        }

        var propertyListToCopy = classe.GetProperties(_config, availableClasses)
        .Where(p => !_config.EnumShortcutMode || !(p is AssociationProperty apo && apo.Association.IsStatic()))
        .Where(property => property is AssociationProperty ap && (ap.Type == AssociationType.OneToMany || ap.Type == AssociationType.ManyToMany) || property is CompositionProperty cp && cp.Kind == "list");

        if (propertyListToCopy.Any())
        {
            fw.WriteLine();
        }

        foreach (var property in propertyListToCopy)
        {
            if (property is AssociationProperty ap || property is CompositionProperty cp && cp.Kind == "list")
            {
                var getterPrefix = property.GetJavaType().ToUpper() == "BOOLEAN" ? "is" : "get";
                fw.WriteLine(2, $"this.{property.GetJavaName().ToFirstLower()} = {classe.Name.ToFirstLower()}.{getterPrefix}{property.GetJavaName().ToFirstUpper()}().stream().collect(Collectors.toList());");
                fw.AddImport("java.util.stream.Collectors");
            }
        }

        if (_config.EnumShortcutMode)
        {
            fw.WriteLine();
            foreach (var ap in classe.GetProperties(_config, availableClasses).OfType<AssociationProperty>().Where(ap => ap.Association.IsStatic()))
            {
                var propertyName = ap.Name.ToFirstLower();
                var getterPrefix = ap.GetJavaType().ToUpper() == "BOOLEAN" ? "is" : "get";
                fw.WriteLine(2, $"this.set{ap.Name}({classe.Name.ToFirstLower()}.{getterPrefix}{ap.Name.ToFirstUpper()}());");
            }
        }

        fw.WriteLine(1, $"}}");
    }

    public void WriteFromMappers(JavaWriter fw, Class classe, List<Class> availableClasses)
    {
        var fromMappers = classe.FromMappers.Where(c => c.Params.All(p => availableClasses.Contains(p.Class))).Select(m => (classe, m))
        .OrderBy(m => m.classe.Name)
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

                fw.WriteParam(param.Name.ToFirstLower(), $"Instance de '{param.Class}'");
            }

            fw.WriteReturns(1, $"Une nouvelle instance de '{classe}'");
            fw.WriteDocEnd(1);
            fw.WriteLine(1, $"public {classe}({string.Join(", ", mapper.Params.Select(p => $"{p.Class} {p.Name.ToFirstLower()}"))}) {{");
            if (classe.Extends != null)
            {
                fw.WriteLine(2, $"super();");
            }

            fw.WriteLine(2, $"this.from({string.Join(", ", mapper.Params.Select(p => p.Name.ToFirstLower()))});");

            fw.WriteLine(1, "}");

            fw.WriteLine();
            fw.WriteDocStart(1, $"Map les champs des classes passées en paramètre dans l'instance courante");
            foreach (var param in mapper.Params)
            {
                if (param.Comment != null)
                {
                    fw.WriteParam(param.Name.ToFirstLower(), param.Comment);
                }
                else
                {
                    fw.WriteParam(param.Name.ToFirstLower(), $"Instance de '{param.Class}'");
                }
            }

            fw.WriteDocEnd(1);
            fw.WriteLine(1, $"protected void from({string.Join(", ", mapper.Params.Select(p => $"{p.Class} {p.Name.ToFirstLower()}"))}) {{");
            if (classe.Extends != null || classe.Decorators.Any(d => d.Decorator.Java?.Extends is not null))
            {
                if (mapper.ParentMapper != null)
                {
                    fw.WriteLine(2, $"super.from({string.Join(", ", mapper.Params.Take(mapper.ParentMapper.Params.Count).Select(p => p.Name))});");
                }
            }

            foreach (var param in mapper.Params.Where(p => p.Mappings.Count > 0))
            {
                fw.WriteLine(2, $"if ({param.Name.ToFirstLower()} != null) {{");
                var mappings = param.Mappings.ToList();
                foreach (var mapping in mappings)
                {
                    var getterPrefix = mapping.Value!.GetJavaType().ToUpper() == "BOOLEAN" ? "is" : "get";
                    if (mapping.Value is AssociationProperty ap)
                    {
                        if (!classe.IsPersistent)
                        {
                            if (mapping.Key is IFieldProperty)
                            {
                                fw.WriteLine(3, $"if ({param.Name.ToFirstLower()}.{getterPrefix}{mapping.Value.GetJavaName().ToFirstUpper()}() != null) {{");
                                if (ap.Type == AssociationType.OneToOne || ap.Type == AssociationType.ManyToOne)
                                {
                                    fw.WriteLine(4, $"this.{mapping.Key.GetJavaName()} = {param.Name.ToFirstLower()}.{getterPrefix}{ap.GetJavaName().ToFirstUpper()}().{getterPrefix}{ap.Property.Name.ToFirstUpper()}();");
                                }
                                else
                                {
                                    fw.WriteLine(4, $"this.{mapping.Key.GetJavaName()} = {param.Name.ToFirstLower()}.{getterPrefix}{ap.GetJavaName().ToFirstUpper()}().stream().filter(t -> t != null).map({ap.GetJavaName().ToFirstLower()} -> {ap.GetJavaName().ToFirstLower()}.{getterPrefix}{ap.Property.Name.ToFirstUpper()}()).collect(Collectors.toList());");
                                    fw.AddImport("java.util.stream.Collectors");
                                }

                                fw.WriteLine(3, "}");

                                if (mappings.IndexOf(mapping) < mappings.Count - 1)
                                {
                                    fw.WriteLine();
                                }
                            }
                            else if (mapping.Key is CompositionProperty cp)
                            {
                                if (mapping.Value == null)
                                {
                                    fw.WriteLine(3, $"this.{mapping.Key.GetJavaName()} = {param.Name};");
                                }
                                else if (cp.Composition.FromMappers.Any(f => f.Params.Count == 1 && f.Params.First().Class == ap.Association))
                                {
                                    var cpMapper = cp.Composition.FromMappers.Find(f => f.Params.Count == 1 && f.Params.First().Class == ap.Association);
                                    var getter = $"{param.Name.ToFirstLower()}.{getterPrefix}{ap.GetJavaName().ToFirstUpper()}()";
                                    if (ap.Type == AssociationType.OneToMany || ap.Type == AssociationType.ManyToMany)
                                    {
                                        fw.WriteLine(3, $"this.{mapping.Key.GetJavaName()} = {getter} == null ? null : {getter}.stream().map({cp.Composition.Name.ToFirstUpper()}::new).collect(Collectors.toList());");
                                        fw.AddImport("java.util.stream.Collectors");
                                    }
                                    else
                                    {
                                        fw.WriteLine(3, $"this.{mapping.Key.GetJavaName()} = {getter} != null ? null : new {cp.Composition.Name.ToFirstUpper()}({getter});");
                                    }
                                }
                                else
                                {
                                    throw new ModelException(classe, $"La propriété {mapping.Key.Name} ne peut pas être mappée avec la propriété {mapping.Value.Name} car il n'existe pas de mapper {ap.Association.Name} -> {cp.Composition.Name}");
                                }
                            }
                        }
                        else
                        {
                            fw.WriteLine(3, $"this.{mapping.Key.GetJavaName()} = {param.Name.ToFirstLower()}.{getterPrefix}{mapping.Value.GetJavaName().ToFirstUpper()}();");
                        }
                    }
                    else
                    {
                        if (mapping.Key is IFieldProperty ifpFrom && mapping.Value is IFieldProperty ifpTo && ifpFrom.Domain != ifpTo.Domain)
                        {
                            var converter = ifpFrom.Domain.ConvertersFrom.FirstOrDefault(c => c.To.Any(t => t == ifpTo.Domain));
                            string conversion = $@"{param.Name.ToFirstLower()}.{getterPrefix}{mapping.Value!.Name.ToFirstUpper()}()";
                            if (converter != null && converter.Java?.Text != null)
                            {
                                var convert = converter.Java.Text;
                                conversion = convert.Replace("{value}", conversion)
                                    .ParseTemplate(ifpFrom.Domain, "java", "from.")
                                    .ParseTemplate(ifpTo.Domain, "java", "to.");
                            }

                            fw.WriteLine(3, $"this.{mapping.Key.GetJavaName()} = {conversion};");
                        }
                        else
                        {
                            fw.WriteLine(3, $"this.{mapping.Key.GetJavaName()} = {param.Name.ToFirstLower()}.{getterPrefix}{mapping.Value!.Name.ToFirstUpper()}();");
                        }
                    }
                }

                if (param.Required)
                {
                    fw.WriteLine(2, "} else {");
                    fw.WriteLine(3, $"throw new IllegalArgumentException(\"{param.Name} cannot be null\");");
                }

                fw.WriteLine(2, "}");

                if (mapper.Params.IndexOf(param) < mapper.Params.Count - 1)
                {
                    fw.WriteLine();
                }
            }

            fw.WriteLine(1, "}");
        }
    }

    private IList<IProperty> GetAllArgsProperties(Class classe, List<Class> availableClasses)
    {
        if (classe.Extends is null)
        {
            return classe.GetProperties(_config, availableClasses);
        }
        else
        {
            return GetAllArgsProperties(classe.Extends, availableClasses).Concat(classe.GetProperties(_config, availableClasses)).ToList();
        }
    }
}