using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketWaveAz.Shared.Abstractions;
using TicketWaveAz.Shared.Dtos.Customer;

namespace TicketWaveAz.Application.UseCases.Event.CreateEvent
{
    public class EventCreateDto
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int AvailableTickets { get; set; }
        public string? ImagePath { get; set; }
    }

    public interface ICreateEventUseCase : IUseCase
    {
        Task<Result<Guid>> ExecuteAsync(EventCreateDto request, CancellationToken cancellationToken);
    }
}
