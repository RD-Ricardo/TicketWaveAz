using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketWaveAz.Application.UseCases.Payment.CreatePayment;
using TicketWaveAz.Domain.Interfaces.Repositories;
using TicketWaveAz.Shared.Abstractions;
using TicketWaveAz.Shared.Abstractions.Errors;

namespace TicketWaveAz.Application.UseCases.Payment.GetPayment
{
    public interface IGetPaymentUseCase : IUseCase
    {
        Task<Result<PaymentDto>> ExecuteAsync(Guid paymentId, CancellationToken cancellationToken);
    }

    public class GetPaymentUseCase : IGetPaymentUseCase
    {
        private readonly IPaymentRepository _paymentRepository;
        public GetPaymentUseCase(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }
        public async Task<Result<PaymentDto>> ExecuteAsync(Guid paymentId, CancellationToken cancellationToken)
        {
            var payment = await _paymentRepository.GetByIdAsync(paymentId, cancellationToken);
            
            if (payment is null)
            {
                return Result<PaymentDto>.Fail(new NotFoundError("Payment not found"));
            }

            var paymentDto = new PaymentDto(
                payment.Id,
                payment.PixCopyAndPaste,
                payment.Status,
                payment.CreatedAt
            );

            return Result<PaymentDto>.Ok(paymentDto);
        }
    }
}
