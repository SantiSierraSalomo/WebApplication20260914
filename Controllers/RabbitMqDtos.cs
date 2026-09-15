namespace WebApplication20260914.Controllers;

public sealed record RabbitMqPublishRequestDto(
    string RoutingKey,
    string Header1,
    string Header2,
    string UserId,
    string AppId,
    string Body);

public sealed record RabbitMqReadResponseDto(
    ulong DeliveryTag,
    string RoutingKey,
    Dictionary<string, string> Headers,
    Dictionary<string, string> Props,
    string Body);
