using Microsoft.AspNetCore.Mvc;
using TicketWaveAz.Application.UseCases.Event.CreateEvent;
using TicketWaveAz.Application.UseCases.Event.GetAllEvent;
using TicketWaveAz.Shared.Dtos.Events;

namespace TicketWaveAz.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] EventCreateDto request, [FromServices] ICreateEventUseCase createEventUseCase, CancellationToken cancellationToken)
        {
            var result = await createEventUseCase.ExecuteAsync(request, cancellationToken);

            if (result.Success)
            {
                return Ok(result.Value);
            }

            return BadRequest(result.Errors);
        }


        [HttpGet]
        public async Task<IActionResult> List([FromServices] IGetAllEventUseCase getAllEventUseCase, CancellationToken cancellationToken)
        {
            var result = await getAllEventUseCase.ExecuteAsync(cancellationToken);

            if (result.Success)
            {
                return Ok(result.Value);
            }

            return BadRequest(result.Errors);
        }
    }
}
