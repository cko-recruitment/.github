using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PaymentGateway.Acceptance.Tests;

public partial class PaymentsControllerTests
{
    private const string relativeUrl = "api/payments";
    private static JsonSerializerOptions jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Structure", "NUnit1032:An IDisposable field/property should be Disposed in a TearDown method", Justification = "Singleton http client")]
    private readonly HttpClient httpClient;

    public PaymentsControllerTests()
    {
        httpClient = SetupFixture.TestServiceProvider.GetRequiredService<HttpClient>();
        jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    }
}