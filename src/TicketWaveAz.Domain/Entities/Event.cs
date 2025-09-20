using TicketWaveAz.Shared.Abstractions;

namespace TicketWaveAz.Domain.Entities
{
    public class Event : BaseEntity
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int AvailableTickets { get; set; }
        public string ImagePath { get; set; } = null!;
    }
}
