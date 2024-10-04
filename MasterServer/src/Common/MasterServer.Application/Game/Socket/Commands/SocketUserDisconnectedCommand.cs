using MasterServer.Application.Common.Interfaces;
using MasterServer.Application.Common.Models;

namespace MasterServer.Application.Game.Socket.Commands
{
    public class SocketUserDisconnectedCommand : IRequestWrapper<EmptyServiceResponse>
    {
        public long Id { get; set; }

        public int ConnectionId { get; set; }

        public Guid SessionId { get; set; }
    }
}
