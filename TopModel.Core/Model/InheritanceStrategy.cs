using System.Runtime.Serialization;

namespace TopModel.Core.Model;

public enum InheritanceStrategy
{
    [EnumMember(Value = "joined-tables")]
    JoinedTables,

    [EnumMember(Value = "single-table")]
    SingleTable,

    [EnumMember(Value = "distinct-tables")]
    DistinctTables,
}
