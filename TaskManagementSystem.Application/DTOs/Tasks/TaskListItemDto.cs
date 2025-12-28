using TaskStatus = TaskManagementSystem.Domain.Enums.TaskStatus;

namespace TaskManagementSystem.Application.DTOs.Tasks;

public sealed class TaskListItemDto
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public TaskStatus Status { get; init; }
    public DateTime? DueDateUtc { get; init; }
}