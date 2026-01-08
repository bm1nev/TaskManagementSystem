namespace TaskManagementSystem.Application.DTOs.Projects;

public sealed class UpdateProjectRequestDto
{
    public string Name {  get; init; } =  string.Empty;
    public string? Description {  get; init; }

}