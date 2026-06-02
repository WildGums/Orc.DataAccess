namespace Orc.DataAccess.Controls;

using Database;

public class ConnectionStringEditContext
{
    public string? ConnectionString { get; set; }
    public DbProviderInfo? Provider { get; set; }
}
