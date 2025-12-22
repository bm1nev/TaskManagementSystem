using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Domain.Entities;
using TaskManagementSystem.Application.Interfaces;
using TaskManagementSystem.Infrastructure.Persistence;

namespace TaskManagementSystem.Infrastructure.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;
    
    public UserRepository(AppDbContext db)
    {
        _db = db;
    }

    public Task<User?> GetByEmailAsync(string normalizedEmail)
    {
        return _db.Users.FirstOrDefaultAsync(u => u.Email == normalizedEmail);
    }

    public Task<User?> GetByIdAsync(Guid userId)
    {
        return _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task AddAsync(User user)
    {
        await _db.Users.AddAsync(user);
    }

    public Task SaveChangesAsync()
    {
        return _db.SaveChangesAsync();
    }
}