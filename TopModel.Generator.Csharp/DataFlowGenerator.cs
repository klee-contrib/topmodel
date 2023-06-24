using System.Data;
using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Core.FileModel;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Csharp;

public class DataFlowGenerator : GeneratorBase<CsharpConfig>
{
    private readonly ILogger<DataFlowGenerator> _logger;

    public DataFlowGenerator(ILogger<DataFlowGenerator> logger)
        : base(logger)
    {
        _logger = logger;
    }

    public override IEnumerable<string> GeneratedFiles => Files.Values.SelectMany(f => f.DataFlows)
        .SelectMany(df => Config.Tags.Intersect(df.ModelFile.Tags).Select(tag => Config.GetDataFlowFilePath(df, tag)))
        .Distinct();

    public override string Name => "CSharpDataFlowGen";

    protected void HandleDataFlow(string fileName, DataFlow dataFlow, string tag)
    {
        int GetSourceNumber(DataFlowSource source)
        {
            return dataFlow.Sources.OrderBy(s => s.Source).Where(s => s.Source == source.Source).ToList().IndexOf(source) + 1;
        }

        string GetConnectionName(DataFlowSource source)
        {
            return $"_{source.Source.ToCamelCase()}Connection{GetSourceNumber(source)}";
        }

        using var w = new CSharpWriter(fileName, _logger, Config.UseLatestCSharp);

        var usings = new List<string>()
        {
            "Kinetix.Etl",
            "Microsoft.Extensions.Logging",
            Config.GetNamespace(dataFlow.Class, tag)
        };

        foreach (var source in dataFlow.Sources)
        {
            usings.Add(Config.GetNamespace(source.Class, GetBestClassTag(source.Class, tag)));
        }

        w.WriteUsings(usings.ToArray());
        w.WriteLine();
        w.WriteNamespace(Config.GetNamespace(dataFlow, tag));

        var name = $"{dataFlow.Name.ToPascalCase()}Flow";

        w.WriteClassDeclaration(name, $"DataFlow<{dataFlow.Class.NamePascal}>");

        foreach (var source in dataFlow.Sources.OrderBy(s => s.Source))
        {
            w.WriteLine(2, $"private IConnection {GetConnectionName(source)};");
        }

        w.WriteLine();

        w.WriteLine(2, $"public {name}(ILogger<{name}> logger, ConnectionPool connectionPool, EtlMonitor monitor)");
        w.WriteLine(3, ": base(logger, connectionPool, monitor)");
        w.WriteLine(2, "{");
        w.WriteLine(2, "}");

        w.WriteLine();
        w.WriteLine(2, $"public override string Name => \"{dataFlow.Name.ToPascalCase()}\";");
        w.WriteLine();
        w.WriteLine(2, $"protected override TargetMode TargetMode => TargetMode.{dataFlow.Type};");

        if (dataFlow.ActiveProperty != null)
        {
            w.WriteLine();
            w.WriteLine(2, $"protected override string ActiveProperty => nameof({dataFlow.Class.NamePascal}.{dataFlow.ActiveProperty.NamePascal});");
        }

        w.WriteLine();
        w.WriteLine(2, $"protected override string TargetName => \"{dataFlow.Target.ToCamelCase()}\";");

        if (dataFlow.DependsOn.Any())
        {
            w.WriteLine();
            w.WriteLine(2, $"public override string[] DependsOn => new[] {{ {string.Join(", ", dataFlow.DependsOn.Select(d => $"\"{d.Name.ToPascalCase()}\""))} }};");
        }

        if (dataFlow.PostQuery)
        {
            w.WriteLine();
            w.WriteLine(2, $"protected override bool PostQuery => true;");
        }

        if (dataFlow.PreQuery)
        {
            w.WriteLine();
            w.WriteLine(2, $"protected override bool PreQuery => true;");
        }

        w.WriteLine();
        w.WriteLine(2, "public override void Dispose()");
        w.WriteLine(2, "{");
        w.WriteLine(3, "base.Dispose();");

        foreach (var source in dataFlow.Sources.OrderBy(s => s.Source))
        {
            w.WriteLine(3, $"{GetConnectionName(source)}?.Dispose();");
        }

        w.WriteLine(2, "}");

        w.WriteLine();
        w.WriteLine(2, $"protected override async Task<IEnumerable<{dataFlow.Class.NamePascal}>> GetData()");
        w.WriteLine(2, "{");

        foreach (var source in dataFlow.Sources.OrderBy(s => s.Source))
        {
            w.WriteLine(3, $"{GetConnectionName(source)} = ConnectionPool.GetConnection(\"{source.Source.ToCamelCase()}\");");
        }

        if (dataFlow.Sources.Count == 1)
        {
            var source = dataFlow.Sources.First();
            w.WriteLine();
            w.Write(3, $"return ");

            if (source.Class != dataFlow.Class)
            {
                w.Write("(");
            }

            w.Write($"await Get{source.Source.ToPascalCase()}Source{GetSourceNumber(source)}({GetConnectionName(source)})");
            w.WriteLine(source.Class != dataFlow.Class ? ")" : ";");

            if (source.Class != dataFlow.Class && source.TargetFromMapper != null)
            {
                var (ns, modelPath) = Config.GetMapperLocation((dataFlow.Class, source.TargetFromMapper), GetBestClassTag(dataFlow.Class, tag));
                w.WriteLine(4, $".Select({Config.GetMapperName(ns, modelPath)}.Create{dataFlow.Class.NamePascal});");
            }
        }
        else if (dataFlow.Sources.Count > 1)
        {
            if (dataFlow.Sources.All(source => !source.JoinProperties.Any()))
            {
                w.WriteLine();
                w.WriteLine(3, "return (await Task.WhenAll(");
                foreach (var source in dataFlow.Sources.OrderBy(s => s.Source))
                {
                    w.Write(4, $"Get{source.Source.ToPascalCase()}Source{GetSourceNumber(source)}({GetConnectionName(source)})");
                    if (dataFlow.Sources.OrderBy(s => s.Source).ToList().IndexOf(source) < dataFlow.Sources.Count - 1)
                    {
                        w.WriteLine(",");
                    }
                    else
                    {
                        w.WriteLine("))");
                    }
                }

                w.WriteLine(3, ".SelectMany(s => s);");
            }
            else if (dataFlow.Sources.All(source => source.JoinProperties.Any()))
            {
                string GetVarName(DataFlowSource source)
                {
                    return $"{source.Source.ToCamelCase()}{GetSourceNumber(source)}";
                }

                string GetJoin(DataFlowSource source)
                {
                    if (source.JoinProperties.Count == 1)
                    {
                        return $"{GetVarName(source)}.{source.JoinProperties.Single().NamePascal}";
                    }

                    return $"({string.Join(", ", source.JoinProperties.Select(jp => $"{GetVarName(source)}.{jp.NamePascal}"))})";
                }

                foreach (var source in dataFlow.Sources.Skip(1))
                {
                    var varName = GetVarName(source);
                    w.WriteLine();
                    w.WriteLine(3, $"var {source.Source.ToCamelCase()}Source{GetSourceNumber(source)} = (await Get{source.Source.ToPascalCase()}Source{GetSourceNumber(source)}({GetConnectionName(source)}))");
                    w.WriteLine(4, $".ToDictionary({varName} => {GetJoin(source)}, {varName} => {varName});");
                }

                w.WriteLine();

                var mainSource = dataFlow.Sources.First();
                w.WriteLine(3, $"return (await Get{mainSource.Source.ToPascalCase()}Source{GetSourceNumber(mainSource)}({GetConnectionName(mainSource)}))");

                foreach (var source in dataFlow.Sources.Skip(1))
                {
                    w.WriteLine(4, $".Select({GetVarName(mainSource)} => {source.Source.ToCamelCase()}Source{GetSourceNumber(source)}.TryGetValue({GetJoin(mainSource)}, out var {GetVarName(source)})");
                    w.WriteLine(5, $"? {GetVarName(source)}.{source.FirstSourceToMapper?.Name.ToPascalCase()}({GetVarName(mainSource)})");
                    w.WriteLine(5, $": {GetVarName(mainSource)}){(dataFlow.Sources.Skip(1).ToList().IndexOf(source) == dataFlow.Sources.Count - 2 ? ";" : string.Empty)}");
                }
            }
        }

        w.WriteLine(2, "}");

        if (dataFlow.PostQuery)
        {
            w.WriteLine();
            w.WriteLine(2, $"protected override partial Task<int> ExecutePostQuery(IConnection connection);");
        }

        if (dataFlow.PreQuery)
        {
            w.WriteLine();
            w.WriteLine(2, $"protected override partial Task<int> ExecutePreQuery(IConnection connection);");
        }

        foreach (var source in dataFlow.Sources.OrderBy(s => s.Source))
        {
            w.WriteLine();
            w.WriteLine(2, $"private static {(source.Mode == DataFlowSourceMode.Partial ? "partial" : "async")} Task<IEnumerable<{source.Class.NamePascal}>> Get{source.Source.ToPascalCase()}Source{GetSourceNumber(source)}(IConnection connection){(source.Mode == DataFlowSourceMode.Partial ? ";" : string.Empty)}");
            if (source.Mode == DataFlowSourceMode.QueryAll)
            {
                w.WriteLine(2, "{");
                w.WriteLine(3, $"return await connection.QueryAllAsync<{source.Class.NamePascal}>();");
                w.WriteLine(2, "}");
            }
        }

        w.WriteLine(1, "}");
        w.WriteNamespaceEnd();
    }

    protected override void HandleFiles(IEnumerable<ModelFile> files)
    {
        foreach (var file in files)
        {
            foreach (var classe in file.DataFlows)
            {
                foreach (var (tag, fileName) in Config.Tags.Intersect(file.Tags)
                     .Select(tag => (tag, fileName: Config.GetDataFlowFilePath(classe, tag)))
                     .DistinctBy(t => t.fileName))
                {
                    HandleDataFlow(fileName, classe, tag);
                }
            }
        }
    }
}
