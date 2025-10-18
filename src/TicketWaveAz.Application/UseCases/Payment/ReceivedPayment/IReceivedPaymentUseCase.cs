using TicketWaveAz.Domain.Interfaces.Repositories;
using TicketWaveAz.Domain.Interfaces.Services;
using TicketWaveAz.Shared.Abstractions;
using TicketWaveAz.Shared.Abstractions.Errors;
using TicketWaveAz.Shared.Dtos.PaymentExternal;

namespace TicketWaveAz.Application.UseCases.Payment.ReceivedPayment
{
    public interface IReceivedPaymentUseCase : IUseCase
    {
        Task<Result<bool>> ExecuteAsync(WebhookEfiDto request, CancellationToken cancellationToken);
    }

    public record PaymendPaidEvent(Guid PaymentId);

    public class ReceivedPaymentUseCase : IReceivedPaymentUseCase
    {
        private readonly IPaymentRepository _paymentRepository;

        private readonly IMessagePublisher _messagePublisher;
        public ReceivedPaymentUseCase(IPaymentRepository paymentRepository, IMessagePublisher messagePublisher)
        {
            _paymentRepository = paymentRepository;
            _messagePublisher = messagePublisher;
        }

        public async Task<Result<bool>> ExecuteAsync(WebhookEfiDto request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Pix == null || !request.Pix.Any())
                {
                    return Result<bool>.Fail(new NotFoundError("No Pix payments found in the request."));
                }


                foreach (var pix in request.Pix)
                {
                    if (string.IsNullOrEmpty(pix.Txid))
                    {
                        return Result<bool>.Fail(new NotFoundError("Txid is required for processing payment."));
                    }

                    var payment = await _paymentRepository.GetByExternalIdAsync(pix.Txid, cancellationToken);
                    
                    if (payment == null)
                    {
                        return Result<bool>.Fail(new NotFoundError($"Payment with Txid {pix.Txid} not found."));
                    }

                    payment.Status = Shared.Enums.PaymentStatusType.Paid;

                    await _paymentRepository.UpdateAsync(payment, cancellationToken);

                    var json = System.Text.Json.JsonSerializer.Serialize(new PaymendPaidEvent(payment.Id));

                    await _messagePublisher.PublishAsync("payment-confirmed", json, cancellationToken);
                }
                return Result<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail(new NotFoundError($"An error occurred while processing the payment: {ex.Message}"));
            }
        }
    }
}
