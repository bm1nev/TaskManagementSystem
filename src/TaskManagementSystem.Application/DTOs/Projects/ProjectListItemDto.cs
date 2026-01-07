namespace TaskManagementSystem.Application.DTOs.Projects;

public sealed class ProjectListItemDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public Guid OwnerId { get; init; }
    public DateTime CreatedAtUtc { get; init; }
}