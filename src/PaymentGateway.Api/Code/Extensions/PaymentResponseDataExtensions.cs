using PaymentGateway.Api.Models;
using PaymentGateway.Clients.Contract;

namespace PaymentGateway.Api.Code.Extensions;

public static class PaymentResponseDataExtensions
{
    public static PaymentModel ToModel(this PaymentResponseData paymentResponseData)
    {
        return new PaymentModel()
        {
            Amount = paymentResponseData.Amount,
            Currency = paymentResponseData.Currency,
            ExpiryMonth = paymentResponseData.ExpiryMonth,
            ExpiryYear = paymentResponseData.ExpiryYear,
            LastFourCardDigits = paymentResponseData.LastFourCardDigits,
            Status = paymentResponseData.Status.ToModel()
        };
    }
}
