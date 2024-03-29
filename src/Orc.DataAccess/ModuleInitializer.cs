﻿using Catel.IoC;
using Catel.Services;
using Orc.DataAccess;

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

        var languageService = serviceLocator.ResolveRequiredType<ILanguageService>();
        languageService.RegisterLanguageSource(new LanguageResourceSource("Orc.DataAccess", "Orc.DataAccess.Properties", "Resources"));

        serviceLocator.RegisterType<IRegistryKeyService, RegistryKeyService>();
    }
}
