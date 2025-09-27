using TicketWaveAz.Application.UseCases.Event.CreateEvent;
using TicketWaveAz.Shared.Abstractions;
using TicketWaveAz.Shared.Dtos.Events;

namespace TicketWaveAz.Application.UseCases.Event.GetAllEvent
{
    public interface IGetAllEventUseCase : IUseCase
    {
        Task<Result<List<EventDto>>> ExecuteAsync(CancellationToken cancellationToken);
    }
}
