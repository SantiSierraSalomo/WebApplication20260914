using System;
using System.Collections.Generic;

namespace WebApplication20260914.Data.Entities;

public partial class Customer
{
    public string CustomerId { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? Country { get; set; }

    public string? City { get; set; }

    public string? Address { get; set; }

    public float? Longitude { get; set; }

    public float? Latitude { get; set; }
}
