
using HeroBattleServer;
using HeroBattleServer.Factory;
using HeroBattleShare.Factory;
using UnityEngine;

public class ServerRunner : MonoBehaviour
{
    GameServer server;
    // Start is called before the first frame update
    void Start()
    {

        IGameFactorySystem gameFactory = new ServerFactorySystem();
        IGameInputSystem inputSystem = new ServerInputSystem(); 
        server = new GameServer(new UnityLogger(), gameFactory, inputSystem);
    }

    // Update is called once per frame
    void Update()
    {
        server.Update();
    }

    private void OnDestroy()
    {
        server.Dispose();
    }
}
