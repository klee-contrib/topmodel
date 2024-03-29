﻿using TopModel.Core.FileModel;
using TopModel.Utils;

namespace TopModel.Core;

public interface IModelWatcher
{
    string Name { get; }

    int Number { get; }

    string FullName => $"{Name.PadRight(18, '.')}@{Number}";

    IEnumerable<string>? GeneratedFiles { get; }

    bool Disabled { get; }

    void OnErrors(IDictionary<ModelFile, IEnumerable<ModelError>> errors);

    void OnFilesChanged(IEnumerable<ModelFile> files, LoggingScope? storeConfig = null);
}