﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DbConnectionProviderListViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.DataAccess.Controls
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Linq;
    using System.Threading.Tasks;
    using Catel.MVVM;
    using Catel.Threading;

    public class DbConnectionProviderListViewModel : ViewModelBase
    {
        #region Fields
        private readonly DbProvider _selectedProvider;
        #endregion

        #region Constructors
        public DbConnectionProviderListViewModel(DbProvider selectedProvider)
        {
            _selectedProvider = selectedProvider;

            Open = new Command(OnOpen);
            Refresh = new Command(OnRefresh);
        }
        #endregion

        #region Properties
        public override string Title => "Select provider";

        public DbProvider DbProvider { get; set; }
        public IList<DbProvider> DbProviders { get; private set; }
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
            DbProviders = DbProviderFactories.GetFactoryClasses().Rows.OfType<DataRow>()
                .Select(x => new DbProvider
                {
                    Name = x["Name"]?.ToString(),
                    Description = x["Description"]?.ToString(),
                    InvariantName = x["InvariantName"]?.ToString(),
                })
                .OrderBy(x => x.Name)
                .ToList();

            DbProvider = DbProviders.FirstOrDefault(x => x.Equals(_selectedProvider));
        }
        #endregion
    }
}
