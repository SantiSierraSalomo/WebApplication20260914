namespace WebApplication20260914.Masterdata.Dtos;

public sealed record ItemDto(string ItemId, string Description, string? FamilyId, float UnitCost, float UnitVol);

public sealed record CreateItemDto(string ItemId, string Description, string? FamilyId, float UnitCost, float UnitVol);

public sealed record UpdateItemDto(string ItemId, string Description, string? FamilyId, float UnitCost, float UnitVol);
