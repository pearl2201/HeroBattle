using Microsoft.AspNetCore.Mvc;

namespace MasterServer.Controllers.Games
{
    public class MatchmakerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
