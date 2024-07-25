using System.Net.Http.Json;
using System.Net.Http;
using System.Net;
using System.Text.Json;
using PaymentGateway.Core;
using PaymentGateway.Clients.Contract;
using PaymentGateway.Api.Models;
using NUnit.Framework.Internal;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace PaymentGateway.EndToEnd.Tests;

public class PaymentsControllerTests
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

    [Test]
    public async Task POST_Should_Return_Created_When_AcquiringBank_Response_Received()
    {
        //given
        var paymentRequestModel = new PaymentRequestModel()
        {
            Amount = 100,
            CardNumber = "2222405343248877",
            ExpiryMonth = 4,
            ExpiryYear = 2025,
            CVV = "123",
            Currency = "GBP"
        };
        using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, relativeUrl);
        httpRequestMessage.Content = JsonContent.Create(paymentRequestModel, null, jsonSerializerOptions);

        //when
        using var response = await httpClient.SendAsync(httpRequestMessage);

        //then
        Assert.That(response, Is.Not.Null);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

        var responseContent = await response.Content.ReadAsStringAsync();
        var responseModel = JsonSerializer.Deserialize<PaymentModel>(responseContent, jsonSerializerOptions);

        Assert.That(responseModel, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(responseModel.Currency.ToString(), Is.EqualTo(paymentRequestModel.Currency));
            Assert.That(responseModel.Amount, Is.EqualTo(paymentRequestModel.Amount));
            Assert.That(responseModel.Status, Is.EqualTo(PaymentStatusModel.Authorized));
            Assert.That(responseModel.ExpiryYear, Is.EqualTo(paymentRequestModel.ExpiryYear));
            Assert.That(responseModel.ExpiryMonth, Is.EqualTo(paymentRequestModel.ExpiryMonth));
            Assert.That(responseModel.LastFourCardDigits, Is.EqualTo(paymentRequestModel.CardNumber[^4..]));
        });

        //GET
        //given
        var location = response.Headers.Location;
        Assert.That(location, Is.Not.Null);

        //when
        using var httpGetRequestMessage = new HttpRequestMessage(HttpMethod.Get, location);
        using var getResponse = await httpClient.SendAsync(httpGetRequestMessage);

        //then
        Assert.That(getResponse, Is.Not.Null);
        Assert.That(getResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var getResponseContent = await response.Content.ReadAsStringAsync();
        var getResponseModel = JsonSerializer.Deserialize<PaymentModel>(getResponseContent, jsonSerializerOptions);

        Assert.That(getResponseModel, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(getResponseModel.Currency, Is.EqualTo(responseModel.Currency));
            Assert.That(getResponseModel.Amount, Is.EqualTo(responseModel.Amount));
            Assert.That(getResponseModel.Status, Is.EqualTo(responseModel.Status));
            Assert.That(getResponseModel.ExpiryYear, Is.EqualTo(responseModel.ExpiryYear));
            Assert.That(getResponseModel.ExpiryMonth, Is.EqualTo(responseModel.ExpiryMonth));
            Assert.That(getResponseModel.LastFourCardDigits, Is.EqualTo(responseModel.LastFourCardDigits));
        });
    }
}