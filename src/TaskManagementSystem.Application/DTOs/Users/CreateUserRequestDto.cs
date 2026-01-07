namespace TaskManagementSystem.Application.DTOs.Users;

public sealed class CreateUserRequestDto
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string Role { get; init; } = "User";
}