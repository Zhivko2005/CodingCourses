using System;
using CodingCourses.Common.Entities;
using CodingCourses.Data;
using CodingCourses.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CodingCourses.Services;

public class CourseService : ICourseService
{
    private readonly AppDbContext _context;
    public CourseService(AppDbContext context)
    {
        _context = context;
    }
    public IEnumerable<CourseResponseDto> GetAllCourses()
    {
        return _context.Courses
        .Include(c => c.Teacher)
        .Include(c => c.CourseCategories)
            .ThenInclude(cc => cc.Category)
        .Select(c => new CourseResponseDto
        {
            Id = c.Id,
            Title = c.Title,
            Description = c.Description,
            Price = c.Price,
            InstructorName = c.Teacher.Username
        }).ToList();
    }
    public CourseResponseDto? GetCourseById(int courseId)
    {
        var course = _context.Courses
        .Include(c => c.Teacher)
        .Include(c => c.CourseCategories)
            .ThenInclude(cc => cc.Category)  
        .FirstOrDefault(c => c.Id == courseId);

        if (course == null) return null;

        return new CourseResponseDto
        {
            Id = course.Id,
            Title = course.Title,
            Description = course.Description,
            Price = course.Price,
            InstructorName = course.Teacher.Username,
        
            Categories = course.CourseCategories
                .Select(cc => cc.Category.CategoryName)
                .ToList()
        };
      
    }
    public IEnumerable<CourseResponseDto> GetCoursesByCategory(int categoryId)
    {
        return _context.CourseCategories
            .Where(cc => cc.CategoryId == categoryId)
            .Include(cc => cc.Course) 
            .Select(cc => new CourseResponseDto
            {
                Id = cc.Course.Id,
                Title = cc.Course.Title,
                Description = cc.Course.Description,
                Price = cc.Course.Price,    
                InstructorName = cc.Course.Teacher.Username
            })
            .ToList();
}
    public CourseResponseDto CreateCourse(CourseCreateDto dto, int teacherId)
    {
        var course = new Course
        {
            Title = dto.Title,
            Description = dto.Description,
            Price = dto.Price,
            TeacherId = teacherId
        };
        if (dto.CategoryIds != null)
        {
            
            foreach (var catId in dto.CategoryIds)
            {
                course.CourseCategories.Add(new CourseCategory 
                { 
                    CategoryId = catId 
                });
            }
        
        }
        _context.Courses.Add(course);
        _context.SaveChanges();

        return  GetCourseById(course.Id)!;
    }
    public bool DeleteCourse(int courseId, int currentUserId, bool isAdmin)
    {
        var course = _context.Courses.Find(courseId);
        if (course == null)
        {
            return false;
        }
        if (!isAdmin && course.TeacherId != currentUserId)
        {
            return false;
        }
        _context.Courses.Remove(course);
        _context.SaveChanges();
        return true;
    }
    public bool AssignCategory(int courseId, int categoryId, int currentUserId, bool isAdmin)
    {
        var course = _context.Courses.Find(courseId);
        var category = _context.Categories.Find(categoryId);    
        if (course == null || category == null) return false;
 
        if (!isAdmin && course.TeacherId != currentUserId) return false;

     
        var alreadyExists = _context.CourseCategories
            .Any(cc => cc.CourseId == courseId && cc.CategoryId == categoryId);

        if (alreadyExists) return true;  

        var courseCategory = new CourseCategory
        {
            CourseId = courseId,
            CategoryId = categoryId
        };

        _context.CourseCategories.Add(courseCategory);
        _context.SaveChanges();
        return true;
    }
    public bool RemoveCategory(int courseId, int categoryId, int currentUserId, bool isAdmin)
    {
        // 1. Намираме курса, за да проверим кой е учителят му
        var course = _context.Courses.Find(courseId);
        if (course == null) 
        {
            return false;
        }

        // 2. Проверка за права (Само учителят на курса или Админ)
        if (!isAdmin && course.TeacherId != currentUserId)
        {
            return false;
        }

        // 3. Търсим записа в свързващата таблица
        var courseCategory = _context.CourseCategories
            .FirstOrDefault(cc => cc.CourseId == courseId && cc.CategoryId == categoryId);

        if (courseCategory == null)
        {
            // Ако такава връзка не съществува, няма какво да трием
            return false;
        }
    // 4. Изтриваме връзката
        _context.CourseCategories.Remove(courseCategory);
        _context.SaveChanges();

        return true;
    }
}
