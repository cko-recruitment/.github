﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.Clients;
using System.Text.Json.Serialization;
using System.Text.Json;

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
            .AddEnvironmentVariables()
            .Build();
        var baseUrl = configuration.GetSection("Services:AcquiringBank:BaseUrl").Value;

        services
            .AddTransient<AcquiringBankClient>()
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
            .AddHttpClient<AcquiringBankClient>(c => c.BaseAddress = new Uri(baseUrl!));

        return services.BuildServiceProvider();
    }
}
