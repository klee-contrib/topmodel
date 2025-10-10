using Microsoft.Extensions.Logging;
using TopModel.Core.Model;
using TopModel.Core.Model.Implementation;
using TopModel.Core.Utils;
using TopModel.Utils;

namespace TopModel.Generator.Jpa.ClassGeneration;

/// <summary>
/// Générateur de fichiers de modèles JPA.
/// </summary>
public class JpaEntityGenerator(ILogger<JpaEntityGenerator> logger, IFileWriterProvider writerProvider)
    : JavaClassGeneratorBase(logger, writerProvider)
{
    public override string Name => "JpaEntityGen";

    protected override bool FilterClass(Class classe)
    {
        return !classe.Abstract && classe.IsPersistent && !Config.CanClassUseEnums(classe, Classes);
    }

    protected override IEnumerable<JavaAnnotation> GetAnnotations(Class classe, string tag)
    {
        foreach (var a in base.GetAnnotations(classe, tag))
        {
            yield return a;
        }

        yield return new JavaAnnotation("Entity", imports: $"{JavaxOrJakarta}.persistence.Entity");
        if (Classes.Any(c => c.Extends == classe))
        {
            yield return new JavaAnnotation(
                "Inheritance",
                imports: $"{JavaxOrJakarta}.persistence.Inheritance"
            ).AddAttribute("strategy", "InheritanceType.JOINED", $"{JavaxOrJakarta}.persistence.InheritanceType");
        }

        var tableAnnotation = new JavaAnnotation("Table", imports: $"{JavaxOrJakarta}.persistence.Table").AddAttribute(
            "name",
            $@"""{classe.SqlName}"""
        );
        if (classe.UniqueKeys.Count > 0)
        {
            var uks = classe.UniqueKeys.Select(uk =>
                new JavaAnnotation(
                    "UniqueConstraint",
                    imports: $"{JavaxOrJakarta}.persistence.UniqueConstraint"
                ).AddAttribute("columnNames", uk.Select(u => $@"""{u.SqlName}""").ToArray())
            );

            tableAnnotation.AddAttribute("uniqueConstraints", uks);
        }

        yield return tableAnnotation;
        if (classe.PrimaryKey.Count() > 1)
        {
            yield return new JavaAnnotation("IdClass", imports: $"{JavaxOrJakarta}.persistence.IdClass").AddAttribute(
                "value",
                $"{classe.NamePascal}.{classe.NamePascal}Id.class"
            );
        }

        if (classe.Reference)
        {
            var cacheAnnotation = new JavaAnnotation("Cache", imports: "org.hibernate.annotations.Cache");
            if (Config.CanClassUseEnums(classe))
            {
                yield return new JavaAnnotation("Immutable", imports: "org.hibernate.annotations.Immutable");
                cacheAnnotation.AddAttribute(
                    "usage",
                    "CacheConcurrencyStrategy.READ_ONLY",
                    "org.hibernate.annotations.CacheConcurrencyStrategy"
                );
            }
            else
            {
                cacheAnnotation.AddAttribute(
                    "usage",
                    "CacheConcurrencyStrategy.READ_WRITE",
                    "org.hibernate.annotations.CacheConcurrencyStrategy"
                );
            }

            yield return cacheAnnotation;
        }
    }

    protected override string GetFileName(Class classe, string tag)
    {
        return Path.Combine(
            Config.OutputDirectory,
            Config.ResolveVariables(Config.EntitiesPath, tag, module: classe.Namespace.Module).ToFilePath(),
            $"{classe.NamePascal}.java"
        );
    }

    protected override IEnumerable<JavaMethod> GetGetters(Class classe, string tag)
    {
        foreach (var getter in base.GetGetters(classe, tag))
        {
            yield return getter;
        }

        var mapIdGetter = GetMapIdPropertyGetter(classe, tag);
        if (mapIdGetter != null)
        {
            yield return mapIdGetter;
        }
    }

    protected override IEnumerable<JavaMethod> GetSetters(Class classe, string tag)
    {
        foreach (var setter in base.GetSetters(classe, tag))
        {
            yield return setter;
        }

        var mapIdSetter = GetMapIdPropertySetter(classe, tag);
        if (mapIdSetter != null)
        {
            yield return mapIdSetter;
        }
    }

    protected override IEnumerable<JavaField> GetFields(Class classe, string tag)
    {
        if (classe.PrimaryKey.Count() == 1 && classe.PrimaryKey.First() is AssociationProperty ap)
        {
            yield return new JavaField(JpaModelPropertyGenerator.GetPropertyType(ap.Property), ap.NameCamel)
            {
                Comment =
                {
                    @$"Identifiant technique mappé avec celui de la classe {{@link {ap.Association.GetImport(Config, tag)}}} {ap.Association.NamePascal}",
                },
            }.Add(JpaModelPropertyGenerator.IdAnnotation);
        }
        foreach (var field in JpaModelPropertyGenerator.GetProperties(classe, tag))
        {
            yield return field;
        }
    }

    protected virtual string GetterToCompareCompositePkPk(IProperty pk)
    {
        if (pk is AssociationProperty ap)
        {
            if (Config.EnumsAsEnums)
            {
                return string.Empty;
            }
            else
            {
                return $".{JpaModelPropertyGenerator.GetGetterName(ap.Property)}()";
            }
        }
        else if (pk is AliasProperty al && al.Property is AssociationProperty asp)
        {
            return $".get{JpaModelPropertyGenerator.GetGetterName(asp.Property)}()";
        }

        return string.Empty;
    }

    protected override void HandleClass(string fileName, Class classe, string tag)
    {
        var javaClass = base.InitClass(classe, tag);

        if (Config.AssociationAdders)
        {
            javaClass.Methods.AddRange(GetAdders(classe, tag));
        }
        if (Config.AssociationRemovers)
        {
            javaClass.Methods.AddRange(GetRemovers(classe, tag));
        }

        var packageName = Config.GetPackageName(classe, tag);
        using var fw = this.OpenJavaWriter(fileName, packageName, codePage: null);
        fw.Write(0, javaClass);
    }

    protected override IEnumerable<JavaMethod> GetMethods(Class classe, string tag)
    {
        foreach (var method in base.GetMethods(classe, tag))
        {
            yield return method;
        }
        if (Config.AssociationAdders)
        {
            foreach (var method in GetAdders(classe, tag))
            {
                yield return method;
            }
        }
        if (Config.AssociationRemovers)
        {
            foreach (var method in GetRemovers(classe, tag))
            {
                yield return method;
            }
        }
    }

    protected override IEnumerable<JavaClass> GetInnerClasses(Class classe, string tag)
    {
        if (Config.FieldsEnum.Contains(AnnotationConstraint.Persisted))
        {
            var fieldEnum = GetFieldsEnum(classe, tag);
            yield return fieldEnum;
        }

        if (classe.PrimaryKey.Count() > 1)
        {
            yield return GetCompositePrimaryKeyClass(classe, tag);
        }
    }

    protected virtual JavaMethod? GetMapIdPropertySetter(Class classe, string tag)
    {
        if (classe.PrimaryKey.Count() == 1 && classe.PrimaryKey.FirstOrDefault() is AssociationProperty ap)
        {
            var propertyName = classe.PrimaryKey.First().NameCamel;
            var propertyType = JpaModelPropertyGenerator.GetPropertyType(ap.Property);
            string setterName = $"set{ap.NamePascal}";
            var method = new JavaMethod("void", setterName)
            {
                Visibility = "public",
                Comment = $"Setter for {propertyName}",
            }.AddParameter(
                new JavaMethodParameter(propertyType, propertyName)
                {
                    Comment =
                        $"Set the value of {{@link {classe.GetImport(Config, tag)}#{propertyName} {propertyName}}}",
                }
            );
            method.Imports.AddRange(Config.GetDomainImports(ap.Property, tag));
            method.AddBodyLine(@$"this.{propertyName} = {propertyName};");
            return method;
        }
        return null;
    }

    private IEnumerable<JavaMethod> GetAdders(Class classe, string tag)
    {
        foreach (var ap in classe.Properties.OfType<AssociationProperty>().Where(t => t.Type.IsToMany()))
        {
            if (ap.ReverseProperty != null)
            {
                var propertyName = ap.NameByClassCamel;
                var adder = new JavaMethod("void", $"add{ap.Association.NamePascal}{ap.Role}")
                {
                    Comment = $"Add a value to {{@link {classe.GetImport(Config, tag)}#{propertyName} {propertyName}}}",
                }
                    .AddParameter(
                        new JavaMethodParameter(ap.Association.NamePascal, ap.Association.NameCamel)
                        {
                            Comment = $"value to add to {ap.ReverseProperty.NameByClassCamel}",
                        }
                    )
                    .AddBodyLine(@$"this.{propertyName}.add({ap.Association.NameCamel});");
                if (ap.ReverseProperty.Type.IsToMany())
                {
                    adder.AddBodyLine(
                        @$"{ap.Association.NameCamel}.get{ap.ReverseProperty.NameByClassPascal}().add(this);"
                    );
                }
                else
                {
                    adder.AddBodyLine(@$"{ap.Association.NameCamel}.set{ap.ReverseProperty.NameByClassPascal}(this);");
                }

                yield return adder;
            }
        }
    }

    private JavaMethod? GetMapIdPropertyGetter(Class classe, string tag)
    {
        if (classe.PrimaryKey.Count() == 1 && classe.PrimaryKey.FirstOrDefault() is AssociationProperty ap)
        {
            var propertyType = JpaModelPropertyGenerator.GetPropertyType(ap.Property);
            string getterName = $"get{ap.NamePascal}";
            var method = new JavaMethod(propertyType, getterName)
            {
                Visibility = "public",
                Comment = $"Getter for {ap.NameCamel}",
                ReturnComment = $"value of {{@link {classe.GetImport(Config, tag)}#{ap.NameCamel} {ap.NameCamel}}}",
            };
            method.AddBodyLine(@$"return this.{ap.NameCamel};");
            return method;
        }
        return null;
    }

    protected virtual void WriteCompositePrimaryKeyClass(JavaWriter fw, Class classe, string tag)
    {
        if (classe.PrimaryKey.Count() <= 1)
        {
            return;
        }

        fw.Write(1, GetCompositePrimaryKeyClass(classe, tag));
    }

    protected virtual JavaClass GetCompositePrimaryKeyClass(Class classe, string tag)
    {
        var javaClass = new JavaClass($"{classe.NamePascal}Id") { Visibility = "public", Modifier = "static" };
        foreach (var pk in classe.PrimaryKey)
        {
            var annotations = new List<JavaAnnotation>();
            annotations.AddRange(JpaModelPropertyGenerator.GetDomainAnnotations(pk, tag));
            if (pk is AssociationProperty ap && !(Config.CanClassUseEnums(ap.Association) && Config.EnumsAsEnums))
            {
                annotations.AddRange(JpaModelPropertyGenerator.GetJpaAssociationAnnotations(ap, tag));
            }
            else
            {
                annotations.Add(JpaModelPropertyGenerator.GetColumnAnnotation(pk));

                if (JpaModelPropertyGenerator.ShouldWriteEnumAnnotation(pk))
                {
                    annotations.Add(JpaModelPropertyGenerator.EnumAnnotation);
                }
            }
            var field = new JavaField(
                JpaModelPropertyGenerator.GetPropertyType(pk),
                JpaModelPropertyGenerator.GetPropertyName(pk)
            ).AddRange(annotations);
            javaClass.Add(field);
        }

        foreach (var pk in classe.PrimaryKey)
        {
            var getter = JpaModelPropertyGenerator.GetGetter(tag, pk);
            var setter = JpaModelPropertyGenerator.GetSetter(tag, pk);
            javaClass.Add(getter);
            javaClass.Add(setter);
        }

        var equalsMethod = new JavaMethod("boolean", "equals") { Visibility = "public" }
            .AddParameter(new JavaMethodParameter("java.util.Objects", "Object", "o"))
            .AddBodyLine("if (o == this) {")
            .AddBodyLine(1, "return true;")
            .AddBodyLine("}")
            .AddBodyLine()
            .AddBodyLine("if (o == null) {")
            .AddBodyLine(1, "return false;")
            .AddBodyLine("}")
            .AddBodyLine()
            .AddBodyLine("if (this.getClass() != o.getClass()) {")
            .AddBodyLine(1, "return false;")
            .AddBodyLine("}")
            .AddBodyLine()
            .AddBodyLine($"{classe.NamePascal}Id oId = ({classe.NamePascal}Id) o;");
        var associations = classe.PrimaryKey.Where(p =>
            p is AssociationProperty || p is AliasProperty ap && ap.Property is AssociationProperty
        );
        if (associations.Any())
        {
            equalsMethod.AddBodyLine();
            equalsMethod.AddBodyLine(
                @$"if ({string.Join(" || ", associations.Select(pk => pk.NameByClassCamel).Select(pk => $"this.{pk} == null || oId.{pk} == null"))}) {{"
            );
            equalsMethod.AddBodyLine(1, "return false;");
            equalsMethod.AddBodyLine("}");
        }

        equalsMethod.AddBodyLine();
        equalsMethod.AddBodyLine(
            $@"return {string.Join("\n && ", classe.PrimaryKey.Select(pk => $@"Objects.equals(this.{pk.NameByClassCamel}{GetterToCompareCompositePkPk(pk)}, oId.{pk.NameByClassCamel}{GetterToCompareCompositePkPk(pk)})"))};"
        );

        javaClass.Add(equalsMethod);

        var hashCodeMethod = new JavaMethod("int", "hashCode") { Visibility = "public" }.AddAnnotation(
            new JavaAnnotation("Override")
        );
        hashCodeMethod.AddBodyLine(
            $"return Objects.hash({string.Join(", ", classe.PrimaryKey.Select(pk => $"{(pk is AssociationProperty || pk is AliasProperty ap && ap.Property is AssociationProperty ? $"{pk.NameByClassCamel} == null ? null : " : string.Empty)}{pk.NameByClassCamel}{GetterToCompareCompositePkPk(pk)}"))});"
        );
        hashCodeMethod.Imports.Add("java.util.Objects");
        javaClass.Add(hashCodeMethod);
        return javaClass;
    }

    protected virtual void WriteConstructors(Class classe, string tag, JavaWriter fw)
    {
        foreach (var constructor in GetConstuctors(classe, tag))
        {
            fw.Write(1, constructor);
        }
    }

    protected virtual void WriteRemovers(JavaWriter fw, Class classe, string tag)
    {
        foreach (var remover in GetRemovers(classe, tag))
        {
            fw.Write(1, remover);
        }
    }

    private IEnumerable<JavaMethod> GetRemovers(Class classe, string tag)
    {
        foreach (var ap in classe.Properties.OfType<AssociationProperty>().Where(t => t.Type.IsToMany()))
        {
            if (ap.ReverseProperty != null)
            {
                var propertyName = ap.NameByClassCamel;
                var remover = new JavaMethod("void", $"remove{ap.Association.NamePascal}{ap.Role}")
                {
                    Comment =
                        $"Remove a value from {{@link {classe.GetImport(Config, tag)}#{propertyName} {propertyName}}}",
                }
                    .AddParameter(
                        new JavaMethodParameter(ap.Association.NamePascal, ap.Association.NameCamel)
                        {
                            Comment = $"{ap.Association.NameCamel} value to remove",
                        }
                    )
                    .AddBodyLine(@$"this.{propertyName}.remove({ap.Association.NameCamel});");
                if (ap.ReverseProperty.Type.IsToMany())
                {
                    remover.AddBodyLine(
                        @$"{ap.Association.NameCamel}.get{ap.ReverseProperty.NameByClassPascal}().remove(this);"
                    );
                }
                else
                {
                    remover.AddBodyLine(
                        @$"{ap.Association.NameCamel}.set{ap.ReverseProperty.NameByClassPascal}(null);"
                    );
                }
                yield return remover;
            }
        }
    }
}
