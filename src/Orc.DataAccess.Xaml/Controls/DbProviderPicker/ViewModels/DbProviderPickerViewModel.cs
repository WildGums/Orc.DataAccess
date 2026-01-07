namespace Orc.DataAccess.Controls;

using System;
using System.Threading.Tasks;
using Catel.MVVM;
using Catel.Services;
using Database;

public class DbProviderPickerViewModel : ViewModelBase
{
    private readonly IUIVisualizerService _uiVisualizerService;

    public DbProviderPickerViewModel(IUIVisualizerService uiVisualizerService, IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
        _uiVisualizerService = uiVisualizerService;

        ChangeDbProvider = new TaskCommand(serviceProvider, OnChangeDbProviderAsync);
    }

    public DbProviderInfo? DbProvider { get; set; }
    public TaskCommand ChangeDbProvider { get; }

    private async Task OnChangeDbProviderAsync()
    {
        var dialogResult = await _uiVisualizerService.ShowDialogAsync<DbConnectionProviderListViewModel>(DbProvider);
        if (dialogResult.DialogResult ?? false)
        {
            var vm = dialogResult.GetViewModel<DbConnectionProviderListViewModel>();
            DbProvider = vm!.DbProvider;
        }
    }
}
