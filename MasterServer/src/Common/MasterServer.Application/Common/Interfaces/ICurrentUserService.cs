using MasterServer.Domain.Entities;
using System.Security.Claims;

namespace MasterServer.Application.Common.Interfaces;

public interface ICurrentPlayerService
{
    long? PlayerId { get; }

    string? PlayerName { get; }

    string? Email { get; }

    ClaimsPrincipal? Principals { get; }
}
