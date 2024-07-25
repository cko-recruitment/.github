using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PaymentGateway.Acceptance.Tests;

public partial class PaymentsControllerTests
{
    private const string relativeUrl = "api/payments";
    private readonly JsonSerializerOptions jsonSerializerOptions;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Structure", "NUnit1032:An IDisposable field/property should be Disposed in a TearDown method", Justification = "Singleton http client")]
    private readonly HttpClient httpClient;

    public PaymentsControllerTests()
    {
        httpClient = SetupFixture.TestServiceProvider.GetRequiredService<HttpClient>();
        jsonSerializerOptions = SetupFixture.TestServiceProvider.GetRequiredService<JsonSerializerOptions>();
    }
}