using TaskManagementSystem.Application.DTOs.Users;
using TaskManagementSystem.Application.Interfaces;
using TaskManagementSystem.Domain.Entities;
using TaskManagementSystem.Domain.Enums;

namespace TaskManagementSystem.Application.Services;

public sealed class UserAdminService
{
    private readonly IUserRepository _users;
    private readonly IPasswordHasher _passwordHasher;

    public UserAdminService(IUserRepository users, IPasswordHasher passwordHasher)
    {
        _users = users;
        _passwordHasher = passwordHasher;
    }

    public async Task<Guid> CreateUserAsync(CreateUserRequestDto request)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        
        var existing = await _users.GetByEmailAsync(email);
        if (existing is not null)
            throw new InvalidOperationException("User already exists.");

        if (!Enum.TryParse<UserRole>(request.Role, true, out var role))
            throw new InvalidOperationException("Invalid role.");

        var user = new User(email, "TEMP_HASH", role);
        var hash = _passwordHasher.Hash(request.Password);
        user.SetPasswordHash(hash);
        
        await _users.AddAsync(user);
        await _users.SaveChangesAsync();
        
        return user.Id;
    }
}