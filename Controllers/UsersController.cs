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
            return Ok(user);
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
    }
}
