using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Application.DTOs.Projects;
using TaskManagementSystem.Application.Services;
using TaskManagementSystem.Domain.Entities;
using TaskManagementSystem.WebApi.Security;

namespace TaskManagementSystem.WebApi.Controllers;

[ApiController]
[Route("api/projects/{projectId:guid}/members")]
[Authorize]
public sealed class ProjectMembersController : ControllerBase
{
    private readonly ProjectMembersService _members;
    
    public ProjectMembersController(ProjectMembersService members)
        {
        _members = members;
        }

    [HttpPost]
    public async Task<IActionResult> Add(
        Guid projectId,
        AddProjectMemberRequestDto request)
    {
        var currentUserId = User.GetUserId();
        await _members.AddMemberAsync(projectId, currentUserId, request);
        return Ok();
    }
}