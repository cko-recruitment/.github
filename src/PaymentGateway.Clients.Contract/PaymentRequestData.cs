using PaymentGateway.Core;

namespace PaymentGateway.Clients.Contract;

public record PaymentRequestData
{
    public required string CardNumber { get; set; }

    public required int ExpiryMonth { get; set; }

    public required int ExpiryYear { get; set; }

    public required Currency Currency { get; set; }

    public required int Amount { get; set; }

    public required string CVV { get; set; }
}
