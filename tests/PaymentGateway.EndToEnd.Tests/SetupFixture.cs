using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace PaymentGateway.EndToEnd.Tests;

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
            .AddJsonFile("appsettings.e2e.json", true)
            .AddEnvironmentVariables()
            .Build();

        services
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
            .AddHttpClient(String.Empty, c => c.BaseAddress = new Uri(configuration.GetSection("Services:Api:BaseUrl").Value!));

        return services.BuildServiceProvider();
    }
}
