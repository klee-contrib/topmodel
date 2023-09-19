using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Core.Model.Implementation;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Jpa;

/// <summary>
/// Générateur de fichiers de modèles JPA.
/// </summary>
public class JpaModelGenerator : ClassGeneratorBase<JpaConfig>
{
    private readonly ILogger<JpaModelGenerator> _logger;
    private readonly ModelConfig _modelConfig;

    private readonly Dictionary<string, string> _newableTypes = new()
    {
        ["List"] = "ArrayList",
        ["Set"] = "HashSet"
    };

    private JpaModelConstructorGenerator? _jpaModelConstructorGenerator;
    private JpaModelPropertyGenerator? _jpaModelPropertyGenerator;

    public JpaModelGenerator(ILogger<JpaModelGenerator> logger, ModelConfig modelConfig)
        : base(logger)
    {
        _logger = logger;
        _modelConfig = modelConfig;
    }

    public override string Name => "JpaModelGen";

    private List<Class> AvailableClasses => Classes.ToList();

    private JpaModelConstructorGenerator JpaModelConstructorGenerator
    {
        get
        {
            _jpaModelConstructorGenerator ??= new JpaModelConstructorGenerator(Config);
            return _jpaModelConstructorGenerator;
        }
    }

    private JpaModelPropertyGenerator JpaModelPropertyGenerator
    {
        get
        {
            _jpaModelPropertyGenerator ??= new JpaModelPropertyGenerator(Config, Classes);
            return _jpaModelPropertyGenerator;
        }
    }

    protected override bool FilterClass(Class classe)
    {
        return !classe.Abstract;
    }

    protected override string GetFileName(Class classe, string tag)
    {
        return Config.GetClassFileName(classe, tag);
    }

    protected override void HandleClass(string fileName, Class classe, string tag)
    {
        CheckClass(classe);

        var packageName = Config.GetPackageName(classe, tag);
        using var fw = new JavaWriter(fileName, _logger, packageName, null);

        WriteImports(fw, classe, tag);
        fw.WriteLine();

        WriteAnnotations(fw, classe, tag);

        var extends = Config.GetClassExtends(classe);
        var implements = Config.GetClassImplements(classe).ToList();

        if (!classe.IsPersistent)
        {
            implements.Add("Serializable");
            fw.AddImport("java.io.Serializable");
        }

        fw.WriteClassDeclaration(classe.NamePascal, null, extends, implements);

        if (!classe.IsPersistent)
        {
            fw.WriteLine("	/** Serial ID */");
            fw.WriteLine(1, "private static final long serialVersionUID = 1L;");
        }

        JpaModelPropertyGenerator.WriteProperties(fw, classe, tag);
        JpaModelPropertyGenerator.WriteCompositePrimaryKeyClass(fw, classe);
        JpaModelConstructorGenerator.WriteNoArgConstructor(fw, classe);
        if (Config.MappersInClass)
        {
            JpaModelConstructorGenerator.WriteFromMappers(fw, classe, AvailableClasses, tag);
        }

        if (Config.CanClassUseEnums(classe, Classes))
        {
            JpaModelConstructorGenerator.WriteEnumConstructor(fw, classe, AvailableClasses, tag, _modelConfig);
        }

        WriteGetters(fw, classe, tag);
        WriteSetters(fw, classe, tag);
        if (!Config.UseJdbc)
        {
            WriteAdders(fw, classe, tag);
            WriteRemovers(fw, classe, tag);
            if (Config.EnumShortcutMode)
            {
                WriteEnumShortcuts(fw, classe, tag);
            }
        }

        if (Config.MappersInClass)
        {
            WriteToMappers(fw, classe, tag);
        }

        if ((Config.FieldsEnum & Target.Persisted) > 0 && classe.IsPersistent
            || (Config.FieldsEnum & Target.Dto) > 0 && !classe.IsPersistent)
        {
            if (Config.FieldsEnumInterface != null)
            {
                fw.AddImport(Config.FieldsEnumInterface.Replace("<>", string.Empty));
            }

            WriteFieldsEnum(fw, classe, tag);
        }

        fw.WriteLine("}");
    }

    private void CheckClass(Class classe)
    {
        foreach (var property in classe.GetProperties(AvailableClasses).OfType<CompositionProperty>())
        {
            if (!classe.IsPersistent && property.Composition.IsPersistent)
            {
                throw new ModelException(property, $"La propriété ${property} persistée ne peut pas faire l'objet d'une composition dans la classe {classe.Name} car elle ne l'est pas");
            }
        }

        foreach (var property in classe.GetProperties(AvailableClasses).OfType<AssociationProperty>())
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

    private void WriteAdders(JavaWriter fw, Class classe, string tag)
    {
        if (classe.IsPersistent && Config.AssociationAdders)
        {
            foreach (var ap in classe.GetProperties(AvailableClasses).OfType<AssociationProperty>().Where(t => t.Type.IsToMany()))
            {
                var reverse = ap is ReverseAssociationProperty rap ? rap.ReverseProperty : ap.Association.GetProperties(AvailableClasses).OfType<ReverseAssociationProperty>().FirstOrDefault(r => r.ReverseProperty == ap);
                if (reverse != null)
                {
                    var propertyName = ap.NameByClassCamel;
                    fw.WriteLine();
                    fw.WriteDocStart(1, $"Add a value to {{@link {classe.GetImport(Config, tag)}#{propertyName} {propertyName}}}");
                    fw.WriteLine(1, $" * @param {ap.Association.NameCamel} value to add");
                    fw.WriteDocEnd(1);
                    fw.WriteLine(1, @$"public void add{ap.Association.NamePascal}{ap.Role}({ap.Association.NamePascal} {ap.Association.NameCamel}) {{");
                    fw.WriteLine(2, @$"this.{propertyName}.add({ap.Association.NameCamel});");
                    if (reverse.Type.IsToMany())
                    {
                        fw.WriteLine(2, @$"{ap.Association.NameCamel}.get{reverse.NameByClassPascal}().add(this);");
                    }
                    else
                    {
                        fw.WriteLine(2, @$"{ap.Association.NameCamel}.set{reverse.NameByClassPascal}(this);");
                    }

                    fw.WriteLine(1, "}");
                }
            }
        }
    }

    private void WriteAnnotations(JavaWriter fw, Class classe, string tag)
    {
        fw.WriteDocStart(0, classe.Comment);
        fw.WriteDocEnd(0);
        var javaOrJakarta = Config.PersistenceMode.ToString().ToLower();
        fw.AddImport($"{javaOrJakarta}.annotation.Generated");
        fw.WriteLine("@Generated(\"TopModel : https://github.com/klee-contrib/topmodel\")");

        if (classe.IsPersistent)
        {
            if (Config.UseJdbc)
            {
                var table = @$"@Table(name = ""{classe.SqlName.ToLower()}"")";
                fw.AddImport($"org.springframework.data.relational.core.mapping.Table");
                fw.WriteLine(table);
            }
            else
            {
                var table = @$"@Table(name = ""{classe.SqlName}""";
                fw.AddImport($"{javaOrJakarta}.persistence.Table");
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
                fw.AddImport($"{javaOrJakarta}.persistence.Entity");
                fw.WriteLine("@Entity");
                fw.WriteLine(table);
                if (classe.PrimaryKey.Count() > 1)
                {
                    fw.WriteLine($"@IdClass({classe.NamePascal}.{classe.NamePascal}Id.class)");
                    fw.AddImport($"{javaOrJakarta}.persistence.IdClass");
                }

                if (classe.Reference)
                {
                    fw.AddImports(new List<string>()
                {
                    "org.hibernate.annotations.Cache",
                    "org.hibernate.annotations.CacheConcurrencyStrategy"
                });
                    if (Config.CanClassUseEnums(classe))
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
            }
        }

        foreach (var a in Config.GetDecoratorAnnotations(classe))
        {
            fw.WriteLine($"{(a.StartsWith("@") ? string.Empty : "@")}{a}");
        }
    }

    private void WriteEnumShortcuts(JavaWriter fw, Class classe, string tag)
    {
        foreach (var ap in classe.GetProperties(AvailableClasses).OfType<AssociationProperty>().Where(ap => Config.CanClassUseEnums(ap.Association)))
        {
            fw.AddImport($"{Config.GetEnumPackageName(ap.Association, tag)}.{Config.GetEnumName(ap.Association)}");
            var isMultiple = ap.Type.IsToMany();
            {
                var propertyName = ap.NameCamel;
                fw.WriteLine();
                fw.WriteDocStart(1, $"Set the value of {{@link {classe.GetImport(Config, tag)}#{propertyName} {propertyName}}}");
                fw.WriteLine(1, " * Cette méthode permet définir la valeur de la FK directement");
                fw.WriteLine(1, $" * @param {propertyName} value to set");
                fw.WriteDocEnd(1);
                fw.WriteLine(1, @$"public void {(isMultiple ? ap.NameCamel + ap.Property.NamePascal : ap.NameCamel).WithPrefix("set")}({(isMultiple ? $"List<{Config.GetType(ap.Property)}>" : Config.GetType(ap.Property))} {propertyName}) {{");
                fw.WriteLine(2, $"if ({propertyName} != null) {{");
                if (!isMultiple)
                {
                    fw.WriteLine(3, @$"this.{ap.NameByClassCamel} = new {ap.Association.NamePascal}({propertyName});");
                }
                else
                {
                    var type = Config.GetType(ap, AvailableClasses, useClassForAssociation: classe.IsPersistent && !Config.UseJdbc).Split('<').First();

                    if (_newableTypes.TryGetValue(type, out var newableType))
                    {
                        fw.WriteLine(3, @$"if (this.{ap.NameByClassCamel} != null) {{");
                        fw.WriteLine(4, @$"this.{ap.NameByClassCamel}.clear();");
                        fw.WriteLine(3, "} else {");
                        fw.AddImport($"java.util.{newableType}");
                        fw.WriteLine(4, @$"this.{ap.NameByClassCamel} = new {newableType}<>();");
                        fw.WriteLine(3, "}");
                        fw.WriteLine(3, @$"this.{ap.NameByClassCamel}.addAll({propertyName}.stream().map({ap.Association.NamePascal}::new).collect(Collectors.to{type}()));");
                        fw.AddImport("java.util.stream.Collectors");
                    }
                }

                fw.WriteLine(2, "} else {");
                fw.WriteLine(3, @$"this.{ap.NameByClassCamel} = null;");
                fw.WriteLine(2, "}");
                fw.WriteLine(1, "}");
            }

            {
                fw.WriteLine();
                fw.WriteDocStart(1, $"Getter for {ap.NameCamel}");
                fw.WriteLine(1, " * Cette méthode permet de manipuler directement la foreign key de la liste de référence");
                fw.WriteReturns(1, $"value of {{@link {classe.GetImport(Config, tag)}#{ap.NameByClassCamel} {ap.NameByClassCamel}}}");
                fw.WriteDocEnd(1);
                fw.WriteLine(1, "@Transient");
                fw.AddImport(Config.PersistenceMode.ToString().ToLower() + ".persistence.Transient");
                fw.WriteLine(1, @$"public {(isMultiple ? $"List<{Config.GetType(ap.Property)}>" : Config.GetType(ap.Property))} get{(isMultiple ? ap.NameCamel.ToFirstUpper() + ap.Property.NameCamel.ToFirstUpper() : ap.NameCamel.ToFirstUpper())}() {{");
                if (!isMultiple)
                {
                    fw.WriteLine(2, @$"return this.{ap.NameByClassCamel} != null ? this.{ap.NameByClassCamel}.get{ap.Property.NameCamel.ToFirstUpper()}() : null;");
                }
                else
                {
                    var listOrSet = Config.GetType(ap, AvailableClasses, useClassForAssociation: classe.IsPersistent && !Config.UseJdbc).Split('<').First();
                    fw.WriteLine(2, @$"return this.{ap.NameByClassCamel} != null ? this.{ap.NameByClassCamel}.stream().map({ap.Association.NamePascal}::get{ap.Property.NameCamel.ToFirstUpper()}).collect(Collectors.to{listOrSet}()) : null;");
                    fw.AddImport("java.util.stream.Collectors");
                }

                fw.WriteLine(1, "}");
            }
        }
    }

    private void WriteFieldsEnum(JavaWriter fw, Class classe, string tag)
    {
        fw.WriteLine();
        fw.WriteDocStart(1, $"Enumération des champs de la classe {{@link {classe.GetImport(Config, tag)} {classe.NamePascal}}}");
        fw.WriteDocEnd(1);
        string enumDeclaration = @$"public enum Fields ";
        if (Config.FieldsEnumInterface != null)
        {
            enumDeclaration += $"implements {Config.FieldsEnumInterface.Split(".").Last().Replace("<>", $"<{classe.NamePascal}>")}";
        }

        enumDeclaration += " {";
        fw.WriteLine(1, enumDeclaration);

        var props = classe.GetProperties(AvailableClasses).Select(prop =>
        {
            string name;
            if (prop is AssociationProperty ap && !Config.UseJdbc)
            {
                name = ap.NameByClassCamel.ToConstantCase();
            }
            else
            {
                name = prop.Name.ToConstantCase();
            }

            var javaType = Config.GetType(prop, useClassForAssociation: classe.IsPersistent && !Config.UseJdbc);
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

    private void WriteGetters(JavaWriter fw, Class classe, string tag)
    {
        var properties = Config.UseJdbc ? classe.Properties : classe.GetProperties(AvailableClasses);
        foreach (var property in properties)
        {
            var propertyName = Config.UseJdbc ? property.NameCamel : property.NameByClassCamel;
            fw.WriteLine();
            fw.WriteDocStart(1, $"Getter for {propertyName}");
            fw.WriteReturns(1, $"value of {{@link {classe.GetImport(Config, tag)}#{propertyName} {propertyName}}}");
            fw.WriteDocEnd(1);

            var getterPrefix = Config.GetType(property, Classes, true) == "boolean" ? "is" : "get";
            fw.WriteLine(1, @$"public {Config.GetType(property, useClassForAssociation: classe.IsPersistent && !Config.UseJdbc)} {propertyName.ToPascalCase().WithPrefix(getterPrefix)}() {{");
            if (property is AssociationProperty ap && ap.Type.IsToMany())
            {
                var type = Config.GetType(ap, AvailableClasses, useClassForAssociation: classe.IsPersistent && !Config.UseJdbc).Split('<').First();
                if (_newableTypes.TryGetValue(type, out var newableType))
                {
                    fw.WriteLine(2, $"if(this.{propertyName} == null)");
                    fw.AddImport($"java.util.{newableType}");
                    fw.WriteLine(3, $"this.{propertyName} = new {newableType}<>();");
                }
            }

            fw.WriteLine(2, @$"return this.{propertyName};");
            fw.WriteLine(1, "}");
        }
    }

    private void WriteImports(JavaWriter fw, Class classe, string tag)
    {
        var imports = classe.GetImports(Config, tag, AvailableClasses);
        imports.AddRange(Config.GetDecoratorImports(classe));
        foreach (var property in classe.GetProperties(AvailableClasses))
        {
            imports.AddRange(property.GetTypeImports(Config, tag));
        }

        fw.AddImports(imports);
    }

    private void WriteRemovers(JavaWriter fw, Class classe, string tag)
    {
        if (classe.IsPersistent && Config.AssociationRemovers)
        {
            foreach (var ap in classe.GetProperties(AvailableClasses).OfType<AssociationProperty>().Where(t => t.Type.IsToMany()))
            {
                var reverse = ap is ReverseAssociationProperty rap ? rap.ReverseProperty : ap.Association.GetProperties(AvailableClasses).OfType<ReverseAssociationProperty>().FirstOrDefault(r => r.ReverseProperty == ap);
                if (reverse != null)
                {
                    var propertyName = ap.NameByClassCamel;
                    fw.WriteLine();
                    fw.WriteDocStart(1, $"Remove a value from {{@link {classe.GetImport(Config, tag)}#{propertyName} {propertyName}}}");
                    fw.WriteLine(1, $" * @param {ap.Association.NameCamel} value to remove");
                    fw.WriteDocEnd(1);
                    fw.WriteLine(1, @$"public void remove{ap.Association.NamePascal}{ap.Role}({ap.Association.NamePascal} {ap.Association.NameCamel}) {{");
                    fw.WriteLine(2, @$"this.{propertyName}.remove({ap.Association.NameCamel});");
                    if (reverse.Type.IsToMany())
                    {
                        fw.WriteLine(2, @$"{ap.Association.NameCamel}.get{reverse.NameByClassPascal}().remove(this);");
                    }
                    else
                    {
                        fw.WriteLine(2, @$"{ap.Association.NameCamel}.set{reverse.NameByClassPascal}(null);");
                    }

                    fw.WriteLine(1, "}");
                }
            }
        }
    }

    private void WriteSetters(JavaWriter fw, Class classe, string tag)
    {
        var properties = Config.UseJdbc ? classe.Properties : classe.GetProperties(AvailableClasses);
        foreach (var property in properties)
        {
            var propertyName = Config.UseJdbc ? property.NameCamel : property.NameByClassCamel;
            fw.WriteLine();
            fw.WriteDocStart(1, $"Set the value of {{@link {classe.GetImport(Config, tag)}#{propertyName} {propertyName}}}");
            fw.WriteLine(1, $" * @param {propertyName} value to set");
            fw.WriteDocEnd(1);
            fw.WriteLine(1, @$"public void {propertyName.WithPrefix("set")}({Config.GetType(property, useClassForAssociation: classe.IsPersistent && !Config.UseJdbc)} {propertyName}) {{");
            fw.WriteLine(2, @$"this.{propertyName} = {propertyName};");
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
            fw.WriteDocStart(1, $"Mappe '{classe}' vers '{mapper.Class.NamePascal}'");
            if (mapper.Comment != null)
            {
                fw.WriteLine(1, $" * {mapper.Comment}");
            }

            fw.WriteParam("target", $"Instance pré-existante de '{mapper.Class.NamePascal}'. Une nouvelle instance sera créée si non spécifié.");
            fw.WriteReturns(1, $"Une instance de '{mapper.Class.NamePascal}'");

            fw.WriteDocEnd(1);
            if (mapper.ParentMapper != null && mapper.ParentMapper.Name == mapper.Name)
            {
                fw.WriteLine(1, $"@Override");
            }

            var (mapperNs, mapperModelPath) = Config.GetMapperLocation(toMapper);

            fw.WriteLine(1, $"public {mapper.Class.NamePascal} {mapper.Name.Value.ToCamelCase()}({mapper.Class.NamePascal} target) {{");
            fw.WriteLine(2, $"return {Config.GetMapperName(mapperNs, mapperModelPath)}.{mapper.Name.Value.ToCamelCase()}(this, target);");
            fw.AddImport(Config.GetMapperImport(mapperNs, mapperModelPath, tag)!);
            fw.WriteLine(1, "}");

            if (toMappers.IndexOf(toMapper) < toMappers.Count - 1)
            {
                fw.WriteLine();
            }
        }
    }
}