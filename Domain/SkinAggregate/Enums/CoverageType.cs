using System.Runtime.Serialization;

namespace Domain.SkinAggregate.Enums;
public enum CoverageType {
    [EnumMember(Value = "Covered")]
    Covered,
    [EnumMember(Value = "Underboob")]
    Underboob,
    [EnumMember(Value = "Sideboob")]
    Sideboob,
    [EnumMember(Value = "Inner Sideboob")]
    InnerSideboob,
    [EnumMember(Value = "Boob Window")]
    BoobWindow,
    [EnumMember(Value = "Other")]
    Other,
}
