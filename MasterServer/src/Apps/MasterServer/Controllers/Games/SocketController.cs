using MasterServer.Application.Common.Attributes;
using MasterServer.Application.Common.Models;
using MasterServer.Application.Game.Socket.Commands;
using Microsoft.AspNetCore.Mvc;

namespace MasterServer.Controllers.Games
{
    [Route("api/v1/game/socket")]
    [QueryApiKey(SecurityConst.AwsApiKey)]
    [ApiController]
    public class SocketController : ApiControllerBase
    {
        private readonly ILogger<SocketController> _logger;

        public SocketController(ILogger<SocketController> logger)
        {
            _logger = logger;
        }

        [HttpPost("connect")]
        public async Task<ActionResult<ServiceResult<EmptyServiceResponse>>> OnUserConnected(SocketUserConnectedCommand command, CancellationToken cancellationToken)
        {
            var data = await Mediator.Send(command, cancellationToken);
            return Ok(data);
        }

        [HttpPost("disconnect")]
        public async Task<ActionResult<ServiceResult<EmptyServiceResponse>>> OnUserDisconnected(SocketUserDisconnectedCommand command, CancellationToken cancellationToken)
        {
            var data = await Mediator.Send(command, cancellationToken);
            return Ok(data);
        }

        [HttpPost("message")]
        public async Task<ActionResult<ServiceResult<EmptyServiceResponse>>> OnUserMessage(SocketUserMessageCommand command, CancellationToken cancellationToken)
        {
            var data = await Mediator.Send(command, cancellationToken);
            return Ok(data);
        }


    }
}
