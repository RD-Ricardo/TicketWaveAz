namespace TicketWaveAz.Shared.Abstractions.Errors
{
    public class UnauthorizedError : Error
    {
        public UnauthorizedError(string message) : base(message, ErrorType.Unauthorized) { }
    }
}
