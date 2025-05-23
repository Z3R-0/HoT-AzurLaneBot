﻿using Domain.SkinAggregate;

namespace Domain.VisualTraitAggregate;

public class SkinVisualTrait : IEntity {
    public Guid Id { get; set; }
    public Guid SkinId { get; set; }
    public required Skin Skin { get; set; }

    public Guid VisualTraitId { get; set; }
    public required VisualTrait VisualTrait { get; set; }
}
