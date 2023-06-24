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
        .SelectMany(df => Config.Tags.Intersect(df.ModelFile.Tags)
            .SelectMany(tag => new[] { Config.GetDataFlowFilePath(df, tag), Config.GetDataFlowRegistrationFilePath(df, tag) }))
        .Distinct();

    public override string Name => "CSharpDataFlowGen";

    protected override void HandleFiles(IEnumerable<ModelFile> files)
    {
        foreach (var file in files)
        {
            foreach (var dataFlow in file.DataFlows)
            {
                foreach (var (tag, fileName) in Config.Tags.Intersect(file.Tags)
                    .Select(tag => (tag, fileName: Config.GetDataFlowFilePath(dataFlow, tag)))
                    .DistinctBy(t => t.fileName))
                {
                    HandleDataFlow(fileName, dataFlow, tag);
                    HandleDataFlowPartial(fileName.Replace($"{Path.DirectorySeparatorChar}generated", string.Empty).Replace(".cs", ".partial.cs"), dataFlow, tag);
                }
            }
        }

        foreach (var g in Files.Values.SelectMany(f => f.DataFlows)
            .SelectMany(df => Config.Tags.Intersect(df.ModelFile.Tags)
                .Select(tag => (tag, df, fileName: Config.GetDataFlowRegistrationFilePath(df, tag))))
            .GroupBy(g => g.fileName))
        {
            HandleRegistrationFile(g.Key, g.Select(i => i.df), g.First().tag);
        }
    }

    private void HandleDataFlow(string fileName, DataFlow dataFlow, string tag)
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

    private void HandleDataFlowPartial(string fileName, DataFlow dataFlow, string tag)
    {
        if (!dataFlow.Sources.Any(s => s.Mode == DataFlowSourceMode.Partial) && !dataFlow.PostQuery && !dataFlow.PreQuery)
        {
            return;
        }

        if (File.Exists(fileName))
        {
            return;
        }

        using var w = new CSharpWriter(fileName, _logger, Config.UseLatestCSharp) { EnableHeader = false };

        w.WriteUsings(new[] { "Kinetix.Etl" }.Concat(dataFlow.Sources.Select(source => Config.GetNamespace(source.Class, GetBestClassTag(source.Class, tag)))).ToArray());
        w.WriteLine();
        w.WriteNamespace(Config.GetNamespace(dataFlow, tag));
        w.WriteClassDeclaration($"{dataFlow.Name.ToPascalCase()}Flow", null);

        if (dataFlow.PostQuery)
        {
            w.WriteLine(2, $"protected override partial async Task<int> ExecutePostQuery(IConnection connection)");
            w.WriteLine(2, "{");
            w.WriteLine(2, "}");
        }

        if (dataFlow.PreQuery)
        {
            if (dataFlow.PostQuery)
            {
                w.WriteLine();
            }

            w.WriteLine(2, $"protected override partial async Task<int> ExecutePreQuery(IConnection connection)");
            w.WriteLine(2, "{");
            w.WriteLine(2, "}");
        }

        var partialSources = dataFlow.Sources.Where(d => d.Mode == DataFlowSourceMode.Partial).OrderBy(s => s.Source);
        foreach (var source in partialSources)
        {
            if (dataFlow.PostQuery || dataFlow.PreQuery || partialSources.ToList().IndexOf(source) > 0)
            {
                w.WriteLine();
            }

            w.WriteLine(2, $"private static partial async Task<IEnumerable<{source.Class.NamePascal}>> Get{source.Source.ToPascalCase()}Source{dataFlow.Sources.OrderBy(s => s.Source).Where(s => s.Source == source.Source).ToList().IndexOf(source) + 1}(IConnection connection)");
            w.WriteLine(2, "{");
            w.WriteLine(2, "}");
        }

        w.WriteLine(1, "}");
        w.WriteNamespaceEnd();
    }

    private void HandleRegistrationFile(string fileName, IEnumerable<DataFlow> flows, string tag)
    {
        var firstFlow = flows.First();
        using var w = new CSharpWriter(fileName, _logger, Config.UseLatestCSharp);

        w.WriteUsings("Kinetix.Etl", "Microsoft.Extensions.DependencyInjection");
        w.WriteLine();
        w.WriteNamespace(Config.GetNamespace(firstFlow, tag));
        w.WriteLine(1, "public static class ServiceExtensions");
        w.WriteLine(1, "{");
        w.WriteLine(2, $"public static IServiceCollection Add{firstFlow.ModelFile.Namespace.ModuleFlat}DataFlows(this IServiceCollection services)");
        w.WriteLine(2, "{");
        w.WriteLine(3, "return services");
        foreach (var flow in flows.OrderBy(f => f.Name))
        {
            w.WriteLine(4, $".AddSingleton<IDataFlow, {flow.Name.ToPascalCase()}Flow>(){(flows.OrderBy(f => f.Name).ToList().IndexOf(flow) == flows.Count() - 1 ? ";" : string.Empty)}");
        }

        w.WriteLine(2, "}");
        w.WriteLine(1, "}");
        w.WriteNamespaceEnd();
    }
}
