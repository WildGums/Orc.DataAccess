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

public static class DbProviderExtensions
{
    private static readonly ILog Log = LogManager.GetCurrentClassLogger();

    private static readonly Dictionary<string, Dictionary<Type, IList<Type>>> ConnectedTypes = new();
    private static readonly Dictionary<string, Dictionary<Type, object>> ConnectedInstances = new();

    public static T GetOrCreateConnectedInstance<T>(this DbProvider dbProvider)
        where T : notnull
    {
        ArgumentNullException.ThrowIfNull(dbProvider);

        var providerInvariantName = dbProvider.ProviderInvariantName;
        var instances = GetConnectedInstances(providerInvariantName);

        if (!instances.TryGetValue(typeof(T), out var instance))
        {
            instance = CreateConnectedInstance<T>(dbProvider);
            instances.Add(typeof(T), instance);
        }

        return (T)instance;
    }

    public static T CreateConnectedInstance<T>(this DbProvider dbProvider, params object[] parameters)
        where T : notnull
    {
        ArgumentNullException.ThrowIfNull(dbProvider);

        var connectedType = dbProvider.GetConnectedTypes<T>().First();

#pragma warning disable IDISP001 // Dispose created
        var typeFactory = dbProvider.GetTypeFactory();
#pragma warning restore IDISP001 // Dispose created
        return (T)typeFactory.CreateRequiredInstanceWithParametersAndAutoCompletion(connectedType, parameters);
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

        return types;
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

    public static void ConnectType<TBaseType>(this DbProvider dbProvider, Type type)
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
    public static IList<DbDataSource> GetDataSources(this DbProvider dbProvider)
    {
        ArgumentNullException.ThrowIfNull(dbProvider);

        var dataSourceProvider = dbProvider.GetOrCreateConnectedInstance<IDbDataSourceProvider>();
        return dataSourceProvider.GetDataSources();
    }

    public static DbConnection? CreateConnection(this DbProvider dbProvider, DatabaseSource databaseSource)
    {
        ArgumentNullException.ThrowIfNull(dbProvider);
        ArgumentNullException.ThrowIfNull(databaseSource);

        if (string.IsNullOrEmpty(databaseSource.ConnectionString))
        {
            throw Log.ErrorAndCreateException<InvalidOperationException>("Invalid source");
        }

        return CreateConnection(dbProvider, databaseSource.ConnectionString);
    }

    public static DbSourceGatewayBase CreateDbSourceGateway(this DbProvider dbProvider, DatabaseSource databaseSource)
    {
        ArgumentNullException.ThrowIfNull(dbProvider);
        ArgumentNullException.ThrowIfNull(databaseSource);

        return dbProvider.CreateConnectedInstance<DbSourceGatewayBase>(databaseSource);
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
