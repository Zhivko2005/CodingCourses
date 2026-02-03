using Microsoft.AspNetCore.Mvc;
using CodingCourses.Services;
using CodingCourses.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CodingCourses.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LessonsController : ControllerBase
{
    private readonly ILessonService _lessonService;
    public LessonsController(ILessonService ls)
     { 
        _lessonService = ls; 
    }

    [HttpGet("course/{courseId}")]
    public IActionResult GetByCourse(int courseId)
    {
        var lessons = _lessonService.GetLessonsByCourse(courseId);
        return Ok(lessons);
    }

    [HttpPost]
    [Authorize(Roles = "Instructor")]
    public IActionResult Create(LessonCreateDto dto)
    {
        var userId = GetUserId();
        var result = _lessonService.CreateLesson(dto, userId);
        
        if (result == null)
        {
            return Forbid();
        }
        
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Instructor")]
    public IActionResult Update(int id, LessonCreateDto dto)
    {
        var userId = GetUserId();
        var result = _lessonService.UpdateLesson(id, dto, userId);
        
        if (result == null)
        {
            return Forbid();
        }
        
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Instructor,Admin")]
    public IActionResult Delete(int id)
    {
        var userId = GetUserId();
        var isAdmin = User.IsInRole("Admin");
        
        var success = _lessonService.DeleteLesson(id, userId, isAdmin);
        
        if (success == false)
        {
            return Forbid();
        }
        
        return NoContent();
    }

    private int GetUserId()
    {
        var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.Parse(claim!);
    }
}