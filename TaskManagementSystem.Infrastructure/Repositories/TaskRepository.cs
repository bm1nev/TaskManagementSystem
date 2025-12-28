using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Application.Interfaces;
using TaskManagementSystem.Domain.Entities;
using TaskManagementSystem.Infrastructure.Persistence;

namespace TaskManagementSystem.Infrastructure.Repositories;

public sealed class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _db;
    
    public TaskRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(TaskItem task)
    {
        await _db.Tasks.AddAsync(task);
    }

    public Task<List<TaskItem>> GetForProjectAsync(Guid projectId)
    {
       return _db.Tasks
           .AsNoTracking()
           .Where(t=>t.ProjectId == projectId)
           .OrderBy(t=>t.CreatedAtUtc)
           .ToListAsync();
    }
    
    public Task SaveChangesAsync()
    {
       return _db.SaveChangesAsync();
    }
}