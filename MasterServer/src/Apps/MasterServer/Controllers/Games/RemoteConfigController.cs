using Microsoft.AspNetCore.Mvc;

namespace MasterServer.Controllers.Games
{
    public class RemoteConfigController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
