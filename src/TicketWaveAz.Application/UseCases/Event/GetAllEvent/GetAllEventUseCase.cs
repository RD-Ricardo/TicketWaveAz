using TicketWaveAz.Domain.Interfaces.Repositories;
using TicketWaveAz.Domain.Interfaces.Services;
using TicketWaveAz.Shared.Abstractions;
using TicketWaveAz.Shared.Dtos.Events;

namespace TicketWaveAz.Application.UseCases.Event.GetAllEvent
{
    public class GetAllEventUseCase : IGetAllEventUseCase   
    {
        private readonly IEventRepository _eventRepository;

        private readonly IStorageService _storageService;

        public GetAllEventUseCase(IEventRepository eventRepository, IStorageService storageService)
        {
            _eventRepository = eventRepository;
            _storageService = storageService;
        }

        public async Task<Result<List<EventDto>>> ExecuteAsync(CancellationToken cancellationToken)
        {
            var events = await _eventRepository.GetAllAsync(cancellationToken);

            var dtos = events.Select(e => 
            {
                var imageUrl = _storageService.GetSignedUrlAsync(e.ImagePath, TimeSpan.FromMinutes(5)).Result;

                return new EventDto(e.Id, e.Name, e.Description, e.Price, imageUrl, e.CreatedAt);

            }).ToList();

            return Result<List<EventDto>>.Ok(dtos);
        }
    }
}
