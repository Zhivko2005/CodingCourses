using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CodingCourses.Services;
using Microsoft.AspNetCore.Authorization;
namespace CodingCourses.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("register")]
        public IActionResult Register([FromBody] DTOs.UserRegisterDto userRegisterDto)
        {
            var user = _authService.Register(userRegisterDto);
            if (user == null)
            {
                return BadRequest("User with this email already exists.");
            }
            return Ok(user);
        }
        [HttpPost("login")]
        public IActionResult Login([FromBody] DTOs.UserLoginDto userLoginDto)
        {
            var token = _authService.Login(userLoginDto);
            if (token == null)
            {
                return Unauthorized("Invalid email or password.");
            }
            return Ok(new { Token = token });
        }
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("Test endpoint is working.");
        }

        [HttpGet("check")]
        [Authorize] // Само проверява дали токенът е валиден
        public IActionResult CheckToken()
        {
        var claims = User.Claims.Select(c => new 
        {    
            Type = c.Type, 
            Value = c.Value 
        })
        .ToList();

    return Ok(new {
        Username = User.Identity?.Name,
        IsAuthenticated = User.Identity?.IsAuthenticated,
        AllClaims = claims
    });
}
    }
}
