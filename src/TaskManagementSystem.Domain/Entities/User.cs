using TaskManagementSystem.Domain.Common;
using TaskManagementSystem.Domain.Enums;

namespace TaskManagementSystem.Domain.Entities;

public class User : BaseEntity
{
    public string Email { get; private set; } = string.Empty;
    
    public string PasswordHash { get; private set; } = string.Empty;

    public UserRole Role { get; private set; } = UserRole.User;

    public bool IsActive { get; private set; } = true;
    
    private User() { }

    public User(string email, string passwordHash, UserRole role = UserRole.User)
    {
        SetEmail(email);
        SetPasswordHash(passwordHash);
        
        Role = role;
        IsActive = true;
    }

    public void SetEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.", nameof(email));
        
        Email = email.Trim().ToLowerInvariant();
    }

    public void SetPasswordHash(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("PasswordHash is required.", nameof(passwordHash));

        PasswordHash = passwordHash;
    }
    
    public void ChangerRole(UserRole newRole) => Role = newRole;
    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}