using System.Net;

namespace PaymentGateway.Acceptance.Tests;

public partial class PaymentsControllerTests
{
    [Test]
    public async Task GET_Should_Return_NotFound_When_PaymentId_Does_NOT_Exist()
    {
        //given
        var paymentId = Guid.NewGuid();
        var url = $"{relativeUrl}/{paymentId}";
        using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);

        //when
        using var response = await httpClient.SendAsync(httpRequestMessage);
        var v = await response.Content.ReadAsStringAsync();

        //then
        Assert.IsNotNull(response);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
}