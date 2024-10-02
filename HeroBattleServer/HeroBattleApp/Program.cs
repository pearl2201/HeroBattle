using HeroBattleApp;
public class Program
{
    private static async Task Main(string[] args)
    {
        CancellationTokenSource cts = new CancellationTokenSource();
        AppDomain.CurrentDomain.ProcessExit += new EventHandler((obj, e) => cts.Cancel());

        GameServer server = new GameServer();
        var fps = TimeSpan.FromSeconds(1f / 20f);
        while (!cts.IsCancellationRequested)
        {
            server.Update();
            await Task.Delay(fps);
        }
        server.Dispose();
    }
}