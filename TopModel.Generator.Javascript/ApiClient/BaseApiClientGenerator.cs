using Microsoft.Extensions.Logging;
using TopModel.Core.FileModel;
using TopModel.Core.Model;
using TopModel.Core.Utils;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Javascript.ApiClient;

public abstract class BaseApiClientGenerator(ILogger<BaseApiClientGenerator> logger, IFileWriterProvider writerProvider)
    : EndpointsGeneratorBase<JavascriptConfig>(logger, writerProvider)
{
    protected virtual string EndpointDefinitionPrefix => "export async function ";

    protected virtual int EndpointIndent => 0;

    protected virtual bool UndefinedInReturnType => true;

    protected virtual string ImportSpacing => string.Empty;

    protected virtual string EndpointReturnType => "Promise";

    protected static bool HasReturns(string returnType)
    {
        return returnType != "undefined" && returnType != "void";
    }

    protected override string GetFilePath(ModelFile file, string tag)
    {
        return Config.GetEndpointsFileName(file, tag);
    }

    protected abstract IEnumerable<(string Import, string Path)> GetFrameworkImports(
        string filePath,
        string tag,
        IList<Endpoint> endpoints
    );

    protected string GetReturnType(IProperty? returns)
    {
        return returns != null ? Config.GetType(returns) : "void";
    }

    protected IProperty? GetReturns(Endpoint endpoint)
    {
        return Config.GetReturns(endpoint);
    }

    protected string GetSafeVariableName(Endpoint endpoint, string varName)
    {
        while (Config.GetParams(endpoint).Any(p => p.NameCamel == varName))
        {
            varName = $"_{varName}";
        }

        return varName;
    }

    protected override void HandleFile(string filePath, string fileName, string tag, IList<Endpoint> endpoints)
    {
        using var fw = OpenFileWriter(filePath, encoderShouldEmitUTF8Identifier: false);

        var frameworkImports = GetFrameworkImports(filePath, tag, endpoints).GroupAndSort();
        var endpointImports = Config.GetEndpointImports(filePath, endpoints, tag).GroupAndSort();

        foreach (var (import, path) in frameworkImports)
        {
            fw.WriteLine(
                $@"import {(import.StartsWith('*') ? import[1..] : $"{{{ImportSpacing}{import}{ImportSpacing}}}")} from ""{path}"";"
            );
        }

        if (frameworkImports.Count > 0 && endpointImports.Count > 0)
        {
            fw.WriteLine();
        }

        foreach (var (import, path) in endpointImports)
        {
            fw.WriteLine($@"import {{{ImportSpacing}{import}{ImportSpacing}}} from ""{path}"";");
        }

        WriteClassDeclarationStart(fw, fileName);

        foreach (var endpoint in endpoints)
        {
            WriteEndpoint(fw, endpoint);
        }

        WriteClassDeclarationEnd(fw);

        if (endpoints.Any(e => e.IsMultipart))
        {
            fw.WriteLine(
                @"
function fillFormData(data: any, formData: FormData, prefix = """") {
    if (Array.isArray(data)) {
        for (const [i, item] of data.entries()) {
            fillFormData(item, formData, prefix + (typeof item === ""object"" && !(item instanceof File) ? `[${i}]` : """"));
        }
    } else if (typeof data === ""object"" && !(data instanceof File)) {
        for (const key in data) {
            fillFormData(data[key], formData, (prefix ? `${prefix}.` : """") + key);
        }
    } else {
        formData.append(prefix, data);
    }
}"
            );
        }
    }

    protected virtual void WriteClassDeclarationEnd(IFileWriter fw) { }

    protected virtual void WriteClassDeclarationStart(IFileWriter fw, string fileName) { }

    protected abstract void WriteEndpoint(IFileWriter fw, Endpoint endpoint);

    protected void WriteEndpointSignature(
        IFileWriter fw,
        Endpoint endpoint,
        params IList<(string Name, string? Description, string Type, string? DefaultValue)> extraParams
    )
    {
        fw.WriteLine();
        fw.WriteLine(EndpointIndent, "/**");
        fw.WriteLine(EndpointIndent, $" * {endpoint.Description}");

        foreach (var param in Config.GetParams(endpoint))
        {
            fw.WriteLine(EndpointIndent, $" * @param {param.GetParamName()} {param.Comment}");
        }
        foreach (var param in extraParams.Where(p => p.Description != null))
        {
            fw.WriteLine(EndpointIndent, $" * @param {param.Name} {param.Description}");
        }

        var returns = GetReturns(endpoint);
        var returnType = GetReturnType(returns);
        var hasReturns = HasReturns(returnType);

        if (hasReturns)
        {
            fw.WriteLine(EndpointIndent, $" * @returns {returns!.Comment}");
        }

        fw.WriteLine(EndpointIndent, " */");
        fw.Write(EndpointIndent, $"{EndpointDefinitionPrefix}{endpoint.NameCamel}(");

        var @params = Config.GetParams(endpoint).ToList();
        foreach (var param in @params)
        {
            var defaultValue = Config.GetValue(param);
            fw.Write(
                $"{param.GetParamName()}{(!param.Required && defaultValue == "undefined" ? "?" : string.Empty)}: {Config.GetType(param)}{(defaultValue != "undefined" ? $" = {defaultValue}" : string.Empty)}{(@params[^1] != param || extraParams.Count > 0 ? ", " : string.Empty)}"
            );
        }

        foreach (var param in extraParams)
        {
            fw.Write(
                $"{param.Name}{(param.DefaultValue == null ? "?" : string.Empty)}: {param.Type}{(param.DefaultValue != null ? $" = {param.DefaultValue}" : string.Empty)}{(extraParams[^1] != param ? ", " : string.Empty)}"
            );
        }

        fw.WriteLine(
            $"): {EndpointReturnType}<{returnType + (hasReturns && !returns!.Required && UndefinedInReturnType ? " | undefined" : string.Empty)}> {{"
        );
    }

    protected void WriteFormDataBody(IFileWriter fw, Endpoint endpoint, string body)
    {
        if (endpoint.IsMultipart)
        {
            fw.WriteLine(EndpointIndent + 1, $"const {body} = new FormData();");
            fw.WriteLine(EndpointIndent + 1, "fillFormData(");
            fw.WriteLine(EndpointIndent + 2, "{");

            foreach (var param in endpoint.GetFormDataParams(Config))
            {
                if (param is not { Composition: not null })
                {
                    fw.Write(EndpointIndent + 3, $@"{param.GetParamName()}");
                }
                else
                {
                    fw.Write(EndpointIndent + 3, $@"...{param.GetParamName()}");
                }

                if (Config.GetParams(endpoint).Last() != param)
                {
                    fw.WriteLine(",");
                }
                else
                {
                    fw.WriteLine();
                }
            }

            fw.WriteLine(EndpointIndent + 2, "},");
            fw.WriteLine(EndpointIndent + 2, body);
            fw.WriteLine(EndpointIndent + 1, ");");
        }
    }
}
