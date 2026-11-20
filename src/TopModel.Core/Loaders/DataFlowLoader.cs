using TopModel.Core.FileModel;
using TopModel.Core.Model;
using TopModel.Utils;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace TopModel.Core.Loaders;

public class DataFlowLoader : ILoader
{
    /// <inheritdoc cref="ILoader.Load" />
    public void Load(Parser parser, ModelFile modelFile, ModelFileLoadConfig config, Declaration declaration)
    {
        var dataFlow = new DataFlow(declaration) { ModelFile = modelFile };
        modelFile.DataFlows.Add(dataFlow);
        parser.ConsumeMapping(prop =>
        {
            _ = parser.TryConsume<Scalar>(out var value);

            switch (prop.Value)
            {
                case "name":
                    dataFlow.Name = new LocatedString(value!);
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
                case "activeProperty":
                    dataFlow.ActivePropertyReference = new PropertyReference(value!);
                    break;
                case "hooks":
                    parser.ConsumeSequence(() =>
                    {
                        string value = parser.Consume<Scalar>().Value;
                        FlowHook? hook = null;
                        switch (value)
                        {
                            case "beforeFlow":
                                hook = FlowHook.BeforeFlow;
                                break;
                            case "afterSource":
                                hook = FlowHook.AfterSource;
                                break;
                            case "map":
                                hook = FlowHook.Map;
                                break;
                            case "beforeTarget":
                                hook = FlowHook.BeforeTarget;
                                break;
                            case "afterFlow":
                                hook = FlowHook.AfterFlow;
                                break;
                        }

                        if (hook != null)
                        {
                            dataFlow.Hooks.Add((FlowHook)hook);
                        }
                    });
                    break;
                case "sources":
                    parser.ConsumeSequence(() =>
                    {
                        var source = new DataFlowSource { DataFlow = dataFlow };
                        parser.ConsumeMapping(prop =>
                        {
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
                                        source.JoinPropertyReferences.Add(
                                            new PropertyReference(parser.Consume<Scalar>())
                                        );
                                    });
                                    break;
                                case "innerJoin":
                                    source.InnerJoin = value!.Value == "true";
                                    break;
                                default:
                                    throw ModelException.From(
                                        CoreMessage.PropertyInvalidForDataFlowSource,
                                        dataFlow,
                                        new Declaration(prop),
                                        prop.Value
                                    );
                            }
                        });
                        dataFlow.Sources.Add(source);
                    });
                    break;
                default:
                    throw ModelException.From(
                        CoreMessage.PropertyInvalidForDataFlow,
                        dataFlow,
                        new Declaration(prop),
                        prop.Value
                    );
            }
        });
    }
}
