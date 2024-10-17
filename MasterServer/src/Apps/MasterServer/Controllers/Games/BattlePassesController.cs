using Microsoft.AspNetCore.Mvc;

namespace MasterServer.Controllers.Games
{
    public class BattlePassesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
