using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketWaveAz.Shared.Abstractions;

namespace TicketWaveAz.Domain.Interfaces.Repositories
{
    public interface IPaymentRepository : IRepository
    {
        Task CreateAsync(Entities.Payment payment, CancellationToken cancellationToken);
        Task<Entities.Payment?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
