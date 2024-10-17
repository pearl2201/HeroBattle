    using Microsoft.AspNetCore.Mvc;

namespace MasterServer.Controllers.Games
{
    public class PlayersController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
