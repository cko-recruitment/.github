using Microsoft.AspNetCore.Mvc;
using Moq;
using PaymentGateway.Api.Code.Extensions;
using PaymentGateway.Api.Controllers;
using PaymentGateway.Api.Models;
using PaymentGateway.Clients.Contract;
using PaymentGateway.Core;
using PaymentGateway.Persistance.Contract;
using System.Net;

namespace PaymentGateway.Unit.Tests;

public class PaymentsControllerTests
{
    private readonly Mock<IAcquiringBankClient> acquiringBankClientMock = new();
    private readonly Mock<IPaymentsRepository> paymentsRepositoryMock = new();

    private readonly PaymentsController controller;

    public PaymentsControllerTests()
    {
        controller = new PaymentsController(acquiringBankClientMock.Object, paymentsRepositoryMock.Object);
    }

    [Test]
    public async Task POST_Should_Return_Created_Response_When_Payment_Authorized()
    {
        //given
        var paymentRequestModel = new PaymentRequestModel()
        {
            Amount = 1,
            CardNumber = "4263982640269299",
            Currency = Currency.USD.ToString(),
            CVV = "123",
            ExpiryMonth = 1,
            ExpiryYear = 2025
        };
        var paymentResponseData = new PaymentResponseData()
        {
            Amount = paymentRequestModel.Amount,
            Currency = Currency.USD,
            ExpiryMonth = paymentRequestModel.ExpiryMonth,
            ExpiryYear = paymentRequestModel.ExpiryYear,
            Id = Guid.NewGuid(),
            LastFourCardDigits = paymentRequestModel.CardNumber[^4..],
            Status = PaymentStatusData.Authorized
        };
        acquiringBankClientMock.Setup(x => x.RequestPaymentAsync(paymentRequestModel.ToData(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(paymentResponseData);

        //when
        var response = await controller.Post(paymentRequestModel, CancellationToken.None);

        //then
        Assert.That(response, Is.Not.Null);
        Assert.That(response, Is.InstanceOf<CreatedAtActionResult>());

        paymentsRepositoryMock.Verify(x => x.SaveAsync(paymentResponseData, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task POST_Should_Return_PaymentRequired_Response_When_Payment_Declined()
    {
        //given
        var paymentRequestModel = new PaymentRequestModel()
        {
            Amount = 1,
            CardNumber = "4263982640269299",
            Currency = Currency.USD.ToString(),
            CVV = "123",
            ExpiryMonth = 1,
            ExpiryYear = 2025
        };
        var paymentResponseData = new PaymentResponseData()
        {
            Amount = paymentRequestModel.Amount,
            Currency = Currency.GBP,
            ExpiryMonth = paymentRequestModel.ExpiryMonth,
            ExpiryYear = paymentRequestModel.ExpiryYear,
            Id = Guid.NewGuid(),
            LastFourCardDigits = paymentRequestModel.CardNumber[^4..],
            Status = PaymentStatusData.Declined
        };
        acquiringBankClientMock.Setup(x => x.RequestPaymentAsync(paymentRequestModel.ToData(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(paymentResponseData);

        //when
        var response = await controller.Post(paymentRequestModel, CancellationToken.None);

        //then
        Assert.That(response, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(response, Is.InstanceOf<ObjectResult>());
            Assert.That(((ObjectResult)response).StatusCode, Is.EqualTo((int)HttpStatusCode.PaymentRequired));
        });

        paymentsRepositoryMock.Verify(x => x.SaveAsync(paymentResponseData, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task GET_Should_Return_NotFound_When_Payment_Not_Found()
    {
        //given
        var paymentId = Guid.NewGuid();

        //when
        var response = await controller.Get(paymentId, CancellationToken.None);

        //then
        Assert.That(response, Is.Not.Null);
        Assert.That(response, Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    public async Task GET_Should_Return_OK_When_Payment_Found()
    {
        //given
        var paymentId = Guid.NewGuid();
        var paymentResponseData = new PaymentResponseData()
        {
            Amount = 5,
            Currency = Currency.GBP,
            ExpiryMonth = 4,
            ExpiryYear = 2025,
            Id = paymentId,
            LastFourCardDigits = "1234",
            Status = PaymentStatusData.Declined
        };

        paymentsRepositoryMock.Setup(x => x.GetAsync(paymentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(paymentResponseData);

        //when
        var response = await controller.Get(paymentId, CancellationToken.None);

        //then
        Assert.That(response, Is.Not.Null);
        Assert.That(response, Is.InstanceOf<OkObjectResult>());
        Assert.That(((OkObjectResult)response).Value, Is.EqualTo(paymentResponseData.ToModel()));
    }
}