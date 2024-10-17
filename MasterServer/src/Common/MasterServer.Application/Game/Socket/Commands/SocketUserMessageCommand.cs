using MasterServer.Application.Common.Interfaces;
using MasterServer.Application.Common.Models;

namespace MasterServer.Application.Game.Socket.Commands
{
    public record SocketUserMessageCommand : IRequestWrapper<EmptyServiceResponse>
    {
        public long Id { get; set; }

        public string Payload { get; set; }

        public string PacketId { get; set; }
    }
}
