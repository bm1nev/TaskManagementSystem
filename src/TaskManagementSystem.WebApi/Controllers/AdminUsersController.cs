using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Application.DTOs.Users;
using TaskManagementSystem.Application.Services;

namespace TaskManagementSystem.WebApi.Controllers;

[ApiController]
[Route("api/admin/users")]
[Authorize(Roles = "Admin")]
public sealed class AdminUsersController : ControllerBase
{
    private readonly UserAdminService _adminService;
    
    public AdminUsersController(UserAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpPost]
    public async Task<ActionResult<object>> Create([FromBody] CreateUserRequestDto request)
    {
        var userId = await _adminService.CreateUserAsync(request);
        return Ok(new { userId });
    }

    [HttpGet]
    public ActionResult<string> ListPlaceholder()
    {
        return Ok("List users later");
    }
}