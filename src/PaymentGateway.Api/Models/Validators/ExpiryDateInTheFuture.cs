using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Api.Models.Validators;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ExpiryDateInTheFuture : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var paymentRequest = value as PaymentRequestModel;

        if(paymentRequest == null)
            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));

        var timeProvider = validationContext.GetRequiredService<TimeProvider>();
        var currentUtcDateTime = timeProvider.GetUtcNow();
        var expiryDate = new DateTime(paymentRequest.ExpiryYear, paymentRequest.ExpiryMonth + 1, 1).AddDays(-1);

        if (expiryDate < currentUtcDateTime)
            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));

        return ValidationResult.Success;
    }
}
