namespace WebApplication20260914.Masterdata.Dtos;

public sealed record CustomerDto(
    string CustomerId,
    string Description,
    string? Country,
    string? City,
    string? Address,
    float? Longitude,
    float? Latitude);

public sealed record CreateCustomerDto(
    string CustomerId,
    string Description,
    string? Country,
    string? City,
    string? Address,
    float? Longitude,
    float? Latitude);

public sealed record UpdateCustomerDto(
    string CustomerId,
    string Description,
    string? Country,
    string? City,
    string? Address,
    float? Longitude,
    float? Latitude);
