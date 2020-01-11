namespace TopModel.Generator
{
    public interface IGenerator
    {
        string Name { get; }
        bool CanGenerate { get; }
        void Generate();
    }
}
