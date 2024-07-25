using PaymentGateway.Core;

namespace PaymentGateway.Api.Models;

public record PaymentModel
{
    public required Guid Id { get; set; }
    public required PaymentStatusModel Status { get; set; }
    public required string LastFourCardDigits { get; set; }
    public required int ExpiryMonth { get; set; }
    public required int ExpiryYear { get; set; }
    public required Currency Currency { get; set; }
    public required int Amount { get; set; }
}
