namespace Orc.DataAccess.Tests;

using System.Data.Common;

public class TestDbProviderFactory : DbProviderFactory
{
    private readonly TestDbConnectionStringBuilder _testDbConnectionStringBuilder = new();

    public bool IsConnectionValid { get; set; } = true;

    public void AddPropertyDescription<T>(string name)
        => _testDbConnectionStringBuilder.AddPropertyDescription<T>(name);

    public override DbConnectionStringBuilder? CreateConnectionStringBuilder()
        => _testDbConnectionStringBuilder;

    public override DbConnection? CreateConnection() => new TestDbConnection
    {
        IsValid = IsConnectionValid
    };
}