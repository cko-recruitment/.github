using PaymentGateway.Clients.Contract;
using PaymentGateway.Clients.Extensions;
using System.Net.Http.Json;
using System.Text.Json;

namespace PaymentGateway.Clients;

public class AcquiringBankClient : IAcquiringBankClient
{
    private const string paymentsRelativeUrl = "/payments";
    private readonly HttpClient httpClient;
    private readonly JsonSerializerOptions jsonSerializerOptions;

    public AcquiringBankClient(HttpClient httpClient, JsonSerializerOptions jsonSerializerOptions)
    {
        this.jsonSerializerOptions = jsonSerializerOptions ?? throw new ArgumentNullException(nameof(jsonSerializerOptions));
        this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<PaymentResponseData> RequestPaymentAsync(PaymentRequestData paymentRequestData, CancellationToken cancellationToken)
    {
        var paymentRequestExternalModel = paymentRequestData.ToExternalModel();
        using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, paymentsRelativeUrl);
        httpRequestMessage.Content = JsonContent.Create(paymentRequestExternalModel, null, jsonSerializerOptions);

        using var response = await httpClient.SendAsync(httpRequestMessage, cancellationToken);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        var paymentResponseExternalModel = JsonSerializer.Deserialize<PaymentResponseExternalModel>(responseContent, jsonSerializerOptions);

        return paymentResponseExternalModel!.ToData(paymentRequestData);
    }
}
