using Microsoft.AspNetCore;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using TicketWaveAz.Application.UseCases.Payment.CreatePayment;
using TicketWaveAz.Application.UseCases.Payment.GetPayment;
using TicketWaveAz.Application.UseCases.Payment.ReceivedPayment;
using TicketWaveAz.Shared.Dtos.PaymentExternal;

namespace TicketWaveAz.Api.Controllers
{
    [ApiController]
    [Route("api/payments")]
    public class PaymentController : ControllerBase
    {
        private readonly string _secretKey = "ticketWave/pix";

        private readonly ILogger<PaymentController> _logger;

        public PaymentController(ILogger<PaymentController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayment(
            [FromBody] CreatePaymentDto request,
            [FromServices] ICreatePaymentUseCase createPaymentUseCase, CancellationToken cancellationToken)
        {
            var result = await createPaymentUseCase.ExecuteAsync(request, cancellationToken);

            if (result.Success)
            {
                return Ok(result.Value);
            }

            return BadRequest(result.Errors);
        }


        [HttpGet("{paymentId:guid}")]
        public async Task<IActionResult> GetPayment(
           [FromRoute] Guid paymentId,
           [FromServices] IGetPaymentUseCase getPaymentUseCase, CancellationToken cancellationToken)
        {
            var result = await getPaymentUseCase.ExecuteAsync(paymentId, cancellationToken);

            if (result.Success)
            {
                return Ok(result.Value);
            }

            return BadRequest(result.Errors);
        }


        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook(
           [FromQuery] string hmac,
           [FromBody] WebhookEfiDto request,
           [FromServices] IReceivedPaymentUseCase receivedPaymentUseCase, CancellationToken cancellationToken)
        {
            if (hmac == _secretKey)
            {
                var result = await receivedPaymentUseCase.ExecuteAsync(request, cancellationToken);

                var requestBody = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

                _logger.LogInformation("Webhook processado com sucesso: {requestBody}", requestBody);

                return Created("Webhook processado com sucesso.", requestBody);
            }

            return Ok();
        }
    }
}
