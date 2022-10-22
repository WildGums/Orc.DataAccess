namespace Orc.DataAccess.Database
{
    using System;
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

        private static readonly CacheStorage<string, CacheStorage<Type, IList<Type>>> ConnectedTypes = new();
        private static readonly CacheStorage<string, CacheStorage<Type, object>> ConnectedInstances = new();

        public static T GetOrCreateConnectedInstance<T>(this DbProvider dbProvider)
            where T : notnull
        {
            ArgumentNullException.ThrowIfNull(dbProvider);

            var instanceCache = ConnectedInstances.GetFromCacheOrFetch(dbProvider.ProviderInvariantName, () => new CacheStorage<Type, object>());
            return (T)instanceCache.GetFromCacheOrFetch(typeof(T), () => CreateConnectedInstance<T>(dbProvider));
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

            var connectedTypesCache = ConnectedTypes.GetFromCacheOrFetch(dbProvider.ProviderInvariantName, () => new CacheStorage<Type, IList<Type>>());
            return connectedTypesCache.GetFromCacheOrFetch(typeof(T), () => dbProvider.FindConnectedTypes<T>().ToList());
        }

        public static void ConnectType<TBaseType>(this DbProvider dbProvider, Type type)
        {
            ArgumentNullException.ThrowIfNull(dbProvider);
            ArgumentNullException.ThrowIfNull(type);

            var connectedTypesCache = ConnectedTypes.GetFromCacheOrFetch(dbProvider.ProviderInvariantName, () => new CacheStorage<Type, IList<Type>>());
            var types = connectedTypesCache.GetFromCacheOrFetch(typeof(TBaseType), () => dbProvider.FindConnectedTypes<TBaseType>().ToList());
            if (!types.Contains(type))
            {
                types.Add(type);
            }
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

        public static IList<DbDataSource> GetDataSources(this DbProvider dbProvider)
        {
            ArgumentNullException.ThrowIfNull(dbProvider);

            var dataSourceProvider = dbProvider.GetOrCreateConnectedInstance<IDbDataSourceProvider>();
            return dataSourceProvider?.GetDataSources() ?? new List<DbDataSource>();
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
}
