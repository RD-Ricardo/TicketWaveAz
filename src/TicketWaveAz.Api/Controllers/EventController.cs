using Microsoft.AspNetCore.Mvc;
using TicketWaveAz.Application.UseCases.Event.CreateEvent;
using TicketWaveAz.Application.UseCases.Event.GetAllEvent;

namespace TicketWaveAz.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {


        [HttpPost("create")]
        public async Task<IActionResult> CreateEvent([FromBody] EventCreateDto request, [FromServices] ICreateEventUseCase createEventUseCase, CancellationToken cancellationToken)
        {
            var result = await createEventUseCase.ExecuteAsync(request, cancellationToken);


            return result.Match(
                e => Ok(e),
                err => BadRequest(err)
              );
        }


        [HttpGet]
        public async Task<IActionResult> CreateEvent([FromServices] IGetAllEventUseCase getAllEventUseCase, CancellationToken cancellationToken)
        {
            var result = await getAllEventUseCase.ExecuteAsync(cancellationToken);

            if (result.Success)
            {
                return OK(result.Error);
            }
            return Ok(result.Value);
        }
    }
}
