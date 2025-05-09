using System.Runtime.Serialization;

namespace Domain.ShipAggregate.Enums;
public enum Rarity {
    [EnumMember(Value = "Common")]
    C,
    [EnumMember(Value = "Rare")]
    R,
    [EnumMember(Value = "Elite")]
    E,
    [EnumMember(Value = "Super Rare")]
    SR,
    [EnumMember(Value = "Ultra Rare")]
    UR,
    [EnumMember(Value = "Priority Research")]
    PR,
    [EnumMember(Value = "Decisive Research")]
    DR,
}
