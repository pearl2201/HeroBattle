using FluentValidation;
using MasterServer.Application.Common.Interfaces;
using MasterServer.Application.Common.Models;
using Microsoft.Extensions.Logging;
using ValidationException = MasterServer.Application.Common.Exceptions.ValidationException;


namespace MasterServer.Application.Game.Authentication.Commands
{
    public class LoginFacebookCommand : BaseLoginCommand
    {
        public string FacebookId { get; set; }

        public string FacebookAccessToken { get; set; }
    }

    public class LoginFacebookCommandValidator : AbstractValidator<LoginFacebookCommand>
    {
        public LoginFacebookCommandValidator()
        {
            RuleFor(v => v.DeviceId).NotEmpty();
            RuleFor(v => v.FacebookId).NotEmpty();
            RuleFor(v => v.FacebookAccessToken).NotEmpty();
        }
    }
    public class LoginFacebookCommandHandler : IRequestHandlerWrapper<LoginFacebookCommand, PlayerLoginResponse>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IIdentityService _identityService;
        private readonly ILogger<LoginFacebookCommandHandler> _logger;

        public LoginFacebookCommandHandler(IApplicationDbContext dbContext, IIdentityService identityService, ILogger<LoginFacebookCommandHandler> logger)
        {
            _dbContext = dbContext;
            _identityService = identityService;
            _logger = logger;
        }
        public async Task<ServiceResult<PlayerLoginResponse>> Handle(LoginFacebookCommand request, CancellationToken cancellationToken)
        {
            var getUserResult = await _identityService.GetOrCreatePlayerByFacebook(request.FacebookAccessToken);
            if (!getUserResult.Succeeded)
            {
                throw new ValidationException("player", $"{request.DeviceId}");
            }
            var player = getUserResult.Data;
            if (player.LockKind == Domain.Enums.PlayerLockKind.Permanent)
            {
                return ServiceResult.Failed<PlayerLoginResponse>(ServiceError.PlayerIsBan);
            }
            var identityResult = await _identityService.ContinueLoginFlow(player, request.Version, request.CountryCode, request.RemoteIpAddress);

            await _dbContext.SaveChangesAsync(CancellationToken.None);
            PlayerLoginResponse playerLoginResponse = new PlayerLoginResponse()
            {
                AccessToken = identityResult.AccessToken,
                RefreshToken = identityResult.RefreshToken,
                Id = player.Id,
                UserName = player.NickName,
                SessionId = identityResult.SessionId
            };

            return ServiceResult.Success(playerLoginResponse);
        }
    }
}
