using System.Runtime.Serialization;

namespace Domain.VisualTraitAggregate.Enums;
public enum TraitType {
    [EnumMember(Value = "Exposure Level")]
    ExposureLevel,  // "Modest", "Revealing"
    [EnumMember(Value = "Pose Style")]
    PoseStyle,      // "Front-Facing", "Side Pose", "Back Turned"
    [EnumMember(Value = "Expression")]
    Expression,     // "Smiling", "Blushing", "Serious"
    [EnumMember(Value = "Accessory")]
    Accessory,      // "Glasses", "Hat", "Gloves", "Necklace"
    [EnumMember(Value = "Skin Line")]
    SkinLine,       // "Maid", "Bunny Girl"
    [EnumMember(Value = "Animation Style")]
    AnimationStyle, // "L2D", "Dynamic" or "Static"
}
