namespace PaymentGateway.Clients.Contract;

public interface IAcquiringBankClient
{
    Task<PaymentResponseData> RequestPaymentAsync(PaymentRequestData paymentRequestData, CancellationToken cancellationToken);
}
