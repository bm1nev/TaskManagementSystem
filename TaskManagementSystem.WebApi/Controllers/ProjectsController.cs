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

    public ProjectsController(ProjectService projects)
    {
        _projects = projects;
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
}