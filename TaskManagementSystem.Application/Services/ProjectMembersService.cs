using TaskManagementSystem.Application.DTOs.Projects;
using TaskManagementSystem.Application.Exceptions;
using TaskManagementSystem.Application.Interfaces;
using TaskManagementSystem.Domain.Entities;
using TaskManagementSystem.Domain.Enums;

namespace TaskManagementSystem.Application.Services;

public sealed class ProjectMembersService
{
    private readonly IProjectRepository _projects;
    private readonly IUserRepository _users;
    
    public ProjectMembersService(IProjectRepository projects, IUserRepository users)
    {
        _projects = projects;
        _users = users;
    }

    public async Task AddMemberAsync(
        Guid projectId,
        Guid currentUserId,
        AddProjectMemberRequestDto request)
    {
        var project = await _projects.GetProjectAsync(projectId)
            ?? throw new NotFoundException($"Project with id {projectId} does not exist.");
        
        var currentMembership = await _projects.GetMemberAsync(projectId, currentUserId)
            ?? throw new ForbiddenException("You are not a project member.");
        
        if (currentMembership.Role is not (ProjectRole.Owner or ProjectRole.Manager))
            throw new ForbiddenException("Insufficient permissions.");
        
        var user = await _users.GetByIdAsync(request.UserId)
            ?? throw new NotFoundException($"User with id {request.UserId} does not exist.");

        var existing = await _projects.GetMemberAsync(projectId, user.Id);
            if (existing is not null)
            throw new InvalidOperationException("User already in project.");
            
        if (Enum.TryParse<ProjectRole>(request.Role, out var role))
            throw new ValidationException($"Role {request.Role} is not supported.");

        var member = new ProjectMember(
            projectId: projectId,
            userId: user.Id,
            role: role,
            invitedByUserId: currentUserId,
            note: request.Note
            );
        
        await _projects.AddMemberAsync(member);
        await _projects.SaveChangesAsync();
    }
}