using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Core.FileModel;
using TopModel.Utils;

namespace TopModel.Generator.Jpa;

/// <summary>
/// Générateur de fichiers de modèles JPA.
/// </summary>
public class JpaModelGenerator : GeneratorBase
{
    private readonly JpaConfig _config;
    private readonly ILogger<JpaModelGenerator> _logger;
    private readonly JpaModelConstructorGenerator _jpaModelConstructorGenerator;
    private readonly JpaModelPropertyGenerator _jpaModelPropertyGenerator;

    public JpaModelGenerator(ILogger<JpaModelGenerator> logger, JpaConfig config)
        : base(logger, config)
    {
        _config = config;
        _logger = logger;
        _jpaModelConstructorGenerator = new JpaModelConstructorGenerator(config);
        _jpaModelPropertyGenerator = new JpaModelPropertyGenerator(config);
    }

    public override string Name => "JpaModelGen";

    public override IEnumerable<string> GeneratedFiles => Files.SelectMany(f => f.Value.Classes).Select(c => GetFileClassName(c));

    private List<Class> AvailableClasses => Classes.ToList();

    protected override void HandleFiles(IEnumerable<ModelFile> files)
    {
        foreach (var c in Classes)
        {
            CheckClass(c);
        }

        var modules = files.SelectMany(f => f.Classes.Select(c => c.Namespace.Module)).Distinct();

        foreach (var module in modules)
        {
            GenerateModule(module);
        }
    }

    private string GetDestinationFolder(Class classe)
    {
        var packageRoot = classe.IsPersistent ? _config.EntitiesPackageName : _config.DtosPackageName;
        var destFolder = Path.Combine(_config.OutputDirectory, _config.ModelRootPath, Path.Combine(packageRoot.Split(".")), classe.Namespace.Module.Replace('.', Path.DirectorySeparatorChar).ToLower());
        return $"{destFolder}";
    }

    private string GetFileClassName(Class classe)
    {
        return Path.Combine(GetDestinationFolder(classe), $"{classe.Name}.java");
    }

    private string GetPackageName(Class classe)
    {
        var packageRoot = classe.IsPersistent ? _config.EntitiesPackageName : _config.DtosPackageName;
        return $"{packageRoot}.{classe.Namespace.Module.ToLower()}";
    }

    private void GenerateModule(string module)
    {
        var classes = Classes.Where(c => c.Namespace.Module == module);

        foreach (var classe in classes)
        {
            var destFolder = GetDestinationFolder(classe);
            var dirInfo = Directory.CreateDirectory(destFolder);
            var packageName = GetPackageName(classe);
            using var fw = new JavaWriter($"{destFolder}/{classe.Name}.java", _logger, packageName, null);

            WriteImports(fw, classe);
            fw.WriteLine();

            WriteAnnotations(module, fw, classe);
            var extends = (classe.Extends?.Name ?? classe.Decorators.Find(d => d.Java?.Extends is not null)?.Java!.Extends!.ParseTemplate(classe)) ?? null;

            var implements = classe.Decorators.SelectMany(d => d.Java!.Implements).Select(i => i.ParseTemplate(classe)).Distinct().ToList();
            if (!classe.IsPersistent)
            {
                implements.Add("Serializable");
            }

            if (classe.Decorators.Any(d => d.Java != null && d.Java.GenerateInterface))
            {
                implements.Add($"I{classe.Name}");
            }

            fw.WriteClassDeclaration(classe.Name, null, extends, implements);

            if (!classe.IsPersistent)
            {
                fw.WriteLine("	/** Serial ID */");
                fw.WriteLine(1, "private static final long serialVersionUID = 1L;");
            }

            _jpaModelPropertyGenerator.WriteProperties(fw, classe, AvailableClasses);

            _jpaModelConstructorGenerator.WriteNoArgConstructor(fw, classe);
            _jpaModelConstructorGenerator.WriteCopyConstructor(fw, classe, AvailableClasses);
            _jpaModelConstructorGenerator.WriteAllArgConstructor(fw, classe, AvailableClasses);
            if (_config.EnumShortcutMode)
            {
                _jpaModelConstructorGenerator.WriteAllArgConstructorEnumShortcut(fw, classe, AvailableClasses);
            }

            _jpaModelConstructorGenerator.WriteFromMappers(fw, classe, AvailableClasses);

            WriteGetters(fw, classe);
            WriteSetters(fw, classe);
            if (_config.EnumShortcutMode)
            {
                WriteEnumShortcuts(fw, classe);
            }

            WriteToMappers(fw, classe);

            if (_config.FieldsEnum && classe.IsPersistent)
            {
                WriteFieldsEnum(fw, classe, AvailableClasses);
            }

            if (classe.Reference && classe.ReferenceValues.Any())
            {
                WriteReferenceValues(fw, classe);
            }

            fw.WriteLine("}");
        }
    }

    private void WriteEnumShortcuts(JavaWriter fw, Class classe)
    {
        foreach (var ap in classe.GetProperties(_config, AvailableClasses).OfType<AssociationProperty>().Where(ap => ap.Association.Reference))
        {
            var isMultiple = ap.Type == AssociationType.ManyToMany || ap.Type == AssociationType.OneToMany;
            {
                var propertyName = ap.Name.ToFirstLower();
                fw.WriteLine();
                fw.WriteDocStart(1, $"Set the value of {{@link {classe.GetImport(_config)}#{propertyName} {propertyName}}}");
                fw.WriteLine(1, " * Cette méthode permet définir la valeur de la FK directement");
                fw.WriteLine(1, $" * @param {propertyName} value to set");
                fw.WriteDocEnd(1);
                fw.WriteLine(1, @$"public void set{ap.Name}({(isMultiple ? $"List<{ap.Property.GetJavaType()}>" : ap.Property.GetJavaType())} {propertyName}) {{");
                fw.WriteLine(2, $"if ({propertyName} != null) {{");
                if (!isMultiple)
                {
                    var constructorArgs = $"{propertyName}";
                    foreach (var p in ap.Association.GetProperties(_config, AvailableClasses).Where(pr => !pr.PrimaryKey))
                    {
                        var getterPrefix = p.GetJavaType().ToUpper() == "BOOLEAN" ? "is" : "get";
                        constructorArgs += $", {propertyName}.{getterPrefix}{p.GetJavaName().ToFirstUpper()}()";
                    }

                    fw.WriteLine(3, @$"this.{ap.GetAssociationName()} = new {ap.Association.Name}({constructorArgs});");
                }
                else
                {
                    var constructorArgs = "p";
                    foreach (var p in ap.Association.GetProperties(_config, AvailableClasses).Where(pr => !pr.PrimaryKey))
                    {
                        constructorArgs += $", p.get{((p is AssociationProperty asp && asp.IsEnum()) ? p.Name : p.GetJavaName()).ToFirstUpper()}()";
                    }

                    fw.WriteLine(3, @$"if (this.{ap.GetAssociationName()} != null) {{");
                    fw.WriteLine(4, @$"this.{ap.GetAssociationName()}.clear();");
                    fw.WriteLine(3, "} else {");
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
                fw.WriteReturns(1, $"value of {{@link {classe.GetImport(_config)}#{ap.GetJavaName()} {ap.GetJavaName()}}}");
                fw.WriteDocEnd(1);
                fw.WriteLine(1, "@Transient");
                fw.WriteLine(1, @$"public {(isMultiple ? $"List<{ap.Property.GetJavaType()}>" : ap.Property.GetJavaType())} get{ap.Name}() {{");
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

    private void WriteReferenceValues(JavaWriter fw, Class classe)
    {
        fw.WriteLine();
        fw.WriteLine(1, $"public enum Values {{");
        var i = 0;

        foreach (var refValue in classe.ReferenceValues.OrderBy(x => x.Name, StringComparer.Ordinal))
        {
            ++i;
            var code = classe.PrimaryKey?.Domain.AutoGeneratedValue == true
                ? refValue.Value[classe.PrimaryKey ?? classe.GetProperties(_config, AvailableClasses).OfType<IFieldProperty>().First()]
                : refValue.Value[classe.PrimaryKey ?? classe.UniqueKeys.First().Single()];

            if (classe.GetProperties(_config, AvailableClasses).Count > 1)
            {
                var lineToWrite = @$"{code.ToUpper()}(";
                lineToWrite += string.Join(", ", classe.GetProperties(_config, AvailableClasses)
                    .Where(p => !p.PrimaryKey)
                    .Select(prop =>
                    {
                        var isString = ((IFieldProperty)prop).GetJavaType() == "String";
                        var fix = isString ? "\"" : string.Empty;
                        var value = refValue.Value.ContainsKey((IFieldProperty)prop) ? refValue.Value[(IFieldProperty)prop] : "null";
                        if (prop is AssociationProperty ap && ap.IsEnum() && ap.Association.ReferenceValues.Any(r => r.Value.ContainsKey(ap.Association.PrimaryKey) && r.Value[ap.Association.PrimaryKey] == value))
                        {
                            value = ap.Association.Name + ".Values." + value;
                        }

                        return fix + value + fix;
                    }));
                lineToWrite += ")";
                lineToWrite += i == classe.ReferenceValues.Count ? "; " : ", //";
                fw.WriteLine(2, lineToWrite);
            }
            else
            {
                fw.WriteLine(2, @$"{code.ToUpper()}{(i == classe.ReferenceValues.Count ? ";" : ", //")}");
            }
        }

        foreach (var prop in classe.GetProperties(_config, AvailableClasses).Where(p => !p.PrimaryKey))
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

        if (classe.GetProperties(_config, AvailableClasses).Count > 1)
        {
            fw.WriteLine();
            fw.WriteDocStart(2, "All arg constructor");
            fw.WriteDocEnd(2);
            var propertiesSignature = string.Join(", ", classe.GetProperties(_config, AvailableClasses).Where(p => !p.PrimaryKey).Select(prop =>
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
            foreach (var prop in classe.GetProperties(_config, AvailableClasses).Where(p => !p.PrimaryKey))
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

        foreach (var prop in classe.GetProperties(_config, AvailableClasses).Where(p => !p.PrimaryKey))
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

        fw.WriteLine(1, "}");
    }

    private void WriteImports(JavaWriter fw, Class classe)
    {
        var imports = classe.GetImports(Files.SelectMany(f => f.Value.Classes).ToList(), _config);
        imports.AddRange(classe.Decorators.SelectMany(d => d.Java!.Imports.Select(i => i.ParseTemplate(classe))));
        foreach (var property in classe.GetProperties(_config, AvailableClasses))
        {
            imports.AddRange(property.GetImports(_config));
        }

        if (classe.Extends != null)
        {
            foreach (var property in classe.Extends.GetProperties(_config, AvailableClasses))
            {
                imports.AddRange(property.GetImports(_config));
            }
        }

        if (classe.Decorators.Any(d => d.Java != null && d.Java.GenerateInterface))
        {
            var entityDto = classe.IsPersistent ? "entities" : "dtos";
            imports.Add($"{string.Join('.', classe.GetImport(_config).Split('.').SkipLast(1))}.interfaces.I{classe.Name}");
        }

        if (_config.FieldsEnum && _config.FieldsEnumInterface != null && classe.IsPersistent && classe.GetProperties(_config, AvailableClasses).Count > 0)
        {
            imports.Add(_config.FieldsEnumInterface.Replace("<>", string.Empty));
        }

        if (_config.EnumShortcutMode && classe.GetProperties(_config, AvailableClasses).Where(p => p is AssociationProperty apo && apo.IsEnum()).Any())
        {
            imports.Add(_config.PersistenceMode.ToString().ToLower() + ".persistence.Transient");
        }

        fw.AddImports(imports.Where(i => string.Join('.', i.Split('.').SkipLast(1).ToList()) != GetPackageName(classe)).Distinct().ToArray());
    }

    private void CheckClass(Class classe)
    {
        foreach (var property in classe.GetProperties(_config, AvailableClasses).OfType<CompositionProperty>())
        {
            if (!classe.IsPersistent && property.Composition.IsPersistent)
            {
                throw new ModelException(property, $"La propriété ${property} persistée ne peut pas faire l'objet d'une composition dans la classe {classe.Name} car elle ne l'est pas");
            }
        }

        foreach (var property in classe.GetProperties(_config, AvailableClasses).OfType<AssociationProperty>())
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

    private void WriteAnnotations(string module, JavaWriter fw, Class classe)
    {
        fw.WriteDocStart(0, classe.Comment);
        fw.WriteDocEnd(0);

        fw.WriteLine("@Generated(\"TopModel : https://github.com/klee-contrib/topmodel\")");

        if (classe.IsPersistent)
        {
            var table = @$"@Table(name = ""{classe.SqlName}""";
            if (classe.UniqueKeys.Count > 0)
            {
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
            if (classe.IsStatic())
            {
                fw.WriteLine("@Immutable");
                fw.WriteLine("@Cache(usage = CacheConcurrencyStrategy.READ_ONLY)");
            }
            else
            {
                fw.WriteLine("@Cache(usage = CacheConcurrencyStrategy.READ_WRITE)");
            }
        }

        foreach (var a in classe.Decorators.SelectMany(d => d.Java!.Annotations).Select(a => a.ParseTemplate(classe)).Distinct())
        {
            fw.WriteLine($"{(a.StartsWith("@") ? string.Empty : "@")}{a.ParseTemplate(classe)}");
        }
    }

    private void WriteGetters(JavaWriter fw, Class classe)
    {
        foreach (var property in classe.GetProperties(_config, AvailableClasses).Where(p => !_config.EnumShortcutMode || !(p is AssociationProperty apo && apo.Association.Reference)))
        {
            fw.WriteLine();
            fw.WriteDocStart(1, $"Getter for {property.GetJavaName()}");
            fw.WriteReturns(1, $"value of {{@link {classe.GetImport(_config)}#{property.GetJavaName()} {property.GetJavaName()}}}");
            fw.WriteDocEnd(1);
            if (classe.Decorators.Any(d => d.Java != null && d.Java.GenerateInterface))
            {
                fw.WriteLine(1, "@Override");
            }

            var getterPrefix = property.GetJavaType().ToUpper() == "BOOLEAN" ? "is" : "get";
            fw.WriteLine(1, @$"public {property.GetJavaType()} {getterPrefix}{property.GetJavaName().ToFirstUpper()}() {{");
            if (property is AssociationProperty ap && (ap.Type == AssociationType.ManyToMany || ap.Type == AssociationType.OneToMany))
            {
                fw.WriteLine(2, $"if(this.{property.GetJavaName()} == null)");
                fw.WriteLine(3, $"this.{property.GetJavaName()} = new ArrayList<>();");
            }

            fw.WriteLine(2, @$"return this.{property.GetJavaName()};");
            fw.WriteLine(1, "}");
        }
    }

    private void WriteToMappers(JavaWriter fw, Class classe)
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

            fw.WriteParam("source", $"Instance de '{classe}'");
            fw.WriteParam("dest", $"Instance pré-existante de '{mapper.Class}'. Une nouvelle instance sera créée si non spécifié.");
            fw.WriteReturns(1, $"Une instance de '{mapper.Class}'");

            fw.WriteDocEnd(1);
            if (mapper.ParentMapper != null && mapper.ParentMapper.Name == mapper.Name)
            {
                fw.WriteLine(1, $"@Override");
            }

            fw.WriteLine(1, $"public {mapper.Class} {mapper.Name.ToFirstLower()}({mapper.Class} dest) {{");
            fw.WriteLine(2, $"dest = dest == null ? new {mapper.Class}() : dest;");
            fw.WriteLine();
            if (mapper.ParentMapper != null)
            {
                fw.WriteLine(2, $"super.{mapper.ParentMapper.Name.ToFirstLower()}(dest);");
            }

            foreach (var mapping in mapper.Mappings)
            {
                var getterPrefix = mapping.Value!.GetJavaType().ToUpper() == "BOOLEAN" ? "is" : "get";
                if (mapping.Value is AssociationProperty ap)
                {
                    if (ap.Property.IsEnum() && _config.EnumShortcutMode)
                    {
                        fw.WriteLine(2, $"dest.set{mapping.Value.Name.ToFirstUpper()}(this.{getterPrefix}{mapping.Key.Name.ToFirstUpper()}());");
                    }
                    else if (mapping.Value.Class.IsPersistent && mapping.Key.Class.IsPersistent)
                    {
                        fw.WriteLine(2, $"dest.set{mapping.Value.GetJavaName().ToFirstUpper()}(this.{getterPrefix}{mapping.Key.GetJavaName().ToFirstUpper()}());");
                    }
                    else if (mapping.Key is CompositionProperty cp)
                    {
                        if (cp.Composition.ToMappers.Any(t => t.Class == ap.Association))
                        {
                            var cpMapper = cp.Composition.ToMappers.Find(t => t.Class == ap.Association)!;
                            fw.WriteLine(2, $@"if (this.get{cp.GetJavaName().ToFirstUpper()}() != null) {{");
                            fw.WriteLine(3, $@"dest.set{mapping.Value.GetJavaName().ToFirstUpper()}(this.{getterPrefix}{cp.GetJavaName().ToFirstUpper()}().{cpMapper.Name.ToFirstLower()}(dest.get{ap.GetJavaName().ToFirstUpper()}()));");
                            fw.WriteLine(2, $@"}}");
                            fw.WriteLine();
                        }
                        else
                        {
                            throw new ModelException(classe, $"La propriété {mapping.Key.Name} ne peut pas être mappée avec la propriété {mapping.Value.Name} car il n'existe pas de mapper {cp.Composition.Name} -> {ap.Association.Name}");
                        }
                    }
                }
                else
                {
                    fw.WriteLine(2, $"dest.set{mapping.Value!.GetJavaName().ToFirstUpper()}(this.{getterPrefix}{mapping.Key.GetJavaName().ToFirstUpper()}());");
                }
            }

            fw.WriteLine();
            fw.WriteLine(2, "return dest;");

            fw.WriteLine(1, "}");

            if (toMappers.IndexOf(toMapper) < toMappers.Count - 1)
            {
                fw.WriteLine();
            }
        }
    }

    private void WriteSetters(JavaWriter fw, Class classe)
    {
        foreach (var property in classe.GetProperties(_config, AvailableClasses).Where(p => !_config.EnumShortcutMode || !(p is AssociationProperty apo && apo.Association.Reference)))
        {
            var propertyName = property.GetJavaName();
            fw.WriteLine();
            fw.WriteDocStart(1, $"Set the value of {{@link {classe.GetImport(_config)}#{propertyName} {propertyName}}}");
            fw.WriteLine(1, $" * @param {propertyName} value to set");
            fw.WriteDocEnd(1);
            fw.WriteLine(1, @$"public void set{propertyName.ToFirstUpper()}({property.GetJavaType()} {propertyName}) {{");
            fw.WriteLine(2, @$"this.{propertyName} = {propertyName};");
            fw.WriteLine(1, "}");
        }
    }

    private void WriteFieldsEnum(JavaWriter fw, Class classe, List<Class> allClasses)
    {
        fw.WriteLine();
        fw.WriteDocStart(1, $"Enumération des champs de la classe {{@link {classe.GetImport(_config)} {classe.Name}}}");
        fw.WriteDocEnd(1);
        string enumDeclaration = @$"public enum Fields ";
        if (_config.FieldsEnumInterface != null)
        {
            enumDeclaration += $"implements {_config.FieldsEnumInterface.Split(".").Last().Replace("<>", $"<{classe.Name}>")}";
        }

        enumDeclaration += " {";
        fw.WriteLine(1, enumDeclaration);

        var props = classe.GetProperties(_config, AvailableClasses).Select(prop =>
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