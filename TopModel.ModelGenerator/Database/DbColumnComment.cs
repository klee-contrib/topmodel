namespace TopModel.ModelGenerator.Database;

public class DbColumnComment
{
    public required string TableName { get; set; }

    public required string ColumnName { get; set; }

    public string? Comment { get; set; }
}

