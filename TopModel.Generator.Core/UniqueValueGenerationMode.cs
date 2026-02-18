using System.Runtime.Serialization;

namespace TopModel.Generator.Core;

public enum UniqueValueGenerationMode
{
    None,

    [EnumMember(Value = "as-const")]
    AsConst,

    [EnumMember(Value = "as-enum")]
    AsEnum,
}
