using MasterServer.Application.Common.Interfaces;
using System.Security.Claims;

namespace MasterServer.Services
{
    public class CurrentPlayerService : ICurrentPlayerService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentPlayerService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public long? PlayerId
        {
            get
            {
                var s = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Sid);
                if (!string.IsNullOrEmpty(s) && long.TryParse(s, out long id))
                {
                    return id;
                }
                return null;
            }
        }

        public string? Email => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);

        public string? PlayerName => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);

        public ClaimsPrincipal? Principals => _httpContextAccessor.HttpContext?.User;

        public string GameVersion
        {
            get
            {
                var s = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Version);
                if (!string.IsNullOrEmpty(s))
                {
                    return s;
                }
                return string.Empty;
            }
        }
    }
}
