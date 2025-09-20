using TicketWaveAz.Application.UseCases.Event.CreateEvent;
using TicketWaveAz.Domain.Interfaces.Repositories;
using TicketWaveAz.Shared.Abstractions;

namespace TicketWaveAz.Application.UseCases.Event.GetAllEvent
{

    public record EventDto(Guid Id, string Name, DateTime CratedAt);


    public interface IGetAllEventUseCase : IUseCase
    {
        Task<Result<List<EventDto>>> ExecuteAsync(CancellationToken cancellationToken);
    }


    public class GetAllEventUseCase : IGetAllEventUseCase   
    {
        private readonly IEventRepository _eventRepository;

        public GetAllEventUseCase(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<Result<List<EventDto>>> ExecuteAsync(CancellationToken cancellationToken)
        {
            var events = await _eventRepository.GetAllAsync(cancellationToken);

            var dtos = events.Select(e => new EventDto(e.Id, e.Name, e.CreatedAt)).ToList();

            return Result<List<EventDto>>.Ok(dtos);
        }
    }
}
