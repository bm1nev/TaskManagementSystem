namespace TaskManagementSystem.Application.DTOs.Tasks;

public sealed class TaskAssigneeDto
{
    public Guid UserId { get; init; }
    public string Email { get; init; } = string.Empty;
}