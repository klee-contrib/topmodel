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
    private readonly IDictionary<string, ModelFile> _files = new Dictionary<string, ModelFile>();

    public JpaModelGenerator(ILogger<JpaModelGenerator> logger, JpaConfig config)
        : base(logger, config)
    {
        _config = config;
        _logger = logger;
    }

    public override string Name => "JpaModelGen";

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

    private void GenerateModule(string module)
    {
        var classes = _files.Values
            .SelectMany(f => f.Classes)
            .Distinct()
            .Where(c => c.Namespace.Module == module);

        foreach (var classe in classes)
        {
            var entityDto = classe.IsPersistent ? "entities" : "dtos";
            var destFolder = Path.Combine(_config.ModelOutputDirectory, Path.Combine(_config.DaoPackageName.Split(".")), entityDto, classe.Namespace.Module.Replace(".", "/").ToLower());
            var dirInfo = Directory.CreateDirectory(destFolder);
            var packageName = $"{_config.DaoPackageName}.{entityDto}.{classe.Namespace.Module.ToLower()}";
            using var fw = new JavaWriter($"{destFolder}/{classe.Name}.java", _logger, null);
            fw.WriteLine($"package {packageName};");

            WriteImports(fw, classe);
            fw.WriteLine();

            WriteAnnotations(module, fw, classe);

            fw.WriteClassDeclaration(classe.Name, null, classe.Extends != null ? classe.Extends.Name : null, "Serializable");

            fw.WriteLine("	/** Serial ID */");
            fw.WriteLine("    private static final long serialVersionUID = 1L;");
            fw.WriteLine();
            WriteProperties(fw, classe);
            WriteGetters(fw, classe);
            if (_config.FieldsEnum && classe.IsPersistent)
            {
                WriteFieldsEnum(fw, classe);
            }
            if (classe.Reference && classe.ReferenceValues != null && classe.ReferenceValues.Count() > 0)
            {
                WriteReferenceValues(fw, classe);
            }

            fw.WriteLine("}");
        }
    }

    private void WriteReferenceValues(JavaWriter fw, Class classe)
    {
        fw.WriteLine();
        if (classe.Properties.Count > 1)
        {
            fw.WriteLine(1, "@AllArgsConstructor");
            fw.WriteLine(1, "@Getter");
        }

        fw.WriteLine(1, $"public enum Values {{");

        var i = 0;

        foreach (var refValue in classe.ReferenceValues!.OrderBy(x => x.Name, StringComparer.Ordinal))
        {
            ++i;
            var code = classe.PrimaryKey?.Domain.Name != "DO_ID"
                ? (string)refValue.Value[classe.PrimaryKey ?? classe.Properties.OfType<IFieldProperty>().First()]
                : (string)refValue.Value[classe.UniqueKeys!.First().First()];
            var label = classe.LabelProperty != null
                ? (string)refValue.Value[classe.LabelProperty]
                : refValue.Name;

            if (classe.Properties.Count > 1)
            {
                var lineToWrite = @$"{code.ToUpper()}";
                lineToWrite += $"({string.Join(", ", classe.Properties.Where(p => !p.PrimaryKey).Select(prop => (((IFieldProperty)prop).Domain.Java!.Type == "String" ? "\"" : string.Empty) + refValue.Value[(IFieldProperty)prop] + (((IFieldProperty)prop).Domain.Java!.Type == "String" ? "\"" : string.Empty)))})";
                lineToWrite += i == classe.ReferenceValues!.Count ? "; " : ", //";
                fw.WriteLine(2, lineToWrite);
            }
            else
            {
                fw.WriteLine(2, @$"{code.ToUpper()}{(i == classe.ReferenceValues!.Count ? ";" : ", //")}");
            }
        }

        foreach (var prop in classe.Properties.Where(p => !p.PrimaryKey))
        {
            fw.WriteLine();
            fw.WriteDocStart(2, ((IFieldProperty)prop).Comment);
            fw.WriteDocEnd(2);
            fw.WriteLine(2, $"private final {((IFieldProperty)prop).Domain.Java!.Type} {prop.Name.ToFirstLower()};");
        }

        fw.WriteLine(1, "}");
    }

    private void WriteImports(JavaWriter fw, Class classe)
    {
        var imports = classe.GetImports(_config);
        foreach (var property in classe.Properties)
        {
            imports.AddRange(property.GetImports(_config));
        }

        if (_config.FieldsEnum && _config.FieldsEnumInterface != null && classe.IsPersistent && classe.Properties.Count > 0)
        {
            imports.Add(_config.FieldsEnumInterface.Replace("<>", string.Empty));
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
        fw.WriteLine("@SuperBuilder");
        fw.WriteLine("@Setter");
        fw.WriteLine("@NoArgsConstructor");
        fw.WriteLine("@AllArgsConstructor");
        fw.WriteLine($"@EqualsAndHashCode{(classe.IsPersistent && classe.PrimaryKey != null ? @$"(of = {{ ""{classe.PrimaryKey!.Name.ToFirstLower()}"" }})" : string.Empty)}");
        fw.WriteLine("@ToString");
        fw.WriteLine("@Generated(\"TopModel : https://github.com/klee-contrib/topmodel\")");

        if (classe.IsPersistent)
        {
            var table = @$"@Table(name = ""{classe.SqlName}""";
            if (classe.UniqueKeys?.Count > 0)
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

        if (classe.Properties.Any(property =>
            property is IFieldProperty t && t.Domain.Java?.Annotations is not null && t.Domain.Java.Annotations.Any(a => a == "@CreatedDate" || a == "@UpdatedDate")))
        {
            fw.WriteLine("@EntityListeners(AuditingEntityListener.class)");
        }
    }

    private void WriteProperties(JavaWriter fw, Class classe)
    {
        foreach (var property in classe.Properties)
        {
            if (property is AssociationProperty ap)
            {
                fw.WriteLine(1, $"private {ap.GetJavaType()} {ap.GetAssociationName()};");
            }
            else
            {
                fw.WriteLine(1, $"private {property.GetJavaType()} {property.Name.ToFirstLower()};");
            }
        }
    }

    private void WriteGetters(JavaWriter fw, Class classe)
    {
        foreach (var property in classe.Properties)
        {
            fw.WriteLine();
            fw.WriteDocStart(1, property.Comment);

            if (property is AssociationProperty ap)
            {
                fw.WriteReturns(1, $"value of {ap.GetAssociationName()}");
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
                        fw.WriteLine(1, @$"@{ap.Type}(cascade=CascadeType.ALL, orphanRemoval = true, fetch = FetchType.LAZY)");
                        fw.WriteLine(1, @$"@JoinColumn(name = ""{pk}"", referencedColumnName = ""{pk}"")");
                        break;
                    case AssociationType.ManyToMany:
                        fw.WriteLine(1, @$"@{ap.Type}(fetch = FetchType.LAZY)");
                        fw.WriteLine(1, @$"@JoinTable(name = ""{ap.Class.SqlName}_{ap.Association.SqlName}"", joinColumns = @JoinColumn(name = ""{pk}""), inverseJoinColumns = @JoinColumn(name = ""{fk}""))");
                        break;
                    case AssociationType.OneToOne:
                        fw.WriteLine(1, @$"@{ap.Type}(fetch = FetchType.LAZY, cascade = CascadeType.ALL, orphanRemoval = true, optional = {(ap.Required ? "false" : "true")})");
                        fw.WriteLine(1, @$"@JoinColumn(name = ""{fk}"", referencedColumnName = ""{apk}"", unique = true)");
                        break;
                }

                fw.WriteLine(1, @$"public {ap.GetJavaType()} get{ap.GetAssociationName().ToFirstUpper()}() {{");
                if (ap.Type == AssociationType.OneToMany || ap.Type == AssociationType.ManyToMany)
                {
                    fw.WriteLine(2, @$"if({ap.GetAssociationName()} == null) this.{ap.GetAssociationName()} = Collections.emptyList();");
                }

                fw.WriteLine(2, @$"return this.{ap.GetAssociationName()};");
            }
            else if (property is CompositionProperty cp)
            {
                fw.WriteReturns(1, $"value of {cp.Composition.Name}");
                fw.WriteDocEnd(1);
                string methodName = @$"get{cp.Name.ToFirstUpper()}";
                fw.WriteLine(1, @$"public {cp.GetJavaType()} {methodName}() {{");

                if (cp.Kind == "list")
                {
                    fw.WriteLine(2, @$"if({cp.Name.ToFirstLower()} == null) this.{cp.Name.ToFirstLower()} = java.util.Collections.emptyList();");
                }

                fw.WriteLine(2, @$"return this.{cp.Name.ToFirstLower()};");
            }
            else if (property is IFieldProperty field)
            {
                if (property is AliasProperty alp)
                {
                    fw.WriteLine(1, $" * Alias of {{@link {alp.Property.Class.GetImport(_config)}#get{alp.Property.Name.ToFirstUpper()}() {alp.Property.Class.Name}#get{alp.Property.Name.ToFirstUpper()}()}} ");
                }

                fw.WriteReturns(1, $"value of {property.Name.ToFirstLower()}");

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
                        fw.WriteLine(1, @$"@SequenceGenerator(name = ""{seqName}"", sequenceName = ""{seqName}"",  initialValue = 1000, allocationSize = 1)");
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
                        fw.WriteLine(1, annotation);
                    }
                }

                fw.WriteLine(1, @$"public {field.GetJavaType()} get{field.Name.ToFirstUpper()}() {{");
                fw.WriteLine(2, @$" return this.{property.Name.ToFirstLower()};");
            }

            fw.WriteLine(1, "}");
        }
    }

    private void WriteFieldsEnum(JavaWriter fw, Class classe)
    {
        fw.WriteLine();
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

            return $"         {name}";
        })));

        fw.WriteLine(1, "}");
    }
}