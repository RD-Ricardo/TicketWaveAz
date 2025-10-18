using Newtonsoft.Json;

namespace TicketWaveAz.Shared.Dtos.PaymentExternal
{
    public class WebhookEfiDto
    {
        [JsonProperty("pix", NullValueHandling = NullValueHandling.Ignore)]
        public List<Pix> Pix { get; set; }
    }

    public class Pix
    {
        [JsonProperty("endToEndId", NullValueHandling = NullValueHandling.Ignore)]
        public string? EndToEndId { get; set; }

        [JsonProperty("txid", NullValueHandling = NullValueHandling.Ignore)]
        public string? Txid { get; set; }

        [JsonProperty("chave", NullValueHandling = NullValueHandling.Ignore)]
        public string? Chave { get; set; }

        [JsonProperty("valor", NullValueHandling = NullValueHandling.Ignore)]
        public string? Valor { get; set; }

        [JsonProperty("horario", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime Horario { get; set; }

        [JsonProperty("infoPagador", NullValueHandling = NullValueHandling.Ignore)]
        public string? InfoPagador { get; set; }
    }
}
