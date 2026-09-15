using System;
using System.Collections.Generic;

namespace WebApplication20260914.Data.Entities;

public partial class Promotion
{
    public string PromotionId { get; set; } = null!;

    public string? Description { get; set; }

    public DateOnly? SellInStart { get; set; }

    public DateOnly? SellInEnd { get; set; }

    public DateOnly? SellOutStart { get; set; }

    public DateOnly? SellOutEnd { get; set; }
}
