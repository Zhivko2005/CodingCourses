using System.Security.Claims;
using CodingCourses.DTOs;
using CodingCourses.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodingCourses.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubmissionsController : ControllerBase
    {
        private readonly ISubmissionService _service;
        public SubmissionsController(ISubmissionService service) 
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(Roles = "Student")]  
        public IActionResult Create(SubmissionCreateDto dto)
        {
            var res = _service.CreateSubmission(dto, int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!));
            if (res == null) 
            {
                return BadRequest("You must be enrolled in the course to submit assignments.");
            }
            return Ok(res);
        }

        [HttpGet("assignment/{assignmentId}")]
        [Authorize] 
        public IActionResult GetSubmissions(int assignmentId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var isAdmin = User.IsInRole("Admin");
            var isInstructor = User.IsInRole("Instructor");

            var result = _service.GetSubmissions(assignmentId, userId, isAdmin, isInstructor);
    
    
            return Ok(result);
        }
    }
}
