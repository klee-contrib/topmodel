using TopModel.Core;

namespace TopModel.LanguageServer;

/// <summary>
/// Regroupe le ModelStore et sa ModelConfig associée pour un fichier de configuration topmodel.
/// </summary>
public record ModelStoreEntry(ModelStore Store, ModelConfig Config);
