using Microsoft.AspNetCore.Mvc;
using CodingCourses.Services;
using CodingCourses.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

[Route("api/[controller]")]
[ApiController]
public class AssignmentsController : ControllerBase
{
    private readonly IAssignmentService _service;
    public AssignmentsController(IAssignmentService service) 
    {
        _service = service;
    }
    [HttpGet("course/{courseId}")]
    [AllowAnonymous] 
    public IActionResult GetByCourse(int courseId)
    {
        var assignments = _service.GetByCourse(courseId);
        return Ok(assignments);
    }
    [HttpPost]
    [Authorize(Roles = "Instructor")]
    public IActionResult Create(AssignmentCreateDto dto)
    {
        var res = _service.CreateAssignment(dto, GetUserId(), User.IsInRole("Admin"));
        if (res == null) 
        {
            return Forbid();
        }
        return Ok(res);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Instructor")]
    public IActionResult Update(int id, AssignmentCreateDto dto)
    {
        var res = _service.UpdateAssignment(id, dto, GetUserId());
        if (res == null)
        {
            return Forbid();
        } 
        return Ok(res);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Instructor,Admin")]
    public IActionResult Delete(int id)
    {
        var success = _service.DeleteAssignment(id, GetUserId(), User.IsInRole("Admin"));
        if (success == false) 
        {
            return Forbid();
        }
        return NoContent();
    }

    private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}