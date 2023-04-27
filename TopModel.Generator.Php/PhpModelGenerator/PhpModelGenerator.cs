using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Generator.Core;
using TopModel.Utils;
using TopModel.Generator.Core;

namespace TopModel.Generator.Php;

/// <summary>
/// Générateur de fichiers de modèles JPA.
/// </summary>
public class PhpModelGenerator : ClassGeneratorBase<PhpConfig>
{
    private readonly ILogger<PhpModelGenerator> _logger;
    private readonly ModelConfig _modelConfig;

    private PhpModelPropertyGenerator? _phpModelPropertyGenerator;

    public PhpModelGenerator(ILogger<PhpModelGenerator> logger, ModelConfig modelConfig)
        : base(logger)
    {
        _logger = logger;
        _modelConfig = modelConfig;
    }

    public override string Name => "PhpModelGen";

    private PhpModelPropertyGenerator PhpModelPropertyGenerator
    {
        get
        {
            _phpModelPropertyGenerator ??= new PhpModelPropertyGenerator(Config);
            return _phpModelPropertyGenerator;
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
        var packageName = Config.GetPackageName(classe, tag);
        using var fw = new PhpWriter(fileName, _logger, packageName, null);

        WriteAttributes(fw, classe, tag);

        var extendsDecorator = classe.Decorators.SingleOrDefault(d => Config.GetImplementation(d.Decorator)?.Extends != null);
        var extends = (classe.Extends?.NamePascal ?? Config.GetImplementation(extendsDecorator.Decorator)?.Extends!.ParseTemplate(classe, extendsDecorator.Parameters)) ?? null;

        var implements = classe.Decorators.SelectMany(d => Config.GetImplementation(d.Decorator)?.Implements.Select(i => i.ParseTemplate(classe, d.Parameters)) ?? Array.Empty<string>()).Distinct().ToList();

        fw.WriteClassDeclaration(classe.NamePascal, null, extends, implements);

        PhpModelPropertyGenerator.WriteProperties(fw, classe, Classes, tag);

        WriteConstructor(fw, classe, Classes, tag);
        WriteGetters(fw, classe, tag);
        WriteSetters(fw, classe, tag);

        fw.WriteLine("}");
    }

    private void WriteConstructor(PhpWriter fw, Class classe, IEnumerable<Class> classes, string tag)
    {
        var collectionProperties = classe.GetProperties(Classes, tag).Where(
            p => p is IFieldProperty fp && Config.GetType(p, Classes, p.Class.IsPersistent) == "Collection"
            || p is CompositionProperty cp && cp.Kind == "list");
        if (collectionProperties.Any())
        {
            fw.WriteLine();
            fw.WriteLine(1, "public function __construct()");
            fw.WriteLine(1, "{");
            fw.AddImport(@"Doctrine\Common\Collections\ArrayCollection");
            foreach (var property in collectionProperties)
            {
                fw.WriteLine(2, $"$this->{property.Name.ToCamelCase()} = new ArrayCollection();");
            }

            fw.WriteLine(1, "}");
        }
    }

    private void WriteAttributes(PhpWriter fw, Class classe, string tag)
    {
        if (classe.IsPersistent)
        {
            fw.AddImport($@"Doctrine\ORM\Mapping\Entity");
            fw.AddImport($@"Doctrine\ORM\Mapping\Table");

            fw.WriteLine(@$"#[Doctrine\ORM\Mapping\Entity(repositoryClass: {classe.Name}Repository::class)]");
            var repositoryNamespace = Config.ResolveVariables(
            Config.RepositoriesPath,
            tag,
            module: classe.Namespace.Module).ToPackageName();
            fw.AddImport(@$"{repositoryNamespace}\{classe.Name}Repository");
            fw.WriteLine(@$"#[Doctrine\ORM\Mapping\Table(name: '{classe.SqlName}')]");
        }

        foreach (var a in classe.Decorators.SelectMany(d => Config.GetImplementation(d.Decorator)?.Annotations.Select(a => a.ParseTemplate(classe, d.Parameters)) ?? Array.Empty<string>()).Distinct())
        {
            fw.WriteLine(a);
        }

        foreach (var i in classe.Decorators.SelectMany(d => Config.GetImplementation(d.Decorator)?.Imports.Select(i => i.ParseTemplate(classe, d.Parameters)) ?? Array.Empty<string>()).Distinct())
        {
            fw.AddImport(i);
        }
    }

    private void WriteGetters(PhpWriter fw, Class classe, string tag)
    {
        foreach (var property in classe.GetProperties(Classes, tag))
        {
            fw.WriteLine();
            if (property is AssociationProperty ap && (ap.Type == AssociationType.OneToMany || ap.Type == AssociationType.ManyToMany))
            {
                fw.WriteDocStart(1);
                fw.WriteReturns(1, $"Collection<{ap.Association}>{(ap.Required ? string.Empty : "|null")}");
                fw.WriteDocEnd(1);
            }

            var getterPrefix = Config.GetType(property, Classes, classe.IsPersistent) == "boolean" ? "is" : "get";
            var required = property is IFieldProperty rp && rp.Required || false;
            fw.WriteLine(1, @$"public function {getterPrefix}{property.GetPhpName(true)}() : {Config.GetType(property, Classes, classe.IsPersistent)}{(required ? string.Empty : "|null")}");
            fw.WriteLine(1, "{");
            fw.WriteLine(2, @$"return $this->{property.GetPhpName()};");
            fw.WriteLine(1, "}");
        }
    }

    private void WriteSetters(PhpWriter fw, Class classe, string tag)
    {
        foreach (var property in classe.GetProperties(Classes, tag))
        {
            var propertyName = property.GetPhpName();
            fw.WriteLine();
            if (property is AssociationProperty ap && (ap.Type == AssociationType.OneToMany || ap.Type == AssociationType.ManyToMany))
            {
                fw.WriteDocStart(1);
                fw.WriteLine(1, $" * @param Collection<{ap.Association}>{(ap.Required ? string.Empty : "|null")} ${propertyName}");
                fw.WriteDocEnd(1);
            }

            fw.WriteLine(1, @$"public function set{propertyName.ToFirstUpper()}({Config.GetType(property, Classes, classe.IsPersistent)}|null ${propertyName}) : self");
            fw.WriteLine(1, "{");
            fw.WriteLine(2, @$"$this->{propertyName} = ${propertyName};");
            fw.WriteLine();
            fw.WriteLine(2, @$"return $this;");
            fw.WriteLine(1, "}");
        }
    }
}