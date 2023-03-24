using Microsoft.Extensions.Logging;
using TopModel.Core;

namespace TopModel.Generator.Jpa;

/// <summary>
/// Générateur de DAOs JPA.
/// </summary>
public class JpaModelInterfaceGenerator : ClassGeneratorBase
{
    private readonly JpaConfig _config;
    private readonly ILogger<JpaModelInterfaceGenerator> _logger;

    public JpaModelInterfaceGenerator(ILogger<JpaModelInterfaceGenerator> logger, JpaConfig config)
        : base(logger, config)
    {
        _config = config;
        _logger = logger;
    }

    public override string Name => "JpaInterfaceGen";

    protected override object? GetDomainType(Domain domain)
    {
        return domain.Java;
    }

    protected override bool FilterClass(Class classe)
    {
        return classe.Abstract;
    }

    protected override string GetFileName(Class classe, string tag)
    {
        return _config.GetClassFileName(classe, tag);
    }

    protected override void HandleClass(string fileName, Class classe, string tag)
    {
        var packageName = _config.GetPackageName(classe, tag);
        using var fw = new JavaWriter(fileName, _logger, packageName, null);

        WriteImports(fw, classe, tag);
        fw.WriteLine();

        var extendsDecorator = classe.Decorators.SingleOrDefault(d => d.Decorator.Java?.Extends != null);
        var extends = (classe.Extends?.NamePascal ?? extendsDecorator.Decorator?.Java!.Extends!.ParseTemplate(classe, extendsDecorator.Parameters)) ?? null;

        var implements = classe.Decorators.SelectMany(d => d.Decorator.Java!.Implements.Select(i => i.ParseTemplate(classe, d.Parameters))).Distinct().ToList();

        fw.WriteLine("@Generated(\"TopModel : https://github.com/klee-contrib/topmodel\")");
        fw.WriteLine($"public interface {classe.NamePascal} {{");

        WriteGetters(fw, classe, tag);

        if (classe.Properties.Any(p => !p.Readonly))
        {
            WriteHydrate(fw, classe);
        }

        fw.WriteLine("}");
    }

    private void WriteHydrate(JavaWriter fw, Class classe)
    {
        var properties = classe.Properties
            .Where(p => !p.Readonly)
            .Where(p => !_config.EnumShortcutMode || !(p is AssociationProperty apo && apo.Association.Reference && (apo.Type == AssociationType.OneToOne || apo.Type == AssociationType.ManyToOne)));

        if (!properties.Any())
        {
            return;
        }

        fw.WriteLine();
        fw.WriteDocStart(1, $"hydrate values of instance");
        foreach (var property in properties)
        {
            var propertyName = property.GetJavaName();
            fw.WriteLine(1, $" * @param {propertyName} value to set");
        }

        fw.WriteDocEnd(1);
        var signature = string.Join(", ", properties.Select(property =>
            {
                var propertyName = property.GetJavaName();
                return $@"{property.GetJavaType()} {property.GetJavaName()}";
            }));

        fw.WriteLine(1, $"void hydrate({signature});");
    }

    private void WriteGetters(JavaWriter fw, Class classe, string tag)
    {
        foreach (var property in classe.Properties.Where(p => !_config.EnumShortcutMode || !(p is AssociationProperty apo && apo.Association.Reference && (apo.Type == AssociationType.OneToOne || apo.Type == AssociationType.ManyToOne))))
        {
            var getterPrefix = property.GetJavaType() == "boolean" ? "is" : "get";
            fw.WriteLine();
            fw.WriteDocStart(1, $"Getter for {property.GetJavaName()}");
            fw.WriteReturns(1, $"value of {{@link {classe.GetImport(_config, tag)}#{property.GetJavaName()} {property.GetJavaName()}}}");
            fw.WriteDocEnd(1);
            fw.WriteLine(1, @$"{property.GetJavaType()} {getterPrefix}{property.GetJavaName(true)}();");
        }
    }

    private void WriteImports(JavaWriter fw, Class classe, string tag)
    {
        var imports = new List<string>
            {
                _config.PersistenceMode.ToString().ToLower() + ".annotation.Generated",
            };
        foreach (var property in classe.Properties)
        {
            imports.AddRange(property.GetTypeImports(_config, tag));

            if (property is CompositionProperty cp && cp.Composition.Namespace.Module == cp.Class?.Namespace.Module)
            {
                imports.Add(cp.Composition.GetImport(_config, tag));
            }
        }

        if (classe.Extends != null)
        {
            foreach (var property in classe.Extends.Properties)
            {
                imports.AddRange(property.GetTypeImports(_config, tag));
            }
        }

        fw.AddImports(imports);
    }
}