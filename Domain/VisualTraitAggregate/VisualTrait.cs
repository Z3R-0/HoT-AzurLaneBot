﻿using Domain.VisualTraitAggregate.Enums;

namespace Domain.VisualTraitAggregate;
public class VisualTrait : IEntity {
    public Guid Id { get; set; }
    public required TraitType TraitType { get; set; }
    public required string Value { get; set; }
}
