namespace Orc.DataAccess.Database;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Catel;
using Catel.Collections;
using Catel.Logging;

public class DbProvider
{
    private static readonly ILog Log = LogManager.GetCurrentClassLogger();

    private static readonly Dictionary<string, DbProvider> Providers = new();
    private static readonly DbProviderFactoryRepository ProviderFactoryRepository = new();

    private static bool IsProvidersInitialized;

    private Type? _connectionType;
    private DbProviderFactory? _dbProviderFactory;
    private DbProviderInfo? _info;

    public DbProvider(DbProviderInfo info)
        : this(info.InvariantName)
    {
        ArgumentNullException.ThrowIfNull(info);

        _info = info;
    }

    public DbProvider(string providerInvariantName)
    {
        ArgumentNullException.ThrowIfNull(providerInvariantName);

        ProviderInvariantName = providerInvariantName;
    }

    protected DbProviderFactory DbProviderFactory => _dbProviderFactory ??= DbProviderFactories.GetFactory(ProviderInvariantName);
#pragma warning disable IDISP004 // Don't ignore created IDisposable.
    public virtual Type ConnectionType => _connectionType ??= DbProviderFactory.CreateConnection()?.GetType() 
                                                              ?? throw Log.ErrorAndCreateException<InvalidOperationException>($"Failed to get '{nameof(ConnectionType)}' value");
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

        ProviderFactoryRepository.Remove(providerInfo);
    }

    public static void RegisterCustomProvider(DbProvider provider)
    {
        ArgumentNullException.ThrowIfNull(provider);

        Providers[provider.ProviderInvariantName] = provider;
    }

    public static DbProvider GetRegisteredProvider(string invariantName)
    {
        var registeredProviders = GetRegisteredProviders();
        if (registeredProviders.TryGetValue(invariantName, out var dbProvider))
        {
            return dbProvider;
        }

        throw Log.ErrorAndCreateException<InvalidOperationException>($"Provider with name '{invariantName}' is not registered");
    }

    public static IReadOnlyDictionary<string, DbProvider> GetRegisteredProviders()
    {
        var providers = Providers;
        if (IsProvidersInitialized)
        {
            return providers;
        }

        using var dataTable = DbProviderFactories.GetFactoryClasses();
        dataTable.Rows.OfType<DataRow>()
            .Select(x => x.ToDbProviderInfo())
            .OrderBy(x => x.Name)
            .Select(x => new DbProvider(x))
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
        this.ConnectType<DbConnection>(_connectionType);

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
            throw Log.ErrorAndCreateException<InvalidOperationException>($"Failed to obtain '{nameof(DbProviderInfo)}'");
        }

        _info = infoRow.ToDbProviderInfo();

        return _info;
    }
}
