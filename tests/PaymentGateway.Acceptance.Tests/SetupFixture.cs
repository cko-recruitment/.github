using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using PaymentGateway.Acceptance.Tests.Code.Configuration;
using PaymentGateway.Api;
using PaymentGateway.Clients.Contract;
using PaymentGateway.Persistance.Contract;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PaymentGateway.Acceptance.Tests;

internal static class SetupFixture
{
    public static ServiceProvider TestServiceProvider { get; }
    private static readonly WebApplicationFactory<Program> webApplicationFactory = new WebApplicationFactory<Program>()
        .WithWebHostBuilder(builder => builder.ConfigureHost());

    static SetupFixture()
    {
        TestServiceProvider = GetTestServiceProvider();
    }

    public static IWebHostBuilder ConfigureHost(this IWebHostBuilder builder)
    {
        builder.UseConfiguration(new ConfigurationBuilder()
                .AddJsonFile("appsettings.acceptance.json", true)
                .Build());
        builder.ConfigureTestServices(services =>
        {
            services.ReplaceScoped<IAcquiringBankClient>(x =>
            {
                var acquiringBankClientMock = new Mock<IAcquiringBankClient>();
                acquiringBankClientMock.Setup(x => x.RequestPaymentAsync(It.IsAny<PaymentRequestData>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync((PaymentRequestData p, CancellationToken t) => new PaymentResponseData()
                    { 
                        Amount = p.Amount,
                        Currency = p.Currency,
                        ExpiryMonth = p.ExpiryMonth,
                        ExpiryYear = p.ExpiryYear,
                        LastFourCardDigits = p.CardNumber[^4..],
                        Status = PaymentStatusData.Authorized
                    });

                return acquiringBankClientMock.Object;
            });
            services.ReplaceScoped<IPaymentsRepository>(x =>
            {
                var paymentsRepositoryMock = new Mock<IPaymentsRepository>();
                paymentsRepositoryMock.Setup(x => x.SaveAsync(It.IsAny<PaymentResponseData>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Guid.NewGuid());

                return paymentsRepositoryMock.Object;
            });
        });
        
        return builder;
    }

    private static ServiceProvider GetTestServiceProvider()
    {
        var services = new ServiceCollection();

        services
            .AddSingleton(webApplicationFactory.CreateClient(
            new WebApplicationFactoryClientOptions()
            {
                AllowAutoRedirect = false
            }))
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
             });

        return services.BuildServiceProvider();
    }
}
