using HeroBattleApp;
using HeroBattleServer;
using HeroBattleServer.Factory;
using HeroBattleShare.Factory;
using Serilog;
public class Program
{
    private static async Task Main(string[] args)
    {
        var logger = new LoggerConfiguration()
                       .WriteTo.Console()
                       .CreateLogger();
        var seriLogger = new SeriLogger(logger);
        CancellationTokenSource cts = new CancellationTokenSource();
        AppDomain.CurrentDomain.ProcessExit += new EventHandler((obj, e) => cts.Cancel());
        IGameFactorySystem gameFactory = new ServerFactorySystem();
        GameServer server = new GameServer(seriLogger, gameFactory);
        var fps = TimeSpan.FromSeconds(1f / 20f);
        while (!cts.IsCancellationRequested)
        {
            server.Update();
            await Task.Delay(fps);
        }
        server.Dispose();
    }
}