using System.Net.Sockets;
using QRCoder;
using QuestPDF.Fluent;
using TicketWaveAz.Application.UseCases.Payment.ReceivedPayment;
using TicketWaveAz.Domain.Interfaces.Repositories;
using TicketWaveAz.Domain.Interfaces.Services;
using TicketWaveAz.Shared.Abstractions;

namespace TicketWaveAz.Application.UseCases.Payment.ProcessPayment
{
    public interface IProcessPaymentUseCase : IUseCase
    {
        Task<Result<bool>> ExecuteAsync(PaymendPaidEvent request);
    }

    public class ProcessPaymentUseCase : IProcessPaymentUseCase
    {
        private readonly IStorageService _storageService;

        private readonly IPaymentRepository _paymentRepository;
        public ProcessPaymentUseCase(IStorageService storageService, IPaymentRepository paymentRepository)
        {
            _storageService = storageService;
            _paymentRepository = paymentRepository;
        }

        public async Task<Result<bool>> ExecuteAsync(PaymendPaidEvent request)
        {
            var payment = await _paymentRepository.GetByIdAsync(request.PaymentId,default);

            if (payment == null)
            {
                return Result<bool>.Fail(new Shared.Abstractions.Errors.NotFoundError("Payment not found."));
            }

            string qrText = $"TicketID:{request.PaymentId}";

            var qrCodeImage = GenerateQrCode(qrText);

            var fileName = $"{Guid.NewGuid()}.pdf";

            byte[] pdfBytes = CreatePdfWithQrCode(qrCodeImage, qrText);

            await _storageService.UploadBytesAsync(fileName, pdfBytes);

            return Result<bool>.Ok(true);
        }

        private byte[] GenerateQrCode(string text)
        {
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q))
            using (PngByteQRCode qrCode = new PngByteQRCode(qrCodeData))
            {
                byte[] qrCodeImage = qrCode.GetGraphic(20);
                return qrCodeImage;
            }
        }

        private byte[] CreatePdfWithQrCode(byte[] qrCodeImage, string ticketId)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(20);
                    page.Header().Text($"Ticket - {ticketId}").FontSize(20).Bold();
                    page.Content().Image(qrCodeImage);
                    page.Footer().AlignCenter().Text("Ticket wave").FontSize(12);
                });
            }).GeneratePdf();
        }
    }
}
