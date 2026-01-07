using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TaskManagementSystem.Application.Interfaces;
using TaskManagementSystem.WebApi.Security;

namespace TaskManagementSystem.WebApi.Controllers;

[ApiController]
[Route("api/users")]
public sealed class UsersController : ControllerBase
{
    private readonly IUserRepository _users;

    public UsersController(IUserRepository users)
    {
        _users = users;
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<object>> Me()
    {
        var userId = User.GetUserId();
        
        var user = await _users.GetByIdAsync(userId);
        if (user is null)
            return NotFound();

        return Ok(new
        {
            user.Id,
            user.Email,
            Role = user.Role.ToString(),
            user.IsActive,
            user.CreatedAtUtc
        });
    }

        [HttpGet("admin-only")]
        [Authorize(Roles = "Admin")]
        public ActionResult<object> AdminOnly()
        {
            return Ok(new { Message = "You are Admin." });
        }
}