namespace TaskManagementSystem.Application.DTOs.Projects;

public sealed class ProjectMemberDto
{
    public Guid UserId { get; init; }
    public string Email { get; init; } = string.Empty;
    
    public string Role { get; init; } = string.Empty;
    
    public Guid? InvitedByUserId { get; init; }
    public string? Note { get; init; }
    
    public DateTime JoinedAtUtc { get; init; }
}