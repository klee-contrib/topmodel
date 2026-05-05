namespace TopModel.ModelGenerator.Database;

public class DbTableComment
{
    public required string TableName { get; set; }

    public string? Comment { get; set; }
}
