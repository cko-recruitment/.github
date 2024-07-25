using PaymentGateway.Clients.Contract;

namespace PaymentGateway.Clients.Extensions;

internal static class PaymentRequestDataExtensions
{
    public static PaymentRequestExternalModel ToExternalModel(this PaymentRequestData paymentRequestData)
    {
        return new PaymentRequestExternalModel()
        {
            Amount = paymentRequestData.Amount,
            CardNumber = paymentRequestData.CardNumber,
            Currency = paymentRequestData.Currency.ToString(),
            CVV = paymentRequestData.CVV,
            ExpiryDate = $"{paymentRequestData.ExpiryMonth:00}/{paymentRequestData.ExpiryYear}"
        };
    }
}
