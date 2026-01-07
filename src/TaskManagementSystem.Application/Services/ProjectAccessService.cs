using TaskManagementSystem.Application.Exceptions;
using TaskManagementSystem.Application.Interfaces;
using TaskManagementSystem.Domain.Entities;
using TaskManagementSystem.Domain.Enums;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;

namespace TaskManagementSystem.Application.Services;

public sealed class ProjectAccessService
{
    private readonly IProjectRepository _projects;
    
    public ProjectAccessService(IProjectRepository projects)
    {
        _projects = projects;
    }

    public async Task<Project> RequireProjectAsync(Guid projectId)
    {
        if (projectId == Guid.Empty)
            throw new ValidationException("Project ID is required.");
        
        var project = await _projects.GetProjectAsync(projectId);
        if (project is null)
            throw new NotFoundException($"Project {projectId} not found.");

        return project;
    }

    public async Task<ProjectMember> RequireMemberAsync(Guid projectId, Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ValidationException("User ID is required.");
        
        var member = await _projects.GetMemberAsync(projectId, userId);
        
        if (member is null)
            throw new ForbiddenException($"You are not a member in project {projectId}.");
        
        if (member.Role is not (ProjectRole.Owner or ProjectRole.Manager))
            throw new ForbiddenException("Insufficient permissions.");

        return member;
    }
    
    public async Task<ProjectMember> RequireOwnerOrManagerAsync(Guid projectId, Guid userId)
    {
        var member = await RequireMemberAsync(projectId, userId);

        if (member.Role is not (ProjectRole.Owner or ProjectRole.Manager))
            throw new ForbiddenException("Insufficient permissions.");

        return member;
    }

    public async Task<bool> IsOwnerOrManagerAsync(Guid projectId, Guid userId)
    {
        if (projectId == Guid.Empty || userId == Guid.Empty)
            return false;

        var member = await _projects.GetMemberAsync(projectId, userId);
        if (member is null)
            return  false;
        
        return member.Role is ProjectRole.Owner or ProjectRole.Manager;
    }
}