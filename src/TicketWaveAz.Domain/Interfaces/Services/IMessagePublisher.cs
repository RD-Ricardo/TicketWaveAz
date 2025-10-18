namespace TicketWaveAz.Domain.Interfaces.Services
{
    public interface IMessagePublisher
    {
        Task PublishAsync(string queue, string message, CancellationToken cancellationToken);
    }
}
