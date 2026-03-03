using System.Runtime.Serialization;

namespace TopModel.Generator.Core;

[Flags]
public enum UniqueValueGenerationMode
{
    None = 0b0,

    [EnumMember(Value = "const-only")]
    ConstOnly = 0b1,

    [EnumMember(Value = "enum-only")]
    EnumOnly = 0b10,

    [EnumMember(Value = "enum-or-const")]
    EnumOrConst = 0b11,
}
