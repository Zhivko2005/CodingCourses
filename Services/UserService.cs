using System;
using CodingCourses.Common.Entities;
using CodingCourses.Data;
using CodingCourses.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CodingCourses.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _context;
    public UserService(AppDbContext context)
    {
        _context = context;
    }
    public IEnumerable<User> GetAllUsers()
    {
        return _context.Users
        .Include(u => u.UserRoles)
        .ThenInclude(ur => ur.Role).ToList();
    }
    public User? GetUserById(int id)
    {
        return _context.Users
        .Include(u => u.UserRoles)
        .ThenInclude(ur => ur.Role)
        .FirstOrDefault(u => u.Id == id);
    }
    public User? UpdateUser(int id, UserUpdateDto userUpdateDto)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return null;
        } 

        if (!string.IsNullOrEmpty(userUpdateDto.Username))
        {
            user.Username = userUpdateDto.Username;
        }

        if (!string.IsNullOrEmpty(userUpdateDto.Email))
        {
            user.Email = userUpdateDto.Email;
        }

        if (!string.IsNullOrEmpty(userUpdateDto.Password))
        {
            user.Password = userUpdateDto.Password;
        }
        user.Username = userUpdateDto.Username;
        user.Password = userUpdateDto.Password;
        user.Email = userUpdateDto.Email;

        _context.SaveChanges();
        return user;
    }
    public bool DeleteUser(int id)
    {
        var user = _context.Users.Find(id);
        if (user == null)
        {
            return false;
        }

        _context.Users.Remove(user);
        _context.SaveChanges();
        return true;
    }
    public bool AssignRole(int userId, int roleId)
    {
        var user = _context.Users.Find(userId);
        var role = _context.Roles.Find(roleId);

        if (user == null || role == null)
        {
            return false;
        }
        var userRole = new UserRole
        {
            UserId = userId,
            RoleId = roleId
        };
        _context.UserRoles.Add(userRole);
        _context.SaveChanges();
        return true;
    }

    public bool RemoveRole(int userId, int roleId)
    {
        var userRole = _context.UserRoles
            .FirstOrDefault(ur => ur.UserId == userId && ur.RoleId == roleId);

        if (userRole == null)
        {
            return false;
        }

        _context.UserRoles.Remove(userRole);
        _context.SaveChanges();
        return true;
    }
    
}
