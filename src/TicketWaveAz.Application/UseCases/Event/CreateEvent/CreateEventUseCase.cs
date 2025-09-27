using TicketWaveAz.Domain.Interfaces.Repositories;
using TicketWaveAz.Domain.Interfaces.Services;
using TicketWaveAz.Shared.Abstractions;
using TicketWaveAz.Shared.Dtos.Events;

namespace TicketWaveAz.Application.UseCases.Event.CreateEvent
{
    public class CreateEventUseCase : ICreateEventUseCase
    {
        private readonly IEventRepository _eventRepository;

        private readonly IStorageService _storageService;
        public CreateEventUseCase(IEventRepository eventRepository, IStorageService storageService)
        {
            _eventRepository = eventRepository;
            _storageService = storageService;
        }

        public async Task<Result<Guid>> ExecuteAsync(EventCreateDto request, CancellationToken cancellationToken)
        {
            var filePath = await _storageService.UploadAsync(request.File.FileName, request.File.Base64, cancellationToken);   

           var @event = new Domain.Entities.Event
           {
               Name = request.Name,
               Description = request.Description,
               ImagePath = filePath,
               Location = request.Location,
               AvailableTickets = request.AvailableTickets,
               Price = request.Price,
               Date = request.Date,
           };

           await _eventRepository.CreateAsync(@event, cancellationToken);
           
           return Result<Guid>.Ok(@event.Id);
        }
    }
}
