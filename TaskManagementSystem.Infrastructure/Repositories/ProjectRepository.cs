using Microsoft.EntityFrameworkCore;
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
}