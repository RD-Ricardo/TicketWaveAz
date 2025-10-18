
using Azure.Messaging.ServiceBus;
using TicketWaveAz.Application.UseCases.Payment.ProcessPayment;
using TicketWaveAz.Application.UseCases.Payment.ReceivedPayment;

namespace TicketWaveAz.Api.Consumers
{
    public class PaymentConfirmedConsumer : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly IConfiguration _configuration;
        public PaymentConfirmedConsumer(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var serviceBus = new ServiceBusClient(_configuration["ServiceBusConnection"], new ServiceBusClientOptions()
            {
                TransportType = ServiceBusTransportType.AmqpWebSockets
            });

            await using var processor = serviceBus.CreateProcessor("payment-confirmed", new ServiceBusProcessorOptions());

            processor.ProcessMessageAsync += ProcessMessage;
            processor.ProcessErrorAsync += ProcessMessageError;

            await processor.StartProcessingAsync(stoppingToken);

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(1000, stoppingToken);
                }
            }
            catch (TaskCanceledException)
            {
            }
            finally
            {
                await processor.StopProcessingAsync();
                Console.WriteLine("Stopped receiving messages");
            }
        }

        private async Task ProcessMessage(ProcessMessageEventArgs eventArgs)
        {
            var scope = _serviceProvider.CreateScope();

            var paymentConfirmedUseCase = scope.ServiceProvider.GetRequiredService<IProcessPaymentUseCase>();

            try
            {
                var messageBody = eventArgs.Message.Body.ToString();

                var paymentConfirmed = System.Text.Json.JsonSerializer.Deserialize<PaymendPaidEvent>(messageBody);

                Console.WriteLine($"Received message: {messageBody}");

                if (paymentConfirmed == null)
                {
                    Console.WriteLine("Received null OrderCreateDto, abandoning message.");
                    await eventArgs.AbandonMessageAsync(eventArgs.Message);
                    return;
                }

                await paymentConfirmedUseCase.ExecuteAsync(paymentConfirmed);

                await eventArgs.CompleteMessageAsync(eventArgs.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing message: {ex.Message}");
                await eventArgs.DeadLetterMessageAsync(eventArgs.Message);
            }
        }

        private async Task ProcessMessageError(ProcessErrorEventArgs eventArgs)
        {
            Console.WriteLine($"Error processing message: {eventArgs.Exception.Message}");
            Console.WriteLine($"Error source: {eventArgs.ErrorSource}");
            Console.WriteLine($"Entity path: {eventArgs.EntityPath}");
            Console.WriteLine($"Fully qualified namespace: {eventArgs.FullyQualifiedNamespace}");
            await Task.CompletedTask;
        }
    }
}
