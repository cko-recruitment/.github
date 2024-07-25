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

    public static IServiceCollection AddDependencies(this IServiceCollection services)
    {
        return services
            .AddSingleton(TimeProvider.System)
            .AddScoped<IAcquiringBankClient, AcquiringBankClient>()
            .AddScoped<IPaymentsRepository, PaymentsRepository>();
    }
}
