using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string normalizedEmail);
    
    Task<User?> GetByIdAsync(Guid userId);
    
    Task AddAsync(User user);
    
    Task SaveChangesAsync();
}