using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CodingCourses.Data;
using CodingCourses.Services;
using CodingCourses.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace CodingCourses.Controllers
{  
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _userService.GetAllUsers();
            return Ok(users);
        }
        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            var response = new UserResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Roles = user.UserRoles.Select(ur => ur.Role.RoleName).ToList()
            };
            return Ok(response);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UserUpdateDto userUpdateDto)
        {
            var updatedUser = _userService.UpdateUser(id, userUpdateDto);
            if (updatedUser == null)
            {
                return NotFound();
            }
            return Ok(updatedUser);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var result = _userService.DeleteUser(id);
            if (!result)
            {
                return NotFound();
            }
            return Ok("User deleted successfully.");
        }
        [HttpPost("{userId}/roles/{roleId}")] 
        public IActionResult AssignRole(int userId, int roleId)
        { 
            var result = _userService.AssignRole(userId, roleId);
            if (!result)
            { 
                return NotFound("User or Role not found.");
            }
            return Ok("Role assigned successfully.");
        }
        [HttpDelete("{userId}/roles/{roleId}")]
        public IActionResult RemoveRole(int userId, int roleId)
        {
            var result = _userService.RemoveRole(userId, roleId);
            if (!result)
            {
                return NotFound("User or Role not found.");
            }
            return Ok("Role removed successfully.");
        }
    }
}
