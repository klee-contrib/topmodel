using System.Runtime.Serialization;

namespace TopModel.Core.Model;

public enum ParamLocation
{
    [EnumMember(Value = "route")]
    Route,

    [EnumMember(Value = "query")]
    Query,

    [EnumMember(Value = "json-body")]
    JsonBody,

    [EnumMember(Value = "form-data")]
    FormData,
}
