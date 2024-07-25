using PaymentGateway.Clients.Contract;

namespace PaymentGateway.Clients.Extensions;

internal static class PaymentResponseExternalModelExtensions
{
    public static PaymentResponseData ToData(this PaymentResponseExternalModel model, PaymentRequestData paymentRequestData)
    {
        var paymentStatus = model.Authorized
            ? PaymentStatusData.Authorized
            : PaymentStatusData.Declined;

        return new PaymentResponseData()
        {
            Amount = paymentRequestData.Amount,
            Currency = paymentRequestData.Currency,
            ExpiryMonth = paymentRequestData.ExpiryMonth,
            ExpiryYear = paymentRequestData.ExpiryYear,
            LastFourCardDigits = paymentRequestData.CardNumber[^4..],
            Status = paymentStatus
        };
    }
}
