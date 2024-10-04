using MasterServer.Domain.Entities;
using System.Security.Claims;

namespace MasterServer.Application.Common.Interfaces;

public interface ICurrentPlayerService
{
    long? PlayerSubId { get; }

    string? PlayerName { get; }

    string? Email { get; }

    ClaimsPrincipal? Principals { get; }

    Task<Player> QueryPlayer(IApplicationDbContext _dbContext);

    Task<Player> QueryDetailPlayerInfo(IApplicationDbContext _dbContext);
}
