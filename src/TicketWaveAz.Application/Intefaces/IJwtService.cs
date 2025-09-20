namespace TicketWaveAz.Application.Intefaces
{
    public interface IJwtService
    {
        string GetJwt(string key);
        string GenerateJwt(string userId, string userName, string roleId, IEnumerable<string>? claims = null);
    }
}
