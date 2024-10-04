using Google.Apis.AndroidPublisher.v3.Data;
using Google.Apis.Auth;
using ICSharpCode.SharpZipLib.Zip;
using MasterServer.Application.Common.Interfaces;
using MasterServer.Application.Common.Models;
using MasterServer.Domain.Entities;
using MasterServer.Domain.Mics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Security.Claims;

namespace MasterServer.Infrastructure.Identity;

public class IdentityService : IIdentityService
{

    private readonly IAuthorizationService _authorizationService;
    private readonly ILogger<IdentityService> _logger;
    private readonly IHttpClientFactory _clientFactory;
    private readonly IApplicationDbContext _dbContext;
    private readonly ServerSetting _serverSetting;
    private readonly IRandomService _randomService;
    public IdentityService(IAuthorizationService authorizationService, ILogger<IdentityService> logger, IHttpClientFactory clientFactory, IApplicationDbContext dbContext)
    {
        _authorizationService = authorizationService;
        _logger = logger;
        _clientFactory = clientFactory;
        _dbContext = dbContext;
    }


    public Task<bool> IsInRoleAsync(ClaimsPrincipal? principal, string role)
    {
        if (principal == null)
        {
            return Task.FromResult(false);
        }

        return Task.FromResult(principal.IsInRole(role));
    }

    public async Task<bool> AuthorizeAsync(ClaimsPrincipal? principal, string policyName)
    {
        if (principal == null)
        {
            return false;
        }

        var result = await _authorizationService.AuthorizeAsync(principal, policyName);

        return result.Succeeded;
    }

    public async Task<ServiceResult<Player>> GetOrCreatePlayerByFirebasePlayerId(string username, string password)
    {
        var player = await _dbContext.Players.Where(x => x.UserName == username).FirstOrDefaultAsync();
        if (player != null && BCrypt.Net.BCrypt.Verify(password, player.HashedPassword))
        {
            return ServiceResult.Failed<Player>(ServiceError.AuthenticationCredentialIsNotCorrect);
        }
        if (player == null)
        {
            player = new Player()
            {
                UserName = username,
                NickName = "Adventure",
                HashedPassword = BCrypt.Net.BCrypt.HashPassword(password)
            };
        }

        return ServiceResult.Success(player);
    }

    public async Task<ServiceResult<Player>> GetOrCreatePlayerByGoogle(string token)
    {
        var verifyResult = await VerifyGoogleTokenId(token);
        if (verifyResult == null)
        {
            return ServiceResult.Failed<Player>(ServiceError.AuthenticationCredentialIsNotCorrect);
        }

        var player = await _dbContext.Players.Where(x => x.GoogleId == verifyResult.UserId).FirstOrDefaultAsync();

        if (player == null)
        {
            player = new Player()
            {
                UserName = verifyResult.Email,
                NickName = "Adventure",
                GoogleId = verifyResult.UserId,
                Email = verifyResult.Email,
                HashedPassword = BCrypt.Net.BCrypt.HashPassword(_randomService.RandomStringOfLength("password", 16))
            };
        }

        return ServiceResult.Success(player);
    }

    public async Task<ServiceResult<Player>> GetOrCreatePlayerByFacebook(string token)
    {
        var verifyResult = await VerifyFacebookTokenId(token);
        if (verifyResult == null)
        {
            return ServiceResult.Failed<Player>(ServiceError.AuthenticationCredentialIsNotCorrect);
        }

        var player = await _dbContext.Players.Where(x => x.GoogleId == verifyResult.UserId).FirstOrDefaultAsync();

        if (player == null)
        {
            player = new Player()
            {
                UserName = verifyResult.Email,
                NickName = "Adventure",
                FacebookId = verifyResult.UserId,
                Email = verifyResult.Email,
                HashedPassword = BCrypt.Net.BCrypt.HashPassword(_randomService.RandomStringOfLength("password", 16))
            };
        }

        return ServiceResult.Success(player);
    }

    private async Task<ValidateTokenResult> VerifyGoogleTokenId(string token)
    {
        try
        {
            // uncomment these lines if you want to add settings: 
            // var validationSettings = new GoogleJsonWebSignature.ValidationSettings
            // { 
            //     Audience = new string[] { "yourServerClientIdFromGoogleConsole.apps.googleusercontent.com" }
            // };
            // Add your settings and then get the payload
            // GoogleJsonWebSignature.Payload payload =  await GoogleJsonWebSignature.ValidateAsync(token, validationSettings);
            var validationSettings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new string[] { _serverSetting.GoogleClientId }
            };
            // Or Get the payload without settings.
            GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(token, validationSettings);
            string userId = payload.Subject;
            string email = payload.Email;
            return new ValidateTokenResult
            {
                UserId = userId,
                Email = email,
            };
        }
        catch (System.Exception)
        {
            Console.WriteLine("invalid google token");

        }
        return null;
    }


    private class ValidateTokenResult
    {
        public string UserId
        {
            get; set;
        }

        public string Email { get; set; }

    }

    private async Task<ValidateTokenResult> VerifyFacebookTokenId(string accessToken)
    {
        // verify access token with facebook API to authenticate
        var client = new HttpClient();

        var response = await client.GetAsync($"https://graph.facebook.com/v8.0/me?access_token={accessToken}");

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var message = await response.Content.ReadAsStringAsync();
        // get data from response and account from db
        var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(message);
        var facebookId = long.Parse(data!["id"]);
        var name = data["name"];
        return new ValidateTokenResult
        {
            UserId = facebookId.ToString()
        };
    }

}
