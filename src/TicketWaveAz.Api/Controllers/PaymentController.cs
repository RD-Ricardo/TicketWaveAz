using Microsoft.AspNetCore.Mvc;
using TicketWaveAz.Application.UseCases.Payment.CreatePayment;
using TicketWaveAz.Application.UseCases.Payment.GetPayment;

namespace TicketWaveAz.Api.Controllers
{
    [ApiController]
    [Route("api/payments")]
    public class PaymentController : ControllerBase
    {
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
    }
}
