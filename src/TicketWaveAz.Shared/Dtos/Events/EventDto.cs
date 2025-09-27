namespace TicketWaveAz.Shared.Dtos.Events
{
    public record EventDto(Guid Id, 
        string Name, 
        string Description, 
        decimal Price, 
        string ImageUrl,  
        DateTime CratedAt);
}
