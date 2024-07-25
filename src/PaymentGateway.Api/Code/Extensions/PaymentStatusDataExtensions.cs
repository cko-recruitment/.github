using PaymentGateway.Api.Models;
using PaymentGateway.Clients.Contract;

namespace PaymentGateway.Api.Code.Extensions;

public static class PaymentStatusDataExtensions
{
    public static PaymentStatusModel ToModel(this PaymentStatusData paymentStatusData)
    {
        return paymentStatusData switch
        {
            PaymentStatusData.Authorized => PaymentStatusModel.Authorized,
            PaymentStatusData.Declined => PaymentStatusModel.Declined,
            _ => throw new NotSupportedException(),
        };
    }
}
