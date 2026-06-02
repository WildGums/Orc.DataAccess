namespace Orc.DataAccess.Controls;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catel;
using Catel.MVVM;
using Catel.Services;
using Database;

public partial class DbConnectionProviderListViewModel : ViewModelBase
{
    private readonly DbProviderInfo? _selectedProvider;
    private readonly ILanguageService _languageService;

    public DbConnectionProviderListViewModel(IServiceProvider serviceProvider,
        ILanguageService languageService)
        : base(serviceProvider)
    {
        _languageService = languageService;

        Open = new TaskCommand(serviceProvider, OnOpenAsync);
        Refresh = new Command(serviceProvider, OnRefresh);

        DbProviders = new List<DbProviderInfo>();
    }

    public DbConnectionProviderListViewModel(DbProviderInfo selectedProvider, 
        IServiceProvider serviceProvider, ILanguageService languageService)
        : this(serviceProvider, languageService)
    {
        _selectedProvider = selectedProvider;
    }

    public override string Title => _languageService.GetRequiredString(nameof(Properties.Resources.Controls_DbConnectionProviderList_Title));
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
        DbProviders = Database.DbProvider.GetRegisteredProviders(ServiceProvider).Select(x => x.Value.Info).ToList();
        DbProvider = DbProviders.FirstOrDefault(x => x.Equals(_selectedProvider));
    }
}
