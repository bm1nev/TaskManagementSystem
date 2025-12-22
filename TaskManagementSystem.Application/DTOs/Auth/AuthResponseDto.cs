namespace TaskManagementSystem.Application.DTOs.Auth;

public sealed class AuthResponseDto
{
    public string AccessToken { get; init; } = string.Empty;
    
    public DateTime ExpiresAtUtc { get; init; }
    
    public string Email { get; init; } = string.Empty;
    
    public string Role { get; init; } = string.Empty;
    
    public Guid UserId { get; init; }
}
