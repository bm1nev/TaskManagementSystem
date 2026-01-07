using Microsoft.AspNetCore.Identity;
using TaskManagementSystem.Application.Interfaces;

namespace TaskManagementSystem.Infrastructure.Auth;

public sealed class AspNetPasswordHasher : IPasswordHasher
{
    private readonly PasswordHasher<object> _hasher = new();

    public string Hash(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password is required.", nameof(password));

        return _hasher.HashPassword(user: null!, password);
    }

    public bool Verify(string hashedPassword, string providedPassword)
    {
        if (string.IsNullOrWhiteSpace(hashedPassword))
            return false;

        if (string.IsNullOrWhiteSpace(providedPassword))
            return false;

        var result = _hasher.VerifyHashedPassword(user: null!, hashedPassword, providedPassword);
        return result != PasswordVerificationResult.Failed;
    }
}