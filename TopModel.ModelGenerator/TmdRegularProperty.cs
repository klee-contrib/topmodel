namespace TopModel.ModelGenerator;

public class TmdRegularProperty : TmdProperty
{
#nullable disable
    public string SqlName { get; set; } = string.Empty;

    public bool Required { get; set; }

    public bool PrimaryKey { get; set; }
}