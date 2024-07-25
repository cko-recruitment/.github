using CommunityToolkit.Diagnostics;
using PaymentGateway.Api.Code.Extensions;
using PaymentGateway.Clients;
using PaymentGateway.Clients.Contract;
using PaymentGateway.Persistance;
using PaymentGateway.Persistance.Contract;

namespace PaymentGateway.Api.Code.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        return services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen();
    }

    public static IServiceCollection AddDependencies(this IServiceCollection services, ConfigurationManager configuration)
    {
        var acquiringBankConfiguration = configuration.GetSection("Services:AcquiringBank").Bind<AcquiringBankConfiguration>();
        Guard.IsNotNull(acquiringBankConfiguration?.BaseUrl);

        services.AddHttpClient<AcquiringBankClient>(c => c.BaseAddress = new Uri(acquiringBankConfiguration.BaseUrl));

        return services
            .AddSingleton(TimeProvider.System)
            .AddScoped<IAcquiringBankClient, AcquiringBankClient>()
            .AddScoped<IPaymentsRepository, PaymentsRepository>();
    }
}
