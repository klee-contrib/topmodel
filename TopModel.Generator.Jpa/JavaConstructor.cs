namespace TopModel.Generator.Jpa;

public class JavaConstructor : JavaMethod
{
    public JavaConstructor(string returnType)
        : base(returnType, string.Empty)
    {
    }

    public override string Signature => $@"{(!string.IsNullOrEmpty(Visibility) ? $"{Visibility} " : string.Empty)}{(GenericTypes.Count() > 0 ? $"<{string.Join(", ", GenericTypes)}> " : string.Empty)}{ReturnType}({string.Join(", ", Parameters.Select(p => p.Declaration))})";
}