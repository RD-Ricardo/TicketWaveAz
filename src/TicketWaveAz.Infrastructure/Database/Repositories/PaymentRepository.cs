using Microsoft.Azure.Cosmos;
using TicketWaveAz.Domain.Entities;
using TicketWaveAz.Domain.Interfaces.Repositories;

namespace TicketWaveAz.Infrastructure.Database.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly Container _container;

        public PaymentRepository(CosmosClient cosmosClient)
        {
            _container = cosmosClient.GetContainer("ticket-db", "Payments");
        }

        public async Task CreateAsync(Payment payment, CancellationToken cancellationToken)
        {
            await _container.CreateItemAsync(payment, cancellationToken: cancellationToken);
        }

        public async Task<Payment?> GetByExternalIdAsync(string externalId, CancellationToken cancellationToken)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.ExternalId = @externalId")
               .WithParameter("@externalId", externalId);

            var iterator = _container.GetItemQueryIterator<Payment>(query);

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync(cancellationToken);
                return response.FirstOrDefault();
            }

            return null;
        }

        public async Task<Payment?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.id = @id")
                .WithParameter("@id", id);

            var iterator = _container.GetItemQueryIterator<Payment>(query);

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync(cancellationToken);
                return response.FirstOrDefault();
            }

            return null;
        }

        public async Task UpdateAsync(Payment payment, CancellationToken cancellationToken)
        {
            await _container.ReplaceItemAsync(payment, payment.Id.ToString(), cancellationToken: cancellationToken);
        }
    }
}
