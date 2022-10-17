namespace Orc.DataAccess.Controls
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Timers;
    using Catel;
    using Catel.Collections;
    using Catel.IoC;
    using Catel.MVVM;
    using Catel.Services;
    using Catel.Threading;
    using Database;
    using Timer = System.Timers.Timer;

    public class ConnectionStringEditViewModel : ViewModelBase
    {
        private static bool IsServersInitialized = false;
        private static readonly FastObservableCollection<string> CachedServers = new FastObservableCollection<string>();

        private readonly IDispatcherService _dispatcherService;

        private readonly DbProviderInfo _initalDbProvider;
        private readonly string _initialConnectionString;
        private readonly IMessageService _messageService;
        private readonly ITypeFactory _typeFactory;
        private readonly IUIVisualizerService _uiVisualizerService;
#pragma warning disable IDISP006 // Implement IDisposable.
        private readonly Timer _initializeTimer = new Timer(200);
#pragma warning restore IDISP006 // Implement IDisposable.

        private bool _isDatabasesInitialized = false;

        public ConnectionStringEditViewModel(string connectionString, DbProviderInfo provider, IMessageService messageService,
            IUIVisualizerService uiVisualizerService, ITypeFactory typeFactory, IDispatcherService dispatcherService)
        {
            Argument.IsNotNull(() => uiVisualizerService);
            Argument.IsNotNull(() => typeFactory);
            Argument.IsNotNull(() => messageService);
            Argument.IsNotNull(() => dispatcherService);

            _messageService = messageService;
            _uiVisualizerService = uiVisualizerService;
            _typeFactory = typeFactory;
            _dispatcherService = dispatcherService;

            _initalDbProvider = provider;
            _initialConnectionString = connectionString;

            InitServers = new Command(() => InitServersAsync(), () => !IsServersRefreshing);
            RefreshServers = new Command(() => RefreshServersAsync(), () => !IsServersRefreshing);

            InitDatabases = new Command(() => InitDatabasesAsync(), () => !IsDatabasesRefreshing);
            RefreshDatabases = new Command(() => RefreshDatabasesAsync(), CanInitDatabases);

            TestConnection = new Command(OnTestConnection);
            ShowAdvancedOptions = new TaskCommand(OnShowAdvancedOptionsAsync, () => ConnectionString is not null);

            _initializeTimer.Elapsed += OnInitializeTimerElapsed;
        }

        public DbConnectionStringProperty DataSource => ConnectionString.TryGetProperty("Data Source")
                                                      ?? ConnectionString.TryGetProperty("Server")
                                                      ?? ConnectionString.TryGetProperty("Host");
        public DbConnectionStringProperty UserId => ConnectionString.TryGetProperty("User ID")
                                                  ?? ConnectionString.TryGetProperty("User name");
        public DbConnectionStringProperty Password => ConnectionString.TryGetProperty("Password");

        public DbConnectionStringProperty Port => ConnectionString.TryGetProperty("Part");
        public DbConnectionStringProperty IntegratedSecurity => ConnectionString.TryGetProperty("Integrated Security");

        public DbConnectionStringProperty InitialCatalog => ConnectionString.TryGetProperty("Initial Catalog")
                                                          ?? ConnectionString.TryGetProperty("Database");

        public bool IsAdvancedOptionsReadOnly { get; set; }

        public bool? IntegratedSecurityValue
        {
            get => IntegratedSecurity?.Value as bool?;
            set
            {
                if (IntegratedSecurity is null)
                {
                    return;
                }

                if (Equals(IntegratedSecurity.Value, value))
                {
                    return;
                }

                IntegratedSecurity.Value = value;
                RaisePropertyChanged(nameof(IntegratedSecurityValue));
            }
        }
        public bool CanLogOnToServer => Password is not null || UserId is not null;
        public bool IsLogOnEnabled => CanLogOnToServer && !(IntegratedSecurityValue ?? false);

        public bool IsServerListVisible { get; set; }
        public bool IsDatabaseListVisible { get; set; }
        public bool IsServersRefreshing { get; private set; }
        public bool IsDatabasesRefreshing { get; private set; }
        public ConnectionState ConnectionState { get; private set; } = ConnectionState.Undefined;
        public override string Title => "Connection properties";
        public DbConnectionString ConnectionString { get; private set; }

        public DbProviderInfo DbProvider { get; set; }

        public Command RefreshServers { get; }
        public Command InitServers { get; }
        public Command TestConnection { get; }
        public TaskCommand ShowAdvancedOptions { get; }
        public Command RefreshDatabases { get; }
        public Command InitDatabases { get; }

        public FastObservableCollection<string> Servers => CachedServers;
        public FastObservableCollection<string> Databases { get; } = new FastObservableCollection<string>();

        private void OnInitializeTimerElapsed(object? sender, ElapsedEventArgs args)
        {
            _dispatcherService.Invoke(SetInitialState);
        }

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            _initializeTimer.Start();
        }

        private void SetInitialState()
        {
            _initializeTimer.Stop();

            using (SuspendChangeNotifications())
            {
                DbProvider = _initalDbProvider;
            }

            ConnectionString = _initalDbProvider?.CreateConnectionString(_initialConnectionString);
        }

        private void OnDbProviderChanged()
        {
            var dbProvider = DbProvider;
            if (dbProvider is null)
            {
                return;
            }

            IsServersInitialized = false;
            _isDatabasesInitialized = false;
            Databases.Clear();
            Servers.Clear();

            ConnectionString = dbProvider.CreateConnectionString();
            SetIntegratedSecurityToDefault();
        }

        private void SetIntegratedSecurityToDefault()
        {
            var integratedSecurityProperty = IntegratedSecurity;
            if (integratedSecurityProperty is null)
            {
                return;
            }

            integratedSecurityProperty.Value = true;
            RaisePropertyChanged(nameof(IntegratedSecurity));
        }

        private async Task OnShowAdvancedOptionsAsync()
        {
            var connectionString = ConnectionString;
            if (connectionString is null)
            {
                return;
            }

            var advancedOptionsViewModel = _typeFactory.CreateInstanceWithParametersAndAutoCompletion<ConnectionStringAdvancedOptionsViewModel>(connectionString);
            advancedOptionsViewModel.IsAdvancedOptionsReadOnly = IsAdvancedOptionsReadOnly;

            await _uiVisualizerService.ShowDialogAsync(advancedOptionsViewModel);
        }

        private void OnTestConnection()
        {
            ConnectionState = ConnectionString.GetConnectionState();

            _messageService.ShowAsync($"{ConnectionState} connection!", "Connection test result");
        }

        private void OnDataSourceChanged()
        {
            _isDatabasesInitialized = false;
            InitDatabases.RaiseCanExecuteChanged();
        }

        public bool CanInitDatabases()
        {
            return !IsDatabasesRefreshing;
        }

        private Task InitServersAsync()
        {
            if (IsServersInitialized)
            {
                return Task.CompletedTask;
            }

            return RefreshServersAsync();
        }

        private Task RefreshServersAsync()
        {
            IsServersRefreshing = true;

            Servers.Clear();

            return TaskHelper.RunAndWaitAsync(() =>
            {
                var provider = Database.DbProvider.GetRegisteredProvider(ConnectionString.DbProvider.InvariantName);
                var dataSources = provider.GetDataSources();
                Servers.AddItems(dataSources.Select(x => x.InstanceName));

                IsServersRefreshing = false;
                IsServersInitialized = true;
                IsServerListVisible = true;
            });
        }

        private Task InitDatabasesAsync()
        {
            return _isDatabasesInitialized ? Task.CompletedTask : RefreshDatabasesAsync();
        }

        private Task RefreshDatabasesAsync()
        {
            var connectionString = ConnectionString;
            if (string.IsNullOrWhiteSpace(connectionString?.ToString()))
            {
                return Task.CompletedTask;
            }

            IsDatabasesRefreshing = true;

            Databases.Clear();

            return TaskHelper.RunAndWaitAsync(() =>
            {
                var connectionState = ConnectionString.GetConnectionState();
                if (connectionState != ConnectionState.Invalid)
                {
                    var schema = connectionString.GetDataSourceSchema();
                    if (schema is not null)
                    {
                        Databases.AddItems(schema.Databases);
                    }
                }
                else
                {
                    ConnectionState = connectionState;
                }

                IsDatabasesRefreshing = false;
                _isDatabasesInitialized = true;
                IsDatabaseListVisible = true;
            });
        }
    }
}
