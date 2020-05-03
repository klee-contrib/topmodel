using System.Collections.Generic;

#nullable disable
namespace TopModel.Generator
{
    public abstract class GeneratorConfigBase
    {
        public IList<string> Kinds { get; set; }
    }
}
