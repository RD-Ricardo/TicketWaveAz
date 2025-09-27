using TicketWaveAz.Shared.Dtos.PaymentExternal;

namespace TicketWaveAz.Domain.Interfaces.Services
{
    public interface IPaymentExternalService
    {
        Task<ResponseEfi?> GeneratePixAsync(decimal value);
    }
}
