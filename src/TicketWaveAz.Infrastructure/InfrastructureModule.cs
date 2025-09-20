using TicketWaveAz.Shared.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TicketWaveAz.Infrastructure.Settings;
using TicketWaveAz.Application.Intefaces;
using TicketWaveAz.Infrastructure.Services;
using Microsoft.Azure.Cosmos;

namespace TicketWaveAz.Infrastructure
{
    public static class InfrastructureModule
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddSingleton(x => new CosmosClient(connectionString: configuration["CosmosDbConnection"]));


            services.AddStorageService(configuration);

            //services.Configure<JwtSettings>(options => configuration.GetSection("JwtSettings").Bind(options));

            //services.AddScoped<IJwtService, JwtService>();

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
            //var storageSection = configuration.GetRequiredSection("StorageSettings")!;

            //services.Configure<StorageSettings>(options => storageSection.Bind(options));

            //var storageSettings = storageSection.Get<StorageSettings>()!;

            //services.AddScoped<IStorageService>();
        }
    }
}
