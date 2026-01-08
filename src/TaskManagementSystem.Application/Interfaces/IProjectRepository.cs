using TaskManagementSystem.Application.DTOs.Projects;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.Application.Interfaces;

public interface IProjectRepository
{
    Task AddProjectAsync(Project project);
    Task AddMemberAsync(ProjectMember member);
    Task SaveChangesAsync();
    
    Task<Project?> GetProjectAsync(Guid projectId);
    Task<ProjectMember?> GetMemberAsync(Guid projectId, Guid userId);
    
    Task<List<Project>> GetProjectsForUserAsync(Guid userId);
    
    Task<ProjectDetailsDto?> GetDetailsAsync(Guid projectId);
    Task<List<ProjectMemberDto>> GetMembersAsync(Guid projectId);
    
    Task<Project?> GetTrackedByIdAsync(Guid projectId);
}