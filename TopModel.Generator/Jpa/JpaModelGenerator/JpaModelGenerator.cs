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

    private readonly IDictionary<string, ModelFile> _files = new Dictionary<string, ModelFile>();

    public JpaModelGenerator(ILogger<JpaModelGenerator> logger, JpaConfig config)
        : base(logger, config)
    {
        _config = config;
        _logger = logger;
        _jpaModelConstructorGenerator = new JpaModelConstructorGenerator(config);
    }

    public override string Name => "JpaModelGen";

    public override IEnumerable<string> GeneratedFiles => _files.SelectMany(f => f.Value.Classes).Select(c => GetFileClassName(c));

    protected override void HandleFiles(IEnumerable<ModelFile> files)
    {
        foreach (var file in files)
        {
            _files[file.Name] = file;
        }

        foreach (var c in _files.Values
                    .SelectMany(f => f.Classes)
                    .Distinct())
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
        var entityDto = classe.IsPersistent ? "entities" : "dtos";
        var destFolder = Path.Combine(_config.ModelOutputDirectory, Path.Combine(_config.DaoPackageName.Split(".")), entityDto, classe.Namespace.Module.Replace(".", "\\").ToLower());
        return $"{destFolder}";
    }

    private string GetFileClassName(Class classe)
    {
        return $"{GetDestinationFolder(classe)}\\{classe.Name}.java";
    }

    private string GetPackageName(Class classe)
    {
        var entityDto = classe.IsPersistent ? "entities" : "dtos";
        return $"{_config.DaoPackageName}.{entityDto}.{classe.Namespace.Module.ToLower()}";
    }

    private void GenerateModule(string module)
    {
        var classes = _files.Values
            .SelectMany(f => f.Classes)
            .Distinct()
            .Where(c => c.Namespace.Module == module);

        foreach (var classe in classes)
        {
            var destFolder = GetDestinationFolder(classe);
            var dirInfo = Directory.CreateDirectory(destFolder);
            var packageName = GetPackageName(classe);
            using var fw = new JavaWriter($"{destFolder}/{classe.Name}.java", _logger, null);
            fw.WriteLine($"package {packageName};");

            WriteImports(fw, classe);
            fw.WriteLine();

            WriteAnnotations(module, fw, classe);
            var extends = (classe.Extends?.Name ?? classe.Decorators.Find(d => d.Java?.Extends is not null)?.Java!.Extends) ?? null;

            var implements = classe.Decorators.SelectMany(d => d.Java!.Implements).Distinct().ToList();
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

            WriteProperties(fw, classe);
            _jpaModelConstructorGenerator.WriteNoArgConstructor(fw, classe);
            _jpaModelConstructorGenerator.WriteCopyConstructor(fw, classe);
            _jpaModelConstructorGenerator.WriteAllArgConstructor(fw, classe);

            var availableClasses = _files.Values.SelectMany(c => c.Classes).ToList();
            _jpaModelConstructorGenerator.WriteFromMappers(fw, classe, availableClasses);

            WriteGetters(fw, classe);
            WriteSetters(fw, classe);
            WriteEquals(fw, classe);
            if (_config.EnumShortcutMode)
            {
                WriteEnumShortcuts(fw, classe);
            }

            WriteToMappers(fw, classe, availableClasses);

            if (_config.FieldsEnum && classe.IsPersistent)
            {
                WriteFieldsEnum(fw, classe);
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
        foreach (var ap in classe.Properties.OfType<AssociationProperty>().Where(ap => ap.Association.Reference && (ap.Type == AssociationType.OneToOne || ap.Type == AssociationType.ManyToOne)))
        {
            {
                var propertyName = ap.Name.ToFirstLower();
                fw.WriteLine();
                fw.WriteDocStart(1, $"Set the value of {{@link {classe.GetImport(_config)}#{propertyName} {propertyName}}}");
                fw.WriteLine(1, " * Cette méthode permet définir la valeur de la FK directement");
                fw.WriteLine(1, $" * @param {propertyName} value to set");
                fw.WriteDocEnd(1);
                fw.WriteLine(1, @$"public void set{ap.Name}({ap.Association.PrimaryKey!.GetJavaType()} {propertyName}) {{");
                fw.WriteLine(2, $"if({propertyName} != null) {{");
                var constructorArgs = $"{propertyName}";
                foreach (var p in ap.Association.Properties.Where(pr => !pr.PrimaryKey))
                {
                    constructorArgs += $", {propertyName}.get{p.Name}()";
                }

                fw.WriteLine(3, @$"this.{ap.GetAssociationName()} = new {ap.Association.Name}({constructorArgs});");
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
                fw.WriteLine(1, @$"public {ap.Association.PrimaryKey!.GetJavaType()} get{ap.Name}() {{");
                fw.WriteLine(2, @$"return this.{ap.GetAssociationName()} != null ? this.{ap.GetAssociationName()}.get{ap.Association.PrimaryKey!.Name}() : null;");
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
                ? refValue.Value[classe.PrimaryKey ?? classe.Properties.OfType<IFieldProperty>().First()]
                : refValue.Value[classe.PrimaryKey ?? classe.UniqueKeys.First().Single()];

            if (classe.Properties.Count > 1)
            {
                var lineToWrite = @$"{code.ToUpper()}";
                lineToWrite += $"({string.Join(", ", classe.Properties.Where(p => !p.PrimaryKey).Select(prop => (((IFieldProperty)prop).Domain.Java!.Type == "String" ? "\"" : string.Empty) + refValue.Value[(IFieldProperty)prop] + (((IFieldProperty)prop).Domain.Java!.Type == "String" ? "\"" : string.Empty)))})";
                lineToWrite += i == classe.ReferenceValues.Count ? "; " : ", //";
                fw.WriteLine(2, lineToWrite);
            }
            else
            {
                fw.WriteLine(2, @$"{code.ToUpper()}{(i == classe.ReferenceValues.Count ? ";" : ", //")}");
            }
        }

        foreach (var prop in classe.Properties.Where(p => !p.PrimaryKey))
        {
            fw.WriteLine();
            fw.WriteDocStart(2, ((IFieldProperty)prop).Comment);
            fw.WriteDocEnd(2);
            fw.WriteLine(2, $"private final {((IFieldProperty)prop).Domain.Java!.Type} {prop.Name.ToFirstLower()};");
        }

        if (classe.Properties.Count > 1)
        {
            fw.WriteLine();
            fw.WriteDocStart(2, "All arg constructor");
            fw.WriteDocEnd(2);
            var propertiesSignature = string.Join(", ", classe.Properties.Where(p => !p.PrimaryKey).Select(p => $"{p.GetJavaType()} {p.GetJavaName()}"));

            fw.WriteLine(2, $"private Values({propertiesSignature}) {{");
            foreach (var property in classe.Properties.Where(p => !p.PrimaryKey))
            {
                fw.WriteLine(3, $"this.{property.GetJavaName()} = {property.GetJavaName()};");
            }

            fw.WriteLine(2, $"}}");
        }

        foreach (var prop in classe.Properties.Where(p => !p.PrimaryKey))
        {
            fw.WriteLine();
            fw.WriteDocStart(2, ((IFieldProperty)prop).Comment);
            fw.WriteDocEnd(2);
            fw.WriteLine(2, $"public {prop.GetJavaType()} get{prop.Name}(){{");
            fw.WriteLine(3, $"return this.{prop.Name.ToFirstLower()};");
            fw.WriteLine(2, $"}}");
        }

        fw.WriteLine(1, "}");
    }

    private void WriteImports(JavaWriter fw, Class classe)
    {
        var imports = classe.GetImports(_config);
        imports.AddRange(classe.Decorators.SelectMany(d => d.Java!.Imports));
        foreach (var property in classe.Properties)
        {
            imports.AddRange(property.GetImports(_config));
        }

        if (classe.Extends != null)
        {
            foreach (var property in classe.Extends.Properties)
            {
                imports.AddRange(property.GetImports(_config));
            }
        }

        if (classe.Decorators.Any(d => d.Java != null && d.Java.GenerateInterface))
        {
            var entityDto = classe.IsPersistent ? "entities" : "dtos";
            imports.Add($"{string.Join('.', classe.GetImport(_config).Split('.').SkipLast(1))}.interfaces.I{classe.Name}");
        }

        if (_config.FieldsEnum && _config.FieldsEnumInterface != null && classe.IsPersistent && classe.Properties.Count > 0)
        {
            imports.Add(_config.FieldsEnumInterface.Replace("<>", string.Empty));
        }

        if (_config.EnumShortcutMode && classe.Properties.Where(p => p is AssociationProperty apo && apo.IsEnum()).Any())
        {
            {
                imports.Add($"javax.persistence.Transient");
            }
        }

        fw.WriteImports(imports.Distinct().ToArray());
    }

    private void CheckClass(Class classe)
    {
        foreach (var property in classe.Properties.OfType<CompositionProperty>())
        {
            if (!classe.IsPersistent && property.Composition.IsPersistent)
            {
                throw new ModelException(property, $"La propriété ${property} persistée ne peut pas faire l'objet d'une composition dans la classe {classe.Name} car elle ne l'est pas");
            }
        }

        foreach (var property in classe.Properties.OfType<AssociationProperty>())
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
            fw.WriteLine("@Cache(usage = CacheConcurrencyStrategy.READ_ONLY)");
            fw.WriteLine("@Immutable");
        }

        foreach (var a in classe.Decorators.SelectMany(d => d.Java!.Annotations).Distinct())
        {
            fw.WriteLine($"{(a.StartsWith("@") ? string.Empty : "@")}{a}");
        }
    }

    private void WriteProperties(JavaWriter fw, Class classe)
    {
        foreach (var property in classe.Properties)
        {
            fw.WriteLine();
            fw.WriteDocStart(1, property.Comment);
            if (property is AssociationProperty ap)
            {
                fw.WriteDocEnd(1);
                var fk = (ap.Role is not null ? ModelUtils.ConvertCsharp2Bdd(ap.Role) + "_" : string.Empty) + ap.Association.PrimaryKey!.SqlName;
                var apk = ap.Association.PrimaryKey.SqlName;
                var pk = classe.PrimaryKey!.SqlName;
                switch (ap.Type)
                {
                    case AssociationType.ManyToOne:
                        fw.WriteLine(1, @$"@{ap.Type}(fetch = FetchType.LAZY, optional = {(ap.Required ? "false" : "true")}, targetEntity = {ap.Association.Name}.class)");
                        fw.WriteLine(1, @$"@JoinColumn(name = ""{fk}"", referencedColumnName = ""{apk}"")");
                        break;
                    case AssociationType.OneToMany:
                        {
                            var inversePropsOneToMany = ap.Association.Properties.Where(p => p is AssociationProperty pap && pap.Association == classe && pap.Type == AssociationType.ManyToOne && (ap.Role is null || ap.Role == pap.Role));
                            var isInversed = inversePropsOneToMany.Count() == 1;
                            fw.WriteLine(1, @$"@{ap.Type}(cascade=CascadeType.ALL, orphanRemoval = true, fetch = FetchType.LAZY{(isInversed ? $@", mappedBy = ""{inversePropsOneToMany.First().GetJavaName()}""" : string.Empty)})");
                            if (!isInversed)
                            {
                                fw.WriteLine(1, @$"@JoinColumn(name = ""{pk}"", referencedColumnName = ""{pk}"")");
                            }

                            break;
                        }

                    case AssociationType.ManyToMany:
                        {
                            var assoNameList = new List<string>();
                            assoNameList.Add(ap.Class.SqlName);
                            assoNameList.Add(ap.Association.SqlName);
                            assoNameList.Sort();
                            var inversePropsManyToMany = ap.Association.Properties.Where(p => p is AssociationProperty pap && pap.Association == classe && pap.Type == AssociationType.ManyToMany && (ap.Role is null || ap.Role == pap.Role));
                            var isInversed = inversePropsManyToMany.Count() == 1 && assoNameList.First() == ap.Class.SqlName;
                            fw.WriteLine(1, @$"@{ap.Type}(fetch = FetchType.LAZY{(isInversed ? $@", mappedBy = ""{inversePropsManyToMany.First().GetJavaName()}"", cascade={{ javax.persistence.CascadeType.PERSIST, javax.persistence.CascadeType.MERGE }}" : string.Empty)})");
                            if (!isInversed)
                            {
                                fw.WriteLine(1, @$"@JoinTable(name = ""{string.Join('_', assoNameList)}"", joinColumns = @JoinColumn(name = ""{pk}""), inverseJoinColumns = @JoinColumn(name = ""{fk}""))");
                            }

                            break;
                        }

                    case AssociationType.OneToOne:
                        fw.WriteLine(1, @$"@{ap.Type}(fetch = FetchType.LAZY, cascade = CascadeType.ALL, orphanRemoval = true, optional = {(ap.Required ? "false" : "true")})");
                        fw.WriteLine(1, @$"@JoinColumn(name = ""{fk}"", referencedColumnName = ""{apk}"", unique = true)");
                        break;
                }

                fw.WriteLine(1, $"private {ap.GetJavaType()} {ap.GetAssociationName()};");
            }
            else if (property is CompositionProperty cp)
            {
                fw.WriteDocEnd(1);
                fw.WriteLine(1, $"private {property.GetJavaType()} {property.Name.ToFirstLower()};");
            }
            else if (property is IFieldProperty field)
            {
                if (property is AliasProperty alp)
                {
                    fw.WriteLine(1, $" * Alias of {{@link {alp.Property.Class.GetImport(_config)}#get{alp.Property.Name.ToFirstUpper()}() {alp.Property.Class.Name}#get{alp.Property.Name.ToFirstUpper()}()}} ");
                }

                fw.WriteDocEnd(1);
                if (field.PrimaryKey && classe.IsPersistent)
                {
                    fw.WriteLine(1, "@Id");
                    if (
                        field.Domain.Java!.Type == "Long"
                        || field.Domain.Java.Type == "long"
                        || field.Domain.Java.Type == "int"
                        || field.Domain.Java.Type == "Integer")
                    {
                        var seqName = $"SEQ_{classe.SqlName}";
                        fw.WriteLine(1, @$"@SequenceGenerator(name = ""{seqName}"", sequenceName = ""{seqName}"", initialValue = 1000)");
                        fw.WriteLine(1, @$"@GeneratedValue(strategy = GenerationType.SEQUENCE, generator = ""{seqName}"")");
                    }
                }

                if (classe.IsPersistent)
                {
                    var column = @$"@Column(name = ""{field.SqlName}"", nullable = {(!field.Required).ToString().ToFirstLower()}{(classe.Reference ? ", updatable = false" : string.Empty)}";
                    if (field.Domain.Length != null)
                    {
                        if (field.Domain.Java!.Type == "String" || field.Domain.Java.Type == "string")
                        {
                            column += $", length = {field.Domain.Length}";
                        }
                        else
                        {
                            column += $", precision = {field.Domain.Length}";
                        }
                    }

                    if (field.Domain.Scale != null)
                    {
                        column += $", scale = {field.Domain.Scale}";
                    }

                    column += ")";
                    fw.WriteLine(1, column);
                }
                else if (field.Required && !field.PrimaryKey)
                {
                    fw.WriteLine(1, @$"@NotNull");
                }

                if (field.PrimaryKey && classe.Reference)
                {
                    fw.WriteLine(1, "@Enumerated(EnumType.STRING)");
                }

                if (field.Domain.Java is not null && field.Domain.Java.Annotations is not null)
                {
                    foreach (var annotation in field.Domain.Java.Annotations)
                    {
                        fw.WriteLine(1, $"{(annotation.StartsWith("@") ? string.Empty : '@')}{annotation}");
                    }
                }

                fw.WriteLine(1, $"private {property.GetJavaType()} {property.GetJavaName()};");
            }
        }
    }

    private IList<IProperty> GetAllArgsProperties(Class classe)
    {
        if (classe.Extends is null)
        {
            return classe.Properties;
        }
        else
        {
            return GetAllArgsProperties(classe.Extends).Concat(classe.Properties).ToList();
        }
    }

    private void WriteGetters(JavaWriter fw, Class classe)
    {
        foreach (var property in classe.Properties.Where(p => !_config.EnumShortcutMode || !(p is AssociationProperty apo && apo.Association.Reference && (apo.Type == AssociationType.OneToOne || apo.Type == AssociationType.ManyToOne))))
        {
            fw.WriteLine();
            fw.WriteDocStart(1, $"Getter for {property.GetJavaName()}");
            fw.WriteReturns(1, $"value of {{@link {classe.GetImport(_config)}#{property.GetJavaName()} {property.GetJavaName()}}}");
            fw.WriteDocEnd(1);
            if (classe.Decorators.Any(d => d.Java != null && d.Java.GenerateInterface))
            {
                fw.WriteLine(1, "@Override");
            }

            fw.WriteLine(1, @$"{((property is AssociationProperty apo && apo.Association.Reference) ? "protected" : "public")} {property.GetJavaType()} get{property.GetJavaName().ToFirstUpper()}() {{");
            if (property is AssociationProperty ap && (ap.Type == AssociationType.ManyToMany || ap.Type == AssociationType.OneToMany))
            {
                fw.WriteLine(2, $"if(this.{property.GetJavaName()} == null)");
                fw.WriteLine(3, "return Collections.emptyList();");
            }

            fw.WriteLine(2, @$"return this.{property.GetJavaName()};");
            fw.WriteLine(1, "}");
        }
    }

    private void WriteEquals(JavaWriter fw, Class classe)
    {
        if (classe.IsPersistent)
        {
            var pk = classe.PrimaryKey!;
            fw.WriteLine();
            fw.WriteDocStart(1, $"Equal function comparing {pk.Name}");
            fw.WriteDocEnd(1);
            fw.WriteLine(1, $@"public boolean equals(Object o) {{");
            fw.WriteLine(2, $"if(o instanceof {classe.Name} {classe.Name.ToFirstLower()}) {{");
            fw.WriteLine(3, $"if(this == {classe.Name.ToFirstLower()})");
            fw.WriteLine(4, $"return true;");
            fw.WriteLine();
            fw.WriteLine(3, $"if({classe.Name.ToFirstLower()} == null || this.get{pk.Name}() == null)");
            fw.WriteLine(4, $"return false;");
            fw.WriteLine();
            fw.WriteLine(3, $"return this.get{pk.Name}().equals({classe.Name.ToFirstLower()}.get{pk.Name}());");
            fw.WriteLine(2, "}");
            fw.WriteLine(2, $"return false;");
            fw.WriteLine(1, "}");
        }
    }

    private void WriteToMappers(JavaWriter fw, Class classe, List<Class> availableClasses)
    {
        var toMappers = classe.ToMappers.Where(p => availableClasses.Contains(p.Class)).Select(m => (classe, m))
        .OrderBy(m => m.m.Name)
        .ToList();

        foreach (var toMapper in toMappers)
        {
            var (clazz, mapper) = toMapper;

            fw.WriteLine();
            fw.WriteDocStart(1, $"Mappe '{classe}' vers '{mapper.Class}'");
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
                if (mapping.Value is AssociationProperty ap)
                {
                    if (ap.Association.PrimaryKey!.IsEnum() && _config.EnumShortcutMode)
                    {
                        fw.WriteLine(2, $"dest.set{mapping.Value.Name.ToFirstUpper()}(this.get{mapping.Key.Name.ToFirstUpper()}());");
                    }
                    else if (mapping.Value.Class.IsPersistent && mapping.Key.Class.IsPersistent)
                    {
                        fw.WriteLine(2, $"dest.set{mapping.Value.GetJavaName().ToFirstUpper()}(this.get{mapping.Key.GetJavaName().ToFirstUpper()}());");
                    }
                }
                else
                {
                    fw.WriteLine(2, $"dest.set{mapping.Value.GetJavaName().ToFirstUpper()}(this.get{mapping.Key.GetJavaName().ToFirstUpper()}());");
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
        foreach (var property in classe.Properties.Where(p => !_config.EnumShortcutMode || !(p is AssociationProperty apo && apo.Association.Reference && (apo.Type == AssociationType.OneToOne || apo.Type == AssociationType.ManyToOne))))
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

    private void WriteFieldsEnum(JavaWriter fw, Class classe)
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
        fw.WriteLine(string.Join(", //\n", classe.Properties.Select(prop =>
        {
            string name;
            if (prop is AssociationProperty ap)
            {
                name = ModelUtils.ConvertCsharp2Bdd(ap.GetAssociationName());
            }
            else
            {
                name = ModelUtils.ConvertCsharp2Bdd(prop.Name);
            }

            return $"        {name}";
        })));

        fw.WriteLine(1, "}");
    }
}