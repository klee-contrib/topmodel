using TopModel.Core.FileModel;

namespace TopModel.Core.Model;

public interface IParameterContainer
{
    LocatedString Name { get; }

    IList<TemplateParameter> TemplateParameters { get; }
}
