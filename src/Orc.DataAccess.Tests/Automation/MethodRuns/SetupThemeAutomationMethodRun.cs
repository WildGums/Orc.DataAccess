namespace Orc.DataAccess.Tests;

using System.Windows;
using Catel.IoC;
using Catel.Services;
using Microsoft.Extensions.DependencyInjection;
using Orc.Automation;
using Orc.Csv;
using Theming;

public class SetupThemeAutomationMethodRun : NamedAutomationMethodRun
{
    public override bool TryInvoke(FrameworkElement owner, AutomationMethod method, out AutomationValue result)
    {
        var serviceCollection = ServiceCollectionHelper.CreateServiceCollection();

        using var serviceProvider = serviceCollection.BuildServiceProvider();

        result = AutomationValue.FromValue(true);

        StyleHelper.CreateStyleForwardersForDefaultStyles();
        ThemeManager.Current.SynchronizeTheme();

        //var dispatcherService = serviceLocator.ResolveType<IDispatcherService>();
        //var csvWriterService = serviceLocator.ResolveType<ICsvWriterService>();
        //var uiVisualizerService = serviceLocator.ResolveType<IUIVisualizerService>();

        return true;
    }
}
