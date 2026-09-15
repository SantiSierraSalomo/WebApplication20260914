namespace WebApplication20260914.Masterdata.Dtos;

public sealed record FamilyDto(string FamilyId, string Description);

public sealed record CreateFamilyDto(string FamilyId, string Description);

public sealed record UpdateFamilyDto(string FamilyId, string Description);
