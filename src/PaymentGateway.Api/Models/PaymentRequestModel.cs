using PaymentGateway.Api.Models.Validators;
using PaymentGateway.Core;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Api.Models;

[ExpiryDateInTheFuture]
public record PaymentRequestModel
{
    [Required]
    [CreditCard]
    public required string CardNumber { get; set; }

    [Required]
    [Range(1, 12)]
    public required int ExpiryMonth { get; set; }

    [Required]
    [Range(2024, Int32.MaxValue)]
    public required int ExpiryYear { get; set; }

    [Required]
    [EnumDataType(typeof(Currency))]
    public required string Currency { get; set; }

    [Required]
    [Range(1, Int32.MaxValue)]
    public required int Amount { get; set; }

    [Required]
    [MinLength(3)]
    [MaxLength(4)]
    [RegularExpression("([0-9]+)")]
    public required string CVV { get; set; }
}
