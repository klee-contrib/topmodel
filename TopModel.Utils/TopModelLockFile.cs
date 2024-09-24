namespace TopModel.Utils;

public class TopModelLockFile
{
    public required string Version { get; set; }

    public Dictionary<string, TopModelLockModule> Modules { get; set; } = [];

    public Dictionary<string, string>? Custom { get; set; }

    public List<string> GeneratedFiles { get; set; } = [];
}
