namespace TicketWaveAz.Infrastructure.Settings
{
    public class StorageSettings
    {
        public string Endpoint { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string AccessKey { get; set; } = string.Empty;
        public string AcessSecret { get; set; } = string.Empty;
        public bool UseSSL { get; set; }
    }
}
