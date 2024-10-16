namespace Orc.DataAccess.Controls;

using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Threading;
using Catel;
using Catel.MVVM;
using Database;

public class ConnectionStringAdvancedOptionsViewModel : ViewModelBase
{
    private readonly DispatcherTimer _updateFilterTimer = new();

    private CollectionViewSource? _propertiesCollectionViewSource;

    public ConnectionStringAdvancedOptionsViewModel(DbConnectionString connectionString)
    {
        ArgumentNullException.ThrowIfNull(connectionString);

        ConnectionString = connectionString;
        ConnectionStringProperties = [];

        _updateFilterTimer.Interval = TimeSpan.FromMilliseconds(200);
        _updateFilterTimer.Tick += OnUpdateFilterTimerTick;
    }

    public override string Title => "Advanced options";
    public DbConnectionStringProperty[] ConnectionStringProperties { get; private set; }
    public bool IsAdvancedOptionsReadOnly { get; set; }
    public DbConnectionString ConnectionString { get; }
    public string? PropertyFilter { get; set; }
    public ICollectionView? ConnectionStringPropertiesCollectionView { get; private set; }

    protected override Task InitializeAsync()
    {
        ConnectionStringProperties = ConnectionString.Properties.Values.Where(x => !x.IsSensitive)
            .OrderBy(x => x.Name)
            .ToArray();

        _propertiesCollectionViewSource = new CollectionViewSource
        {
            Source = ConnectionStringProperties
        };
        _propertiesCollectionViewSource.Filter += OnFilter;

        ConnectionStringPropertiesCollectionView = _propertiesCollectionViewSource.View;

        return base.InitializeAsync();
    }

    private void OnPropertyFilterChanged()
    {
        _updateFilterTimer.Stop();
        _updateFilterTimer.Start();
    }

    private void OnUpdateFilterTimerTick(object? sender, EventArgs e)
    {
        _updateFilterTimer.Stop();

        ConnectionStringPropertiesCollectionView?.Refresh();
    }

    private void OnFilter(object sender, FilterEventArgs e)
    {
        if (e.Item is not DbConnectionStringProperty property)
        {
            e.Accepted = false;
            return;
        }

        var propertyFilter = PropertyFilter;
        if (string.IsNullOrWhiteSpace(propertyFilter))
        {
            e.Accepted = true;
            return;
        }

        e.Accepted = property.Name.ContainsIgnoreCase(propertyFilter)
            || property.Value?.ToString()?.ContainsIgnoreCase(propertyFilter) == true;
    }
}
