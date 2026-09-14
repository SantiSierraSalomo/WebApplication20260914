namespace WebApplication20260914.Configuration;

public sealed class SqlServerOptions
{
    public const string SectionName = "SqlServer";

    public string ConnectionString { get; set; } = string.Empty;
}
