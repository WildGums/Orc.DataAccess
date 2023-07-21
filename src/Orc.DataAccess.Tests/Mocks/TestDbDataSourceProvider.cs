namespace Orc.DataAccess.Tests;

using System;
using System.Collections.Generic;
using Database;

public class TestDbDataSourceProvider : IDbDataSourceProvider
{
    private readonly DbProvider _provider;
    private readonly List<DbDataSource> _dataSources = new();

    public TestDbDataSourceProvider(DbProvider provider)
    {
        ArgumentNullException.ThrowIfNull(provider);

        _provider = provider;
    }

    public void AddDataSource(string name)
    {
        _dataSources.Add(new DbDataSource(_provider.ProviderInvariantName, name));
    }

    public IList<DbDataSource> GetDataSources() => _dataSources;
}