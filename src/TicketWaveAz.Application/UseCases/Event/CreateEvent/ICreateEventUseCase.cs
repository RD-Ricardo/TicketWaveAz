using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketWaveAz.Shared.Abstractions;
using TicketWaveAz.Shared.Dtos.Customer;
using TicketWaveAz.Shared.Dtos.Events;

namespace TicketWaveAz.Application.UseCases.Event.CreateEvent
{

    public interface ICreateEventUseCase : IUseCase
    {
        Task<Result<Guid>> ExecuteAsync(EventCreateDto request, CancellationToken cancellationToken);
    }
}
