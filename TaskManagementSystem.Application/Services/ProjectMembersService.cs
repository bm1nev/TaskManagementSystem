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
    private readonly ProjectAccessService _access;
    
    public ProjectMembersService(IProjectRepository projects, IUserRepository users, ProjectAccessService access)
    {
        _projects = projects;
        _users = users;
        _access =  access;
    }

    public async Task AddMemberAsync(
        Guid projectId,
        Guid currentUserId,
        AddProjectMemberRequestDto request)
    {
        await _access.RequireProjectAsync(projectId);
        
        await _access.RequireOwnerOrManagerAsync(projectId, currentUserId);

        await _access.RequireOwnerOrManagerAsync(projectId, currentUserId);
        
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