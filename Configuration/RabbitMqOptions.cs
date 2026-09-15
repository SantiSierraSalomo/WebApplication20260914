namespace WebApplication20260914.Configuration;

public sealed class RabbitMqOptions
{
    public const string SectionName = "RabbitMq";

    public string Host { get; set; } = string.Empty;

    public int Port { get; set; }

    public string UserName { get; set; } = "guest";

    public string Password { get; set; } = "guest";
}
