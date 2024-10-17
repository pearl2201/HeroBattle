using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SServer
{
    public class ConnectionStatus
    {
        public NetPeer Peer { get; set; }

        public long Id { get; set; }

        public DateTime LastPing { get; set; }

        public string SessionId { get; set; }
    }

    public struct PeerTag
    {
        public long Id { get; set; }

        public string SessionId { get; set; }
    }
}
