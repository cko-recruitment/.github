namespace PaymentGateway.Clients.Contract;

public record PaymentResponseExternalModel
{
    public required bool Authorized { get; set; }
    public string? AuthorizationCode { get; set; }
}
