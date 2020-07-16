// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DbProviderPicker.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.DataAccess.Controls
{
    using System.Windows;
    using Catel.MVVM.Views;
    using Database;

    public sealed partial class DbProviderPicker
    {
        #region Constants
        public static readonly DependencyProperty DbProviderProperty = DependencyProperty.Register(
            nameof(DbProvider), typeof(DbProviderInfo), typeof(DbProviderPicker), new PropertyMetadata(default(DbProviderInfo)));
        #endregion

        #region Constructors
        static DbProviderPicker()
        {
            typeof(DbProviderPicker).AutoDetectViewPropertiesToSubscribe();
        }

        public DbProviderPicker()
        {
            InitializeComponent();
        }
        #endregion

        #region Properties
        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewModelWins)]
        public DbProviderInfo DbProvider
        {
            get { return (DbProviderInfo)GetValue(DbProviderProperty); }
            set { SetValue(DbProviderProperty, value); }
        }
        #endregion
    }
}
