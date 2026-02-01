using System;
using CodingCourses.Common.Entities;
using CodingCourses.DTOs;

namespace CodingCourses.Services;

public interface IAuthService
{
    User? Register(UserRegisterDto userRegisterDto);
    string? Login(UserLoginDto userLoginDto);
}
