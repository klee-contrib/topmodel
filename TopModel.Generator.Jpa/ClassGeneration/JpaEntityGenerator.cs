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
        return !classe.Abstract && classe.IsPersistent && !Config.CanClassUseEnums(classe);
    }

    protected override IEnumerable<JavaAnnotation> GetAnnotations(Class classe, string tag)
    {
        foreach (var a in base.GetAnnotations(classe, tag))
        {
            yield return a;
        }

        yield return new JavaAnnotation("Entity", imports: "jakarta.persistence.Entity");
        if (Config.AvailableClasses.Any(c => c.Extends == classe))
        {
            yield return new JavaAnnotation("Inheritance", imports: "jakarta.persistence.Inheritance").AddAttribute(
                "strategy",
                "InheritanceType.JOINED",
                "jakarta.persistence.InheritanceType"
            );
        }

        var tableAnnotation = new JavaAnnotation("Table", imports: "jakarta.persistence.Table").AddAttribute(
            "name",
            $@"""{classe.SqlName}"""
        );
        if (classe.UniqueKeys.Count > 0)
        {
            var uks = classe.UniqueKeys.Select(uk =>
                new JavaAnnotation("UniqueConstraint", imports: "jakarta.persistence.UniqueConstraint").AddAttribute(
                    "columnNames",
                    uk.Select(u => $@"""{u.SqlName}""").ToArray()
                )
            );

            tableAnnotation.AddAttribute("uniqueConstraints", uks);
        }

        yield return tableAnnotation;
        if (classe.PrimaryKey.Count() > 1)
        {
            yield return new JavaAnnotation("IdClass", imports: "jakarta.persistence.IdClass").AddAttribute(
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

    protected virtual JavaClass GetCompositePrimaryKeyClass(Class classe, string tag)
    {
        var javaClass = new JavaClass($"{classe.NamePascal}Id") { Visibility = "public", Modifier = "static" };
        foreach (var pk in classe.PrimaryKey)
        {
            var annotations = new List<JavaAnnotation>();
            annotations.AddRange(JpaModelPropertyGenerator.GetDomainAnnotations(pk, tag));
            if (
                pk is { Association: Class association }
                && !(Config.CanClassUseEnums(association) && Config.EnumsAsEnums)
            )
            {
                annotations.AddRange(JpaModelPropertyGenerator.GetJpaAssociationAnnotations(pk, tag));
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
        var associations = classe.PrimaryKey.Where(p => p.Association != null);
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
            $"return Objects.hash({string.Join(", ", classe.PrimaryKey.Select(pk => $"{(pk.Association != null ? $"{pk.NameByClassCamel} == null ? null : " : string.Empty)}{pk.NameByClassCamel}{GetterToCompareCompositePkPk(pk)}"))});"
        );
        hashCodeMethod.Imports.Add("java.util.Objects");
        javaClass.Add(hashCodeMethod);
        return javaClass;
    }

    protected override IEnumerable<JavaField> GetFields(Class classe, string tag)
    {
        if (
            classe.PrimaryKey.Count() == 1
            && classe.PrimaryKey.First() is { Association: Class association, AssociationProperty: IProperty ap } pk
        )
        {
            yield return new JavaField(JpaModelPropertyGenerator.GetPropertyType(ap), pk.NameCamel)
            {
                Comment =
                {
                    @$"Identifiant technique mappé avec celui de la classe {{@link {association.GetImport(Config, tag)}}} {association.NamePascal}",
                },
            }.Add(JpaModelPropertyGenerator.IdAnnotation);
        }
        foreach (var field in JpaModelPropertyGenerator.GetFields(classe, tag))
        {
            yield return field;
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

    protected override IEnumerable<JavaClass> GetInnerClasses(Class classe, string tag)
    {
        if (
            Config.FieldsEnum.Contains(AnnotationConstraint.Persisted)
            && JpaModelPropertyGenerator.GetAvailableProperties(classe).Any()
        )
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
        if (
            classe.PrimaryKey.Count() == 1
            && classe.PrimaryKey.FirstOrDefault() is { AssociationProperty: IProperty ap }
        )
        {
            var propertyName = classe.PrimaryKey.First().NameCamel;
            var propertyType = JpaModelPropertyGenerator.GetPropertyType(ap);
            string setterName = $"set{classe.PrimaryKey.First().NamePascal}";
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
            method.Imports.AddRange(Config.GetDomainImports(ap, tag));
            method.AddBodyLine(@$"this.{propertyName} = {propertyName};");
            return method;
        }
        return null;
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

    protected virtual string GetterToCompareCompositePkPk(IProperty pk)
    {
        if (pk is { AssociationProperty: IProperty ap } && !Config.EnumsAsEnums)
        {
            return $".{JpaModelPropertyGenerator.GetGetterName(ap)}()";
        }

        return string.Empty;
    }

    protected virtual void WriteCompositePrimaryKeyClass(JavaWriter fw, Class classe, string tag)
    {
        if (classe.PrimaryKey.Count() <= 1)
        {
            return;
        }

        fw.Write(1, GetCompositePrimaryKeyClass(classe, tag));
    }

    private IEnumerable<JavaMethod> GetAdders(Class classe, string tag)
    {
        foreach (var ap in classe.Properties.Where(p => p.IsAssociationToMany()))
        {
            if (ap.ReverseProperty != null)
            {
                var propertyName = ap.NameByClassCamel;
                var adder = new JavaMethod("void", $"add{ap.Association!.NamePascal}{ap.AssociationRole}")
                {
                    Comment = $"Add a value to {{@link {classe.GetImport(Config, tag)}#{propertyName} {propertyName}}}",
                }
                    .AddParameter(
                        new JavaMethodParameter(ap.Association!.NamePascal, ap.Association!.NameCamel)
                        {
                            Comment = $"value to add to {ap.ReverseProperty!.NameByClassCamel}",
                        }
                    )
                    .AddBodyLine(@$"this.{propertyName}.add({ap.Association!.NameCamel});");
                if (ap.ReverseProperty!.IsAssociationToMany())
                {
                    adder.AddBodyLine(
                        @$"{ap.Association!.NameCamel}.get{ap.ReverseProperty!.NameByClassPascal}().add(this);"
                    );
                }
                else
                {
                    adder.AddBodyLine(
                        @$"{ap.Association!.NameCamel}.set{ap.ReverseProperty!.NameByClassPascal}(this);"
                    );
                }

                yield return adder;
            }
        }
    }

    private JavaMethod? GetMapIdPropertyGetter(Class classe, string tag)
    {
        if (
            classe.PrimaryKey.Count() == 1
            && classe.PrimaryKey.FirstOrDefault() is { AssociationProperty: IProperty ap } pk
        )
        {
            var propertyType = JpaModelPropertyGenerator.GetPropertyType(ap);
            string getterName = $"get{pk.NamePascal}";
            var method = new JavaMethod(propertyType, getterName)
            {
                Visibility = "public",
                Comment = $"Getter for {pk.NameCamel}",
                ReturnComment = $"value of {{@link {classe.GetImport(Config, tag)}#{pk.NameCamel} {pk.NameCamel}}}",
            };
            method.AddBodyLine(@$"return this.{pk.NameCamel};");
            return method;
        }
        return null;
    }

    private IEnumerable<JavaMethod> GetRemovers(Class classe, string tag)
    {
        foreach (var ap in classe.Properties.Where(t => t.IsAssociationToMany()))
        {
            if (ap.ReverseProperty != null)
            {
                var propertyName = ap.NameByClassCamel;
                var remover = new JavaMethod("void", $"remove{ap.Association!.NamePascal}{ap.AssociationRole}")
                {
                    Comment =
                        $"Remove a value from {{@link {classe.GetImport(Config, tag)}#{propertyName} {propertyName}}}",
                }
                    .AddParameter(
                        new JavaMethodParameter(ap.Association!.NamePascal, ap.Association!.NameCamel)
                        {
                            Comment = $"{ap.Association!.NameCamel} value to remove",
                        }
                    )
                    .AddBodyLine(@$"this.{propertyName}.remove({ap.Association!.NameCamel});");
                if (ap.ReverseProperty!.IsAssociationToMany())
                {
                    remover.AddBodyLine(
                        @$"{ap.Association!.NameCamel}.get{ap.ReverseProperty!.NameByClassPascal}().remove(this);"
                    );
                }
                else
                {
                    remover.AddBodyLine(
                        @$"{ap.Association!.NameCamel}.set{ap.ReverseProperty!.NameByClassPascal}(null);"
                    );
                }
                yield return remover;
            }
        }
    }
}
