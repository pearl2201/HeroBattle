using FixMath.NET;
using HeroBattle;
using HeroBattle.FixedMath;
using HeroBattleShare;
using HeroBattleShare.Factory;
using LiteEntitySystem;
using LiteEntitySystem.Transport;
using LiteNetLib;
using LiteNetLib.Utils;
using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using static HeroBattle.GamePackets;

public class NetClient : MonoBehaviour, INetEventListener
{
    private Action<DisconnectInfo> _onDisconnected;
    private NetManager _netManager;
    private NetDataWriter _writer;
    private NetPacketProcessor _packetProcessor;
    private string _userName;
    private NetPeer _server;
    private ClientEntityManager _entityManager;
    private int _ping;

    private int PacketsInPerSecond;
    private int BytesInPerSecond;
    private int PacketsOutPerSecond;
    private int BytesOutPerSecond;

    private float _secondTimer;
    static NetClient()
    {
        LiteEntitySystem.Logger.LoggerImpl = new UnityLogger();
    }


    // Start is called before the first frame update
    void Start()
    {
        AppServices.Instance.GameFactorySystem = UnityFactorySystem.instance;
        EntityManager.RegisterFieldType<Vector2>(Vector2.Lerp);
        _userName = Environment.MachineName + " " + UnityEngine.Random.Range(0, 100000);
        _writer = new NetDataWriter();
        _packetProcessor = new NetPacketProcessor();
        _netManager = new NetManager(this)
        {
            AutoRecycle = true,
            EnableStatistics = true,
            SimulateLatency = true,
            SimulationMinLatency = 50,
            SimulationMaxLatency = 60,
            SimulatePacketLoss = false,
            SimulationPacketLossChance = 10
        };
        _netManager.Start();
        Connect("localhost", null);
    }

    // Update is called once per frame
    void Update()
    {
        _netManager.PollEvents();
        _secondTimer += Time.deltaTime;
        if (_entityManager != null)
        {
            _entityManager.Update();
        }
    }

    private void OnDestroy()
    {
        _netManager.Stop();
    }
    private void SendPacket<T>(T packet, DeliveryMethod deliveryMethod) where T : class, new()
    {
        if (_server == null)
            return;
        _writer.Reset();
        _writer.Put((byte)PacketType.Serialized);
        _packetProcessor.Write(_writer, packet);
        _server.Send(_writer, deliveryMethod);
    }

    void INetEventListener.OnPeerConnected(NetPeer peer)
    {
        Debug.Log("[C] Connected to server ");
        _server = peer;

        SendPacket(new JoinPacket { UserName = _userName }, DeliveryMethod.ReliableOrdered);

        _entityManager = new ClientEntityManager(
            AppServices.Instance.RegisterTypeMap(),
            new InputProcessor<PlayerInputPacket>(),
            new LiteNetLibNetPeer(peer, true),
            (byte)PacketType.EntitySystem,
            NetworkGeneral.GameFPS);
        _entityManager.GetEntities<BaseCharacter>().SubscribeToConstructed(player =>
        {
            if (player.IsLocalControlled)
            {
                //_ourPlayer = player;
                //ClientPlayerView.Create(_clientPlayerViewPrefab, (BasePlayerController)_ourPlayer.Controller);
            }
            else
            {
                //Debug.Log($"[C] Player joined: {player.Name}");
               // RemotePlayerView.Create(_remotePlayerViewPrefab, player);
            }
        }, true);
    }

    void INetEventListener.OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
    {
        _server = null;
        _entityManager = null;
        Debug.Log("[C] Disconnected from server: " + disconnectInfo.Reason);
        if (_onDisconnected != null)
        {
            _onDisconnected(disconnectInfo);
            _onDisconnected = null;
        }
    }

    void INetEventListener.OnNetworkError(IPEndPoint endPoint, SocketError socketError)
    {
        Debug.Log("[C] NetworkError: " + socketError);
    }

    void INetEventListener.OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channel, DeliveryMethod deliveryMethod)
    {
        byte packetType = reader.PeekByte();
        var pt = (PacketType)packetType;
        switch (pt)
        {
            case PacketType.EntitySystem:
                _entityManager.Deserialize(reader.AsReadOnlySpan());
                break;

            case PacketType.Serialized:
                reader.GetByte();
                _packetProcessor.ReadAllPackets(reader);
                break;

            default:
                Debug.Log("Unhandled packet: " + pt);
                break;
        }
    }

    void INetEventListener.OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader,
        UnconnectedMessageType messageType)
    {

    }

    void INetEventListener.OnNetworkLatencyUpdate(NetPeer peer, int latency)
    {
        _ping = latency;
    }

    void INetEventListener.OnConnectionRequest(ConnectionRequest request)
    {
        request.Reject();
    }

    public void Connect(string ip, Action<DisconnectInfo> onDisconnected)
    {
        _onDisconnected = onDisconnected;
        _netManager.Connect(ip, 10515, "ExampleGame");
    }
}
