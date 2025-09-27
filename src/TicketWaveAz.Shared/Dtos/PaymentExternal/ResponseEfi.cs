using Newtonsoft.Json;

namespace TicketWaveAz.Shared.Dtos.PaymentExternal
{
    public class ResponseEfi
    {
        [JsonProperty("txid")]
        public string? Txid { get; set; }

        [JsonProperty("pixCopiaECola")]
        public string? PixCopyAndPaste { get; set; }
    }
}
