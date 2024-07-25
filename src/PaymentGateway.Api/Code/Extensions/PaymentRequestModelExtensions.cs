using PaymentGateway.Api.Models;
using PaymentGateway.Clients.Contract;
using PaymentGateway.Core;

namespace PaymentGateway.Api.Code.Extensions;

public static class PaymentRequestModelExtensions
{
    public static PaymentRequestData ToData(this PaymentRequestModel model)
    {
        return new PaymentRequestData()
        {
            Amount = model.Amount,
            CardNumber = model.CardNumber,
            Currency = Enum.Parse<Currency>(model.Currency),
            CVV = model.CVV,
            ExpiryMonth = model.ExpiryMonth,
            ExpiryYear = model.ExpiryYear
        };
    }
}
