using TaskManagementSystem.Application.DTOs.Projects;
using TaskManagementSystem.Application.Exceptions;
using TaskManagementSystem.Application.Interfaces;
using TaskManagementSystem.Domain.Entities;
using TaskManagementSystem.Domain.Enums;

namespace TaskManagementSystem.Application.Services;

public sealed class ProjectService
{
    private readonly IProjectRepository _projects;
    private readonly ProjectAccessService _access;

    public ProjectService(IProjectRepository projects, ProjectAccessService access)
    {
        _projects = projects ?? throw new ArgumentNullException(nameof(projects));
        _access = access ?? throw new ArgumentNullException(nameof(access));
    }

    public async Task<Guid> CreateAsync(Guid currentUserId, CreateProjectRequestDto request)
    {
        var name = (request.Name ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Project name is required!");

        var project = new Project(
            name: name,
            ownerId: currentUserId,
            description: request.Description
        );

        var membership = new ProjectMember(
            projectId: project.Id,
            userId: currentUserId,
            role: ProjectRole.Owner,
            invitedByUserId: null,
            note: "Auto-added owner membership."
        );

        await _projects.AddMemberAsync(membership);
        await _projects.AddProjectAsync(project);
        await _projects.SaveChangesAsync();

        return project.Id;
    }

    public async Task<List<ProjectListItemDto>> GetMyProjectsAsync(Guid currentUserId)
    {
        var list = await _projects.GetProjectsForUserAsync(currentUserId);

        return list.Select(p => new ProjectListItemDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            OwnerId = p.OwnerId,
            CreatedAtUtc = p.CreatedAtUtc
        }).ToList();
    }

    public async Task<ProjectDetailsDto> GetDetailsAsync(Guid projectId, Guid currentUserId)
    {
        await _access.RequireMemberAsync(projectId, currentUserId);

        var details = await _projects.GetDetailsAsync(projectId);
        if (details is null)
            throw new NotFoundException("Project not found.");

        return details;
    }

    public async Task<List<ProjectMemberDto>> GetMemberAsync(Guid projectId, Guid currentUserId)
    {
        await _access.RequireMemberAsync(projectId, currentUserId);
        
        var members = await _projects.GetMembersAsync(projectId);
        return members;
    }

    public async Task UpdateAsync(Guid projectId, Guid currentUserId, UpdateProjectRequestDto request)
    {
        await _access.RequireOwnerOrManagerAsync(projectId, currentUserId);
        
        var project = await _projects.GetTrackedByIdAsync(projectId)
            ?? throw new NotFoundException("Project not found.");
        
        project.Update(request.Name, request.Description);
        
        await _projects.SaveChangesAsync();
    }

}