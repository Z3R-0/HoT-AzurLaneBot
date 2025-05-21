using System.Runtime.Serialization;

namespace Domain.SkinAggregate.Enums;
public enum Shape {
    [EnumMember(Value = "Round")]
    Round,
    [EnumMember(Value = "Teardrop")]
    Teardrop,
    [EnumMember(Value = "Side Set")]
    SideSet,
    [EnumMember(Value = "Slender")]
    Slender,
}
