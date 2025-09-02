using TopModel.Core.FileModel;

namespace TopModel.Core.Model;

public interface IVariableContainer
{
    IDictionary<string, Variable> Variables { get; }

    IEnumerable<ParameterReference> VariableReferences { get; }

    IEnumerable<TransformReference> TransformReferences { get; }
}
