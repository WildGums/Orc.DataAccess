namespace Orc.DataAccess.Controls;

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Catel;
using Catel.IoC;
using Catel.Logging;
using Catel.Services;
using Database;

#pragma warning disable IDISP006 // Implement IDisposable
[TemplatePart(Name = "PART_ConnectionStringTextBox", Type = typeof(TextBox))]
public class ConnectionStringBuilder : Control
{
    private static readonly ILog Log = LogManager.GetCurrentClassLogger();

    private readonly ITypeFactory _typeFactory;
    private readonly IUIVisualizerService _uiVisualizerService;

    private DbProviderInfo? _dbProvider;
    private TextBox? _connectionStringTextBox;

    private bool _isConnectionStringUpdating;

    public ConnectionStringBuilder()
    {
        _typeFactory = this.GetTypeFactory();
#pragma warning disable IDISP001 // Dispose created.
        var serviceLocator = this.GetServiceLocator();
#pragma warning restore IDISP001 // Dispose created.

        _uiVisualizerService = serviceLocator.ResolveRequiredType<IUIVisualizerService>();

        var editCommandBinding = new CommandBinding
        {
            Command = EditCommand
        };
        editCommandBinding.Executed += OnEditCommandExecuted;
        CommandBindings.Add(editCommandBinding);

        var clearCommandBinding = new CommandBinding
        {
            Command = ClearCommand
        };
        clearCommandBinding.Executed += OnClearCommandExecuted;
        clearCommandBinding.CanExecute += CanClearCommandExecute;
        CommandBindings.Add(clearCommandBinding);
    }

    #region Routed Commands
    public static RoutedCommand EditCommand { get; } = new(nameof(EditCommand), typeof(ConnectionStringBuilder));

    private async void OnEditCommandExecuted(object sender, ExecutedRoutedEventArgs e)
    {
        using (new DisposableToken<ConnectionStringBuilder>(this, x => x.Instance.SetCurrentValue(IsInEditModeProperty, true), 
                   x => x.Instance.SetCurrentValue(IsInEditModeProperty, false)))
        {
            var connectionStringEditViewModel = _typeFactory.CreateRequiredInstanceWithParametersAndAutoCompletion<ConnectionStringEditViewModel>(ConnectionString, _dbProvider);
            connectionStringEditViewModel.IsAdvancedOptionsReadOnly = IsAdvancedOptionsReadOnly;

            if ((await _uiVisualizerService.ShowDialogAsync(connectionStringEditViewModel)).DialogResult == false)
            {
                return;
            }

            using (new DisposableToken<ConnectionStringBuilder>(this, x => x.Instance._isConnectionStringUpdating = true,
                       x => x.Instance._isConnectionStringUpdating = false))
            {
                _dbProvider = connectionStringEditViewModel.DbProvider;
                var connectionString = connectionStringEditViewModel.ConnectionString;

                SetCurrentValue(ConnectionStringProperty, connectionString?.ToString());
                _connectionStringTextBox?.SetCurrentValue(TextBox.TextProperty, connectionString?.ToDisplayString());
                SetCurrentValue(ConnectionStateProperty, connectionStringEditViewModel.ConnectionState);
                SetCurrentValue(DatabaseProviderProperty, connectionString?.DbProvider.InvariantName);
            }
        }
    }

    public static RoutedCommand ClearCommand { get; } = new(nameof(ClearCommand), typeof(ConnectionStringBuilder));

    private void OnClearCommandExecuted(object sender, ExecutedRoutedEventArgs e)
    {
        SetCurrentValue(ConnectionStringProperty, null);
        _connectionStringTextBox?.SetCurrentValue(TextBox.TextProperty, null);
        SetCurrentValue(ConnectionStateProperty, ConnectionState.Undefined);
        _dbProvider = null;
    }

    private void CanClearCommandExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = ConnectionString is not null;
    }
    #endregion

    #region Dependency Properties
    public string? ConnectionString
    {
        get { return (string?)GetValue(ConnectionStringProperty); }
        set { SetValue(ConnectionStringProperty, value); }
    }

    public static readonly DependencyProperty ConnectionStringProperty = DependencyProperty.Register(
        nameof(ConnectionString), typeof(string), typeof(ConnectionStringBuilder), new PropertyMetadata(default(string),
            (sender, _) => ((ConnectionStringBuilder)sender). OnConnectionStringChanged()));

    public string? DatabaseProvider
    {
        get { return (string?)GetValue(DatabaseProviderProperty); }
        set { SetValue(DatabaseProviderProperty, value); }
    }

    public static readonly DependencyProperty DatabaseProviderProperty = DependencyProperty.Register(
        nameof(DatabaseProvider), typeof(string), typeof(ConnectionStringBuilder), new PropertyMetadata(default(string),
            (sender, _) => ((ConnectionStringBuilder)sender).OnDatabaseProviderChanged()));

    public bool IsInEditMode
    {
        get { return (bool)GetValue(IsInEditModeProperty); }
        set { SetValue(IsInEditModeProperty, value); }
    }

    public static readonly DependencyProperty IsInEditModeProperty = DependencyProperty.Register(
        nameof(IsInEditMode), typeof(bool), typeof(ConnectionStringBuilder), new PropertyMetadata(default(bool)));

    public ConnectionState ConnectionState
    {
        get { return (ConnectionState)GetValue(ConnectionStateProperty); }
        set { SetValue(ConnectionStateProperty, value); }
    }

    public static readonly DependencyProperty ConnectionStateProperty = DependencyProperty.Register(
        nameof(ConnectionState), typeof(ConnectionState), typeof(ConnectionStringBuilder), new PropertyMetadata(default(ConnectionState)));


    public bool IsAdvancedOptionsReadOnly
    {
        get { return (bool)GetValue(IsAdvancedOptionsReadOnlyProperty); }
        set { SetValue(IsAdvancedOptionsReadOnlyProperty, value); }
    }

    public static readonly DependencyProperty IsAdvancedOptionsReadOnlyProperty = DependencyProperty.Register(
        nameof(IsAdvancedOptionsReadOnly), typeof(bool), typeof(ConnectionStringBuilder), new PropertyMetadata(false));
    
    public bool IsEditable
    {
        get { return (bool)GetValue(IsEditableProperty); }
        set { SetValue(IsEditableProperty, value); }
    }

    public static readonly DependencyProperty IsEditableProperty = DependencyProperty.Register(
        nameof(IsEditable), typeof(bool), typeof(ConnectionStringBuilder), new PropertyMetadata(false));
    #endregion

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _connectionStringTextBox = GetTemplateChild("PART_ConnectionStringTextBox") as TextBox;
        if (_connectionStringTextBox is null)
        {
            throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_ConnectionStringTextBox'");
        }

        _connectionStringTextBox.TextChanged += OnTextChanged;
    }

    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (_connectionStringTextBox is null)
        {
            return;
        }

        SetCurrentValue(ConnectionStringProperty, _connectionStringTextBox.Text);
    }

    private void OnConnectionStringChanged()
    {
        UpdateConnectionString();
    }

    private void OnDatabaseProviderChanged()
    {
        UpdateConnectionString();
    }

    private void UpdateConnectionString()
    {
        if (_isConnectionStringUpdating)
        {
            return;
        }

        using (new DisposableToken<ConnectionStringBuilder>(this, x => x.Instance._isConnectionStringUpdating = true, 
                   x => x.Instance._isConnectionStringUpdating = false))
        {
            var providerName = DatabaseProvider;
            var connectionString = ConnectionString ?? string.Empty;

            var providers = DbProvider.GetRegisteredProviders();
            DbConnectionString? displayedConnectionsString = null;
            DbProvider? dbProvider = null;
            if (string.IsNullOrEmpty(providerName))
            {
                foreach (var providerKeyValue in providers)
                {
                    var currentProvider = providerKeyValue.Value;

                    try
                    {
                        displayedConnectionsString = currentProvider.CreateConnectionString(connectionString);
                    }
                    catch
                    {
                        continue;
                    }

                    dbProvider = currentProvider;
                    break;
                }
            }
            else
            {
                if (providers.TryGetValue(providerName, out dbProvider))
                {
                    displayedConnectionsString = dbProvider.CreateConnectionString(connectionString);
                }
            }

            _connectionStringTextBox?.SetCurrentValue(TextBox.TextProperty, displayedConnectionsString?.ToDisplayString());
            SetCurrentValue(DatabaseProviderProperty, dbProvider?.ProviderInvariantName);
            _dbProvider = dbProvider?.Info;
        }
    }
}
#pragma warning restore IDISP006 // Implement IDisposable
