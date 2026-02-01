using System;
using CodingCourses.Data;
using CodingCourses.Common.Entities;
using CodingCourses.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;   
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CodingCourses.Services;

public class AuthService : IAuthService 
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public User? Register(UserRegisterDto dto)
    {
        if(_context.Users.Any(u => u.Email == dto.Email))
        {
            return null;  
        }
        var user = new User
        {
            Username =  dto.Username,
            Email = dto.Email,
            Password = dto.Password 
        };
        _context.Users.Add(user);
        _context.SaveChanges();
        return user;
    }

    public string? Login(UserLoginDto userLoginDto)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == userLoginDto.Email);
        if (user == null || user.Password != userLoginDto.Password)
        {
            return null;
        }
        return CreateToken(user);
    }

    private string CreateToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

     
}
