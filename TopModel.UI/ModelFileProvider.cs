using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using TopModel.Core.FileModel;
using TopModel.UI.Graphing;
using Microsoft.Extensions.Caching.Memory;

namespace TopModel.UI
{
    public class ModelFileProvider : IModelWatcher
    {
        private readonly IMemoryCache _svgCache;

        public ModelFileProvider(IMemoryCache svgCache)
        {
            _svgCache = svgCache;
        }

        public event EventHandler? FilesChanged;

        public event EventHandler<FileName>? FileChanged;

        public IDictionary<FileName, ModelFile> Files { get; set; } = new Dictionary<FileName, ModelFile>();

        public string Name => "UI";

        public void OnFilesChanged(IEnumerable<ModelFile> files)
        {
            foreach (var file in files)
            {
                Files[file.Name] = file;
                _svgCache.Remove(file.Name);
                FileChanged?.Invoke(this, file.Name);
            }

            FilesChanged?.Invoke(this, new EventArgs());
        }

        public string GetSvgForFile(FileName name)
        {
            return _svgCache.GetOrCreate(name, _ =>
            {
                var svg = string.Empty;
                var dotFile = new Digraph(Files[name]).ToString();
                var process = new Process();
                process.StartInfo.FileName = "dot.exe";
                process.StartInfo.Arguments = $"-Tsvg";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.StandardInputEncoding = new UTF8Encoding(false);
                process.StartInfo.StandardOutputEncoding = new UTF8Encoding(false);
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.OutputDataReceived += (_, d) => svg += d.Data;
                process.ErrorDataReceived += (_, d) => svg += d.Data;
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.StandardInput.Write(Encoding.UTF8.GetString(Encoding.Default.GetBytes(dotFile)));
                process.StandardInput.Close();
                process.WaitForExit();
                return svg;
            });
        }
    }
}