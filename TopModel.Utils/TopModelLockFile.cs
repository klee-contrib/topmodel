namespace TopModel.Utils;

public class TopModelLockFile
{
    public required string Version { get; set; }

    public Dictionary<string, string> Modules { get; set; } = [];

    public List<string> GeneratedFiles { get; set; } = [];
}
