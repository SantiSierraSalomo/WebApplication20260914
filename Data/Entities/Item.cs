using System;
using System.Collections.Generic;

namespace WebApplication20260914.Data.Entities;

public partial class Item
{
    public string ItemId { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? FamilyId { get; set; }

    public float UnitCost { get; set; }

    public float UnitVol { get; set; }
}
