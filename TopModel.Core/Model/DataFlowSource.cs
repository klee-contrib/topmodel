using TopModel.Core.FileModel;

namespace TopModel.Core.Model;

public class DataFlowSource
{
#nullable disable
    public string Source { get; set; }

    public Class Class { get; set; }

    public ClassReference ClassReference { get; set; }

    public DataFlowSourceMode Mode { get; set; }

#pragma warning disable MA0016
    public List<IProperty> JoinProperties { get; set; } = [];
#pragma warning restore MA0016

    public IList<Reference> JoinPropertyReferences { get; set; } = [];

    public bool InnerJoin { get; set; }

    public DataFlow DataFlow { get; set; }

#nullable enable
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
