using Microsoft.Azure.Cosmos;
using TicketWaveAz.Domain.Entities;
using TicketWaveAz.Domain.Interfaces.Repositories;

namespace TicketWaveAz.Infrastructure.Database.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly Container _container;

        public EventRepository(CosmosClient cosmosClient)
        {
            _container = cosmosClient.GetContainer("ticket-db", "Events");
        }

        public async Task CreateAsync(Event @event, CancellationToken cancellationToken)
        {
           await _container.CreateItemAsync(@event, cancellationToken: cancellationToken);
        }

        public async Task<List<Event>> GetAllAsync(CancellationToken cancellationToken)
        {
            var query = "SELECT * FROM c";

            var queryDefinition = new QueryDefinition(query);
            
            var queryResultSetIterator = _container.GetItemQueryIterator<Event>(queryDefinition);
            
            var events = new List<Event>();
            
            while (queryResultSetIterator.HasMoreResults)
            {
                var response = await queryResultSetIterator.ReadNextAsync(cancellationToken);
                events.AddRange(response.ToList());
            }
            
            return events;
        }

        public async Task<Event?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.id = @id")
                .WithParameter("@id", id);

            var iterator = _container.GetItemQueryIterator<Event>(query);

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync(cancellationToken);
                return response.FirstOrDefault();
            }

            return null;
        }

        public async Task UpdateAsync(Event @event)
        {
            await _container.ReplaceItemAsync<Event>(@event, @event.Id.ToString());
        }
    }
}
