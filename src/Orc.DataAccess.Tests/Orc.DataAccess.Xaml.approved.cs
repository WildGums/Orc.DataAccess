[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.Runtime.Versioning.TargetFramework(".NETCoreApp,Version=v5.0", FrameworkDisplayName="")]
[assembly: System.Windows.Markup.XmlnsDefinition("http://schemas.wildgums.com/orc/dataaccess", "Orc.DataAccess")]
[assembly: System.Windows.Markup.XmlnsPrefix("http://schemas.wildgums.com/orc/dataaccess", "orcdataaccess")]
[assembly: System.Windows.ThemeInfo(System.Windows.ResourceDictionaryLocation.None, System.Windows.ResourceDictionaryLocation.SourceAssembly)]
public static class ModuleInitializer
{
    public static void Initialize() { }
}
namespace Orc.DataAccess.Controls
{
    public class ConnectionStateToColorBrushValueConverter : Catel.MVVM.Converters.ValueConverterBase<Orc.DataAccess.Database.ConnectionState, System.Windows.Media.SolidColorBrush>
    {
        public ConnectionStateToColorBrushValueConverter() { }
        protected override object Convert(Orc.DataAccess.Database.ConnectionState value, System.Type targetType, object parameter) { }
    }
    public class ConnectionStringAdvancedOptionsViewModel : Catel.MVVM.ViewModelBase
    {
        public static readonly Catel.Data.PropertyData ConnectionStringPropertiesProperty;
        public static readonly Catel.Data.PropertyData IsAdvancedOptionsReadOnlyProperty;
        public ConnectionStringAdvancedOptionsViewModel(Orc.DataAccess.Database.DbConnectionString connectionString) { }
        public Orc.DataAccess.Database.DbConnectionString ConnectionString { get; }
        public System.Collections.Generic.IList<Orc.DataAccess.Database.DbConnectionStringProperty> ConnectionStringProperties { get; }
        public bool IsAdvancedOptionsReadOnly { get; set; }
        public override string Title { get; }
        protected override System.Threading.Tasks.Task InitializeAsync() { }
    }
    public sealed class ConnectionStringAdvancedOptionsWindow : Catel.Windows.DataWindow, System.Windows.Markup.IComponentConnector
    {
        public ConnectionStringAdvancedOptionsWindow() { }
        public void InitializeComponent() { }
    }
    [System.Windows.TemplatePart(Name="PART_ConnectionStringTextBox", Type=typeof(System.Windows.Controls.TextBox))]
    public class ConnectionStringBuilder : System.Windows.Controls.Control
    {
        public static readonly System.Windows.DependencyProperty ConnectionStateProperty;
        public static readonly System.Windows.DependencyProperty ConnectionStringProperty;
        public static readonly System.Windows.DependencyProperty DatabaseProviderProperty;
        public static readonly System.Windows.DependencyProperty IsAdvancedOptionsReadOnlyProperty;
        public static readonly System.Windows.DependencyProperty IsInEditModeProperty;
        public ConnectionStringBuilder() { }
        public Orc.DataAccess.Database.ConnectionState ConnectionState { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseProvider { get; set; }
        public bool IsAdvancedOptionsReadOnly { get; set; }
        public bool IsInEditMode { get; set; }
        public static System.Windows.Input.RoutedCommand ClearCommand { get; }
        public static System.Windows.Input.RoutedCommand EditCommand { get; }
        public override void OnApplyTemplate() { }
    }
    public class ConnectionStringEditViewModel : Catel.MVVM.ViewModelBase
    {
        public static readonly Catel.Data.PropertyData ConnectionStateProperty;
        public static readonly Catel.Data.PropertyData ConnectionStringProperty;
        public static readonly Catel.Data.PropertyData DbProviderProperty;
        public static readonly Catel.Data.PropertyData IsAdvancedOptionsReadOnlyProperty;
        public static readonly Catel.Data.PropertyData IsDatabaseListVisibleProperty;
        public static readonly Catel.Data.PropertyData IsDatabasesRefreshingProperty;
        public static readonly Catel.Data.PropertyData IsServerListVisibleProperty;
        public static readonly Catel.Data.PropertyData IsServersRefreshingProperty;
        public ConnectionStringEditViewModel(string connectionString, Orc.DataAccess.Database.DbProviderInfo provider, Catel.Services.IMessageService messageService, Catel.Services.IUIVisualizerService uiVisualizerService, Catel.IoC.ITypeFactory typeFactory, Catel.Services.IDispatcherService dispatcherService) { }
        public bool CanLogOnToServer { get; }
        public Orc.DataAccess.Database.ConnectionState ConnectionState { get; }
        public Orc.DataAccess.Database.DbConnectionString ConnectionString { get; }
        public Orc.DataAccess.Database.DbConnectionStringProperty DataSource { get; }
        public Catel.Collections.FastObservableCollection<string> Databases { get; }
        public Orc.DataAccess.Database.DbProviderInfo DbProvider { get; set; }
        public Catel.MVVM.Command InitDatabases { get; }
        public Catel.MVVM.Command InitServers { get; }
        public Orc.DataAccess.Database.DbConnectionStringProperty InitialCatalog { get; }
        public Orc.DataAccess.Database.DbConnectionStringProperty IntegratedSecurity { get; }
        public bool? IntegratedSecurityValue { get; set; }
        public bool IsAdvancedOptionsReadOnly { get; set; }
        public bool IsDatabaseListVisible { get; set; }
        public bool IsDatabasesRefreshing { get; }
        public bool IsLogOnEnabled { get; }
        public bool IsServerListVisible { get; set; }
        public bool IsServersRefreshing { get; }
        public Orc.DataAccess.Database.DbConnectionStringProperty Password { get; }
        public Orc.DataAccess.Database.DbConnectionStringProperty Port { get; }
        public Catel.MVVM.Command RefreshDatabases { get; }
        public Catel.MVVM.Command RefreshServers { get; }
        public Catel.Collections.FastObservableCollection<string> Servers { get; }
        public Catel.MVVM.TaskCommand ShowAdvancedOptions { get; }
        public Catel.MVVM.Command TestConnection { get; }
        public override string Title { get; }
        public Orc.DataAccess.Database.DbConnectionStringProperty UserId { get; }
        public bool CanInitDatabases() { }
        protected override System.Threading.Tasks.Task InitializeAsync() { }
        protected override void OnPropertyChanged(Catel.Data.AdvancedPropertyChangedEventArgs e) { }
    }
    public sealed class ConnectionStringEditWindow : Catel.Windows.DataWindow, System.Windows.Markup.IComponentConnector
    {
        public ConnectionStringEditWindow() { }
        public void InitializeComponent() { }
    }
    public class DbConnectionProviderListViewModel : Catel.MVVM.ViewModelBase
    {
        public static readonly Catel.Data.PropertyData DbProviderProperty;
        public static readonly Catel.Data.PropertyData DbProvidersProperty;
        public DbConnectionProviderListViewModel(Orc.DataAccess.Database.DbProviderInfo selectedProvider) { }
        public Orc.DataAccess.Database.DbProviderInfo DbProvider { get; set; }
        public System.Collections.Generic.IList<Orc.DataAccess.Database.DbProviderInfo> DbProviders { get; }
        public Catel.MVVM.Command Open { get; }
        public Catel.MVVM.Command Refresh { get; }
        public override string Title { get; }
        protected override System.Threading.Tasks.Task InitializeAsync() { }
    }
    public sealed class DbConnectionProviderListWindow : Catel.Windows.DataWindow, System.Windows.Markup.IComponentConnector
    {
        public DbConnectionProviderListWindow() { }
        public void InitializeComponent() { }
    }
    public sealed class DbProviderPicker : Catel.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector
    {
        public static readonly System.Windows.DependencyProperty DbProviderProperty;
        public DbProviderPicker() { }
        [Catel.MVVM.Views.ViewToViewModel("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewModelWins)]
        public Orc.DataAccess.Database.DbProviderInfo DbProvider { get; set; }
        public void InitializeComponent() { }
    }
    public class DbProviderPickerViewModel : Catel.MVVM.ViewModelBase
    {
        public static readonly Catel.Data.PropertyData DbProviderProperty;
        public DbProviderPickerViewModel(Catel.Services.IUIVisualizerService uiVisualizerService, Catel.IoC.ITypeFactory typeFactory) { }
        public Catel.MVVM.TaskCommand ChangeDbProvider { get; }
        public Orc.DataAccess.Database.DbProviderInfo DbProvider { get; set; }
    }
    public interface IDataSourceProvider
    {
        string DataBasesQuery { get; }
    }
    public class MsSqlDataSourceProvider : Orc.DataAccess.Controls.IDataSourceProvider
    {
        public MsSqlDataSourceProvider() { }
        public string DataBasesQuery { get; }
        public System.Collections.Generic.IList<string> GetDataSources() { }
    }
}
namespace Orc.DataAccess
{
    public static class DataSourcePath
    {
        public const string MicrosoftSqlServerRegPath = "SOFTWARE\\Microsoft\\Microsoft SQL Server";
    }
    public static class SqlConnectionStringExtensions
    {
        public static Orc.DataAccess.Database.DbConnectionStringProperty TryGetProperty(this Orc.DataAccess.Database.DbConnectionString connectionString, string propertyName) { }
    }
}