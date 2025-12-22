using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.Application.Interfaces;

public interface ITokenService
{
    TokenResult CreateAccessToken(User user);
}

public sealed record TokenResult(string AccessToken, DateTime ExpiresAtUtc);