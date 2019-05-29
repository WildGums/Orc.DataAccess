// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DbConnectionProviderListViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.DataAccess.Controls
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Catel.MVVM;
    using Catel.Threading;
    using Database;

    public class DbConnectionProviderListViewModel : ViewModelBase
    {
        #region Fields
        private readonly DbProviderInfo _selectedProvider;
        #endregion

        #region Constructors
        public DbConnectionProviderListViewModel(DbProviderInfo selectedProvider)
        {
            _selectedProvider = selectedProvider;

            Open = new Command(OnOpen);
            Refresh = new Command(OnRefresh);
        }
        #endregion

        #region Properties
        public override string Title => "Select provider";

        public DbProviderInfo DbProvider { get; set; }
        public IList<DbProviderInfo> DbProviders { get; private set; }
        public Command Refresh { get; }
        public Command Open { get; }
        #endregion

        #region Methods
        protected override Task InitializeAsync()
        {
            OnRefresh();

            return base.InitializeAsync();
        }

        private void OnOpen()
        {
            if (DbProvider == null)
            {
                return;
            }

            TaskHelper.RunAndWaitAsync(async () => await CloseViewModelAsync(true));
        }

        private void OnRefresh()
        {
            DbProviders = Database.DbProvider.GetRegisteredProviders().Select(x => x.Value.Info).ToList();
            DbProvider = DbProviders.FirstOrDefault(x => x.Equals(_selectedProvider));
        }
        #endregion
    }
}
