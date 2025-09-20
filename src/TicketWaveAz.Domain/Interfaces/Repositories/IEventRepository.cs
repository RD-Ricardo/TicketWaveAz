using TicketWaveAz.Domain.Entities;
using TicketWaveAz.Shared.Abstractions;

namespace TicketWaveAz.Domain.Interfaces.Repositories
{
    public interface IEventRepository : IRepository
    {
        Task<Event?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<List<Event>> GetAllAsync(CancellationToken cancellationToken);
        Task CreateAsync(Event @event, CancellationToken cancellationToken);
        Task UpdateAsync(Event @event);
    }
}
