namespace Orc.DataAccess.Controls;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Catel.Collections;
using Catel.IoC;
using Catel.Logging;
using Catel.MVVM;
using Catel.Services;
using Database;
using Timer = System.Timers.Timer;

public class ConnectionStringEditViewModel : ViewModelBase
{
    private static readonly ILog Log = LogManager.GetCurrentClassLogger();

    private static readonly FastObservableCollection<string> CachedServers = new();

    private static bool IsServersInitialized;

    private readonly IDispatcherService _dispatcherService;

    private readonly DbProviderInfo? _initalDbProvider;
    private readonly string _initialConnectionString;
    private readonly IMessageService _messageService;
    private readonly ITypeFactory _typeFactory;
    private readonly IUIVisualizerService _uiVisualizerService;
    private readonly Timer _initializeTimer = new(200);

    private bool _isDatabasesInitialized;

    public ConnectionStringEditViewModel(string connectionString, DbProviderInfo? provider, IMessageService messageService,
        IUIVisualizerService uiVisualizerService, ITypeFactory typeFactory, IDispatcherService dispatcherService)
    {
        ArgumentNullException.ThrowIfNull(uiVisualizerService);
        ArgumentNullException.ThrowIfNull(typeFactory);
        ArgumentNullException.ThrowIfNull(messageService);
        ArgumentNullException.ThrowIfNull(dispatcherService);

        _messageService = messageService;
        _uiVisualizerService = uiVisualizerService;
        _typeFactory = typeFactory;
        _dispatcherService = dispatcherService;

        _initalDbProvider = provider;
        _initialConnectionString = connectionString;

        InitServers = new TaskCommand(InitServersAsync, () => !IsServersRefreshing);
        RefreshServers = new TaskCommand(RefreshServersAsync, () => !IsServersRefreshing);

        InitDatabases = new TaskCommand(InitDatabasesAsync, () => !IsDatabasesRefreshing);
        RefreshDatabases = new TaskCommand(RefreshDatabasesAsync, CanInitDatabases);

        TestConnection = new Command(OnTestConnection);
        ShowAdvancedOptions = new TaskCommand(OnShowAdvancedOptionsAsync, () => ConnectionString is not null);

        _initializeTimer.Elapsed += OnInitializeTimerElapsed;
    }

    public DbConnectionStringProperty? DataSource => ConnectionString?.GetProperty("Data Source")
                                                     ?? ConnectionString?.GetProperty("Server")
                                                     ?? ConnectionString?.GetProperty("Host");
    public DbConnectionStringProperty? UserId => ConnectionString?.GetProperty("User ID")
                                                 ?? ConnectionString?.GetProperty("User name");
    public DbConnectionStringProperty? Password => ConnectionString?.GetProperty("Password");

    public DbConnectionStringProperty? Port => ConnectionString?.GetProperty("Port");
    public DbConnectionStringProperty? IntegratedSecurity => ConnectionString?.GetProperty("Integrated Security");

    public DbConnectionStringProperty? InitialCatalog => ConnectionString?.GetProperty("Initial Catalog")
                                                         ?? ConnectionString?.GetProperty("Database");

    public bool IsAdvancedOptionsReadOnly { get; set; }
    public IReadOnlyCollection<DbConnectionPropertyDefinition>? DefaultProperties { get; set; }

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
    public DbConnectionString? ConnectionString { get; private set; }

    public DbProviderInfo? DbProvider { get; set; }

    public TaskCommand RefreshServers { get; }
    public TaskCommand InitServers { get; }
    public Command TestConnection { get; }
    public TaskCommand ShowAdvancedOptions { get; }
    public TaskCommand RefreshDatabases { get; }
    public TaskCommand InitDatabases { get; }

    public FastObservableCollection<string> Servers => CachedServers;
    public FastObservableCollection<string> Databases { get; } = new();

    protected override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        _initializeTimer.Start();
    }

    private void OnInitializeTimerElapsed(object? sender, ElapsedEventArgs args)
    {
        _initializeTimer.Stop();

        _dispatcherService.Invoke(SetInitialState);
    }
    
    private void SetInitialState()
    {
        if (_initalDbProvider is not null)
        {
            using (SuspendChangeNotifications())
            {
                DbProvider = _initalDbProvider;
            }
            ConnectionString = _initalDbProvider.CreateConnectionString(_initialConnectionString);

            //Only apply default properties if connection string was empty
            if (string.IsNullOrWhiteSpace(_initialConnectionString))
            {
                ApplyDefaultProperties();
            }

            return;
        }

        var allKnownProviders = Database.DbProvider.GetRegisteredProviders().Select(x => x.Value.Info).ToArray();
        if (allKnownProviders.Length == 1)
        {
            DbProvider = allKnownProviders.Single();
        }
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

        ConnectionString = dbProvider.CreateConnectionString(string.Empty);
        ApplyDefaultProperties();
        SetIntegratedSecurityToDefault();
    }

    private void OnDefaultPropertiesChanged()
    {
        ApplyDefaultProperties();
    }
    
    private void ApplyDefaultProperties()
    {
        var defaultProperties = DefaultProperties;
        if (defaultProperties is null)
        {
            return;
        }

        var connectionString = ConnectionString;
        if (connectionString is null)
        {
            return;
        }

        foreach (var property in defaultProperties)
        {
            var propertyName = property.Name;
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                continue;
            }

            var existingProperty = connectionString.GetProperty(propertyName)
                                   ?? connectionString.GetProperty(propertyName.Replace(" ", string.Empty));
            if (existingProperty is null)
            {
                continue;
            }

            existingProperty.Value = property.Value;
        }

        RaisePropertyChanged(nameof(ConnectionString));
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

        var advancedOptionsViewModel = _typeFactory.CreateRequiredInstanceWithParametersAndAutoCompletion<ConnectionStringAdvancedOptionsViewModel>(connectionString);
        advancedOptionsViewModel.IsAdvancedOptionsReadOnly = IsAdvancedOptionsReadOnly;

        await _uiVisualizerService.ShowDialogAsync(advancedOptionsViewModel);

        _dispatcherService.BeginInvoke(() =>
        {
            ConnectionState = ConnectionState.Undefined;

            _isDatabasesInitialized = Databases.Any();
        });
    }

    private void OnTestConnection()
    {
        if (ConnectionString is null)
        {
            Log.Warning("Can't test connection, because connection string is not set");

            _messageService.ShowAsync("Connection string is not specified", "Connection test result");

            return;
        }

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
        return IsServersInitialized 
            ? Task.CompletedTask 
            : RefreshServersAsync();
    }

    private async Task RefreshServersAsync()
    {
        var connectionString = ConnectionString;
        if (connectionString is null)
        {
            return;
        }

        IsServersRefreshing = true;

        Servers.Clear();

        await Task.Run(() =>
        {
            var provider = Database.DbProvider.GetRegisteredProvider(connectionString.DbProvider.InvariantName);
            var dataSources = provider.GetDataSources();

            _dispatcherService.BeginInvoke(() => Servers.AddItems(dataSources.Select(x => x.InstanceName)));

            IsServersRefreshing = false;
            IsServersInitialized = true;
            IsServerListVisible = true;
        });
    }

    private async Task InitDatabasesAsync()
    {
        if (_isDatabasesInitialized)
        {
            return;
        }

        await RefreshDatabasesAsync();
    }

    private async Task RefreshDatabasesAsync()
    {
        var connectionString = ConnectionString;
        if (connectionString is null)
        {
            return;
        }

        IsDatabasesRefreshing = true;

        Databases.Clear();

        await Task.Run(() =>
        {
            var connectionState = connectionString.GetConnectionState();
            if (connectionState != ConnectionState.Invalid)
            {
                var schema = connectionString.GetDataSourceSchema();
                if (schema is not null)
                {
                    _dispatcherService.BeginInvoke(() => Databases.AddItems(schema.Databases));
                }
            }
            else
            {
                _dispatcherService.BeginInvoke(() => ConnectionState = connectionState);
            }

            IsDatabasesRefreshing = false;
            _isDatabasesInitialized = true;
            IsDatabaseListVisible = true;
        });
    }
}
