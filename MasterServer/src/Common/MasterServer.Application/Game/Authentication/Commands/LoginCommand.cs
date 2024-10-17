using MasterServer.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterServer.Application.Game.Authentication.Commands
{
    public class LoginCommand : IRequestWrapper<PlayerLoginResponse>
    {
        public string Version { get; set; }

        public string FirebaseTokenId { get; set; }

        public string FirebasePlayerId { get; set; }

        public string RemoteIpAddress { get; set; }

        public string CountryCode { get; set; }
    }

    public record PlayerLoginResponse
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public long Id { get; set; }

        public string UserName { get; set; }
    }
}
