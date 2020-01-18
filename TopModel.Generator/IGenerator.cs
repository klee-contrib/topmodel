using TopModel.Core.FileModel;

namespace TopModel.Generator
{
    public interface IGenerator
    {
        bool CanGenerate { get; }
        string Name { get; }
        void GenerateAll();
        void GenerateFromFile(ModelFile file);
    }
} 
