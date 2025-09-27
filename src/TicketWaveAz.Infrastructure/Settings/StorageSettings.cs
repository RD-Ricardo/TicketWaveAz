namespace TicketWaveAz.Infrastructure.Settings
{
    public class StorageSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string SecretSas { get; set; } = null!;
    }
}
