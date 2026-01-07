namespace TaskManagementSystem.Application.DTOs.Projects;

public sealed class ProjectDetailsDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string?  Description { get; init; }
    
    public Guid OwnerId { get; init; }
    public DateTime CreatedAtUtc { get; init; }

    public List<ProjectMemberDto> Members { get; init; } = new();
}