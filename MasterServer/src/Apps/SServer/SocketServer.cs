using LiteNetLib;
using LiteNetLib.Utils;
using System.Collections.Concurrent;

namespace SServer
{
    public class SocketServer : BackgroundService
    {
        private readonly ILogger<SocketServer> _logger;
        private NetManager server;
        private RabbitMqMessageQueueService _queueService;
        ConcurrentDictionary<long, List<ConnectionStatus>> peerDicts = new ConcurrentDictionary<long, List<ConnectionStatus>>();
        static object lockObject = new object();
        private float clearZombieCountdown;
        public SocketServer(IConfiguration configuration, ILogger<SocketServer> logger, RabbitMqMessageQueueService queueService)
        {
            _logger = logger;
            _queueService = queueService;

            EventBasedNetListener listener = new EventBasedNetListener();
            server = new NetManager(listener);
            server.Start(int.Parse(configuration["Port"]) /* port */);

            listener.ConnectionRequestEvent += OnConnectedRequest;

            listener.PeerConnectedEvent += OnPeerConnectedEvent;

            listener.NetworkReceiveEvent += NetworkReceiveEvent;

            listener.PeerDisconnectedEvent += OnPeerDisconnectedEvent;
            _queueService.RegisterSocketServer(this);
        }

        private void OnConnectedRequest(ConnectionRequest request)
        {
            var key = request.Data.GetString();
            var id = request.Data.GetLong();
            var sessionId = request.Data.GetString();
            if (key == "herobase")
            {
                var peer = request.Accept();
                peer.Tag = new PeerTag
                {
                    Id = id,
                    SessionId = sessionId
                };
            }
            else
            {
                request.Reject();
            }
        }

        private void OnPeerConnectedEvent(NetPeer peer)
        {
            //Console.WriteLine("We got connection: {0}", peer);  // Show peer ip
            //NetDataWriter writer = new NetDataWriter();         // Create writer class
            //writer.Put("Hello");
            //writer.Put("World!");   // Put some string
            //peer.Send(writer, DeliveryMethod.ReliableOrdered);  // Send with reliability
            lock (lockObject)
            {
                var connections = peerDicts.GetOrAdd(((PeerTag)peer.Tag).Id, new List<ConnectionStatus>());
                long userId = ((PeerTag)peer.Tag).Id;
                connections.Add(new ConnectionStatus()
                {
                    LastPing = DateTime.UtcNow,
                    Peer = peer,
                    Id = userId,
                    SessionId = ((PeerTag)peer.Tag).SessionId
                });
                _queueService.PublishPlayerConnect(userId, peer.Id, Guid.Parse(((PeerTag)peer.Tag).SessionId));
            }
        }

        private void NetworkReceiveEvent(NetPeer fromPeer, NetPacketReader dataReader, byte channel, DeliveryMethod deliveryMethod)
        {
            var id = ((PeerTag)fromPeer.Tag).Id;
            if (peerDicts.TryGetValue(id, out var connections))
            {
                foreach (var conn in connections)
                {
                    if (conn.Peer == fromPeer)
                    {
                        conn.LastPing = DateTime.UtcNow;
                    }
                }
                string packetId = dataReader.GetString();
                if (string.Equals(packetId, "ping") || string.Equals(packetId, "pong"))
                {

                }
            }
            dataReader.Recycle();
        }

        private void OnPeerDisconnectedEvent(NetPeer fromPeer, DisconnectInfo disconnectInfo)
        {
            lock (lockObject)
            {
                var id = ((PeerTag)fromPeer.Tag).Id;
                if (peerDicts.TryGetValue(id, out var connections))
                {
                    connections.RemoveAll(x => x.Peer == fromPeer);
                    if (connections.Count == 0)
                    {
                        peerDicts.Remove(id, out var _);
                    }
                    _queueService.PublishPlayerDisconnect(id, fromPeer.Id, Guid.Parse(((PeerTag)fromPeer.Tag).SessionId));
                }

            }
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                server.PollEvents();
                ClearZombieTask();
                await Task.Delay(15);
            }
        }

        private async void ClearZombieTask()
        {
            clearZombieCountdown += 15;
        }
        public void Boardcast(string message)
        {
            NetDataWriter writer = new NetDataWriter();
            writer.Put(message);
            foreach (var cc in peerDicts)
            {
                foreach (var netclient in cc.Value)
                {
                    netclient.Peer.Send(writer, DeliveryMethod.ReliableOrdered);
                }
            }
        }

        public void SendTo(long player, string message)
        {
            NetDataWriter writer = new NetDataWriter();
            writer.Put(message);
            if (peerDicts.TryGetValue(player, out var cc))
            {
                foreach (var netclient in cc)
                {
                    netclient.Peer.Send(writer, DeliveryMethod.ReliableOrdered);
                }
            }
        }

    }
}
