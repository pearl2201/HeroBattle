using MasterServer.Application.Common.Attributes;
using MasterServer.Application.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MasterServer.Controllers.Games
{
    [HeaderApiKey(SecurityConst.GameApiKey)]
    [Route("api/v{version:int}/game/mails")]
    [ApiController]
    [Authorize]
    public class MailsController : ApiControllerBase
    {
        private readonly ILogger<MailsController> _logger;

        public MailsController(ILogger<MailsController> logger)
        {
            _logger = logger;
        }

        [HttpGet("")]
        public async Task<ActionResult<ServiceResult<List<GameMailDto>>>> GetPlayerMailBoxWithPaginatedQuery(CancellationToken cancellationToken)
        {

            var data = await Mediator.Send(new GetPlayerMailboxQuery()
            {

            }, cancellationToken);
            return Ok(data);
        }

        [HttpPost("{mailId}/claim")]
        public async Task<ActionResult<ServiceResult<EmptyServiceResponse>>> ClaimMailRewardCommand(int mailId, CancellationToken cancellationToken)
        {

            var data = await Mediator.Send(new ClaimMailboxRewardCommand()
            {
                MailId = mailId
            }, cancellationToken);
            return Ok(data);
        }
    }
}
