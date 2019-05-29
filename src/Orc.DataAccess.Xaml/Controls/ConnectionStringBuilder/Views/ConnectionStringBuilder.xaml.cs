// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectionStringBuilder.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.DataAccess.Controls
{
    using System.Windows;
    using Catel.MVVM.Views;

    public sealed partial class ConnectionStringBuilder
    {
        #region Constructors
        static ConnectionStringBuilder()
        {
            typeof(ConnectionStringBuilder).AutoDetectViewPropertiesToSubscribe();
        }

        public ConnectionStringBuilder()
        {
            InitializeComponent();
        }
        #endregion

        #region Properties
        [ViewToViewModel(MappingType = ViewToViewModelMappingType.ViewModelToView)]
        public string ConnectionString
        {
            get { return (string)GetValue(ConnectionStringProperty); }
            set { SetValue(ConnectionStringProperty, value); }
        }

        public static readonly DependencyProperty ConnectionStringProperty = DependencyProperty.Register(
            nameof(ConnectionString), typeof(string), typeof(ConnectionStringBuilder), new PropertyMetadata(default(string)));


        [ViewToViewModel(MappingType = ViewToViewModelMappingType.ViewModelToView)]
        public string DatabaseProvider
        {
            get { return (string)GetValue(DatabaseProviderProperty); }
            set { SetValue(DatabaseProviderProperty, value); }
        }

        public static readonly DependencyProperty DatabaseProviderProperty = DependencyProperty.Register(
            nameof(DatabaseProvider), typeof(string), typeof(ConnectionStringBuilder), new PropertyMetadata(default(string)));


        [ViewToViewModel(MappingType = ViewToViewModelMappingType.ViewModelToView)]
        public bool IsInEditMode
        {
            get { return (bool)GetValue(IsInEditModeProperty); }
            set { SetValue(IsInEditModeProperty, value); }
        }

        public static readonly DependencyProperty IsInEditModeProperty = DependencyProperty.Register(
            nameof(IsInEditMode), typeof(bool), typeof(ConnectionStringBuilder), new PropertyMetadata(default(bool)));


        [ViewToViewModel(MappingType = ViewToViewModelMappingType.ViewModelToView)]
        public ConnectionState ConnectionState
        {
            get { return (ConnectionState)GetValue(ConnectionStateProperty); }
            set { SetValue(ConnectionStateProperty, value); }
        }

        public static readonly DependencyProperty ConnectionStateProperty = DependencyProperty.Register(
            nameof(ConnectionState), typeof(ConnectionState), typeof(ConnectionStringBuilder), new PropertyMetadata(default(ConnectionState)));


        [ViewToViewModel(MappingType = ViewToViewModelMappingType.ViewToViewModel)]
        public bool IsAdvancedOptionsReadOnly
        {
            get { return (bool)GetValue(IsAdvancedOptionsReadOnlyProperty); }
            set { SetValue(IsAdvancedOptionsReadOnlyProperty, value); }
        }

        public static readonly DependencyProperty IsAdvancedOptionsReadOnlyProperty = DependencyProperty.Register(
            nameof(IsAdvancedOptionsReadOnly), typeof(bool), typeof(ConnectionStringBuilder), new PropertyMetadata(false));


        #endregion
    }
}
