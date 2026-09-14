using Microsoft.Extensions.Options;

namespace WebApplication20260914.Configuration;

public sealed class SqlServerOptionsValidator : IValidateOptions<SqlServerOptions>
{
    public ValidateOptionsResult Validate(string? name, SqlServerOptions options)
    {
        if (options is null)
        {
            return ValidateOptionsResult.Fail("SqlServer configuration is required.");
        }

        if (string.IsNullOrWhiteSpace(options.ConnectionString))
        {
            return ValidateOptionsResult.Fail("SqlServer:ConnectionString is required.");
        }

        return ValidateOptionsResult.Success;
    }
}
