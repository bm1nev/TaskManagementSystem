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
    
    public Task<TaskItem?> GetByIdAsync(Guid taskId)
    {
        return _db.Tasks
            .Include(t=> t.Assignments)
            .FirstOrDefaultAsync(t => t.Id == taskId);
    }
    
    public async Task<(Guid ProjectId, bool Exists)> GetProjectInfoAsync(Guid taskId)
    {
        var info = await _db.Tasks
            .AsNoTracking()
            .Where(t => t.Id == taskId)
            .Select(t => new { t.ProjectId })
            .FirstOrDefaultAsync();

        return info is null ? (Guid.Empty, false) : (info.ProjectId, true);
    }

    public Task AddAssignmentAsync(TaskAssignment assignment)
    {
        return _db.TaskAssignments.AddAsync(assignment).AsTask();
    }

    
    public Task SaveChangesAsync()
    {
       return _db.SaveChangesAsync();
    }
}