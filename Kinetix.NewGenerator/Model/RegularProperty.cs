namespace Kinetix.NewGenerator.Model
{
    public class RegularProperty : IProperty
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public bool PrimaryKey { get; set; }
        public bool Required { get; set; }
        public Domain Domain { get; set; }
        public string DefaultValue { get; set; }
        public string Comment { get; set; }
    }
}
