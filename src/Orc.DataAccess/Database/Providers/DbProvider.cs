namespace Orc.DataAccess.Database;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Catel;
using Catel.Collections;
using Catel.Logging;
using Microsoft.Extensions.Logging;

public class DbProvider
{
    private static readonly ILogger Logger = LogManager.GetLogger(typeof(DbProvider));

    private static readonly Dictionary<string, DbProvider> Providers = new();
    private static readonly DbProviderFactoryRepository ProviderFactoryRepository = new();

    private static bool IsProvidersInitialized;

    private Type? _connectionType;
    private DbProviderFactory? _dbProviderFactory;
    private DbProviderInfo? _info;
    private readonly IServiceProvider _serviceProvider;

    public DbProvider(DbProviderInfo info, IServiceProvider serviceProvider)
        : this(info.InvariantName, serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(info);

        _info = info;
        _serviceProvider = serviceProvider;
    }

    public DbProvider(string providerInvariantName, IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(providerInvariantName);

        ProviderInvariantName = providerInvariantName;
        _serviceProvider = serviceProvider;
    }

    protected DbProviderFactory DbProviderFactory => _dbProviderFactory ??= DbProviderFactories.GetFactory(ProviderInvariantName);
#pragma warning disable IDISP004 // Don't ignore created IDisposable.
    public virtual Type ConnectionType => _connectionType ??= DbProviderFactory.CreateConnection()?.GetType() 
                                                              ?? throw Logger.LogErrorAndCreateException<InvalidOperationException>("Failed to get 'ConnectionType' value");
#pragma warning restore IDISP004 // Don't ignore created IDisposable.
    public virtual DbProviderInfo Info => GetInfo();
    public string? Dialect { get; }
    public string ProviderInvariantName { get; }

    public static void RegisterProvider(DbProviderInfo providerInfo)
    {
        ArgumentNullException.ThrowIfNull(providerInfo);

        ProviderFactoryRepository.Add(providerInfo);
    }

    public static void UnregisterProvider(DbProviderInfo providerInfo)
    {
        ArgumentNullException.ThrowIfNull(providerInfo);

        IsProvidersInitialized = false;

        ProviderFactoryRepository.Remove(providerInfo);
    }

    public static void RegisterCustomProvider(DbProvider provider)
    {
        ArgumentNullException.ThrowIfNull(provider);

        Providers[provider.ProviderInvariantName] = provider;
    }

    public static DbProvider GetRegisteredProvider(string invariantName, IServiceProvider serviceProvider)
    {
        var registeredProviders = GetRegisteredProviders(serviceProvider);
        if (registeredProviders.TryGetValue(invariantName, out var dbProvider))
        {
            return dbProvider;
        }

        throw Logger.LogErrorAndCreateException<InvalidOperationException>("Provider with name '{InvariantName}' is not registered", invariantName);
    }

    public static IReadOnlyDictionary<string, DbProvider> GetRegisteredProviders(IServiceProvider serviceProvider)
    {
        var providers = Providers;
        if (IsProvidersInitialized)
        {
            return providers;
        }

        providers.Clear();

        using var dataTable = DbProviderFactories.GetFactoryClasses();
        dataTable.Rows.OfType<DataRow>()
            .Select(x => x.ToDbProviderInfo())
            .OrderBy(x => x.Name)
            .Select(x => new DbProvider(x, serviceProvider))
            .ForEach(x => providers[x.ProviderInvariantName] = x);

        IsProvidersInitialized = true;

        return providers;
    }

    public virtual DbConnection? CreateConnection()
    {
        var connection = DbProviderFactory.CreateConnection();
        if (_connectionType is not null || connection is null)
        {
            return connection;
        }

        _connectionType = connection.GetType();
        this.ConnectType<DbConnection>(_serviceProvider, _connectionType);

        return connection;
    }

    public virtual DbConnectionString? CreateConnectionString(string connectionString)
    {
        var connectionStringBuilder = DbProviderFactory.CreateConnectionStringBuilder();
        if (connectionStringBuilder is null)
        {
            return null;
        }

        if (!string.IsNullOrWhiteSpace(connectionString))
        {
            connectionStringBuilder.ConnectionString = connectionString;
        }

        return new DbConnectionString(connectionStringBuilder, Info);
    }

    protected virtual DbProviderInfo GetInfo()
    {
        if (_info is not null)
        {
            return _info;
        }

        using var dataTable = DbProviderFactories.GetFactoryClasses();
        var infoRow = dataTable
            .Rows.OfType<DataRow>()
            .FirstOrDefault(x => x["InvariantName"].ToString() == ProviderInvariantName);

        if (infoRow is null)
        {
            throw Logger.LogErrorAndCreateException<InvalidOperationException>("Failed to obtain 'DbProviderInfo'");
        }

        _info = infoRow.ToDbProviderInfo();

        return _info;
    }
}
