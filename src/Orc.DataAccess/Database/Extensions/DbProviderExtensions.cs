namespace Orc.DataAccess.Database;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Catel;
using Catel.Caching;
using Catel.IoC;
using Catel.Logging;
using Catel.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

public static class DbProviderExtensions
{
    private static readonly ILogger Logger = LogManager.GetLogger(typeof(DbProviderExtensions));

    private static readonly Dictionary<string, Dictionary<Type, IList<Type>>> ConnectedTypes = new();
    private static readonly Dictionary<string, Dictionary<Type, object>> ConnectedInstances = new();

    public static T? GetOrCreateConnectedInstance<T>(this DbProvider dbProvider, IServiceProvider serviceProvider)
        where T : notnull
    {
        ArgumentNullException.ThrowIfNull(dbProvider);

        var providerInvariantName = dbProvider.ProviderInvariantName;
        var instances = GetConnectedInstances(providerInvariantName);

        if (instances.TryGetValue(typeof(T), out var instance))
        {
            return (T)instance;
        }

        instance = CreateConnectedInstance<T>(dbProvider, serviceProvider);
        if (instance is null)
        {
            return default;
        }

        instances[typeof(T)] = instance;
        return (T?)instance;
    }

    public static T? CreateConnectedInstance<T>(this DbProvider dbProvider, IServiceProvider serviceProvider, params object[] parameters)
        where T : notnull
    {
        ArgumentNullException.ThrowIfNull(dbProvider);

        var connectedType = dbProvider.GetConnectedTypes<T>()
            .FirstOrDefault();
        if (connectedType is null)
        {
            return default;
        }

        return (T)ActivatorUtilities.CreateInstance(serviceProvider, connectedType, parameters);
    }

    public static IList<Type> GetConnectedTypes<T>(this DbProvider dbProvider)
    {
        ArgumentNullException.ThrowIfNull(dbProvider);

        var connectedTypes = ConnectedTypes;
        var invariantName = dbProvider.ProviderInvariantName;
        if (!connectedTypes.TryGetValue(invariantName, out var typeBatch))
        {
            typeBatch = new Dictionary<Type, IList<Type>>();
            connectedTypes.Add(invariantName, typeBatch);
        }

        if (!typeBatch.TryGetValue(typeof(T), out var types))
        {
            types = dbProvider.FindConnectedTypes<T>().ToList();
            typeBatch[typeof(T)] = types;
        }

        return types.ToArray();
    }

    private static Dictionary<Type, object> GetConnectedInstances(string providerInvariantName)
    {
        var connectedInstances = ConnectedInstances;
        if (!connectedInstances.TryGetValue(providerInvariantName, out var instances))
        {
            instances = new Dictionary<Type, object>();
            connectedInstances.Add(providerInvariantName, instances);
        }

        return instances;
    }

    public static void ConnectType<TBaseType>(this DbProvider dbProvider, IServiceProvider serviceProvider, Type type)
    {
        ArgumentNullException.ThrowIfNull(dbProvider);
        ArgumentNullException.ThrowIfNull(type);

        var types = GetConnectedTypes<TBaseType>(dbProvider);
        if (!types.Contains(type))
        {
            types.Add(type);
        }
    }

    public static void ConnectInstance<TBaseType>(this DbProvider dbProvider, TBaseType instance)
    {
        ArgumentNullException.ThrowIfNull(instance);

        var instances = GetConnectedInstances(dbProvider.ProviderInvariantName);
        instances[typeof(TBaseType)] = instance;
    }

    private static IEnumerable<Type> FindConnectedTypes<T>(this DbProvider dbProvider)
    {
        ArgumentNullException.ThrowIfNull(dbProvider);

        var providerInvariantName = dbProvider.ProviderInvariantName;
        var attributedSqlCompilerTypes = typeof(T).GetAllAssignableFrom();

        foreach (var attributedSqlCompilerType in attributedSqlCompilerTypes)
        {
            var connectToProviderAttribute = attributedSqlCompilerType.GetCustomAttributeEx(typeof(ConnectToProviderAttribute), true) as ConnectToProviderAttribute;
            if (connectToProviderAttribute is null)
            {
                continue;
            }

            if (connectToProviderAttribute.ProviderInvariantName == providerInvariantName)
            {
                yield return attributedSqlCompilerType;
            }
        }
    }

    //TODO: Make it async
    public static IReadOnlyList<DbDataSource> GetDataSources(this DbProvider dbProvider, IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(dbProvider);

        var dataSourceProvider = dbProvider.GetOrCreateConnectedInstance<IDbDataSourceProvider>(serviceProvider);
        return dataSourceProvider?.GetDataSources() ?? Array.Empty<DbDataSource>();
    }

    public static DbConnection? CreateConnection(this DbProvider dbProvider, DatabaseSource databaseSource)
    {
        ArgumentNullException.ThrowIfNull(dbProvider);
        ArgumentNullException.ThrowIfNull(databaseSource);

        if (string.IsNullOrEmpty(databaseSource.ConnectionString))
        {
            throw Logger.LogErrorAndCreateException<InvalidOperationException>("Invalid source");
        }

        return CreateConnection(dbProvider, databaseSource.ConnectionString);
    }

    public static DbSourceGatewayBase? CreateDbSourceGateway(this DbProvider dbProvider, IServiceProvider serviceProvider, DatabaseSource databaseSource)
    {
        ArgumentNullException.ThrowIfNull(dbProvider);
        ArgumentNullException.ThrowIfNull(databaseSource);

        return dbProvider.CreateConnectedInstance<DbSourceGatewayBase>(serviceProvider, databaseSource);
    }

    public static DbConnection? CreateConnection(this DbProvider dbProvider, string connectionString)
    {
        ArgumentNullException.ThrowIfNull(dbProvider);
        Argument.IsNotNullOrEmpty(() => connectionString);

        var connection = dbProvider.CreateConnection();
        if (connection is null)
        {
            return null;
        }

        connection.ConnectionString = connectionString;

        return connection;
    }
}
