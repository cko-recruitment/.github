namespace PaymentGateway.Clients;

internal class PaymentRequestExternalModel
{
    public required string CardNumber { get; set; }

    public required string ExpiryDate { get; set; }

    public required string Currency { get; set; }

    public required int Amount { get; set; }

    public required string CVV { get; set; }
}
