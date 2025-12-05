using TopModel.Core.FileModel;
using TopModel.Core.Model;
using TopModel.Core.Model.Implementation;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace TopModel.Core.Loaders;

public class AnnotationLoader(FileChecker fileChecker) : ILoader
{
    /// <inheritdoc cref="ILoader.Load" />
    public void Load(Parser parser, ModelFile modelFile, Reference location)
    {
        var annotation = new Annotation()
        {
            ModelFile = modelFile,
            Location = location,
            Namespace = modelFile.Namespace,
        };
        modelFile.Annotations.Add(annotation);

        parser.ConsumeMapping(prop =>
        {
            _ = parser.TryConsume<Scalar>(out var value);

            switch (prop.Value)
            {
                case "name":
                    annotation.Name = new LocatedString(value!);
                    break;
                case "description":
                    annotation.Description = value!.Value;
                    break;
                case "target":
                    annotation.Target = fileChecker.Deserialize<IList<Target>>(parser);
                    break;
                case "global":
                    annotation.Global = value!.Value == "true";
                    break;
                case "parameters":
                    annotation.TemplateParameters = fileChecker.Deserialize<IList<TemplateParameter>>(parser);
                    foreach (var param in annotation.TemplateParameters)
                    {
                        param.Annotation = annotation;
                    }

                    break;
                default:
                    annotation.Implementations[prop.Value] = fileChecker.Deserialize<IList<AnnotationImplementation>>(
                        parser
                    );
                    break;
            }
        });
    }
}
