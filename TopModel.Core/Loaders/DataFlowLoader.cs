using TopModel.Core.FileModel;
using TopModel.Utils;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace TopModel.Core.Loaders;

public class DataFlowLoader : ILoader<DataFlow>
{
    /// <inheritdoc cref="ILoader{T}.Load" />
    public DataFlow Load(Parser parser)
    {
        var dataFlow = new DataFlow();

        parser.ConsumeMapping(() =>
        {
            var prop = parser.Consume<Scalar>();
            _ = parser.TryConsume<Scalar>(out var value);

            switch (prop.Value)
            {
                case "name":
                    dataFlow.Name = new LocatedString(value);
                    break;
                case "target":
                    dataFlow.Target = value!.Value;
                    break;
                case "class":
                    dataFlow.ClassReference = new ClassReference(value!);
                    break;
                case "type":
                    dataFlow.Type = Enum.Parse<DataFlowType>(value!.Value.ToPascalCase());
                    break;
                case "dependsOn":
                    parser.ConsumeSequence(() =>
                    {
                        dataFlow.DependsOnReference.Add(new DataFlowReference(parser.Consume<Scalar>()));
                    });
                    break;
                case "postQuery":
                    dataFlow.PostQuery = value!.Value == "true";
                    break;
                case "preQuery":
                    dataFlow.PreQuery = value!.Value == "true";
                    break;
                case "activeProperty":
                    dataFlow.ActivePropertyReference = new Reference(value!);
                    break;
                case "sources":
                    parser.ConsumeSequence(() =>
                    {
                        var source = new DataFlowSource();
                        parser.ConsumeMapping(() =>
                        {
                            var prop = parser.Consume<Scalar>();
                            _ = parser.TryConsume<Scalar>(out var value);

                            switch (prop.Value)
                            {
                                case "source":
                                    source.Source = value!.Value;
                                    break;
                                case "class":
                                    source.ClassReference = new ClassReference(value!);
                                    break;
                                case "mode":
                                    source.Mode = Enum.Parse<DataFlowSourceMode>(value!.Value.ToPascalCase());
                                    break;
                                case "joinProperties":
                                    parser.ConsumeSequence(() =>
                                    {
                                        source.JoinPropertyReferences.Add(new Reference(parser.Consume<Scalar>()));
                                    });
                                    break;
                                case "innerJoin":
                                    source.InnerJoin = value!.Value == "true";
                                    break;
                                default:
                                    throw new ModelException(dataFlow, $"Propriété ${prop} inconnue pour une source de flux de données");
                            }
                        });
                        dataFlow.Sources.Add(source);
                    });
                    break;
                default:
                    throw new ModelException(dataFlow, $"Propriété ${prop} inconnue pour un flux de données");
            }
        });

        return dataFlow;
    }
}