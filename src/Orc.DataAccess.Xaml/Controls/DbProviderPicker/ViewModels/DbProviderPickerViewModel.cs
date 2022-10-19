namespace Orc.DataAccess.Controls
{
    using System.Threading.Tasks;
    using Catel;
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

        public DbProviderInfo DbProvider { get; set; }
        public TaskCommand ChangeDbProvider { get; }

        private async Task OnChangeDbProviderAsync()
        {
            var dbProviderListViewModel = _typeFactory.CreateInstanceWithParametersAndAutoCompletion<DbConnectionProviderListViewModel>(DbProvider);
            if (await _uiVisualizerService.ShowDialogAsync(dbProviderListViewModel) ?? false)
            {
                DbProvider = dbProviderListViewModel.DbProvider;
            }
        }
    }
}
