namespace TaskManagementSystem.Application.DTOs.Tasks;

public sealed class ChangeTaskStatusRequestDto
{
    public string Status { get; init; } = string.Empty;
}