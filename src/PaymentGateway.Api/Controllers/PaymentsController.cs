using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Api.Code.Extensions;
using PaymentGateway.Api.Models;
using PaymentGateway.Clients.Contract;
using PaymentGateway.Persistance.Contract;
using System.Net;

namespace PaymentGateway.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController(
    IAcquiringBankClient acquiringBankClient,
    IPaymentsRepository paymentsRepository) : ControllerBase
{
    private readonly IAcquiringBankClient acquiringBankClient = acquiringBankClient ?? throw new ArgumentNullException(nameof(acquiringBankClient));
    private readonly IPaymentsRepository paymentsRepository = paymentsRepository ?? throw new ArgumentNullException(nameof(paymentsRepository));

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        var paymentResponseData = await paymentsRepository.GetAsync(id, cancellationToken);
        if(paymentResponseData == null)
            return NotFound();

        return Ok(paymentResponseData.ToModel());
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] PaymentRequestModel paymentRequestModel, CancellationToken cancellationToken)
    {
        var paymentResponseData = await acquiringBankClient.RequestPaymentAsync(paymentRequestModel.ToData(), cancellationToken);
        await paymentsRepository.SaveAsync(paymentResponseData, cancellationToken);
        
        var paymentModel = paymentResponseData.ToModel();

        return paymentModel.Status == PaymentStatusModel.Authorized
            ? CreatedAtAction(nameof(Get), new { paymentModel.Id }, paymentModel)
            : new ObjectResult(paymentModel)
            {
                StatusCode = (int)HttpStatusCode.PaymentRequired,
            };
    }
}
