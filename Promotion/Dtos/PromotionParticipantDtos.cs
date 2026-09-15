namespace WebApplication20260914.Promotion.Dtos;

public sealed record PromotionParticipantDto(
    string PromotionId,
    string ItemId,
    string CustomerId,
    float BaseFrcQty,
    float Uplift);

public sealed record CreatePromotionParticipantDto(
    string PromotionId,
    string ItemId,
    string CustomerId,
    float BaseFrcQty,
    float Uplift);

public sealed record UpdatePromotionParticipantDto(
    string PromotionId,
    string ItemId,
    string CustomerId,
    float BaseFrcQty,
    float Uplift);
