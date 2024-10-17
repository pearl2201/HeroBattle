using Microsoft.AspNetCore.Mvc;

namespace MasterServer.Controllers.Games
{
    public class FriendsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
