namespace Orc.DataAccess.Controls;

using System;
using System.Threading.Tasks;
using Catel.IoC;
using Catel.MVVM;
using Catel.Services;
using Database;

public class DbProviderPickerViewModel : ViewModelBase
{
    private readonly ITypeFactory _typeFactory;
    private readonly IUIVisualizerService _uiVisualizerService;

    public DbProviderPickerViewModel(IUIVisualizerService uiVisualizerService, ITypeFactory typeFactory)
    {
        ArgumentNullException.ThrowIfNull(uiVisualizerService);
        ArgumentNullException.ThrowIfNull(typeFactory);

        _uiVisualizerService = uiVisualizerService;
        _typeFactory = typeFactory;

        ChangeDbProvider = new TaskCommand(OnChangeDbProviderAsync);
    }

    public DbProviderInfo? DbProvider { get; set; }
    public TaskCommand ChangeDbProvider { get; }

    private async Task OnChangeDbProviderAsync()
    {
        var dbProviderListViewModel = _typeFactory.CreateRequiredInstanceWithParametersAndAutoCompletion<DbConnectionProviderListViewModel>(DbProvider);
        var dialogResult = await _uiVisualizerService.ShowDialogAsync(dbProviderListViewModel);
        if (dialogResult.DialogResult ?? false)
        {
            DbProvider = dbProviderListViewModel.DbProvider;
        }
    }
}
