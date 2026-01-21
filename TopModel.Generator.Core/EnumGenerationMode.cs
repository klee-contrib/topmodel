using System.Runtime.Serialization;

namespace TopModel.Generator.Core;

public enum EnumGenerationMode
{
    None,

    [EnumMember(Value = "as-const")]
    AsConst,

    [EnumMember(Value = "as-enum")]
    AsEnum,
}
