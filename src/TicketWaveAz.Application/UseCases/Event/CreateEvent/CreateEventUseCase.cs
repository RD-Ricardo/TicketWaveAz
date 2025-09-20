using TicketWaveAz.Domain.Interfaces.Repositories;
using TicketWaveAz.Shared.Abstractions;

namespace TicketWaveAz.Application.UseCases.Event.CreateEvent
{
    public class CreateEventUseCase : ICreateEventUseCase
    {
        private readonly IEventRepository _eventRepository;
        public CreateEventUseCase(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<Result<Guid>> ExecuteAsync(EventCreateDto request, CancellationToken cancellationToken)
        {
           var @event = new Domain.Entities.Event
           {
               Name = request.Name,
               Description = request.Description,
               ImagePath = request.ImagePath,
               Location = request.Location,
               AvailableTickets = request.AvailableTickets,
               Price = request.Price,
               Date = request.Date
           };

           await _eventRepository.CreateAsync(@event, cancellationToken);
           
           return Result<Guid>.Ok(@event.Id);
        }
    }
}
