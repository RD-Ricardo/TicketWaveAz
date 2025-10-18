using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using TicketWaveAz.Domain.Interfaces.Services;

namespace TicketWaveAz.Infrastructure.Services
{
    public class MessagePublisher : IMessagePublisher
    {
        private readonly ServiceBusClient _serviceBusClient;

        public MessagePublisher(IConfiguration configuration)
        {
            _serviceBusClient = new ServiceBusClient(configuration["ServiceBusConnection"]);
        }

        public async Task PublishAsync(string queue, string message, CancellationToken cancellationToken)
        {
            ServiceBusSender sender = _serviceBusClient.CreateSender(queue);
            ServiceBusMessage busMessage = new ServiceBusMessage(body: message);
            await sender.SendMessageAsync(busMessage, cancellationToken);
        }
    }
}
