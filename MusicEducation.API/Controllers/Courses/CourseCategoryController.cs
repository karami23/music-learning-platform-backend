using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicEducation.API.DTOs.Courses.CourseCategory;
using MusicEducation.Application.Commands.Courses.CourseCategory.ActivateCourseCategory;
using MusicEducation.Application.Commands.Courses.CourseCategory.CreateCourseCategory;
using MusicEducation.Application.Commands.Courses.CourseCategory.DeactivateCourseCategory;
using MusicEducation.Application.Commands.Courses.CourseCategory.UpdateCourseCategory;

using MusicEducation.Application.Queries.Courses.Categories;

namespace MusicEducation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class CourseCategoryController : ControllerBase
{
    private readonly ActivateCourseCategoryHandler _activateCourseCategoryHandler;
    private readonly CreateCourseCategoryHandler _createCourseCategoryHandler;
    private readonly DeactivateCourseCategoryHandler _deactivateCourseCategoryHandler;
    private readonly UpdateCourseCategoryHandler _updateCourseCategoryHandler;

    private readonly GetActiveCourseCategoriesHandler _getActiveCourseCategoriesHandler;
    private readonly GetAllCourseCategoriesHandler _getAllCourseCategoriesHandler;
    private readonly GetCourseCategoryByIdHandler _getCourseCategoryByIdHandler;

    public CourseCategoryController(
        ActivateCourseCategoryHandler activateCourseCategoryHandler,
        CreateCourseCategoryHandler createCourseCategoryHandler,
        DeactivateCourseCategoryHandler deactivateCourseCategoryHandler,
        UpdateCourseCategoryHandler updateCourseCategoryHandler,
        GetActiveCourseCategoriesHandler getActiveCourseCategoriesHandler,
        GetAllCourseCategoriesHandler getAllCourseCategoriesHandler,
        GetCourseCategoryByIdHandler getCourseCategoryByIdHandler)
    {
        _activateCourseCategoryHandler = activateCourseCategoryHandler;
        _createCourseCategoryHandler = createCourseCategoryHandler;
        _deactivateCourseCategoryHandler = deactivateCourseCategoryHandler;
        _updateCourseCategoryHandler = updateCourseCategoryHandler;

        _getActiveCourseCategoriesHandler = getActiveCourseCategoriesHandler;
        _getAllCourseCategoriesHandler = getAllCourseCategoriesHandler;
        _getCourseCategoryByIdHandler = getCourseCategoryByIdHandler;
    }


    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(
        [FromBody] CreateCourseCategoryRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateCourseCategoryCommand(
            request.Name,
            request.Description);

        var categoryId = await _createCourseCategoryHandler.HandleAsync(
            command,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { categoryId },
            new { categoryId });
    }

    [HttpPut("{categoryId:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(
        int categoryId,
        [FromBody] UpdateCourseCategoryRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateCourseCategoryCommand(
            categoryId,
            request.Name,
            request.Description);

        await _updateCourseCategoryHandler.HandleAsync(
            command,
            cancellationToken);

        return NoContent();
    }

    [HttpPut("{categoryId:int}/activate")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Activate(
        int categoryId,
        CancellationToken cancellationToken)
    {
        var command = new ActivateCourseCategoryCommand(
            categoryId);

        await _activateCourseCategoryHandler.HandleAsync(
            command,
            cancellationToken);

        return NoContent();
    }

    [HttpPut("{categoryId:int}/deactivate")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Deactivate(
        int categoryId,
        CancellationToken cancellationToken)
    {
        var command = new DeactivateCourseCategoryCommand(
            categoryId);

        await _deactivateCourseCategoryHandler.HandleAsync(
            command,
            cancellationToken);

        return NoContent();
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetActive()
    {
        var query = new GetActiveCourseCategoriesQuery();

        var result = await _getActiveCourseCategoriesHandler
            .HandleAsync(query);

        return Ok(result);
    }

    [HttpGet("all")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllCourseCategoriesQuery();

        var result = await _getAllCourseCategoriesHandler
            .HandleAsync(query);

        return Ok(result);
    }

    [HttpGet("{categoryId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(
        int categoryId)
    {
        var query = new GetCourseCategoryByIdQuery(
            categoryId);

        var result = await _getCourseCategoryByIdHandler
            .HandleAsync(query);

        return Ok(result);
    }
}