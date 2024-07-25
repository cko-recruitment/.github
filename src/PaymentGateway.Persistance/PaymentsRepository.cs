using PaymentGateway.Clients.Contract;
using PaymentGateway.Persistance.Contract;

namespace PaymentGateway.Persistance;

public class PaymentsRepository : IPaymentsRepository
{
    private readonly Dictionary<Guid, PaymentResponseData> paymentResponseDataItems = [];

    public PaymentsRepository()
    {
    }

    public PaymentsRepository(Dictionary<Guid, PaymentResponseData> paymentResponseDataItems)
    {
        this.paymentResponseDataItems = paymentResponseDataItems;
    }

    public async Task<PaymentResponseData?> GetAsync(Guid paymentId, CancellationToken cancellationToken)
    {
        return await Task.FromResult(paymentResponseDataItems
            .Where(x => x.Key == paymentId)
            .Select(x => x.Value)
            .FirstOrDefault());
    }

    public async Task<Guid> SaveAsync(PaymentResponseData paymentResponseData, CancellationToken cancellationToken)
    {
        var paymentId = Guid.NewGuid();
        paymentResponseDataItems.Add(paymentId, paymentResponseData);
        return await Task.FromResult(paymentId);
    }
}
