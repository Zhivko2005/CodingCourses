using System;
using CodingCourses.Common.Entities;
using CodingCourses.DTOs;
namespace CodingCourses.Services;

public interface ICourseService
{
    IEnumerable<CourseResponseDto> GetAllCourses();
    CourseResponseDto? GetCourseById(int courseId);
    IEnumerable<CourseResponseDto> GetCoursesByCategory(int categoryId);
    IEnumerable<CourseResponseDto> GetCoursesByInstructor(int instructorId);
    CourseResponseDto CreateCourse(CourseCreateDto courseRequest, int teacherId);
    bool DeleteCourse(int courseId, int currentUserId, bool isAdmin);   
    bool AssignCategory(int courseId, int categoryId, int currentUserId, bool isAdmin);
    bool RemoveCategory(int courseId, int categoryId, int currentUserId, bool isAdmin); 
}
