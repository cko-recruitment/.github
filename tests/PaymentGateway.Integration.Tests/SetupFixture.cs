using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.Clients;

namespace PaymentGateway.Integration.Tests;

internal static class SetupFixture
{
    public static ServiceProvider TestServiceProvider { get; }

    static SetupFixture()
    {
        TestServiceProvider = GetTestServiceProvider();
    }

    private static ServiceProvider GetTestServiceProvider()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.integration.json", true)
            .Build();
        var baseUrl = configuration.GetSection("Services:AcquiringBank:BaseUrl").Value;

        services.AddTransient<AcquiringBankClient>();
        services.AddHttpClient<AcquiringBankClient>(c => c.BaseAddress = new Uri(baseUrl!));

        return services.BuildServiceProvider();
    }
}
