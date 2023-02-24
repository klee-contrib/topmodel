using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Utils;

namespace TopModel.Generator.Jpa;

/// <summary>
/// Générateur de fichiers de modèles JPA.
/// </summary>
public class JpaModelGenerator : ClassGeneratorBase
{
    private readonly JpaConfig _config;
    private readonly ILogger<JpaModelGenerator> _logger;
    private readonly JpaModelConstructorGenerator _jpaModelConstructorGenerator;
    private readonly JpaModelPropertyGenerator _jpaModelPropertyGenerator;

    private readonly ModelConfig _modelConfig;

    public JpaModelGenerator(ILogger<JpaModelGenerator> logger, JpaConfig config, ModelConfig modelConfig)
        : base(logger, config)
    {
        _config = config;
        _logger = logger;
        _jpaModelConstructorGenerator = new JpaModelConstructorGenerator(config);
        _jpaModelPropertyGenerator = new JpaModelPropertyGenerator(config);
        _modelConfig = modelConfig;
    }

    public override string Name => "JpaModelGen";

    private List<Class> AvailableClasses => Classes.ToList();

    protected override bool FilterClass(Class classe)
    {
        return !classe.Abstract;
    }

    protected override string GetFileName(Class classe, string tag)
    {
        return Path.Combine(
            _config.OutputDirectory,
            _config.ResolveVariables(_config.ModelRootPath, tag),
            Path.Combine(_config.ResolveVariables(classe.IsPersistent ? _config.EntitiesPackageName : _config.DtosPackageName, tag).Split(".")),
            classe.Namespace.Module.Replace('.', Path.DirectorySeparatorChar).ToLower(),
            $"{classe.Name}.java");
    }

    protected override void HandleClass(string fileName, Class classe, string tag)
    {
        CheckClass(classe, tag);

        var packageName = GetPackageName(classe, tag);
        using var fw = new JavaWriter(fileName, _logger, packageName, null);

        WriteImports(fw, classe, tag);
        fw.WriteLine();

        WriteAnnotations(fw, classe);

        var extendsDecorator = classe.Decorators.SingleOrDefault(d => d.Decorator.Java?.Extends != null);
        var extends = (classe.Extends?.Name ?? extendsDecorator.Decorator?.Java!.Extends!.ParseTemplate(classe, extendsDecorator.Parameters)) ?? null;

        var implements = classe.Decorators.SelectMany(d => d.Decorator.Java!.Implements.Select(i => i.ParseTemplate(classe, d.Parameters))).Distinct().ToList();
        if (!classe.IsPersistent)
        {
            implements.Add("Serializable");
        }

        fw.WriteClassDeclaration(classe.Name, null, extends, implements);

        if (!classe.IsPersistent)
        {
            fw.WriteLine("	/** Serial ID */");
            fw.WriteLine(1, "private static final long serialVersionUID = 1L;");
        }

        _jpaModelPropertyGenerator.WriteProperties(fw, classe, AvailableClasses, tag);

        _jpaModelConstructorGenerator.WriteNoArgConstructor(fw, classe);
        _jpaModelConstructorGenerator.WriteCopyConstructor(fw, classe, AvailableClasses, tag);
        _jpaModelConstructorGenerator.WriteAllArgConstructor(fw, classe, AvailableClasses, tag);
        if (_config.EnumShortcutMode)
        {
            _jpaModelConstructorGenerator.WriteAllArgConstructorEnumShortcut(fw, classe, AvailableClasses, tag);
        }

        _jpaModelConstructorGenerator.WriteFromMappers(fw, classe, AvailableClasses, tag);

        WriteGetters(fw, classe, tag);
        WriteSetters(fw, classe, tag);
        if (_config.EnumShortcutMode)
        {
            WriteEnumShortcuts(fw, classe, tag);
        }

        WriteToMappers(fw, classe, tag);

        if ((_config.FieldsEnum & Target.Persisted) > 0 && classe.IsPersistent
            || (_config.FieldsEnum & Target.Dto) > 0 && !classe.IsPersistent)
        {
            if (_config.FieldsEnumInterface != null)
            {
                fw.AddImport(_config.FieldsEnumInterface.Replace("<>", string.Empty));
            }

            WriteFieldsEnum(fw, classe, tag);
        }

        if (classe.EnumKey != null)
        {
            WriteReferenceValues(fw, classe, tag);
        }

        fw.WriteLine("}");
    }

    private string GetPackageName(Class classe, string tag)
    {
        var packageRoot = _config.ResolveVariables(classe.IsPersistent ? _config.EntitiesPackageName : _config.DtosPackageName, tag);
        return $"{packageRoot}.{classe.Namespace.Module.ToLower()}";
    }

    private void WriteEnumShortcuts(JavaWriter fw, Class classe, string tag)
    {
        foreach (var ap in classe.GetProperties(_config, AvailableClasses, tag).OfType<AssociationProperty>().Where(ap => ap.Association.IsStatic()))
        {
            var isMultiple = ap.Type == AssociationType.ManyToMany || ap.Type == AssociationType.OneToMany;
            {
                var propertyName = ap.Name.ToFirstLower();
                fw.WriteLine();
                fw.WriteDocStart(1, $"Set the value of {{@link {classe.GetImport(_config, tag)}#{propertyName} {propertyName}}}");
                fw.WriteLine(1, " * Cette méthode permet définir la valeur de la FK directement");
                fw.WriteLine(1, $" * @param {propertyName} value to set");
                fw.WriteDocEnd(1);
                fw.WriteLine(1, @$"public void set{(isMultiple ? ap.Name + ap.Property.Name : ap.Name)}({(isMultiple ? $"List<{ap.Property.GetJavaType()}>" : ap.Property.GetJavaType())} {propertyName}) {{");
                fw.WriteLine(2, $"if ({propertyName} != null) {{");
                if (!isMultiple)
                {
                    fw.WriteLine(3, @$"this.{ap.GetAssociationName()} = {propertyName}.getEntity();");
                }
                else
                {
                    var constructorArgs = "p";
                    foreach (var p in ap.Association.GetProperties(_config, AvailableClasses, tag).Where(pr => !pr.PrimaryKey))
                    {
                        constructorArgs += $", p.get{((p is AssociationProperty asp && asp.IsEnum()) ? p.Name : p.GetJavaName()).ToFirstUpper()}()";
                    }

                    fw.WriteLine(3, @$"if (this.{ap.GetAssociationName()} != null) {{");
                    fw.WriteLine(4, @$"this.{ap.GetAssociationName()}.clear();");
                    fw.WriteLine(3, "} else {");
                    fw.AddImport("java.util.ArrayList");
                    fw.WriteLine(4, @$"this.{ap.GetAssociationName()} = new ArrayList<>();");
                    fw.WriteLine(3, "}");
                    fw.WriteLine(3, @$"this.{ap.GetAssociationName()}.addAll({propertyName}.stream().map(p -> new {ap.Association.Name}({constructorArgs})).collect(Collectors.toList()));");
                    fw.AddImport("java.util.stream.Collectors");
                }

                fw.WriteLine(2, "} else {");
                fw.WriteLine(3, @$"this.{ap.GetAssociationName()} = null;");
                fw.WriteLine(2, "}");
                fw.WriteLine(1, "}");
            }

            {
                fw.WriteLine();
                fw.WriteDocStart(1, $"Getter for {ap.Name.ToFirstLower()}");
                fw.WriteLine(1, " * Cette méthode permet de manipuler directement la foreign key de la liste de référence");
                fw.WriteReturns(1, $"value of {{@link {classe.GetImport(_config, tag)}#{ap.GetJavaName()} {ap.GetJavaName()}}}");
                fw.WriteDocEnd(1);
                fw.WriteLine(1, "@Transient");
                fw.WriteLine(1, @$"public {(isMultiple ? $"List<{ap.Property.GetJavaType()}>" : ap.Property.GetJavaType())} get{(isMultiple ? ap.Name + ap.Property.Name : ap.Name)}() {{");
                if (!isMultiple)
                {
                    fw.WriteLine(2, @$"return this.{ap.GetAssociationName()} != null ? this.{ap.GetAssociationName()}.get{ap.Property.Name}() : null;");
                }
                else
                {
                    fw.WriteLine(2, @$"return this.{ap.GetAssociationName()} != null ? this.{ap.GetAssociationName()}.stream().map({ap.Association.Name}::get{ap.Property.Name}).collect(Collectors.toList()) : null;");
                    fw.AddImport("java.util.stream.Collectors");
                }

                fw.WriteLine(1, "}");
            }
        }
    }

    private void WriteReferenceValues(JavaWriter fw, Class classe, string tag)
    {
        fw.WriteLine();
        var codeProperty = classe.EnumKey!;

        if (codeProperty.IsEnum())
        {
            fw.WriteLine(1, $"public enum Values {{");
            var i = 0;

            foreach (var refValue in classe.Values.OrderBy(x => x.Name, StringComparer.Ordinal))
            {
                ++i;
                var code = refValue.Value[codeProperty];

                if (classe.GetProperties(_config, AvailableClasses, tag).Count > 1)
                {
                    var lineToWrite = @$"{code.ToUpper()}(";
                    lineToWrite += string.Join(", ", classe.GetProperties(_config, AvailableClasses, tag)
                        .Where(p => p != codeProperty)
                        .Select(prop =>
                        {
                            var isString = ((IFieldProperty)prop).GetJavaType() == "String";
                            var value = refValue.Value.ContainsKey((IFieldProperty)prop) ? refValue.Value[(IFieldProperty)prop] : "null";
                            if (prop is AssociationProperty ap && codeProperty.PrimaryKey && ap.Association.Values.Any(r => r.Value.ContainsKey(ap.Association.PrimaryKey.Single()) && r.Value[ap.Association.PrimaryKey.Single()] == value))
                            {
                                value = ap.Association.Name + ".Values." + value;
                                isString = false;
                                fw.AddImport(ap.Association.GetImport(_config, tag));
                            }

                            if (_modelConfig.I18n.TranslateReferences && classe.DefaultProperty == prop)
                            {
                                value = refValue.ResourceKey;
                            }

                            var quote = isString ? "\"" : string.Empty;
                            return quote + value + quote;
                        }));
                    lineToWrite += ")";
                    lineToWrite += i == classe.Values.Count ? "; " : ", //";
                    fw.WriteLine(2, lineToWrite);
                }
                else
                {
                    fw.WriteLine(2, @$"{code.ToUpper()}{(i == classe.Values.Count ? ";" : ", //")}");
                }
            }

            foreach (var prop in classe.GetProperties(_config, AvailableClasses, tag).Where(p => p != codeProperty))
            {
                fw.WriteLine();
                fw.WriteDocStart(2, ((IFieldProperty)prop).Comment);
                fw.WriteDocEnd(2);
                var type = ((IFieldProperty)prop).GetJavaType();
                var name = prop.GetJavaName().ToFirstLower();
                if (prop is AssociationProperty ap && ap.IsEnum())
                {
                    type = $"{ap.Association.Name}.Values";
                    name = prop.Name.ToString().ToFirstLower();
                }

                fw.WriteLine(2, $"private final {type} {name};");
            }

            if (classe.GetProperties(_config, AvailableClasses, tag).Count > 1)
            {
                fw.WriteLine();
                fw.WriteDocStart(2, "All arg constructor");
                fw.WriteDocEnd(2);
                var propertiesSignature = string.Join(", ", classe.GetProperties(_config, AvailableClasses, tag).Where(p => p != codeProperty).Select(prop =>
                {
                    var type = ((IFieldProperty)prop).GetJavaType();
                    var name = prop.GetJavaName().ToFirstLower();
                    if (prop is AssociationProperty ap && ap.IsEnum())
                    {
                        type = $"{ap.Association.Name}.Values";
                        name = prop.Name.ToString().ToFirstLower();
                    }

                    return $"{type} {name}";
                }));

                fw.WriteLine(2, $"private Values({propertiesSignature}) {{");
                foreach (var prop in classe.GetProperties(_config, AvailableClasses, tag).Where(p => p != codeProperty))
                {
                    var type = ((IFieldProperty)prop).GetJavaType();
                    var name = prop.GetJavaName().ToFirstLower();
                    if (prop is AssociationProperty ap && ap.IsEnum())
                    {
                        type = $"{ap.Association.Name}.Values";
                        name = prop.Name.ToString().ToFirstLower();
                    }

                    fw.WriteLine(3, $"this.{name} = {name};");
                }

                fw.WriteLine(2, $"}}");
            }

            fw.WriteLine();
            fw.WriteDocStart(2, "Méthode permettant de récupérer l'entité correspondant au code");
            fw.WriteReturns(2, @$"instance de {{@link {classe.GetImport(_config, tag)}}} correspondant au code courant");
            fw.WriteDocEnd(2);
            fw.WriteLine(2, $"public {classe.Name} getEntity() {{");
            var properties = string.Join(", ", classe.GetProperties(_config, AvailableClasses, tag).Where(p => p != codeProperty).Select(prop =>
            {
                if (prop is AssociationProperty ap && ap.IsEnum())
                {
                    return prop.Name.ToString().ToFirstLower() + ".getEntity()";
                }
                else
                {
                    return prop.GetJavaName().ToFirstLower();
                }
            }));

            fw.WriteLine(3, $"return new {classe.Name}(this{(classe.GetProperties(_config, AvailableClasses, tag).Count > 1 ? ", " + properties : string.Empty)});");
            fw.WriteLine(2, $"}}");
            foreach (var prop in classe.GetProperties(_config, AvailableClasses, tag).Where(p => p != codeProperty))
            {
                fw.WriteLine();
                fw.WriteDocStart(2, ((IFieldProperty)prop).Comment);
                fw.WriteDocEnd(2);
                var type = ((IFieldProperty)prop).GetJavaType();
                var name = prop.GetJavaName().ToFirstLower();
                if (prop is AssociationProperty ap && ap.IsEnum())
                {
                    type = $"{ap.Association.Name}.Values";
                    name = prop.Name.ToString().ToFirstLower();
                }

                var getterPrefix = prop.GetJavaType().ToUpper() == "BOOLEAN" ? "is" : "get";
                fw.WriteLine(2, $"public {type} {getterPrefix}{name.ToFirstUpper()}(){{");
                fw.WriteLine(3, $"return this.{name};");
                fw.WriteLine(2, $"}}");
            }
        }
        else
        {
            fw.WriteLine();
            fw.WriteDocStart(1, @$"Classe static encapsulant les différentes valeurs que peut prendre {{@link {classe.GetImport(_config, tag)}#{codeProperty.GetJavaName()} {codeProperty.GetJavaName()}}}");
            fw.WriteDocEnd(1);
            fw.WriteLine(1, @$"public static class Values {{");
            foreach (var refValue in classe.Values.OrderBy(x => x.Name, StringComparer.Ordinal))
            {
                var lineToWrite = @$"public static String {refValue.Name.ToConstantCase()} = ""{refValue.Value[(IFieldProperty)codeProperty]}""";
                lineToWrite += ";";
                fw.WriteLine(2, lineToWrite);
            }
        }

        fw.WriteLine(1, "}");
    }

    private void WriteImports(JavaWriter fw, Class classe, string tag)
    {
        var imports = classe.GetImports(Files.SelectMany(f => f.Value.Classes).ToList(), _config, tag);
        imports.AddRange(classe.Decorators.SelectMany(d => d.Decorator.Java!.Imports.Select(i => i.ParseTemplate(classe, d.Parameters))));
        foreach (var property in classe.GetProperties(_config, AvailableClasses, tag))
        {
            imports.AddRange(property.GetTypeImports(_config, tag));
            imports.AddRange(property.GetPersistenceImports(_config));
        }

        if (classe.Extends != null)
        {
            foreach (var property in classe.Extends.GetProperties(_config, AvailableClasses, tag))
            {
                imports.AddRange(property.GetTypeImports(_config, tag));
            }
        }

        if (_config.EnumShortcutMode && classe.GetProperties(_config, AvailableClasses, tag).Where(p => p is AssociationProperty apo && apo.IsEnum()).Any())
        {
            imports.Add(_config.PersistenceMode.ToString().ToLower() + ".persistence.Transient");
        }

        fw.AddImports(imports.Where(i => string.Join('.', i.Split('.').SkipLast(1).ToList()) != GetPackageName(classe, tag)).Distinct().ToArray());
    }

    private void CheckClass(Class classe, string tag)
    {
        foreach (var property in classe.GetProperties(_config, AvailableClasses, tag).OfType<CompositionProperty>())
        {
            if (!classe.IsPersistent && property.Composition.IsPersistent)
            {
                throw new ModelException(property, $"La propriété ${property} persistée ne peut pas faire l'objet d'une composition dans la classe {classe.Name} car elle ne l'est pas");
            }
        }

        foreach (var property in classe.GetProperties(_config, AvailableClasses, tag).OfType<AssociationProperty>())
        {
            if (!classe.IsPersistent)
            {
                throw new ModelException(classe, $"La classe {classe} est non persistée, elle ne peut donc pas être associée à {property.Association.Name}");
            }

            if (!property.Association.IsPersistent)
            {
                throw new ModelException(classe, $"La classe {property.Association} est non persistée, elle ne peut donc pas être associée à {classe.Name}");
            }
        }
    }

    private void WriteAnnotations(JavaWriter fw, Class classe)
    {
        fw.WriteDocStart(0, classe.Comment);
        fw.WriteDocEnd(0);

        fw.WriteLine("@Generated(\"TopModel : https://github.com/klee-contrib/topmodel\")");
        var javaOrJakarta = _config.PersistenceMode.ToString().ToLower();

        if (classe.IsPersistent)
        {
            var table = @$"@Table(name = ""{classe.SqlName}""";
            if (classe.UniqueKeys.Any())
            {
                fw.AddImport($"{javaOrJakarta}.persistence.UniqueConstraint");
                table += ", uniqueConstraints = {";
                var isFirstConstraint = true;
                foreach (var unique in classe.UniqueKeys)
                {
                    if (!isFirstConstraint)
                    {
                        table += ",";
                    }

                    table += "\n    ";
                    isFirstConstraint = false;
                    table += "@UniqueConstraint(columnNames = {";
                    var isFirstColumn = true;
                    foreach (var u in unique)
                    {
                        if (!isFirstColumn)
                        {
                            table += ",";
                        }

                        isFirstColumn = false;
                        table += $"\"{u.SqlName}\"";
                    }

                    table += "})";
                }

                table += "}";
            }

            table += ")";
            fw.WriteLine("@Entity");
            fw.WriteLine(table);
        }

        if (classe.Reference)
        {
            fw.AddImports(new List<string>()
                {
                    "org.hibernate.annotations.Cache",
                    "org.hibernate.annotations.CacheConcurrencyStrategy"
                });
            if (classe.IsStatic())
            {
                fw.AddImport("org.hibernate.annotations.Immutable");
                fw.WriteLine("@Immutable");
                fw.WriteLine("@Cache(usage = CacheConcurrencyStrategy.READ_ONLY)");
            }
            else
            {
                fw.WriteLine("@Cache(usage = CacheConcurrencyStrategy.READ_WRITE)");
            }
        }

        foreach (var a in classe.Decorators.SelectMany(d => d.Decorator.Java!.Annotations.Select(a => a.ParseTemplate(classe, d.Parameters))).Distinct())
        {
            fw.WriteLine($"{(a.StartsWith("@") ? string.Empty : "@")}{a}");
        }
    }

    private void WriteGetters(JavaWriter fw, Class classe, string tag)
    {
        foreach (var property in classe.GetProperties(_config, AvailableClasses, tag))
        {
            fw.WriteLine();
            fw.WriteDocStart(1, $"Getter for {property.GetJavaName()}");
            fw.WriteReturns(1, $"value of {{@link {classe.GetImport(_config, tag)}#{property.GetJavaName()} {property.GetJavaName()}}}");
            fw.WriteDocEnd(1);

            var getterPrefix = property.GetJavaType().ToUpper() == "BOOLEAN" ? "is" : "get";
            fw.WriteLine(1, @$"public {property.GetJavaType()} {getterPrefix}{property.GetJavaName().ToFirstUpper()}() {{");
            if (property is AssociationProperty ap && (ap.Type == AssociationType.ManyToMany || ap.Type == AssociationType.OneToMany))
            {
                fw.WriteLine(2, $"if(this.{property.GetJavaName()} == null)");
                fw.AddImport("java.util.ArrayList");
                fw.WriteLine(3, $"this.{property.GetJavaName()} = new ArrayList<>();");
            }

            fw.WriteLine(2, @$"return this.{property.GetJavaName()};");
            fw.WriteLine(1, "}");
        }
    }

    private void WriteToMappers(JavaWriter fw, Class classe, string tag)
    {
        var toMappers = classe.ToMappers.Where(p => AvailableClasses.Contains(p.Class)).Select(m => (classe, m))
        .OrderBy(m => m.m.Name)
        .ToList();

        foreach (var toMapper in toMappers)
        {
            var (clazz, mapper) = toMapper;

            fw.WriteLine();
            fw.WriteDocStart(1, $"Mappe '{classe}' vers '{mapper.Class}'");
            if (mapper.Comment != null)
            {
                fw.WriteLine(1, $" * {mapper.Comment}");
            }

            fw.WriteParam("target", $"Instance pré-existante de '{mapper.Class}'. Une nouvelle instance sera créée si non spécifié.");
            fw.WriteReturns(1, $"Une instance de '{mapper.Class}'");

            fw.WriteDocEnd(1);
            if (mapper.ParentMapper != null && mapper.ParentMapper.Name == mapper.Name)
            {
                fw.WriteLine(1, $"@Override");
            }

            fw.WriteLine(1, $"public {mapper.Class} {mapper.Name.Value.ToCamelCase()}({mapper.Class} target) {{");
            fw.WriteLine(2, $"return {_config.GetMapperClassName(classe, mapper)}.{mapper.Name.Value.ToCamelCase()}(this, target);");
            fw.AddImport(_config.GetMapperImport(classe, mapper, tag)!);
            fw.WriteLine(1, "}");

            if (toMappers.IndexOf(toMapper) < toMappers.Count - 1)
            {
                fw.WriteLine();
            }
        }
    }

    private void WriteSetters(JavaWriter fw, Class classe, string tag)
    {
        foreach (var property in classe.GetProperties(_config, AvailableClasses, tag))
        {
            var propertyName = property.GetJavaName();
            fw.WriteLine();
            fw.WriteDocStart(1, $"Set the value of {{@link {classe.GetImport(_config, tag)}#{propertyName} {propertyName}}}");
            fw.WriteLine(1, $" * @param {propertyName} value to set");
            fw.WriteDocEnd(1);
            fw.WriteLine(1, @$"public void set{propertyName.ToFirstUpper()}({property.GetJavaType()} {propertyName}) {{");
            fw.WriteLine(2, @$"this.{propertyName} = {propertyName};");
            fw.WriteLine(1, "}");
        }
    }

    private void WriteFieldsEnum(JavaWriter fw, Class classe, string tag)
    {
        fw.WriteLine();
        fw.WriteDocStart(1, $"Enumération des champs de la classe {{@link {classe.GetImport(_config, tag)} {classe.Name}}}");
        fw.WriteDocEnd(1);
        string enumDeclaration = @$"public enum Fields ";
        if (_config.FieldsEnumInterface != null)
        {
            enumDeclaration += $"implements {_config.FieldsEnumInterface.Split(".").Last().Replace("<>", $"<{classe.Name}>")}";
        }

        enumDeclaration += " {";
        fw.WriteLine(1, enumDeclaration);

        var props = classe.GetProperties(_config, AvailableClasses, tag).Select(prop =>
        {
            string name;
            if (prop is AssociationProperty ap)
            {
                name = ap.GetAssociationName().ToConstantCase();
            }
            else
            {
                name = prop.Name.ToConstantCase();
            }

            var javaType = prop.GetJavaType();
            javaType = javaType.Split("<").First();
            return $"        {name}({javaType}.class)";
        });

        fw.WriteLine(string.Join(", //\n", props) + ";");

        fw.WriteLine();

        fw.WriteLine(2, "private Class<?> type;");
        fw.WriteLine();
        fw.WriteLine(2, "private Fields(Class<?> type) {");
        fw.WriteLine(3, "this.type = type;");
        fw.WriteLine(2, "}");

        fw.WriteLine();

        fw.WriteLine(2, "public Class<?> getType() {");
        fw.WriteLine(3, "return this.type;");
        fw.WriteLine(2, "}");

        fw.WriteLine(1, "}");
    }
}