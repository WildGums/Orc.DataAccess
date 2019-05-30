using System.Linq;
using Catel.IoC;
using Catel.Logging;
using Catel.MVVM;
using Catel.Services;
using Orc.Controls;
using ConnectionStringAdvancedOptionsViewModel = Orc.DataAccess.Controls.ConnectionStringAdvancedOptionsViewModel;
using ConnectionStringAdvancedOptionsWindow = Orc.DataAccess.Controls.ConnectionStringAdvancedOptionsWindow;
using ConnectionStringBuilder = Orc.DataAccess.Controls.ConnectionStringBuilder;
using ConnectionStringBuilderService = Orc.DataAccess.Controls.ConnectionStringBuilderService;
using ConnectionStringBuilderViewModel = Orc.DataAccess.Controls.ConnectionStringBuilderViewModel;
using ConnectionStringEditViewModel = Orc.DataAccess.Controls.ConnectionStringEditViewModel;
using ConnectionStringEditWindow = Orc.DataAccess.Controls.ConnectionStringEditWindow;
using DbConnectionProviderListViewModel = Orc.DataAccess.Controls.DbConnectionProviderListViewModel;
using DbConnectionProviderListWindow = Orc.DataAccess.Controls.DbConnectionProviderListWindow;
using IConnectionStringBuilderService = Orc.DataAccess.Controls.IConnectionStringBuilderService;

/// <summary>
/// Used by the ModuleInit. All code inside the Initialize method is ran as soon as the assembly is loaded.
/// </summary>
public static class ModuleInitializer
{
    /// <summary>
    /// Initializes the module.
    /// </summary>
    public static void Initialize()
    {
        var serviceLocator = ServiceLocator.Default;

        serviceLocator.RegisterTypeIfNotYetRegistered<IConnectionStringBuilderServiceInitializer, ConnectionStringBuilderServiceInitializer>();
        serviceLocator.RegisterType<IConnectionStringBuilderService, ConnectionStringBuilderService>();

        var viewModelLocator = serviceLocator.ResolveType<IViewModelLocator>();
        viewModelLocator.Register(typeof(ConnectionStringBuilder), typeof(ConnectionStringBuilderViewModel));
        viewModelLocator.Register(typeof(ConnectionStringEditWindow), typeof(ConnectionStringEditViewModel));
        viewModelLocator.Register(typeof(ConnectionStringAdvancedOptionsWindow), typeof(ConnectionStringAdvancedOptionsViewModel));
        viewModelLocator.Register(typeof(DbConnectionProviderListWindow), typeof(DbConnectionProviderListViewModel));
        
        var languageService = serviceLocator.ResolveType<ILanguageService>();
        languageService.RegisterLanguageSource(new LanguageResourceSource("Orc.DataAccess", "Orc.DataAccess.Properties", "Resources"));
    }
}
