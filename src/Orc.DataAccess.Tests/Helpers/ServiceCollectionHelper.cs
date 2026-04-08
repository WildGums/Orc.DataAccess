namespace Orc.DataAccess.Tests;

using Catel;
using Microsoft.Extensions.DependencyInjection;
using Orc.DataAccess;

internal static class ServiceCollectionHelper
{
    public static IServiceCollection CreateServiceCollection()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddLogging();
        serviceCollection.AddCatelCore();
        serviceCollection.AddCatelMvvm();
        serviceCollection.AddOrcDataAccess();
        serviceCollection.AddOrcDataAccessXaml();

        return serviceCollection;
    }
}
