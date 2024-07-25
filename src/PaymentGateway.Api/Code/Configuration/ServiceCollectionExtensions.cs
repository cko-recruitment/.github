using CommunityToolkit.Diagnostics;
using PaymentGateway.Api.Code.Extensions;
using PaymentGateway.Clients;
using PaymentGateway.Clients.Contract;
using PaymentGateway.Persistance;
using PaymentGateway.Persistance.Contract;
using System.Text.Json;
using System.Text.Json.Serialization;

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

        services.AddHttpClient(String.Empty, c =>
        {
            c.BaseAddress = new Uri(acquiringBankConfiguration.BaseUrl);
        });

        return services
            .AddSingleton(x =>
            {
                var jsonSerializerOptions = new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    WriteIndented = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                };
                jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

                return jsonSerializerOptions;
            })
            .AddSingleton(TimeProvider.System)
            .AddSingleton<IPaymentsRepository, PaymentsRepository>()
            .AddScoped<IAcquiringBankClient, AcquiringBankClient>();
    }
}
