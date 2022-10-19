namespace Orc.DataAccess.Database
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Linq;
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
            var instanceCache = ConnectedInstances.GetFromCacheOrFetch(dbProvider.ProviderInvariantName, () => new CacheStorage<Type, object>());
            return (T)instanceCache.GetFromCacheOrFetch(typeof(T), () => CreateConnectedInstance<T>(dbProvider));
        }

        public static T CreateConnectedInstance<T>(this DbProvider dbProvider, params object[] parameters)
            where T : notnull
        {
            var connectedType = dbProvider.GetConnectedTypes<T>().First();

#pragma warning disable IDISP001 // Dispose created
            var typeFactory = dbProvider.GetTypeFactory();
#pragma warning restore IDISP001 // Dispose created
            return (T)typeFactory.CreateRequiredInstanceWithParametersAndAutoCompletion(connectedType, parameters);
        }

        public static IList<Type> GetConnectedTypes<T>(this DbProvider provider)
        {
            var connectedTypesCache = ConnectedTypes.GetFromCacheOrFetch(provider.ProviderInvariantName, () => new CacheStorage<Type, IList<Type>>());
            return connectedTypesCache.GetFromCacheOrFetch(typeof(T), () => provider.FindConnectedTypes<T>().ToList());
        }

        public static void ConnectType<TBaseType>(this DbProvider provider, Type type)
        {
            var connectedTypesCache = ConnectedTypes.GetFromCacheOrFetch(provider.ProviderInvariantName, () => new CacheStorage<Type, IList<Type>>());
            var types = connectedTypesCache.GetFromCacheOrFetch(typeof(TBaseType), () => provider.FindConnectedTypes<TBaseType>().ToList());
            if (!types.Contains(type))
            {
                types.Add(type);
            }
        }

        private static IEnumerable<Type> FindConnectedTypes<T>(this DbProvider provider)
        {
            var providerInvariantName = provider.ProviderInvariantName;
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
            var dataSourceProvider = dbProvider.GetOrCreateConnectedInstance<IDbDataSourceProvider>();
            return dataSourceProvider?.GetDataSources() ?? new List<DbDataSource>();
        }

        public static DbConnection? CreateConnection(this DbProvider dbProvider, DatabaseSource databaseSource)
        {
            if (string.IsNullOrEmpty(databaseSource.ConnectionString))
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Invalid source");
            }

            return CreateConnection(dbProvider, databaseSource.ConnectionString);
        }

        public static DbSourceGatewayBase CreateDbSourceGateway(this DbProvider dbProvider, DatabaseSource databaseSource)
        {
            return dbProvider.CreateConnectedInstance<DbSourceGatewayBase>(databaseSource);
        }

        public static DbConnection? CreateConnection(this DbProvider dbProvider, string connectionString)
        {
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
