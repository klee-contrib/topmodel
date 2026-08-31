using TopModel.Core.FileModel;

namespace TopModel.Core.Model;

public class DataFlowSource
{
    public string Source { get; internal set; } = null!;

    public Class Class { get; internal set; } = null!;

    public ClassReference ClassReference { get; internal set; } = null!;

    public DataFlowSourceMode Mode { get; internal set; }

    public IList<IProperty> JoinProperties { get; } = [];

    public IList<Reference> JoinPropertyReferences { get; internal set; } = [];

    public bool InnerJoin { get; internal set; }

    public required DataFlow DataFlow { get; init; }

    public FromMapper? TargetFromMapper
    {
        get =>
            DataFlow.Class.FromMappers.FirstOrDefault(fm =>
                fm.Params.Count == 1 && fm.ClassParams.First().Class == Class
            );
    }

    public ClassMappings? FirstSourceToMapper
    {
        get
        {
            var joinedSources = DataFlow.Sources.Where(s => s.JoinProperties.Any()).ToList();
            if (joinedSources.Count <= 1 || joinedSources[0] == this)
            {
                return null;
            }

            return Class.ToMappers.FirstOrDefault(mapper => mapper.Class == joinedSources[0].Class);
        }
    }
}
