using TaskManagementSystem.Application.DTOs.Tasks;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.Application.Interfaces;

public interface ITaskRepository
{
    Task AddAsync(TaskItem task);
    
    Task<List<TaskItem>> GetForProjectAsync(Guid projectId);
    
    Task SaveChangesAsync();
    
    Task<(Guid ProjectId, bool Exists)> GetProjectInfoAsync(Guid taskId);
    
    Task<TaskItem?> GetByIdAsync(Guid taskId);
    
    Task<List<TaskListItemDto>> GetForProjectWithAssigneesAsync(Guid projectId);
    
    Task AddAssignmentAsync(TaskAssignment assignment);
}