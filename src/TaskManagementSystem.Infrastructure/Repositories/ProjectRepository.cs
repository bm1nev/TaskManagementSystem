using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Application.DTOs.Projects;
using TaskManagementSystem.Application.Interfaces;
using TaskManagementSystem.Domain.Entities;
using TaskManagementSystem.Infrastructure.Persistence;

namespace TaskManagementSystem.Infrastructure.Repositories;

public sealed class ProjectRepository : IProjectRepository
{
    private readonly AppDbContext _db;

    public ProjectRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task AddProjectAsync(Project project)
    {
        await _db.Projects.AddAsync(project);
    }

    public async Task AddMemberAsync(ProjectMember member)
    {
        await _db.ProjectMembers.AddAsync(member);
    }

    public Task SaveChangesAsync()
    {
        return _db.SaveChangesAsync();
    }

    public Task<List<Project>> GetProjectsForUserAsync(Guid userId)
    {
        return _db.Projects
            .AsNoTracking()
            .Where(p => _db.ProjectMembers.Any(pm => pm.ProjectId == p.Id && pm.UserId == userId))
            .ToListAsync();
    }

    public Task<Project?> GetProjectAsync(Guid projectId)
    {
        return _db.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
    }

    public Task<ProjectMember?> GetMemberAsync(Guid projectId, Guid userId)
    {
        return _db.ProjectMembers
            .FirstOrDefaultAsync(pm => pm.ProjectId == projectId && pm.UserId == userId);
    }
    
    public async Task<ProjectDetailsDto?> GetDetailsAsync(Guid projectId)
    {
        var project = await _db.Projects
            .AsNoTracking()
            .Where(p => p.Id == projectId)
            .Select(p => new
            {
                p.Id,
                p.Name,
                p.Description,
                p.OwnerId,
                p.CreatedAtUtc
            })
            .FirstOrDefaultAsync();

        if (project is null)
            return null;

        var members = await GetMembersAsync(projectId);

        return new ProjectDetailsDto
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            OwnerId = project.OwnerId,
            CreatedAtUtc = project.CreatedAtUtc,
            Members = members
        };
    }

    public Task<List<ProjectMemberDto>> GetMembersAsync(Guid projectId)
    {
        return _db.ProjectMembers
            .AsNoTracking()
            .Where(pm => pm.ProjectId == projectId)
            .Join(
                _db.Users,
                pm => pm.UserId,
                u => u.Id,
                (pm, u) => new ProjectMemberDto
                {
                    UserId = u.Id,
                    Email = u.Email ?? string.Empty,
                    Role = pm.Role.ToString(),
                    InvitedByUserId = pm.InvitedByUserId,
                    Note = pm.Note,
                    JoinedAtUtc = pm.CreatedAtUtc
                }
            )
            .OrderBy(x => x.Email)
            .ToListAsync();
    }

    public Task<Project?> GetTrackedByIdAsync(Guid projectId)
    {
        return _db.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
    }

    public Task<ProjectMember?> GetTrackedMemberAsync(Guid projectId, Guid userId)
    {
        return _db.ProjectMembers
            .FirstOrDefaultAsync(pm => pm.ProjectId == projectId && pm.UserId == userId);
    }

    public Task DeleteMemberAsync(ProjectMember member)
    {
        _db.ProjectMembers.Remove(member);
        return Task.CompletedTask;
    }
}