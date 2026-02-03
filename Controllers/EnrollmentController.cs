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
    public class EnrollmentController : ControllerBase
    {
        private readonly IEnrollmentService _service;
        public EnrollmentController(IEnrollmentService service) 
        {
            _service = service;
        }
        [HttpPost]
        [HttpPost("enroll")]
        [Authorize]
        public IActionResult Enroll([FromBody] EnrollmentRequestDto dto)
        {
            var currentUserId = GetUserId();
            var isAdmin = User.IsInRole("Admin");

            var res = _service.EnrollStudent(dto, currentUserId, isAdmin);
    
            if (res == null) 
            {
                return Forbid(); 
            }
    
            return Ok(res);
        }
        [HttpGet("my")]
        [Authorize(Roles = "Student")]
        public IActionResult GetMyEnrollments()
        {
            var res = _service.GetMyEnrollments(GetUserId());
            return Ok(res);
        }

    
        [HttpGet("course/{courseId}/participants")]
        [Authorize(Roles = "Instructor,Admin")]
        public IActionResult GetParticipants(int courseId)
        {   
            var res = _service.GetCourseParticipants(courseId, GetUserId(), User.IsInRole("Admin"));
            return Ok(res);
        }
        [HttpDelete("course/{courseId}/unenroll/{studentId}")]
        [Authorize]
        public IActionResult UnenrollStudent(int courseId, int studentId)
        {
            var currentUserId = GetUserId();
            var isAdmin = User.IsInRole("Admin");

            var success = _service.UnenrollStudent(courseId, studentId, currentUserId, isAdmin);
            if (!success)
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
}
