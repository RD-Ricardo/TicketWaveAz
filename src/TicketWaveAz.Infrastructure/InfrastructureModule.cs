using TicketWaveAz.Shared.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TicketWaveAz.Infrastructure.Settings;
using Microsoft.Azure.Cosmos;
using TicketWaveAz.Domain.Interfaces.Services;
using TicketWaveAz.Infrastructure.Services;

namespace TicketWaveAz.Infrastructure
{
    public static class InfrastructureModule
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(x => new CosmosClient(connectionString: configuration["CosmosDbConnection"]));

            services.AddMemoryCache();

            services.AddScoped<IPaymentExternalService, PaymentExternalService>();

            services.AddStorageService(configuration);

            services.Scan(scan => scan
                .FromAssemblies(typeof(InfrastructureModule).Assembly)
                .AddClasses(classes => classes.AssignableTo<IRepository>())
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            );

            return services;
        }

        private static void AddStorageService(this IServiceCollection services, IConfiguration configuration)
        {
            var storageSection = configuration.GetRequiredSection("StorageSettings")!;

            services.Configure<StorageSettings>(options => storageSection.Bind(options));

            var storageSettings = storageSection.Get<StorageSettings>()!;

            var blobClient = new Azure.Storage.Blobs.BlobServiceClient(storageSettings.ConnectionString);

            services.AddScoped<IStorageService, StorageService>();

            services.AddSingleton(blobClient);
        }
    }
}
