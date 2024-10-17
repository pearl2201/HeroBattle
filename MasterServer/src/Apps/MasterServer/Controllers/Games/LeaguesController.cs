using MasterServer.Application.Common.Attributes;
using MasterServer.Application.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MasterServer.Controllers.Games
{
    [HeaderApiKey(SecurityConst.GameApiKey)]
    [Route("api/v{version:int}/game/leagues")]
    [ApiController]
    [Authorize]
    public class LeaguesController : ApiControllerBase
    {
        private readonly ILogger<LeaguesController> _logger;

        public LeaguesController(ILogger<LeaguesController> logger)
        {
            _logger = logger;
        }

        [HttpGet("active")]
        [AllowAnonymous]
        public async Task<ActionResult<ServiceResult<LeagueSeasonViewModel>>> GetActiveLeagues(CancellationToken cancellationToken)
        {
            var data = await Mediator.Send(new GetActiveLeagueSeasonInfo(), cancellationToken);
            return Ok(data);
        }


        [HttpGet("active/self")]
        public async Task<ActionResult<ServiceResult<PlayerLeagueResponse>>> GetUserLeague(CancellationToken cancellationToken)
        {
            var data = await Mediator.Send(new QueryUserLeagueInfo(), cancellationToken);
            return Ok(data);
        }

        [HttpPost("claims")]
        public async Task<ActionResult<ServiceResult<EmptyServiceResponse>>> ReceiveLeagueRewards(ReceiveRankRewardsCommand receiveRankRewardsCommand, CancellationToken cancellationToken)
        {
            var data = await Mediator.Send(receiveRankRewardsCommand, cancellationToken);
            return Ok(data);
        }


        [HttpPost("league-points/add")]
        public async Task<ActionResult<ServiceResult<EmptyServiceResponse>>> AddLeaguePointToPlayer(AddLeaguePointToPlayerCommand addLeaguePointToPlayerCommand, CancellationToken cancellationToken)
        {
            var data = await Mediator.Send(addLeaguePointToPlayerCommand, cancellationToken);
            return Ok(data);
        }
    }
}
