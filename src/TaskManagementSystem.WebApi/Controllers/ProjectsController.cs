using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Application.DTOs.Projects;
using TaskManagementSystem.Application.Services;
using TaskManagementSystem.WebApi.Security;

namespace TaskManagementSystem.WebApi.Controllers;

[ApiController]
[Route("api/Projects")]
[Authorize]
public sealed class ProjectsController : ControllerBase
{
    private readonly ProjectService _projects;
    private readonly ProjectMembersService _members;

    public ProjectsController(ProjectService projects, ProjectMembersService members)
    {
        _projects = projects;
        _members = members;
    }

    [HttpPost]
    public async Task<ActionResult<object>> Create(CreateProjectRequestDto request)
    {
        var userId = User.GetUserId();
        var projectId = await _projects.CreateAsync(userId, request);
        return Ok(new { projectId });
    }

    [HttpGet]
    public async Task<ActionResult> MyProjects()
    {
        var userId = User.GetUserId();
        var list = await _projects.GetMyProjectsAsync(userId);
        return Ok(list);
    }
    
    [HttpGet("{projectId:guid}")]
    public async Task<IActionResult> Details(Guid projectId)
    {
        var currentUserId = User.GetUserId();
        var dto = await _projects.GetDetailsAsync(projectId, currentUserId);
        return Ok(dto);
    }
    
    [HttpGet("{projectId:guid}/members")]
    public async Task<IActionResult> Members(Guid projectId)
    {
        var currentUserId = User.GetUserId();
        
        var members = await _projects.GetMemberAsync(projectId, currentUserId);
        return Ok(members);
    }

    [HttpPut("{projectId:guid}")]
    public async Task<IActionResult> Update(Guid projectId, UpdateProjectRequestDto request)
    {
        var currentUserId = User.GetUserId();
        await _projects.UpdateAsync(projectId, currentUserId, request);
        return NoContent();
    }

    [HttpDelete("{projectId:guid}/members/{userId:guid}")]
    public async Task<IActionResult> RemoveMember(Guid projectId, Guid userId)
    {
        var currentUserId = User.GetUserId();
        await _members.RemoveMemberAsync(projectId, currentUserId, userId);
        return NoContent();
    }

    [HttpPost("{projectId:guid}/leave")]
    public async Task<IActionResult> Leave(Guid projectId)
    {
        var currentUserId = User.GetUserId();
        await _members.LeaveAsync(projectId, currentUserId);
        return NoContent();
    }

}