using MasterServer.Application.Common.Attributes;
using MasterServer.Application.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MasterServer.Controllers.Games
{
    [Route("api/v1/shop")]
    [HeaderApiKey(SecurityConst.GameApiKey)]
    [ApiController]
    [Authorize]
    public class EconomyController : ApiControllerBase
    {
        private readonly ILogger<EconomyController> _logger;

        public EconomyController(ILogger<EconomyController> logger)
        {
            _logger = logger;
        }

    }
}
