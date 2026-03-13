using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CodingCourses.Services;
using CodingCourses.DTOs;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace CodingCourses.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;
        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet] 
        public IActionResult GetAllCourses()
        {
            var courses = _courseService.GetAllCourses();
            return Ok(courses);
        }
        [HttpGet("{id}")]
        public IActionResult GetCourseById(int id)
        {
            var course = _courseService.GetCourseById(id);
            if (course == null)
            {
                return NotFound("No course found with the given ID.");
            }
            return Ok(course);
        }
        [HttpGet("my-courses")]
        public IActionResult GetCoursesByInstructor(int instructorId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) 
            {
                return Unauthorized("User is not authenticated to view courses.");
            }
            instructorId = int.Parse(userIdClaim.Value);
            var courses = _courseService.GetCoursesByInstructor(instructorId);
            return Ok(courses);
        }
        [HttpGet("category/{categoryId}")]
        public IActionResult GetByCategory(int categoryId)
        {
            var courses = _courseService.GetCoursesByCategory(categoryId);
            return Ok(courses);
        }
        [HttpPost]
        [Authorize(Roles = "Admin,Instructor")]
        public IActionResult CreateCourse([FromBody] CourseCreateDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) 
            {
                return Unauthorized("User is not authenticated to create a course.");
            }
            var instructorId = int.Parse(userIdClaim.Value);
            var result = _courseService.CreateCourse(dto, instructorId);
            return CreatedAtAction(nameof(GetCourseById), new { id = result.Id }, result);

        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Instructor")]
        public IActionResult DeleteCourse(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");

            var result = _courseService.DeleteCourse(id, int.Parse(userIdClaim!.Value), isAdmin);
            if (!result)
            {
                return Forbid("You are not authorized to delete this course or it does not exist.");
            }
            return NoContent();
        }
        
        [HttpPost("assign-category")]
        [Authorize(Roles = "Instructor,Admin")]
        public IActionResult AssignCategory([FromBody] CourseCategoryRequestDto dto)
        {
            var success = _courseService.AssignCategory(dto.CourseId, dto.CategoryId, GetUserId(), User.IsInRole("Admin"));
    
            if (!success)
            {
                return BadRequest("Could not assign category. Check IDs and permissions.");
            }
            return Ok("Category assigned successfully.");
        }

        [HttpDelete("remove-category")]
        [Authorize(Roles = "Instructor,Admin")]
        public IActionResult RemoveCategory([FromBody] CourseCategoryRequestDto dto)
        { 
            var success = _courseService.RemoveCategory(dto.CourseId, dto.CategoryId, GetUserId(), User.IsInRole("Admin"));
    
            if (!success)
            {
                return BadRequest("Could not remove category. Check IDs and permissions.");
            }
            return NoContent();
        }
        public int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return int.Parse(userIdClaim!.Value);
        }
    }
}