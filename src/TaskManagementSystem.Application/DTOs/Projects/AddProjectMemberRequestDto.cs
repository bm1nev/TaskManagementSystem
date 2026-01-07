namespace TaskManagementSystem.Application.DTOs.Projects;

public sealed class AddProjectMemberRequestDto
{
    public Guid UserId { get; init; }
    public string Role { get; init; } = "Member";
    public string? Note { get; init; }
}