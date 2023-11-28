using TopModel.Core.FileModel;

namespace TopModel.Core;

public class DataFlowSource
{
#nullable disable
    public string Source { get; set; }

    public Class Class { get; set; }

    public ClassReference ClassReference { get; set; }

    public DataFlowSourceMode Mode { get; set; }

    public List<IFieldProperty> JoinProperties { get; set; } = new();

    public List<Reference> JoinPropertyReferences { get; set; } = new();

    public bool InnerJoin { get; set; }

    public DataFlow DataFlow { get; set; }

#nullable enable
    public FromMapper? TargetFromMapper
    {
        get => DataFlow.Class.FromMappers.FirstOrDefault(fm => fm.Params.Count == 1 && fm.ClassParams.First().Class == Class);
    }

    public ClassMappings? FirstSourceToMapper
    {
        get
        {
            var joinedSources = DataFlow.Sources.Where(s => s.JoinProperties.Any()).ToList();
            if (joinedSources.Count <= 1 || joinedSources.First() == this)
            {
                return null;
            }

            return Class.ToMappers.FirstOrDefault(mapper => mapper.Class == joinedSources.First().Class);
        }
    }
}