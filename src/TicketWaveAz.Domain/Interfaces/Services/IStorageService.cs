namespace TicketWaveAz.Domain.Interfaces.Services
{
    public interface IStorageService 
    {
        Task<string> UploadAsync(string fileName,  string base64, CancellationToken cancellationToken = default);
        Task<string> GetSignedUrlAsync(string fileName, TimeSpan expirationName);
    }
}
