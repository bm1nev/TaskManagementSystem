using System.Security.Claims;

namespace TaskManagementSystem.WebApi.Security;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var sub = user.FindFirstValue(ClaimTypes.NameIdentifier)
                  ?? user.FindFirstValue("sub");
        
        if (string.IsNullOrWhiteSpace(sub))
            throw new InvalidOperationException("Missing user id claim.");
        return Guid.Parse(sub);
    }

    public static string GetRole(this ClaimsPrincipal user)
    {
        return user.FindFirstValue(ClaimTypes.Role) ?? string.Empty;
    }
}