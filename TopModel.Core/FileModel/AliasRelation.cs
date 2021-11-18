using YamlDotNet.Core.Events;

namespace TopModel.Core.FileModel;

internal class AliasRelation : ClassRelation
{
    public List<Reference> IncludeReferences { get; } = new();

    public List<Reference> ExcludeReferences { get; } = new();

    public void AddInclude(Scalar scalar)
    {
        IncludeReferences.Add(new Reference(scalar));
    }

    public void AddExclude(Scalar scalar)
    {
        ExcludeReferences.Add(new Reference(scalar));
    }
}