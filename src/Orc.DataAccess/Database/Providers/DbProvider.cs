namespace Orc.DataAccess.Database
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Linq;
    using Catel;
    using Catel.Collections;

    public class DbProvider
    {
        #region Fields
        private static readonly Dictionary<string, DbProvider> Providers = new Dictionary<string, DbProvider>();
        private static readonly DbProviderFactoryRepository ProviderFactoryRepository = new DbProviderFactoryRepository();

        private static bool IsProvidersInitialized = false;

        private Type _connectionType;
        private DbProviderFactory _dbProviderFactory;
        private DbProviderInfo _info;
        #endregion

        #region Constructors
        public DbProvider(DbProviderInfo info)
            : this(info.InvariantName)
        {
            Argument.IsNotNull(() => info);

            _info = info;
        }

        public DbProvider(string providerInvariantName)
        {
            Argument.IsNotNullOrWhitespace(() => providerInvariantName);

            ProviderInvariantName = providerInvariantName;
        }
        #endregion

        #region Properties
        protected DbProviderFactory DbProviderFactory => _dbProviderFactory ??= DbProviderFactories.GetFactory(ProviderInvariantName);
#pragma warning disable IDISP004 // Don't ignore created IDisposable.
        public virtual Type ConnectionType => _connectionType ??= DbProviderFactory.CreateConnection()?.GetType();
#pragma warning restore IDISP004 // Don't ignore created IDisposable.
        public virtual DbProviderInfo Info => GetInfo();
        public string Dialect { get; }
        public string ProviderInvariantName { get; }
        #endregion

        #region Methods
        public static void RegisterProvider(DbProviderInfo providerInfo)
        {
            Argument.IsNotNull(() => providerInfo);

            ProviderFactoryRepository.Add(providerInfo);
        }

        public static void UnregisterProvider(DbProviderInfo providerInfo)
        {
            Argument.IsNotNull(() => providerInfo);

            ProviderFactoryRepository.Remove(providerInfo);
        }

        public static void RegisterCustomProvider(DbProvider provider)
        {
            Argument.IsNotNull(() => provider);

            Providers[provider.ProviderInvariantName] = provider;
        }

        public static DbProvider GetRegisteredProvider(string invariantName)
        {
            var registeredProviders = GetRegisteredProviders();
            if (registeredProviders.TryGetValue(invariantName, out var dbProvider))
            {
                return dbProvider;
            }

            return null;
        }

        public static IReadOnlyDictionary<string, DbProvider> GetRegisteredProviders()
        {
            var providers = Providers;
            if (IsProvidersInitialized)
            {
                return providers;
            }

            using (var dataTable = DbProviderFactories.GetFactoryClasses())
            {
                dataTable.Rows.OfType<DataRow>()
                                .Select(x => x.ToDbProviderInfo())
                                .OrderBy(x => x.Name)
                                .Select(x => new DbProvider(x))
                                .ForEach(x => providers[x.ProviderInvariantName] = x);

                IsProvidersInitialized = true;

                return providers;
            }
        }

        public virtual DbConnection CreateConnection()
        {
            var connection = DbProviderFactory.CreateConnection();
            if (_connectionType is null)
            {
                _connectionType = connection?.GetType();
                this.ConnectType<DbConnection>(_connectionType);
            }

            return connection;
        }

        public virtual DbConnectionString CreateConnectionString(string connectionString = null)
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

            using (var dataTable = DbProviderFactories.GetFactoryClasses())
            {
                var infoRow = dataTable
                    .Rows.OfType<DataRow>()
                    .FirstOrDefault(x => x["InvariantName"]?.ToString() == ProviderInvariantName);

                if (infoRow is null)
                {
                    return null;
                }

                _info = infoRow.ToDbProviderInfo();

                return _info;
            }
        }
        #endregion
    }
}
