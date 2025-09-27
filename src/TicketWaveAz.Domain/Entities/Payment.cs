using TicketWaveAz.Shared.Abstractions;
using TicketWaveAz.Shared.Enums;

namespace TicketWaveAz.Domain.Entities
{
    public class Payment : BaseEntity
    {
        public string ExternalId { get; set; }
        public Guid EventId { get; set; }
        public PaymentStatusType Status { get; set; }
        public string PixCopyAndPaste { get; set; }
    }
}
