using System;
using System.Collections.Generic;

namespace WebApplication20260914.Data.Entities;

public partial class PromotionParticipant
{
    public string PromotionId { get; set; } = null!;

    public string ItemId { get; set; } = null!;

    public string CustomerId { get; set; } = null!;

    public float BaseFrcQty { get; set; }

    public float Uplift { get; set; }
}
