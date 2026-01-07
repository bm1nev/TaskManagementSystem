using TaskManagementSystem.Application.DTOs.Auth;
using TaskManagementSystem.Application.Exceptions;
using TaskManagementSystem.Application.Interfaces;
using TaskManagementSystem.Domain.Entities;
using TaskManagementSystem.Domain.Enums;

namespace TaskManagementSystem.Application.Services;

public sealed class AuthService
{
    private readonly IUserRepository _users;
    private readonly ITokenService _tokens;
    private readonly IPasswordHasher _passwordHasher;
    
    public AuthService(
        IUserRepository users,
        ITokenService tokens,
        IPasswordHasher passwordHasher)
    {
        _users = users;
        _tokens = tokens;
        _passwordHasher = passwordHasher;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
    {
        var email = NormalizeEmail(request.Email);
            
        // 1) uniqueness
        var existing = await _users.GetByEmailAsync(email);
        if (existing is not null)
            throw new ValidationException("Email is already registered.");
        
        // 2) user create
        var user = new User(email: email, passwordHash: "TEMP_HASH", role: UserRole.User);
        
        // 3) password hash
        var hash = _passwordHasher.Hash(request.Password);
        user.SetPasswordHash(hash);
        
        // 4) record
        await _users.AddAsync(user);
        await _users.SaveChangesAsync();
        
        // 5) token
        var token = _tokens.CreateAccessToken(user);

        return new AuthResponseDto()
        {
            AccessToken = token.AccessToken,
            ExpiresAtUtc = token.ExpiresAtUtc,
            Email = user.Email,
            Role = user.Role.ToString(),
            UserId = user.Id
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
    {
        var email = NormalizeEmail(request.Email);
        
        var user = await _users.GetByEmailAsync(email);
        if (user is null)
            throw new ValidationException("Invalid credentials.");

        if (!user.IsActive)
            throw new ForbiddenException("User is deactivated.");
        
        var result = _passwordHasher.Verify( user.PasswordHash, request.Password);
        if (!_passwordHasher.Verify(user.PasswordHash, request.Password))
            throw new ValidationException("Invalid credentials.");
        
        var token = _tokens.CreateAccessToken(user);
        return new AuthResponseDto()
        {
            AccessToken = token.AccessToken,
            ExpiresAtUtc = token.ExpiresAtUtc,
            Email = user.Email,
            Role = user.Role.ToString(),
            UserId = user.Id
        };
    }

    private static string NormalizeEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.");
        
        return email.Trim().ToLowerInvariant();
    }
}