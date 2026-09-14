using Microsoft.Extensions.Options;

namespace WebApplication20260914.Configuration;

public sealed class RabbitMqOptionsValidator : IValidateOptions<RabbitMqOptions>
{
    public ValidateOptionsResult Validate(string? name, RabbitMqOptions options)
    {
        if (options is null)
        {
            return ValidateOptionsResult.Fail("RabbitMq configuration is required.");
        }

        if (string.IsNullOrWhiteSpace(options.Host))
        {
            return ValidateOptionsResult.Fail("RabbitMq:Host is required.");
        }

        if (options.Port is < 1 or > 65535)
        {
            return ValidateOptionsResult.Fail("RabbitMq:Port must be between 1 and 65535.");
        }

        return ValidateOptionsResult.Success;
    }
}
