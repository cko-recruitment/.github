using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.Clients;
using PaymentGateway.Clients.Contract;
using PaymentGateway.Core;

namespace PaymentGateway.Integration.Tests;

public class AcquiringBankClientTests
{
    private readonly AcquiringBankClient acquiringBankClient;

    public AcquiringBankClientTests()
    {
        acquiringBankClient = SetupFixture.TestServiceProvider.GetRequiredService<AcquiringBankClient>();
    }

    [Test]
    public async Task Should_Return_Accepted_Payment_Request()
    {
        //given
        var paymentRequestData = new PaymentRequestData()
        {
            Amount = 100,
            CardNumber = "2222405343248877",
            ExpiryMonth = 4,
            ExpiryYear = 2025,
            CVV = "123",
            Currency = Currency.GBP
        };

        //when
        var paymentResponse = await acquiringBankClient.RequestPaymentAsync(paymentRequestData, CancellationToken.None);

        //then
        Assert.That(paymentResponse, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(paymentResponse.Status, Is.EqualTo(PaymentStatusData.Authorized));
            Assert.That(paymentResponse.LastFourCardDigits, Is.EqualTo("8877"));
            Assert.That(paymentResponse.Currency, Is.EqualTo(Currency.GBP));
            Assert.That(paymentResponse.ExpiryYear, Is.EqualTo(paymentRequestData.ExpiryYear));
            Assert.That(paymentResponse.ExpiryMonth, Is.EqualTo(paymentRequestData.ExpiryMonth));
        });
    }

    [Test]
    public async Task Should_Return_Declined_Payment_Request()
    {
        //given
        var paymentRequestData = new PaymentRequestData()
        {
            Amount = 60000,
            CardNumber = "2222405343248112",
            ExpiryMonth = 1,
            ExpiryYear = 2026,
            CVV = "456",
            Currency = Currency.USD
        };

        //when
        var paymentResponse = await acquiringBankClient.RequestPaymentAsync(paymentRequestData, CancellationToken.None);

        //then
        Assert.That(paymentResponse, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(paymentResponse.Status, Is.EqualTo(PaymentStatusData.Declined));
            Assert.That(paymentResponse.LastFourCardDigits, Is.EqualTo("8112"));
            Assert.That(paymentResponse.Currency, Is.EqualTo(Currency.USD));
            Assert.That(paymentResponse.ExpiryYear, Is.EqualTo(paymentRequestData.ExpiryYear));
            Assert.That(paymentResponse.ExpiryMonth, Is.EqualTo(paymentRequestData.ExpiryMonth));
        });
    }
}
