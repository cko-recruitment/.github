using PaymentGateway.Api.Models;
using PaymentGateway.Core;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace PaymentGateway.Acceptance.Tests;

public partial class PaymentsControllerTests
{
    private static int validExpiryYear = DateTime.UtcNow.Year + 1;

    [Test]
    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(null)]
    public async Task POST_Should_Return_BadRequest_When_Amount_Is_Invalid_Or_Missing(int? amount)
    {
        //given
        var paymentRequestModel = new
        {
            Amount = amount,
            CardNumber = "2222405343248877",
            Currency = Currency.USD.ToString(),
            CVV = "123",
            ExpiryMonth = 1,
            ExpiryYear = validExpiryYear
        };
        using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, relativeUrl);
        httpRequestMessage.Content = JsonContent.Create(paymentRequestModel);

        //when
        using var response = await httpClient.SendAsync(httpRequestMessage);

        //then
        Assert.That(response, Is.Not.Null);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [TestCase("1234")]
    [TestCase("")]
    [TestCase("12345678901234567890")]
    [TestCase(null)]
    public async Task POST_Should_Return_BadRequest_When_CardNumber_Is_Invalid_Or_Missing(string? cardNumber)
    {
        //given
        var paymentRequestModel = new
        {
            Amount = 1,
            CardNumber = cardNumber,
            Currency = Currency.USD.ToString(),
            CVV = "123",
            ExpiryMonth = 1,
            ExpiryYear = validExpiryYear
        };
        using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, relativeUrl);
        httpRequestMessage.Content = JsonContent.Create(paymentRequestModel);

        //when
        using var response = await httpClient.SendAsync(httpRequestMessage);

        //then
        Assert.That(response, Is.Not.Null);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [TestCase(null)]
    [TestCase("invalid")]
    [TestCase("ABC")]
    public async Task POST_Should_Return_BadRequest_When_Currency_Is_Invalid_Or_Missing(string? currency)
    {
        //given
        var paymentRequestModel = new
        {
            Amount = 1,
            CardNumber = "2222405343248877",
            Currency = currency,
            CVV = "123",
            ExpiryMonth = 1,
            ExpiryYear = validExpiryYear
        };
        using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, relativeUrl);
        httpRequestMessage.Content = JsonContent.Create(paymentRequestModel);

        //when
        using var response = await httpClient.SendAsync(httpRequestMessage);

        //then
        Assert.That(response, Is.Not.Null);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [TestCase(null)]
    [TestCase("12")]
    [TestCase("12345")]
    public async Task POST_Should_Return_BadRequest_When_CVV_Is_Invalid_Or_Missing(string? cvv)
    {
        //given
        var paymentRequestModel = new
        {
            Amount = 1,
            CardNumber = "2222405343248877",
            Currency = Currency.USD.ToString(),
            CVV = cvv,
            ExpiryMonth = 1,
            ExpiryYear = validExpiryYear
        };
        using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, relativeUrl);
        httpRequestMessage.Content = JsonContent.Create(paymentRequestModel);

        //when
        using var response = await httpClient.SendAsync(httpRequestMessage);

        //then
        Assert.That(response, Is.Not.Null);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [TestCase(null)]
    [TestCase(0)]
    [TestCase(-1)]
    public async Task POST_Should_Return_BadRequest_When_ExpiryMonth_Is_Invalid_Or_Missing(int? expiryMonth)
    {
        //given
        var paymentRequestModel = new
        {
            Amount = 1,
            CardNumber = "2222405343248877",
            Currency = Currency.USD.ToString(),
            CVV = "123",
            ExpiryMonth = expiryMonth,
            ExpiryYear = validExpiryYear
        };
        using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, relativeUrl);
        httpRequestMessage.Content = JsonContent.Create(paymentRequestModel);

        //when
        using var response = await httpClient.SendAsync(httpRequestMessage);

        //then
        Assert.That(response, Is.Not.Null);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [TestCase(null)]
    [TestCase(0)]
    [TestCase(-1)]
    public async Task POST_Should_Return_BadRequest_When_ExpiryYear_Is_Invalid_Or_Missing(int? expiryYear)
    {
        //given
        var paymentRequestModel = new
        {
            Amount = 1,
            CardNumber = "2222405343248877",
            Currency = Currency.USD.ToString(),
            CVV = "123",
            ExpiryMonth = 1,
            ExpiryYear = expiryYear
        };
        using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, relativeUrl);
        httpRequestMessage.Content = JsonContent.Create(paymentRequestModel);

        //when
        using var response = await httpClient.SendAsync(httpRequestMessage);

        //then
        Assert.That(response, Is.Not.Null);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task POST_Should_Return_BadRequest_When_ExpiryDate_Is_In_The_Past()
    {
        //given
        var expiryYear = DateTime.UtcNow.Year - 1;
        var paymentRequestModel = new
        {
            Amount = 1,
            CardNumber = "2222405343248877",
            Currency = Currency.USD.ToString(),
            CVV = "123",
            ExpiryMonth = 1,
            ExpiryYear = expiryYear
        };
        using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, relativeUrl);
        httpRequestMessage.Content = JsonContent.Create(paymentRequestModel);

        //when
        using var response = await httpClient.SendAsync(httpRequestMessage);

        //then
        Assert.That(response, Is.Not.Null);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task POST_Should_Return_PaymentRequired_When_AcquiringBank_Declines_Payment()
    {
        //given
        var paymentRequestModel = new
        {
            Amount = 1,
            CardNumber = "4263982640269299",
            Currency = Currency.USD.ToString(),
            CVV = "123",
            ExpiryMonth = 1,
            ExpiryYear = validExpiryYear
        };
        using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, relativeUrl);
        httpRequestMessage.Content = JsonContent.Create(paymentRequestModel);

        //when
        using var response = await httpClient.SendAsync(httpRequestMessage);

        //then
        Assert.That(response, Is.Not.Null);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

        var responseContent = await response.Content.ReadAsStringAsync();
        var responseModel = JsonSerializer.Deserialize<PaymentModel>(responseContent, jsonSerializerOptions);

        Assert.That(responseModel, Is.Not.Null);
        Assert.That(responseModel.Id, Is.Not.EqualTo(Guid.Empty));
        Assert.That(responseModel.Currency.ToString(), Is.EqualTo(paymentRequestModel.Currency));
        Assert.That(responseModel.Amount, Is.EqualTo(paymentRequestModel.Amount));
        Assert.That(responseModel.Status, Is.EqualTo(PaymentStatusModel.Authorized));
        Assert.That(responseModel.ExpiryYear, Is.EqualTo(paymentRequestModel.ExpiryYear));
        Assert.That(responseModel.ExpiryMonth, Is.EqualTo(paymentRequestModel.ExpiryMonth));
        Assert.That(responseModel.LastFourCardDigits, Is.EqualTo(paymentRequestModel.CardNumber[^4..]));
    }
}