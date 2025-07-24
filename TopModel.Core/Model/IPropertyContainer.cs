using TopModel.Core.FileModel;

namespace TopModel.Core.Model;

public interface IPropertyContainer : IAnnotationContainer
{
    ModelFile ModelFile { get; }

    LocatedString Name { get; }

    string NamePascal { get; }

    string NameCamel { get; }

    Namespace Namespace { get; }

    IList<DecoratorInstance> Decorators { get; }

    IEnumerable<Decorator> AllDecorators
    {
        get
        {
            if (this is Decorator self)
            {
                yield return self;
            }

            foreach (var (d, _) in Decorators)
            {
                yield return d;

                foreach (var subD in ((IPropertyContainer)d).AllDecorators)
                {
                    yield return subD;
                }
            }
        }
    }

    IList<DecoratorReference> DecoratorReferences { get; }

    IList<IProperty> Properties { get; }

    bool PreservePropertyCasing { get; }
}
