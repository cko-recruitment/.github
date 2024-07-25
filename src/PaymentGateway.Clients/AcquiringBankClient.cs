using PaymentGateway.Clients.Contract;

namespace PaymentGateway.Clients;

public class AcquiringBankClient : IAcquiringBankClient
{
    public Task<PaymentResponseData> RequestPaymentAsync(PaymentRequestData paymentRequestData, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
