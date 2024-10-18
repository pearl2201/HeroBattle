using FluentValidation;
using MasterServer.Application.Common.Interfaces;
using MasterServer.Application.Common.Models;
using Microsoft.Extensions.Logging;
using ValidationException = MasterServer.Application.Common.Exceptions.ValidationException;
namespace MasterServer.Application.Game.Authentication.Commands
{
    public class LoginGoogleCommand : BaseLoginCommand
    {
        public string GoogleId { get; set; }

        public string GoogleAccessToken { get; set; }
    }

    public class LoginGoogleCommandValidator : AbstractValidator<LoginGoogleCommand>
    {
        public LoginGoogleCommandValidator()
        {
            RuleFor(v => v.GoogleId).NotEmpty();
            RuleFor(v => v.GoogleAccessToken).NotEmpty();
        }
    }
    public class LoginGoogleCommandHandler : IRequestHandlerWrapper<LoginGoogleCommand, PlayerLoginResponse>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IIdentityService _identityService;
        private readonly ILogger<LoginGoogleCommandHandler> _logger;

        public LoginGoogleCommandHandler(IApplicationDbContext dbContext, IIdentityService identityService, ILogger<LoginGoogleCommandHandler> logger)
        {
            _dbContext = dbContext;
            _identityService = identityService;
            _logger = logger;
        }
        public async Task<ServiceResult<PlayerLoginResponse>> Handle(LoginGoogleCommand request, CancellationToken cancellationToken)
        {
            var getUserResult = await _identityService.GetOrCreatePlayerByFacebook(request.GoogleAccessToken);
            if (!getUserResult.Succeeded)
            {
                throw new ValidationException("player", $"{request.GoogleId}-{request.GoogleAccessToken}");
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
