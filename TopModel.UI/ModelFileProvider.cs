﻿using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Caching.Memory;
using TopModel.Core;
using TopModel.Core.FileModel;
using TopModel.UI.Graphing;
using TopModel.Utils;

namespace TopModel.UI;

public class ModelFileProvider : IModelWatcher
{
    private readonly IMemoryCache _svgCache;

    public ModelFileProvider(IMemoryCache svgCache)
    {
        _svgCache = svgCache;
    }

    public event EventHandler<string>? FileChanged;

    public event EventHandler? FilesChanged;

    public IDictionary<string, ModelFile> Files { get; set; } = new Dictionary<string, ModelFile>();

    public string Name => "UI";

    public int Number { get; init; }

    public IEnumerable<string>? GeneratedFiles => null;

    public bool Disabled => false;

    public (string Svg, double Width, double Height) GetSvgForFile(string name)
    {
        return _svgCache.GetOrCreate(name, _ =>
        {
            var svg = string.Empty;

            var delay = 0;
            while (!Files.ContainsKey(name))
            {
                Thread.Sleep(100);
                delay += 100;
                if (delay > 10_000)
                {
                    throw new KeyNotFoundException(name.ToString());
                }
            }

            var dotFile = new Digraph(Files[name]).ToString();
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "dot.exe",
                    Arguments = $"-Tsvg",
                    UseShellExecute = false,
                    StandardInputEncoding = new UTF8Encoding(false),
                    StandardOutputEncoding = new UTF8Encoding(false),
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                }
            };
            process.OutputDataReceived += (_, d) => svg += d.Data;
            process.ErrorDataReceived += (_, d) => svg += d.Data;
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.StandardInput.Write(Encoding.UTF8.GetString(Encoding.Default.GetBytes(dotFile)));
            process.StandardInput.Close();
            process.WaitForExit();
            var g = Regex.Match(svg, "<g");
            var size = Regex.Match(svg, "viewBox=\"([\\d \\.]+)\"").Groups[1].Value.Split(" ");
            return (Svg: svg[g.Index..^6], Width: double.Parse(size[2], CultureInfo.InvariantCulture), Height: double.Parse(size[3], CultureInfo.InvariantCulture));
        });
    }

    /// <inheritdoc cref="IModelWatcher.OnErrors" />
    public void OnErrors(IDictionary<ModelFile, IEnumerable<ModelError>> errors)
    {
    }

    /// <inheritdoc cref="IModelWatcher.OnFilesChanged" />
    public void OnFilesChanged(IEnumerable<ModelFile> files, LoggingScope? storeConfig = null)
    {
        foreach (var file in files)
        {
            Files[file.Name] = file;
            _svgCache.Remove(file.Name);
            FileChanged?.Invoke(this, file.Name);
        }

        FilesChanged?.Invoke(this, new EventArgs());
    }
}