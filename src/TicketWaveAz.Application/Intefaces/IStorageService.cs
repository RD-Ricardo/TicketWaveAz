namespace TicketWaveAz.Application.Intefaces
{
    public interface IStorageService
    {
        Task<string> DownloadBase64Async(string containerName, string fileName);
        Task<MemoryStream> DownloadAsync(string bucketName, string fileName);
        Task<string> UploadBase64Async(string base64content, string containerName, string fileName);
        Task<string> UploadAsync(Stream content, string bucketName, string fileName);
    }
}
