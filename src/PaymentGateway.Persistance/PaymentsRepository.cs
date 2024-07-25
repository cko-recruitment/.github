using PaymentGateway.Clients.Contract;
using PaymentGateway.Persistance.Contract;

namespace PaymentGateway.Persistance;

public class PaymentsRepository : IPaymentsRepository
{
    public Task<PaymentResponseData> GetAsync(Guid paymentId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SaveAsync(PaymentResponseData paymentResponseData, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
