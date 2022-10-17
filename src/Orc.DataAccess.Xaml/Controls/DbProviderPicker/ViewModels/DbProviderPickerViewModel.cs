﻿namespace Orc.DataAccess.Controls
{
    using System.Threading.Tasks;
    using Catel;
    using Catel.IoC;
    using Catel.MVVM;
    using Catel.Services;
    using Database;

    public class DbProviderPickerViewModel : ViewModelBase
    {
        #region Fields
        private readonly ITypeFactory _typeFactory;
        private readonly IUIVisualizerService _uiVisualizerService;
        #endregion

        #region Constructors
        public DbProviderPickerViewModel(IUIVisualizerService uiVisualizerService, ITypeFactory typeFactory)
        {
            Argument.IsNotNull(() => uiVisualizerService);
            Argument.IsNotNull(() => typeFactory);

            _uiVisualizerService = uiVisualizerService;
            _typeFactory = typeFactory;

            ChangeDbProvider = new TaskCommand(OnChangeDbProviderAsync);
        }
        #endregion

        #region Properties
        public DbProviderInfo DbProvider { get; set; }
        public TaskCommand ChangeDbProvider { get; }
        #endregion

        #region Methods
        private async Task OnChangeDbProviderAsync()
        {
            var dbProviderListViewModel = _typeFactory.CreateInstanceWithParametersAndAutoCompletion<DbConnectionProviderListViewModel>(DbProvider);
            if (await _uiVisualizerService.ShowDialogAsync(dbProviderListViewModel) ?? false)
            {
                DbProvider = dbProviderListViewModel.DbProvider;
            }
        }
        #endregion
    }
}
