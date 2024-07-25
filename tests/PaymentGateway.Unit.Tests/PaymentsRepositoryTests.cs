using PaymentGateway.Clients.Contract;
using PaymentGateway.Core;
using PaymentGateway.Persistance;

namespace PaymentGateway.Unit.Tests;

public class PaymentsRepositoryTests
{
    [Test]
    public async Task Should_Save_Payments()
    {
        //given
        var paymentResponseDataItems = new Dictionary<Guid, PaymentResponseData>();
        var paymentsRepository = new PaymentsRepository(paymentResponseDataItems);
        var paymentId = Guid.NewGuid();
        var expectedPaymentResponseData = new PaymentResponseData()
        {
            Amount = 5,
            Currency = Currency.GBP,
            ExpiryMonth = 4,
            ExpiryYear = 2025,
            LastFourCardDigits = "1234",
            Status = PaymentStatusData.Declined
        };

        //when
        await paymentsRepository.SaveAsync(expectedPaymentResponseData, CancellationToken.None);

        //then
        Assert.That(paymentResponseDataItems, Is.Not.Null);
        Assert.That(paymentResponseDataItems.Count, Is.EqualTo(1));
    }

    [Test]
    public async Task Should_Return_Existing_Payment_By_Id()
    {
        //given
        var paymentId = Guid.NewGuid();
        var expectedPaymentResponseData = new PaymentResponseData()
        {
            Amount = 5,
            Currency = Currency.GBP,
            ExpiryMonth = 4,
            ExpiryYear = 2025,
            LastFourCardDigits = "1234",
            Status = PaymentStatusData.Declined
        };
        var paymentResponseDataItems = new Dictionary<Guid, PaymentResponseData>();
        paymentResponseDataItems.Add(paymentId, expectedPaymentResponseData);
        var paymentsRepository = new PaymentsRepository(paymentResponseDataItems);

        //when
        var paymentResponseData = await paymentsRepository.GetAsync(paymentId, CancellationToken.None);

        //then
        Assert.That(paymentResponseDataItems, Is.Not.Null);
    }

    [Test]
    public async Task Should_Return_NULL_When_Payment_With_Id_Not_Found()
    {
        //given
        var paymentId = Guid.NewGuid();
        var paymentResponseDataItems = new Dictionary<Guid, PaymentResponseData>();
        var paymentsRepository = new PaymentsRepository(paymentResponseDataItems);

        //when
        var paymentResponseData = await paymentsRepository.GetAsync(paymentId, CancellationToken.None);

        //then
        Assert.That(paymentResponseData, Is.Null);
    }
}
