namespace Orc.DataAccess.Tests;

using System.Windows;
using Catel.IoC;
using Catel.Services;
using Orc.Automation;
using Orc.Csv;
using Theming;

public class SetupThemeAutomationMethodRun : NamedAutomationMethodRun
{
    public override bool TryInvoke(FrameworkElement owner, AutomationMethod method, out AutomationValue result)
    {
        result = AutomationValue.FromValue(true);

        StyleHelper.CreateStyleForwardersForDefaultStyles();
        ThemeManager.Current.SynchronizeTheme();

        //TODO: Looks like some of it can't be loaded correctly without this calls
#pragma warning disable IDISP001 // Dispose created
        var typeFactory = this.GetTypeFactory();
        var serviceLocator = this.GetServiceLocator();
#pragma warning restore IDISP001 // Dispose created

        var dispatcherService = serviceLocator.ResolveType<IDispatcherService>();
        var csvWriterService = serviceLocator.ResolveType<ICsvWriterService>();
        var uiVisualizerService = serviceLocator.ResolveType<IUIVisualizerService>();

        return true;
    }
}