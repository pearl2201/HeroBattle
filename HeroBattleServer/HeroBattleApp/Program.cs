using HeroBattleApp;
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

        GameServer server = new GameServer(seriLogger);
        var fps = TimeSpan.FromSeconds(1f / 20f);
        while (!cts.IsCancellationRequested)
        {
            server.Update();
            await Task.Delay(fps);
        }
        server.Dispose();
    }
}