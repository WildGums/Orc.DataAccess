namespace Orc
{
    using Catel.Services;
    using Catel.ThirdPartyNotices;
    using Microsoft.Extensions.DependencyInjection;
    using Orc.DataAccess;

    /// <summary>
    /// Core module which allows the registration of default services in the service collection.
    /// </summary>
    public static class OrcDataAccessModule
    {
        public static IServiceCollection AddOrcDataAccess(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IRegistryKeyService, RegistryKeyService>();

            serviceCollection.AddSingleton<ILanguageSource>(new LanguageResourceSource("Orc.DataAccess", "Orc.DataAccess.Properties", "Resources"));

            serviceCollection.AddSingleton<IThirdPartyNotice>((x) => new LibraryThirdPartyNotice("Orc.DataAccess", "https://github.com/wildgums/orc.dataaccess"));
            serviceCollection.AddSingleton<IThirdPartyNotice>((x) => new ResourceBasedThirdPartyNotice("ExcelDataReader", "https://github.com/ExcelDataReader/ExcelDataReader", "Orc.DataAccess", "Orc.DataAccess", "Resources.ThirdPartyNotices.exceldatareader.txt"));

            return serviceCollection;
        }
    }
}
