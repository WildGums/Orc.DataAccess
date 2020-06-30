﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectionStringBuilder.cs" company="WildGums">
//   Copyright (c) 2008 - 2020 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.DataAccess.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.Services;
    using Database;

    [TemplatePart(Name = "PART_ConnectionStringTextBox", Type = typeof(TextBox))]
    public class ConnectionStringBuilder : Control
    {
        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly ITypeFactory _typeFactory;
        private readonly IServiceLocator _serviceLocator;
        private readonly IUIVisualizerService _uiVisualizerService;

        private DbProviderInfo _dbProvider;

        private TextBox _connectionStringTextBox;
        #endregion

        #region Constructors
        public ConnectionStringBuilder()
        {
            _typeFactory = this.GetTypeFactory();
            _serviceLocator = this.GetServiceLocator();

            _uiVisualizerService = _serviceLocator.ResolveType<IUIVisualizerService>();

            var editCommandBinding = new CommandBinding {Command = EditCommand};
            editCommandBinding.Executed += OnEditCommandExecuted;
            CommandBindings.Add(editCommandBinding);

            var clearCommandBinding = new CommandBinding {Command = ClearCommand};
            clearCommandBinding.Executed += OnClearCommandExecuted;
            clearCommandBinding.CanExecute += CanClearCommandExecute;
            CommandBindings.Add(clearCommandBinding);
        }
        #endregion

        #region Routed Commands
        public static RoutedCommand EditCommand { get; } = new RoutedCommand(nameof(EditCommand), typeof(ConnectionStringBuilder));

        private async void OnEditCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SetCurrentValue(IsInEditModeProperty, true);

            var connectionStringEditViewModel = _typeFactory.CreateInstanceWithParametersAndAutoCompletion<ConnectionStringEditViewModel>(ConnectionString, _dbProvider);
            connectionStringEditViewModel.IsAdvancedOptionsReadOnly = IsAdvancedOptionsReadOnly;

            if (await _uiVisualizerService.ShowDialogAsync(connectionStringEditViewModel) ?? false)
            {
                _dbProvider = connectionStringEditViewModel.DbProvider;
                var connectionString = connectionStringEditViewModel.ConnectionString;

                SetCurrentValue(ConnectionStringProperty, connectionString?.ToString());
                _connectionStringTextBox.SetCurrentValue(TextBox.TextProperty, connectionString?.ToDisplayString());
                SetCurrentValue(ConnectionStateProperty, connectionStringEditViewModel.ConnectionState);
                SetCurrentValue(DatabaseProviderProperty, connectionString?.DbProvider.InvariantName);
            }

            SetCurrentValue(IsInEditModeProperty, false);
        }

        public static RoutedCommand ClearCommand { get; } = new RoutedCommand(nameof(ClearCommand), typeof(ConnectionStringBuilder));

        private void OnClearCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SetCurrentValue(ConnectionStringProperty, null);
            _connectionStringTextBox.SetCurrentValue(TextBox.TextProperty, null);
            SetCurrentValue(ConnectionStateProperty, ConnectionState.Undefined);
            _dbProvider = null;
        }

        private void CanClearCommandExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ConnectionString != null;
        }
        #endregion

        #region Dependency Properties
        public string ConnectionString
        {
            get { return (string)GetValue(ConnectionStringProperty); }
            set { SetValue(ConnectionStringProperty, value); }
        }

        public static readonly DependencyProperty ConnectionStringProperty = DependencyProperty.Register(
            nameof(ConnectionString), typeof(string), typeof(ConnectionStringBuilder), new PropertyMetadata(default(string)));

        public string DatabaseProvider
        {
            get { return (string)GetValue(DatabaseProviderProperty); }
            set { SetValue(DatabaseProviderProperty, value); }
        }

        public static readonly DependencyProperty DatabaseProviderProperty = DependencyProperty.Register(
            nameof(DatabaseProvider), typeof(string), typeof(ConnectionStringBuilder), new PropertyMetadata(default(string)));

        public bool IsInEditMode
        {
            get { return (bool)GetValue(IsInEditModeProperty); }
            set { SetValue(IsInEditModeProperty, value); }
        }

        public static readonly DependencyProperty IsInEditModeProperty = DependencyProperty.Register(
            nameof(IsInEditMode), typeof(bool), typeof(ConnectionStringBuilder), new PropertyMetadata(default(bool)));

        public ConnectionState ConnectionState
        {
            get { return (ConnectionState)GetValue(ConnectionStateProperty); }
            set { SetValue(ConnectionStateProperty, value); }
        }

        public static readonly DependencyProperty ConnectionStateProperty = DependencyProperty.Register(
            nameof(ConnectionState), typeof(ConnectionState), typeof(ConnectionStringBuilder), new PropertyMetadata(default(ConnectionState)));


        public bool IsAdvancedOptionsReadOnly
        {
            get { return (bool)GetValue(IsAdvancedOptionsReadOnlyProperty); }
            set { SetValue(IsAdvancedOptionsReadOnlyProperty, value); }
        }

        public static readonly DependencyProperty IsAdvancedOptionsReadOnlyProperty = DependencyProperty.Register(
            nameof(IsAdvancedOptionsReadOnly), typeof(bool), typeof(ConnectionStringBuilder), new PropertyMetadata(false));
        #endregion

        #region Methods
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _connectionStringTextBox = GetTemplateChild("PART_ConnectionStringTextBox") as TextBox;
            if (_connectionStringTextBox is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_ConnectionStringTextBox'");
            }
        }
        #endregion
    }
}
