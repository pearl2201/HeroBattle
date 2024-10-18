using MasterServer.Application.Common.Attributes;
using MasterServer.Application.Common.Models;
using MasterServer.Application.Game.Authentication.Commands;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace MasterServer.Controllers.Games
{
    [HeaderApiKey(SecurityConst.GameApiKey)]
    [Route("api/v{version:int}/game/authentication")]
    [ApiController]
    public class AuthenticationController : ApiControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;


        public AuthenticationController(ILogger<AuthenticationController> logger)
        {
            _logger = logger;
        }

        [HttpPost("login/anonymous")]
        public async Task<ActionResult<ServiceResult<PlayerLoginResponse>>> LoginAnonymous(LoginAnonymousCommand playerLoginCommand, CancellationToken cancellationToken)
        {
            playerLoginCommand.RemoteIpAddress = GetRemoteIpAddress(HttpContext);
            playerLoginCommand.CountryCode = GetCountryCode(HttpContext);
            var data = await Mediator.Send(playerLoginCommand, cancellationToken);
            return Ok(data);
        }

        [HttpPost("login/facebook")]
        public async Task<ActionResult<ServiceResult<PlayerLoginResponse>>> LoginFacebook(LoginFacebookCommand playerLoginCommand, CancellationToken cancellationToken)
        {
            playerLoginCommand.RemoteIpAddress = GetRemoteIpAddress(HttpContext);
            playerLoginCommand.CountryCode = GetCountryCode(HttpContext);
            var data = await Mediator.Send(playerLoginCommand, cancellationToken);
            return Ok(data);
        }


        [HttpPost("login/google")]
        public async Task<ActionResult<ServiceResult<PlayerLoginResponse>>> LoginGoogle(LoginGoogleCommand playerLoginCommand, CancellationToken cancellationToken)
        {
            playerLoginCommand.RemoteIpAddress = GetRemoteIpAddress(HttpContext);
            playerLoginCommand.CountryCode = GetCountryCode(HttpContext);
            var data = await Mediator.Send(playerLoginCommand, cancellationToken);
            return Ok(data);
        }


        [HttpPost("login/credentials")]
        public async Task<ActionResult<ServiceResult<PlayerLoginResponse>>> LoginCredentials(LoginCredentialsCommand playerLoginCommand, CancellationToken cancellationToken)
        {
            playerLoginCommand.RemoteIpAddress = GetRemoteIpAddress(HttpContext);
            playerLoginCommand.CountryCode = GetCountryCode(HttpContext);
            var data = await Mediator.Send(playerLoginCommand, cancellationToken);
            return Ok(data);
        }



        [HttpPost("refresh-token")]
        public async Task<ActionResult<ServiceResult<PlayerLoginResponse>>> RefreshToken(RefreshAccessTokenCommand playerLoginCommand, CancellationToken cancellationToken)
        {
            playerLoginCommand.RemoteIpAddress = GetRemoteIpAddress(HttpContext);
            var data = await Mediator.Send(playerLoginCommand, cancellationToken);
            return Ok(data);
        }

        public static string GetRemoteIpAddress(HttpContext httpContext)
        {
            if (httpContext.Request.Headers.TryGetValue("cf-connecting-ip", out StringValues remoteIpAddress))
            {
                if (remoteIpAddress.Any())
                {
                    return remoteIpAddress.First();
                }

            }
            return httpContext.Connection.RemoteIpAddress.ToString();
        }

        public static string GetCountryCode(HttpContext httpContext)
        {
            if (httpContext.Request.Headers.TryGetValue("cf-ipcountry", out StringValues ipCountries))
            {
                if (ipCountries.Any())
                {
                    return ipCountries.First();
                }

            }
            return "unknow";
        }
    }
}
