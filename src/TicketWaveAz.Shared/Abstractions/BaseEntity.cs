using Newtonsoft.Json;

namespace TicketWaveAz.Shared.Abstractions
{
    public abstract class BaseEntity
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        protected BaseEntity()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.Now;
            UpdatedAt = null;
        }
    }
}
