using System.Globalization;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using TicketWaveAz.Domain.Interfaces.Services;
using TicketWaveAz.Shared.Dtos.PaymentExternal;

namespace TicketWaveAz.Infrastructure.Services
{
    public class PaymentExternalService : IPaymentExternalService
    {
        private readonly IConfiguration _configuration;

        private readonly IMemoryCache _memoryCache;

        private readonly IStorageService _storageService;

        private X509Certificate2? certificate21;

        private const string CertificateCacheKey = "CertificateKey";
        private string ClientId => _configuration["EfiPayment:client_id"]!;
        private string ClientSecret => _configuration["EfiPayment:client_secret"]!;

        public PaymentExternalService(IStorageService storageService, IMemoryCache memoryCache, IConfiguration configuration)
        {
            _storageService = storageService;
            _memoryCache = memoryCache;
            _configuration = configuration;
        }

        public async Task<ResponseEfi?> GeneratePixAsync(decimal value)
        {
            var response = await Authenticate();

            var content = await response.Content.ReadAsStringAsync();

            var accessToken = JObject.Parse(content)["access_token"]!.ToString();

            value = 0.02m;

            var valueString = value.ToString("0.00", CultureInfo.InvariantCulture);

            var body = new
            {
                calendario = new
                {
                    expiracao = 900
                },
                valor = new
                {
                    original = valueString
                },
                chave = "b9de40b7-dddd-4bb8-bb05-d52b607045c3",
                solicitacaoPagador = "Cobrança dos serviços prestados."
            };

            HttpClientHandler clientHandler = new();

            if (certificate21 is null)
                throw new Exception("Certificado não carregado");

            clientHandler.ClientCertificateOptions = ClientCertificateOption.Manual;

            clientHandler.ClientCertificates.Add(GetOrCreateCertificate()!);

            var httpClient = new HttpClient(clientHandler);

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api-pix.gerencianet.com.br/v2/cob");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            request.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

            var result = await httpClient.SendAsync(request);

            if (result.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ResponseEfi>(await result.Content.ReadAsStringAsync())!;
            }

            return null;
        }

        private async Task<HttpResponseMessage> Authenticate()
        {
            string credentials = $"{ClientId}:{ClientSecret}";
            string base64Auth = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials));

            certificate21 = GetOrCreateCertificate();

            var handler = new HttpClientHandler();
            handler.ClientCertificates.Add(certificate21!);
            handler.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, errors) => true;


            using var httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://pix.api.efipay.com.br")
            };

            using var request = new HttpRequestMessage(HttpMethod.Post, "/oauth/token");
            request.Headers.Add("Authorization", $"Basic {base64Auth}");
            request.Content = new StringContent(
                "{ \"grant_type\": \"client_credentials\" }",
                Encoding.UTF8,
                "application/json"
            );

            var response = await httpClient.SendAsync(request);
            return response;
        }

        private X509Certificate2? GetOrCreateCertificate()
        {
            if (_memoryCache.TryGetValue(CertificateCacheKey, out X509Certificate2? certificate))
            {
                return certificate;
            }

            certificate = GenerateCertificate();

            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30),
                SlidingExpiration = TimeSpan.FromMinutes(10)
            };

            _memoryCache.Set(CertificateCacheKey, certificate, cacheEntryOptions);

            return certificate;
        }

        private X509Certificate2? GenerateCertificate()
        {
            var presignedUri = _storageService.GetSignedUrlAsync("producao-654259-CEARA_PROD.p12", TimeSpan.FromMinutes(2)).Result;

            certificate21 = LoadCertificateFromBlobStorageAsync(presignedUri).GetAwaiter().GetResult();

            return certificate21;
        }

        public async Task<X509Certificate2> LoadCertificateFromBlobStorageAsync(string presignedUrl)
        {
            using HttpClient httpClient = new HttpClient();

            byte[] certData = await httpClient.GetByteArrayAsync(presignedUrl);

            return new X509Certificate2(certData, "",
                X509KeyStorageFlags.MachineKeySet |
                X509KeyStorageFlags.Exportable |
                X509KeyStorageFlags.PersistKeySet);
        }

    }
}
