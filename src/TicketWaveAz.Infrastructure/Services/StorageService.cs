using Azure.Storage.Blobs;
using TicketWaveAz.Domain.Interfaces.Services;

namespace TicketWaveAz.Infrastructure.Services
{
    public class StorageService : IStorageService
    {
        private BlobServiceClient _blobServiceClient;

        private BlobContainerClient _blobContainerClient;
        public StorageService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
            _blobContainerClient = blobServiceClient.GetBlobContainerClient("ticket-wave-storage");
        }


        public async Task<string> GetSignedUrlAsync(string fileName, TimeSpan expirationName)
        {
            var blobClient = _blobContainerClient.GetBlobClient(fileName);

            if (await blobClient.ExistsAsync())
            {
                var sasUri = blobClient.GenerateSasUri(Azure.Storage.Sas.BlobSasPermissions.Read, DateTimeOffset.UtcNow.Add(expirationName));
                return sasUri.AbsoluteUri;
            }

            return string.Empty;
        }

        public async Task<string> UploadAsync(string fileName, string base64, CancellationToken cancellationToken = default)
        {
            var blobClient = _blobContainerClient.GetBlobClient(fileName);
            
            var bytes = Convert.FromBase64String(base64);
            
            using (var stream = new MemoryStream(bytes))
            {
                await blobClient.UploadAsync(stream, true, cancellationToken);
            }
            
            return fileName;
        }

        public async Task<string> UploadBytesAsync(string fileName, byte[] fileBytes, CancellationToken cancellationToken = default)
        {
            var blobClient = _blobContainerClient.GetBlobClient(fileName);

            using (var stream = new MemoryStream(fileBytes))
            {
                await blobClient.UploadAsync(stream, true, cancellationToken);
            }

            return fileName;
        }
    }
}
