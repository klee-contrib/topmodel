namespace TopModel.Core;

public class Variable
{
    public static Variable Property { get; } = new();

    public static Variable PropertyContainer { get; } = new();

    public static Variable Converter { get; } = new();
}
