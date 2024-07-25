using PaymentGateway.Clients.Contract;

namespace PaymentGateway.Persistance.Contract;

public interface IPaymentsRepository
{
    Task SaveAsync(PaymentResponseData paymentResponseData, CancellationToken cancellationToken);
    Task<PaymentResponseData?> GetAsync(Guid paymentId, CancellationToken cancellationToken);
}
