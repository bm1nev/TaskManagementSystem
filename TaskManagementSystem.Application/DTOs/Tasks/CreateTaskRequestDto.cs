namespace TaskManagementSystem.Application.DTOs.Tasks;

public sealed class CreateTaskRequestDto
{
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public DateTime? DueDateUtc { get; init; }
}