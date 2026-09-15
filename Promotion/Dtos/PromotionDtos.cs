namespace WebApplication20260914.Promotion.Dtos;

public sealed record PromotionDto(
    string PromotionId,
    string? Description,
    DateOnly? SellInStart,
    DateOnly? SellInEnd,
    DateOnly? SellOutStart,
    DateOnly? SellOutEnd);

public sealed record CreatePromotionDto(
    string PromotionId,
    string? Description,
    DateOnly? SellInStart,
    DateOnly? SellInEnd,
    DateOnly? SellOutStart,
    DateOnly? SellOutEnd);

public sealed record UpdatePromotionDto(
    string PromotionId,
    string? Description,
    DateOnly? SellInStart,
    DateOnly? SellInEnd,
    DateOnly? SellOutStart,
    DateOnly? SellOutEnd);
