using TaskManagementSystem.Application.DTOs.Projects;
using TaskManagementSystem.Application.Interfaces;
using TaskManagementSystem.Domain.Entities;
using TaskManagementSystem.Domain.Enums;

namespace TaskManagementSystem.Application.Services;

public sealed class ProjectService
{
    private readonly IProjectRepository _projects;
    
    public ProjectService(IProjectRepository projects)
    {
        _projects = projects;
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
        
        return list.Select(p=> new ProjectListItemDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            OwnerId = p.OwnerId,
            CreatedAtUtc = p.CreatedAtUtc
        }).ToList();
    }
}