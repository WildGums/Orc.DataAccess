﻿namespace Orc.DataAccess.Controls;

using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Catel.MVVM;
using CsvHelper;
using Database;

public class DbConnectionProviderListViewModel : ViewModelBase
{
    private readonly DbProviderInfo _selectedProvider;

    public DbConnectionProviderListViewModel(DbProviderInfo selectedProvider)
    {
        _selectedProvider = selectedProvider;

        Open = new TaskCommand(OnOpenAsync);
        Refresh = new Command(OnRefresh);

        DbProviders = new List<DbProviderInfo>();
    }

    public override string Title => "Select provider";
    public DbProviderInfo? DbProvider { get; set; }
    public IList<DbProviderInfo> DbProviders { get; private set; }
    public Command Refresh { get; }
    public TaskCommand Open { get; }

    protected override Task InitializeAsync()
    {
        OnRefresh();

        return base.InitializeAsync();
    }

    private async Task OnOpenAsync()
    {
        if (DbProvider is null)
        {
            return;
        }

        await CloseViewModelAsync(true);
    }

    private void OnRefresh()
    {
        DbProviders = Database.DbProvider.GetRegisteredProviders().Select(x => x.Value.Info).ToList();
        DbProvider = DbProviders.FirstOrDefault(x => x.Equals(_selectedProvider));
    }

    private void On()
    {
        Orc.DataAccess.Database.
            DbProvider.UnregisterProvider(new DbProviderInfo(string.Empty, "Microsoft.Data.SqlClient", string.Empty, string.Empty));

        DbProviderFactories.UnregisterFactory("Microsoft.Data.SqlClient");
    }
}
