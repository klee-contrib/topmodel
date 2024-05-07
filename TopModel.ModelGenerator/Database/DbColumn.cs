namespace TopModel.ModelGenerator.Database;

public class DbColumn
{
    public required string TableName { get; set; }

    public required string ColumnName { get; set; }

    public required string DataType { get; set; }

    public required string Precision { get; set; }

    public required string Scale { get; set; }

    public bool Nullable { get; set; }
}
