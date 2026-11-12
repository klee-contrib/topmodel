using Microsoft.Extensions.Logging;
using TopModel.Core.Model;
using TopModel.Utils;

namespace TopModel.Generator.Csharp;

public class CSharpEnumValuesGenerator(ILogger<CSharpEnumValuesGenerator> logger, IFileWriterProvider writerProvider)
    : CSharpClassGenerator(logger, writerProvider)
{
    public override string Name => "CSharpEnumValuesGen";

    protected override bool FilterClass(Class classe)
    {
        return Config.HasSeparateEnumValues(classe);
    }

    protected override string GetFileName(Class classe, string tag)
    {
        return Config.GetEnumValuesFileName(classe);
    }

    protected override void HandleClass(string fileName, Class classe, string tag)
    {
        using var w = this.OpenCSharpWriter(fileName);
        if (classe.Extends != null)
        {
            w.AddUsing(Config.GetEnumValuesNamespace(classe.Extends));
        }

        w.WriteNamespace(Config.GetEnumValuesNamespace(classe));
        w.WriteSummary(classe.Comment);
        w.WriteClassDeclaration(
            Config.GetEnumValuesTypeName(classe),
            classe.Extends != null ? Config.GetEnumValuesTypeName(classe.Extends) : null,
            Config.UseRecords,
            classe.Type
        );

        GenerateConstProperties(w, classe);
        GenerateEnumValues(w, classe);
        GenerateFlags(w, classe);
        GenerateReadonlyEnumClassInstances(w, classe);

        foreach (var property in Config.GetProperties(classe).Where(p => !p.IsReverseProperty))
        {
            w.AddUsings(Config.GetDomainImports(property, tag));
            w.AddUsings(Config.GetValueImports(property));
            if (property.EnumProperty is IProperty enumProperty)
            {
                w.AddUsing(Config.GetEnumValuesNamespace(enumProperty.Class));
            }

            foreach (var value in classe.Values.Where(v => v.Value.ContainsKey(property)))
            {
                w.AddUsings(Config.GetValueImports(property, value.Value[property]));
            }

            w.WriteSummary(1, property.Comment);
            var type = Config.GetType(
                property,
                nonNullable: property.AssociationMultiple && Config.UseClassForAssociation(property)
                    || property.Required && (Config.RequiredNonNullable(tag) || property.Composition != null)
            );
            var targetClass =
                property.Composition ?? (Config.UseClassForAssociation(property) ? property.Association : null);
            if (targetClass != null)
            {
                if (targetClass.Enum != EnumMode.Class)
                {
                    throw new InvalidOperationException(
                        $"La propriété '{classe.NamePascal}.{property.NamePascal}' référence une classe sans valeurs métier séparées : '{targetClass.NamePascal}'."
                    );
                }

                type = type.Replace(Config.GetTypeName(targetClass), Config.GetEnumValuesTypeName(targetClass));
                w.AddUsing(Config.GetEnumValuesNamespace(targetClass));
            }

            var defaultValue = Config.GetDefaultValue(property, tag);
            var required = Config.RequiredNonNullable(tag) && property.Required && defaultValue == "null";
            w.WriteLine(
                1,
                $"public {(required ? "required " : string.Empty)}{type} {property.NamePascal} {{ get; {(property.Readonly ? "init" : "set")}; }}{(defaultValue != "null" ? $" = {defaultValue};" : string.Empty)}"
            );
            w.WriteLine();
        }

        GenerateReadonlyEnumKeyMapper(w, classe);
        w.WriteLine("}");
    }
}
