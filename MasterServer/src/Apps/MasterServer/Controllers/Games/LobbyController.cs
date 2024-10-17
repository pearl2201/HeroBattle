using Microsoft.AspNetCore.Mvc;

namespace MasterServer.Controllers.Games
{
    public class LobbyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
