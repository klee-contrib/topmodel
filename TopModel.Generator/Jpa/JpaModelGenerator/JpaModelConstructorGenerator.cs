﻿using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Core.FileModel;
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
        if (classe.Extends != null || classe.Decorators.Any(d => d.Java?.Extends is not null))
        {
            fw.WriteLine(2, $"super();");
        }

        fw.WriteLine(1, $"}}");
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

    public void WriteAllArgConstructor(JavaWriter fw, Class classe)
    {
        fw.WriteLine();
        fw.WriteDocStart(1, "All arg constructor");
        var properties = GetAllArgsProperties(classe);

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
            var parentAllArgConstructorArguments = string.Join(", ", GetAllArgsProperties(classe.Extends).Select(p => $"{p.GetJavaName()}"));
            fw.WriteLine(2, $"super({parentAllArgConstructorArguments});");
        }
        else if (classe.Decorators.Any(d => d.Java?.Extends is not null))
        {
            fw.WriteLine(2, $"super();");
        }

        foreach (var property in classe.Properties)
        {
            fw.WriteLine(2, $"this.{property.GetJavaName()} = {property.GetJavaName()};");
        }

        fw.WriteLine(1, $"}}");
    }

    public void WriteCopyConstructor(JavaWriter fw, Class classe)
    {
        fw.WriteLine();
        fw.WriteDocStart(1, "Copy constructor");
        fw.WriteLine(1, $" * @param {classe.Name.ToFirstLower()} to copy");
        var properties = classe.Properties;
        fw.WriteDocEnd(1);
        fw.WriteLine(1, $"public {classe.Name}({classe.Name} {classe.Name.ToFirstLower()}) {{");
        if (classe.Extends != null)
        {
            var parentAllArgConstructorArguments = string.Join(", ", GetAllArgsProperties(classe.Extends).Select(p => $"{p.GetJavaName()}"));
            fw.WriteLine(2, $"super({classe.Name.ToFirstLower()});");
        }
        else if (classe.Decorators.Any(d => d.Java?.Extends is not null))
        {
            fw.WriteLine(2, $"super();");
        }

        fw.WriteLine(2, $"if({classe.Name.ToFirstLower()} == null) {{");
        fw.WriteLine(3, $"return;");
        fw.WriteLine(2, "}");
        fw.WriteLine();

        foreach (var property in classe.Properties.Where(p => !_config.EnumShortcutMode || !(p is AssociationProperty apo && apo.Association.Reference && (apo.Type == AssociationType.OneToOne || apo.Type == AssociationType.ManyToOne))))
        {
            if (!(property is AssociationProperty ap && (ap.Type == AssociationType.OneToMany || ap.Type == AssociationType.ManyToMany) || property is CompositionProperty cp && cp.Kind == "list"))
            {
                fw.WriteLine(2, $"this.{property.GetJavaName().ToFirstLower()} = {classe.Name.ToFirstLower()}.get{property.GetJavaName().ToFirstUpper()}();");
            }
        }

        var propertyListToCopy = classe.Properties
        .Where(p => !_config.EnumShortcutMode || !(p is AssociationProperty apo && apo.Association.Reference && (apo.Type == AssociationType.OneToOne || apo.Type == AssociationType.ManyToOne)))
        .Where(property => property is AssociationProperty ap && (ap.Type == AssociationType.OneToMany || ap.Type == AssociationType.ManyToMany) || property is CompositionProperty cp && cp.Kind == "list");

        if (propertyListToCopy.Count() > 0)
        {
            fw.WriteLine();
        }

        foreach (var property in propertyListToCopy)
        {
            if (property is AssociationProperty ap && (ap.Type == AssociationType.OneToMany || ap.Type == AssociationType.ManyToMany) || property is CompositionProperty cp && cp.Kind == "list")
            {
                fw.WriteLine(2, $"this.{property.GetJavaName().ToFirstLower()} = {classe.Name.ToFirstLower()}.get{property.GetJavaName().ToFirstUpper()}().stream().toList();");
            }
        }

        if (_config.EnumShortcutMode)
        {
            fw.WriteLine();
            foreach (var ap in classe.Properties.OfType<AssociationProperty>().Where(ap => ap.Association.Reference && (ap.Type == AssociationType.OneToOne || ap.Type == AssociationType.ManyToOne)))
            {
                var propertyName = ap.Name.ToFirstLower();
                fw.WriteLine(2, $"this.set{ap.Name}({classe.Name.ToFirstLower()}.get{ap.Name.ToFirstUpper()}());");
            }
        }

        fw.WriteLine(1, $"}}");
    }

    public void WriteAliasConstructor(JavaWriter fw, Class classe)
    {
        fw.WriteLine();
        fw.WriteDocStart(1, "Alias constructor");
        fw.WriteLine(1, @$" * Ce constructeur permet d'initialiser un objet {classe.Name} avec comme paramètres les classes dont les propriétés sont référencées {classe.Name}.");
        fw.WriteLine(1, @$" * A ne pas utiliser pour construire un Dto en plusieurs requêtes.");
        fw.WriteLine(1, @$" * Voir la <a href=""https://klee-contrib.github.io/topmodel/#/generator/jpa?id=constructeurs-par-alias"">documentation</a>");
        var aliasClasses = ImportsJpaExtensions.GetAliasClass(classe);

        if (aliasClasses.Count == 0)
        {
            return;
        }

        var classSignature = string.Join(", ", aliasClasses.Select(c => $"{c.Class.Name} {c.Name.ToFirstLower()}"));
        foreach (var c in aliasClasses)
        {
            fw.WriteLine(1, $" * @param {c.Name} {c.Class.Comment}");
        }

        fw.WriteDocEnd(1);
        fw.WriteLine(1, $"public {classe.Name}({classSignature}) {{");
        if (classe.Extends != null)
        {
            var parentAliasConstructorArguments = string.Join(", ", ImportsJpaExtensions.GetAliasClass(classe.Extends).Select(c => $"{c.Name.ToFirstLower()}"));
            fw.WriteLine(2, $"super({parentAliasConstructorArguments});");
        }
        else if (classe.Decorators.Any(d => d.Java?.Extends is not null))
        {
            fw.WriteLine(2, $"super();");
        }

        string currentArg = string.Empty;
        foreach (var p in classe.Properties.OfType<AliasProperty>().ToList().OrderBy(p => p.OriginalProperty!.Class.Name))
        {
            var prefix = p.Prefix?.ToFirstLower() ?? string.Empty;
            var suffix = p.Suffix ?? string.Empty;
            var argName = $"{prefix}{(!string.IsNullOrEmpty(prefix) ? p.OriginalProperty?.Class?.Name : p.OriginalProperty?.Class?.Name?.ToFirstLower() ?? string.Empty)}{suffix}";
            if (currentArg != argName)
            {
                if (currentArg != string.Empty)
                {
                    fw.WriteLine(2, "}");
                    fw.WriteLine();
                }

                fw.WriteLine(2, $"if({argName} != null) {{");
            }

            currentArg = argName;

            if (p.OriginalProperty is AssociationProperty ap)
            {
                if (!ap.IsEnum() || !_config.EnumShortcutMode)
                {
                    fw.WriteLine();
                    fw.WriteLine(3, $"if({argName}.get{p.OriginalProperty.GetJavaName().ToFirstUpper()}{p.Suffix ?? string.Empty}() != null) {{");

                    if (ap.Type == AssociationType.ManyToMany || ap.Type == AssociationType.OneToMany)
                    {
                        fw.WriteLine(4, $"this.{p.GetJavaName()} = {argName}.get{p.OriginalProperty.GetJavaName().ToFirstUpper()}().stream().map({ap.Association.Name}::get{ap.Association.PrimaryKey!.Name}).toList();");
                    }
                    else
                    {
                        fw.WriteLine(4, $"this.{p.GetJavaName()} = {argName}.get{p.OriginalProperty.GetJavaName().ToFirstUpper()}().get{ap.Association.PrimaryKey!.Name}();");
                    }

                    fw.WriteLine(3, $"}}");
                }
                else
                {
                    fw.WriteLine(3, $"this.{p.GetJavaName()} = {argName}{p.Suffix ?? string.Empty}.get{ap.Name}();");
                }
            }
            else
            {
                fw.WriteLine(3, $"this.{p.GetJavaName()} = {argName}.get{p.OriginalProperty!.Name.ToFirstUpper()}();");
            }
        }

        if (currentArg != string.Empty)
        {
            fw.WriteLine(2, $"}}");
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
            foreach (var param in mapper.Params)
            {
                fw.WriteParam(param.Name.ToFirstLower(), $"Instance de '{param.Class}'");
            }

            fw.WriteReturns(1, $"Une nouvelle instance de '{classe}'");
            fw.WriteDocEnd(1);
            fw.WriteLine(1, $"public {classe}({string.Join(", ", mapper.Params.Select(p => $"{p.Class} {p.Name.ToFirstLower()}"))}) {{");

            if (classe.Extends != null || classe.Decorators.Any(d => d.Java?.Extends is not null))
            {
                fw.WriteLine(2, $"super();");
            }

            foreach (var param in mapper.Params)
            {
                fw.WriteLine(2, $"if({param.Name.ToFirstLower()} != null) {{");
                var mappings = param.Mappings.ToList();
                foreach (var mapping in mappings)
                {
                    if (mapping.Value is AssociationProperty ap)
                    {
                        if (!_config.EnumShortcutMode)
                        {
                            if (!classe.IsPersistent)
                            {
                                fw.WriteLine();
                                fw.WriteLine(3, $"if({param.Name.ToFirstLower()}.get{mapping.Value.GetJavaName().ToFirstUpper()}() != null) {{");
                                fw.WriteLine(4, $"this.{mapping.Key.GetJavaName()} = {param.Name.ToFirstLower()}.get{mapping.Value.GetJavaName().ToFirstUpper()}().get{ap.Association.PrimaryKey!.Name}();");
                                fw.WriteLine(3, $"}}");
                            }
                            else
                            {
                                fw.WriteLine(3, $"this.{mapping.Key.GetJavaName()} = {param.Name.ToFirstLower()}.get{mapping.Value.GetJavaName().ToFirstUpper()}();");
                            }
                        }
                        else
                        {
                            if (mapping.Key.Class.IsPersistent)
                            {
                                fw.WriteLine(3, $"this.{mapping.Key.GetJavaName()} = {param.Name.ToFirstLower()}.get{ap.GetJavaName().ToFirstUpper()}();");
                            }
                            else
                            {
                                fw.WriteLine(3, $"this.{mapping.Key.Name.ToFirstLower()} = {param.Name.ToFirstLower()}.get{ap.Name}();");
                            }
                        }
                    }
                    else
                    {
                        fw.WriteLine(3, $"this.{mapping.Key.GetJavaName()} = {param.Name.ToFirstLower()}.get{mapping.Value.Name.ToFirstUpper()}();");
                    }
                }

                fw.WriteLine(2, "};");
            }

            fw.WriteLine(1, "}");
        }
    }
}