using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.Application.Interfaces;

public interface IProjectRepository
{
    Task AddProjectAsync(Project project);
    Task AddMemberAsync(ProjectMember member);
    Task SaveChangesAsync();
    
    Task<List<Project>> GetProjectsForUserAsync(Guid userId);
}