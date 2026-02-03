using System;
using CodingCourses.Common.Entities;
using CodingCourses.DTOs;
namespace CodingCourses.Services;

public interface ICourseService
{
    IEnumerable<CourseResponseDto> GetAllCourses();
    CourseResponseDto? GetCourseById(int courseId);
    CourseResponseDto CreateCourse(CourseCreateDto courseRequest, int teacherId);
    bool DeleteCourse(int courseId, int currentUserId, bool isAdmin);    
}
