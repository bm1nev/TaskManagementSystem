using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.Application.Interfaces;

public interface ITaskRepository
{
    Task AddAsync(TaskItem task);
    Task<List<TaskItem>> GetForProjectAsync(Guid projectId);
    Task SaveChangesAsync();
}