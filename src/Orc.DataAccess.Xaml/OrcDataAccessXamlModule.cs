namespace Orc;

using System;
using Catel.IoC;
using Catel.MVVM;
using Catel.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orc.DataAccess.Controls;

/// <summary>
/// Core module which allows the registration of default services in the service collection.
/// </summary>
public static class OrcDataAccessXamlModule
{
    public static IServiceCollection AddOrcDataAccessXaml(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<ViewModelLocatorInitializer>();

        serviceCollection.AddSingleton<ILanguageSource>(new LanguageResourceSource("Orc.DataAccess.Xaml", "Orc.DataAccess.Properties", "Resources"));

        return serviceCollection;
    }

    private class ViewModelLocatorInitializer : IConstructAtStartup
    {
        public ViewModelLocatorInitializer(IViewModelLocator viewModelLocator)
        {
            viewModelLocator.Register(typeof(ConnectionStringEditWindow), typeof(ConnectionStringEditViewModel));
            viewModelLocator.Register(typeof(ConnectionStringAdvancedOptionsWindow), typeof(ConnectionStringAdvancedOptionsViewModel));
            viewModelLocator.Register(typeof(DbConnectionProviderListWindow), typeof(DbConnectionProviderListViewModel));
        }
    }
}
