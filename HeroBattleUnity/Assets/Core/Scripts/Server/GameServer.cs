using FixMath.NET;
using HeroBattle;
using HeroBattle.FixedMath;
using LiteEntitySystem;
using LiteEntitySystem.Transport;
using LiteNetLib;
using LiteNetLib.Utils;
using System;
using System.Net;
using System.Net.Sockets;
using static HeroBattle.GamePackets;

namespace HeroBattleApp
{
    public class GameServer : INetEventListener, IDisposable
    {
        private NetManager _netManager;
        private NetPacketProcessor _packetProcessor;
        public ushort Tick => _serverEntityManager.Tick;
        private ServerEntityManager _serverEntityManager;
        private LiteEntitySystem.ILogger _logger;
        public GameServer(LiteEntitySystem.ILogger logger)
        {
            _logger = logger;
            LiteEntitySystem.Logger.LoggerImpl = logger;

            _netManager = new NetManager(this)
            {
                AutoRecycle = true,
                PacketPoolSize = 1000,
                SimulateLatency = true,
                SimulationMinLatency = 50,
                SimulationMaxLatency = 60,
                SimulatePacketLoss = false,
                SimulationPacketLossChance = 10
            };

            _packetProcessor = new NetPacketProcessor();
            _packetProcessor.SubscribeReusable<JoinPacket, NetPeer>(OnJoinReceived);

            EntityManager.RegisterFieldType<Vector2f>();
            EntityManager.RegisterFieldType<Fix64>();
            var typesMap = new EntityTypesMap<GameEntities>()
                 .Register(GameEntities.Player, e => new BaseCharacter(e))
                 .Register(GameEntities.Minion, e => new BaseMinion(e))
                 .Register(GameEntities.BotController, e => new ServerBotController(e))
                ;
            
            _serverEntityManager = ServerEntityManager.Create<PlayerInputPacket>(
                typesMap,
                (byte)PacketType.EntitySystem,
                NetworkGeneral.GameFPS,
                ServerSendRate.EqualToFPS);
            

            //_serverEntityManager.AddSignleton<UnityPhysicsManager>();

            for (int i = 0; i < 10; i++)
            {
                int botNum = i;
                var botPlayer = _serverEntityManager.AddEntity<BaseCharacter>(e =>
                {

                });
                _serverEntityManager.AddAIController<ServerBotController>(e => e.StartControl(botPlayer));
            }

            for (int i = 0; i < 200; i++)
            {
                int botNum = i;
                var botPlayer = _serverEntityManager.AddEntity<BaseMinion>(e =>
                {
                    e.speed = new HeroBattle.FixedMath.Vector2f(Fix64.One, Fix64.One);
                });
            }

            _netManager.Start(10515);
            _logger.Log("Server start at " + 10515);
        }

        private void OnJoinReceived(JoinPacket joinPacket, NetPeer peer)
        {
            _logger.Log("[S] Join packet received: " + joinPacket.UserName);

            var serverPlayer = _serverEntityManager.AddPlayer(new LiteNetLibNetPeer(peer, true));
            var player = _serverEntityManager.AddEntity<BaseCharacter>(e =>
            {
                //e.Spawn(new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f)));
                //e.Name.Value = joinPacket.UserName;
            });
            _serverEntityManager.AddController<BaseCharacterController>(serverPlayer, player);
        }


        void INetEventListener.OnPeerConnected(NetPeer peer)
        {
            _logger.Log("[S] Player connected: " + peer);
        }

        void INetEventListener.OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            _logger.Log("[S] Player disconnected: " + disconnectInfo.Reason);

            if (peer.Tag != null)
            {
                _serverEntityManager.RemovePlayer((LiteNetLibNetPeer)peer.Tag);
            }
        }

        void INetEventListener.OnNetworkError(IPEndPoint endPoint, SocketError socketError)
        {
            _logger.Log("[S] NetworkError: " + socketError);
        }

        void INetEventListener.OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channel, DeliveryMethod deliveryMethod)
        {
            byte packetType = reader.PeekByte();
            switch ((PacketType)packetType)
            {
                case PacketType.EntitySystem:
                    _serverEntityManager.Deserialize((LiteNetLibNetPeer)peer.Tag, reader.AsReadOnlySpan());
                    break;

                case PacketType.Serialized:
                    reader.GetByte();
                    _packetProcessor.ReadAllPackets(reader, peer);
                    break;

                default:
                    _logger.LogError("Unhandled packet: " + packetType);
                    break;
            }
        }

        void INetEventListener.OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader,
            UnconnectedMessageType messageType)
        {

        }

        void INetEventListener.OnNetworkLatencyUpdate(NetPeer peer, int latency)
        {

        }

        void INetEventListener.OnConnectionRequest(ConnectionRequest request)
        {
            request.AcceptIfKey("ExampleGame");
        }

        public void Update()
        {
            _netManager.PollEvents();
            _serverEntityManager?.Update();
        }
        public void Dispose()
        {
            _netManager.Stop();
            _serverEntityManager = null;
        }
    }
}
