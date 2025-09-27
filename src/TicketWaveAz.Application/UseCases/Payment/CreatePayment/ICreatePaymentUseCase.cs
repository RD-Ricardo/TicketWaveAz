using TicketWaveAz.Domain.Interfaces.Repositories;
using TicketWaveAz.Domain.Interfaces.Services;
using TicketWaveAz.Shared.Abstractions;
using TicketWaveAz.Shared.Abstractions.Errors;
using TicketWaveAz.Shared.Enums;

namespace TicketWaveAz.Application.UseCases.Payment.CreatePayment
{

    public class CreatePaymentDto
    {
        public Guid EventId { get; set; }
        public int QuantityTicket { get; set; }
        public string? CustomerEmail { get; set; } 
    }

    public record PaymentDto(
      Guid Id,
      string PixCopyAndPaste,
      PaymentStatusType Status,
      DateTime CreatedAt);

    public interface ICreatePaymentUseCase : IUseCase
    {
        Task<Result<PaymentDto>> ExecuteAsync(CreatePaymentDto input, CancellationToken cancellationToken);
    }

    public class CreatePaymentUseCase : ICreatePaymentUseCase
    {
        private readonly IEventRepository _eventRepository;

        private readonly IPaymentRepository _paymentRepository;

        private readonly IPaymentExternalService _paymentExternalService;
        public CreatePaymentUseCase(IEventRepository eventRepository, IPaymentRepository paymentRepository, IPaymentExternalService paymentExternalService)
        {
            _eventRepository = eventRepository;
            _paymentRepository = paymentRepository;
            _paymentExternalService = paymentExternalService;
        }

        public async Task<Result<PaymentDto>> ExecuteAsync(CreatePaymentDto input, CancellationToken cancellationToken)
        {
            var @event = await _eventRepository.GetByIdAsync(input.EventId, CancellationToken.None);

            if (@event is null)
            {
                return Result<PaymentDto>.Fail(new NotFoundError("Event not found"));
            }

            var paymentExternal = await _paymentExternalService.GeneratePixAsync(@event.Price * input.QuantityTicket);

            if (paymentExternal is null || string.IsNullOrEmpty(paymentExternal.Txid) || string.IsNullOrEmpty(paymentExternal.PixCopyAndPaste))
            {
                return Result<PaymentDto>.Fail(new InvalidError("Erro ao gerar pagamento"));
            }

            var payment = new Domain.Entities.Payment
            {
                EventId = input.EventId,
                Status = PaymentStatusType.Pending,
                PixCopyAndPaste = paymentExternal.PixCopyAndPaste,
                ExternalId = paymentExternal.Txid
            };

            await _paymentRepository.CreateAsync(payment, cancellationToken);

            return Result<PaymentDto>.Ok(new PaymentDto(
                payment.Id,
                payment.PixCopyAndPaste,
                payment.Status,
                payment.CreatedAt));
        }
    }
}
