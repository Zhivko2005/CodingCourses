using System;
using System.Data.SqlTypes;
using CodingCourses.Common.Entities;
using CodingCourses.DTOs;

namespace CodingCourses.Services;

public interface IUserService
{
    IEnumerable<UserResponseDto> GetAllUsers();
    User? GetUserById(int id); 
    User? UpdateUser(int id, UserUpdateDto userUpdateDto );
    bool DeleteUser(int id);

    bool AssignRole(int userId, int roleId);
    bool RemoveRole(int userId, int roleId);
}
 