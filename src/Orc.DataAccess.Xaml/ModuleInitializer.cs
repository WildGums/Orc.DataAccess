// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModuleInitializer.cs" company="WildGums">
//   Copyright (c) 2008 - 2019 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Catel.IoC;
using Catel.MVVM;
using Catel.Services;
using Orc.DataAccess.Controls;

/// <summary>
///     Used by the ModuleInit. All code inside the Initialize method is ran as soon as the assembly is loaded.
/// </summary>
public static class ModuleInitializer
{
    /// <summary>
    ///     Initializes the module.
    /// </summary>
    public static void Initialize()
    {
        var serviceLocator = ServiceLocator.Default;

        var viewModelLocator = serviceLocator.ResolveType<IViewModelLocator>();
        viewModelLocator.Register(typeof(ConnectionStringEditWindow), typeof(ConnectionStringEditViewModel));
        viewModelLocator.Register(typeof(ConnectionStringAdvancedOptionsWindow), typeof(ConnectionStringAdvancedOptionsViewModel));
        viewModelLocator.Register(typeof(DbConnectionProviderListWindow), typeof(DbConnectionProviderListViewModel));

        var languageService = serviceLocator.ResolveType<ILanguageService>();
        languageService.RegisterLanguageSource(new LanguageResourceSource("Orc.DataAccess", "Orc.DataAccess.Properties", "Resources"));
    }
}
