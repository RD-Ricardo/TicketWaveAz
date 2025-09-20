using TicketWaveAz.Application.Intefaces;

namespace TicketWaveAz.Infrastructure.Services
{
    public class JwtService : IJwtService
    {
        public string GenerateJwt(string userId, string userName, string roleId, IEnumerable<string>? claims = null)
        {
            throw new NotImplementedException();
        }

        public string GetJwt(string key)
        {
            throw new NotImplementedException();
        }
    }
}
